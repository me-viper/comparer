$requiredCommands = ("dotnet")

foreach ($command in $requiredCommands) {
    if ((Get-Command $command -ErrorAction SilentlyContinue) -eq $null) {
        Write-Host "$command must be on path" -ForegroundColor Red
        exit
    }
}

$baseUrl = 'http://localhost:55603'
$startUrl = "{0}/swagger" -f $baseUrl

# At this point it's worth checking if server is already running. On the other hand
# starting another instance will jsut silently fail so it's not big deal.
Start-Process powershell {dotnet run --project .\src\ComparerService.App\ComparerService.App.csproj --urls "http://localhost:55603"}

do {
    Start-Sleep -Seconds 5
    $serverStatus = 404;
    Write-Host "Waiting for server to start..."
    $serverStatus = (curl $startUrl).StatusCode
    Write-Host $serverStatus

} while ($serverStatus -ne 200)

Start-Process $startUrl