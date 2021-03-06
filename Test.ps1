param (
	[string]$Configuration = 'Debug'
)

$ErrorActionPreference = "Stop"

$Here = "$(Split-Path -parent $MyInvocation.MyCommand.Definition)"

$DotNet = "${env:ProgramFiles}\dotnet\dotnet.exe"

$Tests = Join-Path $Here "StateActiveDuty.Tests\StateActiveDuty.Tests.csproj"

& $DotNet test $Tests

EXIT $LASTEXITCODE