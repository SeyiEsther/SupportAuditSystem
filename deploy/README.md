# Deploying the Support Audit System

This is an ASP.NET Core 8 Razor Pages application with its own SQL Server
database. It runs **as its own process on the remote desktop machine**, on its
own port, with no web server in front of it — the app hosts itself.

## Database

- Server: **CSMSVR02**
- Catalog: **RittalSupportSW** (separate from RittalTLSW)
- The connection string lives in `appsettings.json` under `ConnectionStrings:Default`.
- Entity Framework owns the schema. Migrations are applied automatically on
  startup (`Database.Migrate()`), so a fresh deploy against an empty database
  builds the schema and seeds Stores/Dispatch. To apply out of band:
  `dotnet ef database update`.
- **Turnkey option — `deploy/create-database.sql`.** One idempotent script that
  creates the `RittalSupportSW` database, the `db_Public_User` login/user, the
  full schema, the Stores/Dispatch seed data, and the EF migration-history rows
  (so the app's startup migration then makes no further changes). Run it once
  with SSMS or `sqlcmd -S csmsvr02 -E -i deploy/create-database.sql`. Safe to
  re-run. Change the login password in the script before production use and keep
  the connection string in step.

## Publish

```
dotnet publish -c Release -o publish
```

Copy the `publish` folder onto the remote desktop machine, e.g.
`C:\Apps\SupportAuditSystem`.

## Run it

```
cd C:\Apps\SupportAuditSystem
SupportAuditSystem.exe
```

It binds **http://*:52288** (set in `appsettings.json` under
`Kestrel:Endpoints:Http:Url` — change the port there if it clashes). Browse
`http://localhost:52288` on the machine itself, or `http://<machine-name>:52288`
from elsewhere on the network.

Leaving a console window open is fine for testing, but for day-to-day use start
it automatically:

1. **Task Scheduler → Create Task.**
2. General: *Run whether user is logged on or not*; give it an account that can
   reach CSMSVR02.
3. Triggers: *At startup*.
4. Actions: *Start a program* → `C:\Apps\SupportAuditSystem\SupportAuditSystem.exe`,
   Start in → `C:\Apps\SupportAuditSystem`.
5. Settings: untick *Stop the task if it runs longer than…*.

To update it later: stop the task (or close the window), copy the new `publish`
output over the folder, start it again. The files are locked while it runs, so
stop it first.

## Network access

If other machines need to reach it, allow the port through Windows Firewall on
the remote desktop machine:

```
netsh advfirewall firewall add rule name="Support Audit System" dir=in action=allow protocol=TCP localport=52288
```

## Windows Authentication

The app negotiates the Windows identity itself — there is nothing to configure
outside it. Browsers on the domain pass the signed-in user automatically for a
local or intranet address; if negotiation isn't available the app falls back to
the account it is running under rather than failing.

Make sure the account the app runs under can reach CSMSVR02 and can write the
Data Protection key folder (a sibling `SupportAudit-dataprotection-keys` folder,
or set `DataProtection:KeyPath`). Without a writable key folder, a restart
invalidates open forms (HTTP 400 on save).

## Admins

Admin access is granted by display name / username / AD group in
`appsettings.json` under `Admin`. In Development, `Admin:GrantAll` opens
everything.
