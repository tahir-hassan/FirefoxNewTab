param([string]$Path)

if (-not (Test-Path $Path)) {
    Write-Error "[$Path] does not exist"
    return;
}

$process = Get-Process FirefoxNewTab -ErrorAction Ignore;
$command = Get-Command FirefoxNewTab -ErrorAction Ignore;

if ($process) {
    $process | Stop-Process;
}

Publish-VSProject -Path $PSScriptRoot\FirefoxNewTab\FirefoxNewTab.csproj -BuildPath $Path;

if ($process -and $command) {
    . $command;
}

