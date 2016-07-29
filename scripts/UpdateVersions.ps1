# ----------------------------------------------------------------
# -
# -                     Sebastian Bušek, 2016
# -
# ----------------------------------------------------------------
# This script would then apply semantic version to your assemblies.

# Make sure path to source code directory is available
if (-not $Env:BUILD_SOURCESDIRECTORY)
{
    Write-Error ("BUILD_SOURCESDIRECTORY environment variable is missing.")
    exit 1
}
elseif (-not (Test-Path $Env:BUILD_SOURCESDIRECTORY))
{
    Write-Error "BUILD_SOURCESDIRECTORY does not exist: $Env:BUILD_SOURCESDIRECTORY"
    exit 1
}
Write-Verbose "BUILD_SOURCESDIRECTORY: $Env:BUILD_SOURCESDIRECTORY"

# Make sure there is a build number
if (-not $Env:BUILD_BUILDNUMBER)
{
    Write-Error ("BUILD_BUILDNUMBER environment variable is missing.")
    exit 1
}
Write-Verbose "BUILD_BUILDNUMBER: $Env:BUILD_BUILDNUMBER"

# Setup working directory
$WorkingDirectory = $Env:BUILD_SOURCESDIRECTORY

# Get build VSO variables
$MajorVersion = 1
$MinorVersion = 0
$PatchVersion = 2
$BuildVersion = $Env:BUILD_BUILDNUMBER

$NewVersion = "$MajorVersion.$MinorVersion.$PatchVersion.$BuildVersion"
Write-Verbose "Version: $NewVersion"

# Apply the version to the assembly property files
$files = gci $WorkingDirectory -recurse -include "project.json"
if($files)
{
    Write-Verbose "Will apply $NewVersion to $($files.count) files."

    foreach ($file in $files) {
        $filecontent = Get-Content $file.FullName -encoding UTF8 | ConvertFrom-Json
        $filecontent.version = $NewVersion
        $filecontent | ConvertTo-Json | set-content $file.FullName -encoding UTF8
        Write-Verbose "$file.FullName - version applied"
    }
}
else
{
    Write-Warning "Found no files."
}
