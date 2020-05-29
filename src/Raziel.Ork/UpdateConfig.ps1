# az login
# Connect-AzureRmAccount

$json = Get-Content -Raw -Path "./Config.json" | ConvertFrom-Json;
$group = "Raziel";


For ($i = 0; $i -le $json.count - 1; $i++) {

    $webAppName = $json[$i].name;
    $webApp = Get-AzureRmWebApp -ResourceGroupName $group -Name $webAppName
    $configs = $json[$i].configs;

    $configs.PSObject.Properties | ForEach-Object {
        $newSettings[ -join ("Settings:", $_.Name) ] = $_.Value
    }
 

    # Create a list of the current settings
    $currentSettings = $webApp.SiteConfig.AppSettings;
    $newSettings = @{ }
    ForEach ($item in $currentSettings) {
        $newSettings[$item.Name] = $item.Value
    }

    # Add or update the user defined settings

    $configs.PSObject.Properties | ForEach-Object {
        $newSettings[ -join ("Settings:", $_.Name) ] = $_.Value
    }

    Set-AzureRmWebApp -AppSettings $newSettings -Name $webAppName -ResourceGroupName $group


}







