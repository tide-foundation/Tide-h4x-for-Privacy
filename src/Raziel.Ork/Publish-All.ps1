$location = Get-Location
$location = $location.Path

$restartApp = {
    param ($number)
    Restart-AzureRmWebApp -ResourceGroupName Raziel -Name raziel-ork-$number
}

For ($i = 1; $i -le 25; $i++) {
    $pwd = az webapp deployment list-publishing-profiles --name raziel-ork-$i --resource-group Raziel --query '[].userPWD' -o tsv
    $pwd = $pwd[0];
    $project = -join ($location, '\Raziel.Ork.csproj');
    $profile = -join ("raziel-ork-", $i, " - Web Deploy.pubxml");
    
    dotnet publish $project /p:PublishProfile=$profile /p:Password=$pwd -v q
    
    if ($LASTEXITCODE -eq 0) {
        Write-Host "Ork" $i "deployed successfully" -ForegroundColor Green
        Start-Job -ArgumentList $i -ScriptBlock $restartApp
    }
    else {
        Write-Host "Ork" $i "failed to deploy" -ForegroundColor Red
    }
}
