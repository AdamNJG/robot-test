Write-Host "Building and Running Robot Simulator..." -ForegroundColor Cyan

$project = "RobotSimulator/RobotSimulator.csproj"
$input = "commands.txt"

if (-not (Test-Path $input)) {
    Write-Error "commands.txt not found!"
    exit 1
}

Get-Content $input | dotnet run --project $project

Write-Host "`nDone!" -ForegroundColor Green