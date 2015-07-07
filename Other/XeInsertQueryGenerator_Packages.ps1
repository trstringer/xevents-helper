$Release = "SQL 2012"
$DataSource = "localhost\sql2012"

$ConnectionString = "data source = $DataSource; initial catalog = master; trusted_connection = true;"
$SqlConnection = New-Object System.Data.SqlClient.SqlConnection($ConnectionString)

$SqlCmd = New-Object System.Data.SqlClient.SqlCommand
$SqlCmd.Connection = $SqlConnection

$SqlCmd.CommandText = "select Release = '$Release', * from sys.dm_xe_packages;"

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

        "if not exists 
(
    select 1
    from dbo.XePackages p
    inner join dbo.Release r
    on p.ReleaseId = r.Id
    where r.FriendlyName = '$Release'
    and p.name = '$($Row["name"].ToString())'
    and p.guid = '$($Row["guid"].ToString())'
)
    insert into dbo.XePackages
    (
        ReleaseId,
        name,
        guid,
        description,
        capabilities,
        capabilities_desc,
        module_guid,
        module_address
    )
    select 
        Id, 
        '$($Row["name"].ToString())',
        '$($Row["guid"].ToString())',
        '$($Row["description"].ToString().Replace("'", "''"))',
        $Capabilities,
        $CapabilitiesDesc,
        '$($Row["module_guid"].ToString())',
        0x$([System.BitConverter]::ToString($Output.Rows[0]["module_address"]).Replace("-", """))
    from dbo.Release
    where FriendlyName = '$Release';"
    }