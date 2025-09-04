param(
    [Parameter(Mandatory=$true)]
    [string]$NewName
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

$OldName = 'VsaSample'

Get-ChildItem -Recurse -File |
    Where-Object {
        $_.FullName -notmatch '\\.git' -and
        $_.Name -notin @('rename-template.sh', 'rename-template.ps1')
    } |
    ForEach-Object {
        $content = [System.IO.File]::ReadAllText($_.FullName)
        $updated = $content -replace $OldName, $NewName
        [System.IO.File]::WriteAllText($_.FullName, $updated)
    }

$SolutionPath = "$OldName.sln"
if (Test-Path $SolutionPath) {
    Rename-Item $SolutionPath "$NewName.sln"
}

$srcDir = Join-Path 'src' $OldName
if (Test-Path $srcDir) {
    $newSrcDir = Join-Path 'src' $NewName
    Rename-Item $srcDir $newSrcDir
    $projFile = Join-Path $newSrcDir "$OldName.csproj"
    if (Test-Path $projFile) {
        Rename-Item $projFile (Join-Path $newSrcDir "$NewName.csproj")
    }
}

$testDir = Join-Path 'tests' "$OldName.Tests"
if (Test-Path $testDir) {
    $newTestDir = Join-Path 'tests' "$NewName.Tests"
    Rename-Item $testDir $newTestDir
    $testProjFile = Join-Path $newTestDir "$OldName.Tests.csproj"
    if (Test-Path $testProjFile) {
        Rename-Item $testProjFile (Join-Path $newTestDir "$NewName.Tests.csproj")
    }
}

Write-Host "Renamed project to $NewName"
