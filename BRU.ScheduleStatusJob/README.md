# Scheduled status reconciliation

Deploy this job with the web application. It calls the existing
`ScheduleAutomationService.UpdateScheduleStatuses` every five minutes through
Windows Task Scheduler, including when IIS is idle or no users visit the site.
Schedule page loads only read data; the manual update button still requires
`route.write`.

## Build and deploy

On Windows with Visual Studio MSBuild web build tools and the .NET Framework
4.6.1 targeting pack (the same prerequisites as the application):

```powershell
nuget restore BRU.WEBFORMS.ASPNET.APP\packages.config -PackagesDirectory packages
msbuild BRU.ScheduleStatusJob\BRU.ScheduleStatusJob.csproj /p:Configuration=Release /p:Platform=AnyCPU
```

Copy the **entire** `BRU.ScheduleStatusJob\bin\Release` directory to a persistent
directory outside the website, such as `C:\BRU\ScheduleStatusJob`. Configure
`BRU.ScheduleStatusJob.exe.config` to target the same SQL database as the website.
Keep `Integrated Security=True`; the job rejects SQL-password authentication.
Use the same Windows time zone as the web server because schedule times use
`DateTime.Now`.

## Authorize the task identity

Use a dedicated Windows service account with **Log on as a batch job**, read/execute
access to the deployed files, and a SQL Server login mapped to a database user.
Restrict modification of the job executable, dependencies, configuration, and task
definition to deployment administrators. The account needs these database grants
(replace `[BRU_ScheduleJob]` with its mapped database user):

```sql
GRANT SELECT ON OBJECT::dbo.route_schedule TO [BRU_ScheduleJob];
GRANT SELECT ON OBJECT::dbo.route TO [BRU_ScheduleJob];
GRANT SELECT ON OBJECT::dbo.bus TO [BRU_ScheduleJob];
GRANT SELECT ON OBJECT::dbo.employee TO [BRU_ScheduleJob];
GRANT SELECT ON OBJECT::dbo.sale TO [BRU_ScheduleJob];
GRANT UPDATE ON OBJECT::dbo.route_schedule (schedule_status) TO [BRU_ScheduleJob];
```

SQL Server authorizes the scheduled Windows identity independently of web roles
and sessions. Do not give the account database owner or general write membership.
Use a domain account when connecting to a remote SQL Server with Windows authentication.

## Register and verify

Run once from elevated Windows PowerShell 5.1 on the job host:

```powershell
.\BRU.ScheduleStatusJob\Register-ScheduledJob.ps1 `
    -ExecutablePath 'C:\BRU\ScheduleStatusJob\BRU.ScheduleStatusJob.exe' `
    -Credential (Get-Credential -Message 'Dedicated schedule job account')

Start-ScheduledTask -TaskName 'BRU Schedule Status Reconciliation'
Get-ScheduledTaskInfo -TaskName 'BRU Schedule Status Reconciliation'
```

Registration starts recurring execution one minute later and repeats indefinitely.
The task runs while the account is logged out, catches up after a missed trigger,
prevents overlapping scheduled runs, and retries failures three times at one-minute
intervals. Registration fails if the named task already exists; update that task
deliberately during redeployment. Update its stored credential when the account's
password changes.

Wait for the task to finish and check that `LastRunTime` advanced and
`LastTaskResult` is `0`. Exit `1` means configuration, database authorization, or
reconciliation failed. Run the executable under the same account to capture its
console diagnostics when investigating a failure. Monitor task failures and stale
`LastRunTime` in the deployment's normal monitoring system.

Before enabling it in production, verify against a test database with no browser
session: overdue planned/in-progress trips become completed, a departed trip
becomes in-progress, and future/cancelled trips stay unchanged. Verify that an
account without SQL write permission fails without committing status changes.
Then request/filter/refresh the schedule page with a read-only account and verify
that it causes no schedule updates; a forged manual-update postback must be denied.

Task registration and database grants are required deployment steps; building or
publishing the website alone does not install the scheduled task.
