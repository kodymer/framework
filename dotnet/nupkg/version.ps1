param(
    [switch]$template
)

$params = @{}
if ($template) {
    $params.template = $true
}

& (Join-Path $PSScriptRoot 'pack.ps1') @params

Read-Host "Presiona Enter para continuar con la subida de paquetes"

& (Join-Path $PSScriptRoot 'push.ps1') @params
