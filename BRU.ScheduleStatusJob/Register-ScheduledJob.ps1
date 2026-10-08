#Requires -Version 5.1
#Requires -RunAsAdministrator
[CmdletBinding()]
param(
    [Parameter(Mandatory = $true)]
    [string] $ExecutablePath,
    [Parameter(Mandatory = $true)]
    [PSCredential] $Credential
)

$ErrorActionPreference = 'Stop'
$executable = (Resolve-Path -LiteralPath $ExecutablePath).Path
if (-not (Test-Path -LiteralPath $executable -PathType Leaf) -or
    [IO.Path]::GetFileName($executable) -ne 'BRU.ScheduleStatusJob.exe') {
    throw 'ExecutablePath must point to the deployed BRU.ScheduleStatusJob.exe.'
}
if (-not (Test-Path -LiteralPath ($executable + '.config') -PathType Leaf)) {
    throw 'Deploy and configure BRU.ScheduleStatusJob.exe.config before registering the task.'
}

$action = New-ScheduledTaskAction -Execute $executable -WorkingDirectory (Split-Path $executable)
$trigger = New-ScheduledTaskTrigger -Once -At (Get-Date).AddMinutes(1) `
    -RepetitionInterval (New-TimeSpan -Minutes 5)
$settings = New-ScheduledTaskSettingsSet -StartWhenAvailable -MultipleInstances IgnoreNew `
    -ExecutionTimeLimit (New-TimeSpan -Minutes 10) -RestartCount 3 `
    -RestartInterval (New-TimeSpan -Minutes 1)

# Password logon lets the task run while logged out and authenticate to SQL Server.
# Task Scheduler stores the credential; it is never saved in application config.
Register-ScheduledTask -TaskName 'BRU Schedule Status Reconciliation' `
    -Action $action -Trigger $trigger -Settings $settings -RunLevel Limited `
    -User $Credential.UserName -Password $Credential.GetNetworkCredential().Password `
    -Description 'Reconcile trip statuses every five minutes, independent of IIS and page visits.'
