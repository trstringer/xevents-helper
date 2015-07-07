$Release = "SQL 2012"
$DataSource = "localhost\sql2012"

$ConnectionString = "data source = $DataSource; initial catalog = master; trusted_connection = true;"
$SqlConnection = New-Object System.Data.SqlClient.SqlConnection($ConnectionString)

$SqlCmd = New-Object System.Data.SqlClient.SqlCommand
$SqlCmd.Connection = $SqlConnection

$SqlCmd.CommandText = "select Release = '$Release', * from sys.dm_xe_object_columns;"

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
        if ($Row["column_value"] -eq [System.DBNull]::Value) {
            $ColumnValue = "NULL"
        }
        else {
            $ColumnValue = "'$($Row["column_value"].ToString())'"
        }

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

        if ($Row["description"] -eq [System.DBNull]::Value) {
            $Description = "NULL"
        }
        else {
            $Description = "'$($Row["description"].ToString().Replace("'", "''"))'"
        }

        "if not exists 
(
    select 1
    from dbo.XeObjectColumns oc
    inner join dbo.Release r
    on oc.ReleaseId = r.Id
    where r.FriendlyName = '$Release'
    and oc.name = '$($Row["name"].ToString())'
    and oc.object_name = '$($Row["object_name"].ToString())'
    and oc.column_type = '$($Row["column_type"].ToString())'
)
    insert into dbo.XeObjectColumns
    (
        ReleaseId,
        name,
        column_id,
        object_name,
        object_package_guid,
        type_name,
        type_package_guid,
        column_type,
        column_value,
        capabilities,
        capabilities_desc,
        description
    )
    select 
        Id, 
        '$($Row["name"].ToString())',
        $($Row["column_id"].ToString()),
        '$($Row["object_name"].ToString())',
        '$($Row["object_package_guid"].ToString())',
        '$($Row["type_name"].ToString())',
        '$($Row["type_package_guid"].ToString())',
        '$($Row["column_type"].ToString())',
        $ColumnValue,
        $Capabilities,
        $CapabilitiesDesc,
        $Description
    from dbo.Release
    where FriendlyName = '$Release';"
    }