$Release = "SQL 2008 R2"
$DataSource = "localhost\sql2008r2"

$ConnectionString = "data source = $DataSource; initial catalog = master; trusted_connection = true;"
$SqlConnection = New-Object System.Data.SqlClient.SqlConnection($ConnectionString)

$SqlCmd = New-Object System.Data.SqlClient.SqlCommand
$SqlCmd.Connection = $SqlConnection

$SqlCmd.CommandText = "select Release = '$Release', * from sys.dm_xe_objects;"

$sda = New-Object System.Data.SqlClient.SqlDataAdapter($SqlCmd)
$Output = New-Object System.Data.DataTable

try {
    $sda.Fill($Output) | Out-Null
}
finally {
    $sda.Dispose()
    $SqlCmd.Dispose()
    $SqlConnection.Dispose()
}

$Queries = 
    foreach ($Row in $Output.Rows) {
        if ($Row["capabilities"] -eq [System.DBNull]::Value) {
            $Capabilities = "NULL"
        }
        else {
            $Capabilities = "$($Row["capabilities"].ToString())"
        }
        if ($Row["capabilities_desc"] -eq [System.DBNull]::Value) {
            $CapabilitiesDesc = "NULL"
        }
        else {
            $CapabilitiesDesc = "'$($Row["capabilities_desc"].ToString())'"
        }
        if ($Row["type_name"] -eq [System.DBNull]::Value) {
            $TypeName = "NULL"
        }
        else {
            $TypeName = "'$($Row["type_name"].ToString())'"
        }
        if ($Row["type_package_guid"] -eq [System.DBNull]::Value) {
            $TypePackageGuid = "NULL"
        }
        else {
            $TypePackageGuid = "'$($Row["type_package_guid"].ToString())'"
        }
        if ($Row["type_size"] -eq [System.DBNull]::Value) {
            $TypeSize = "NULL"
        }
        else {
            $TypeSize = "$($Row["type_size"].ToString())"
        }

        "if not exists 
(
    select 1
    from dbo.XeObjects o
    inner join dbo.Release r
    on o.ReleaseId = r.Id
    where r.FriendlyName = '$Release'
    and o.name = '$($Row["name"].ToString())'
    and o.object_type = '$($Row["object_type"].ToString())'
    and o.package_guid = '$($Row["package_guid"].ToString())'
)
    insert into dbo.XeObjects
    (
        ReleaseId,
        name,
        object_type,
        package_guid,
        description,
        capabilities,
        capabilities_desc,
        type_name,
        type_package_guid,
        type_size
    )
    select 
        Id, 
        '$($Row["name"].ToString())',
        '$($Row["object_type"].ToString())',
        '$($Row["package_guid"].ToString())',
        '$($Row["description"].ToString().Replace("'", "''"))',
        $Capabilities,
        $CapabilitiesDesc,
        $TypeName,
        $TypePackageGuid,
        $TypeSize
    from dbo.Release
    where FriendlyName = '$Release';"
    }