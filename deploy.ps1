$request = "https://api.nuget.org/v3/registration5-gz-semver2/osu.framework.live2d/index.json"
$entries = Invoke-WebRequest $request | ConvertFrom-Json | Select-Object -expand items

$year = (Get-Date).Year
$monthDay = [int]([String](Get-Date).Month + [String](Get-Date).Day)
$current = $entries[0].items.catalogEntry.version.Split(".")
$revision = 0

if ( ([int]($current[0]) -eq $year) -and ([int]($current[1]) -eq $monthDay) ) {
    $revision = [int]($current[2]) + 1
}

if ([string]$monthDay.length -le 2) {
    $monthDay = "0" + $monthDay
}

$version = [string]::Format("{0}.{1}.{2}", [string]$year, [string]$monthDay, [string]$revision)
& dotnet.exe pack ./osu.Framework.Live2D/osu.Framework.Live2D.csproj -c Release -o output /p:Version=$version
& dotnet.exe nuget push ./output/osu.Framework.Live2D.$version.nupkg --api-key $env:NUGET_API_KEY --skip-duplicate --no-symbols true


# Mirroring to GitHub Package Registry
dotnet.exe nuget add source https://nuget.pkg.github.com/vignette-project/index.json -n github -u vignette-project -p $env:GITHUB_TOKEN --store-password-in-clear-text
dotnet.exe nuget push ./output/osu.Framework.Live2D.$version.nupkg --source "github" --skip-duplicate --no-symbols true