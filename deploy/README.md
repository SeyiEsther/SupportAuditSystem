# Deploying the Support Audit System

This is an ASP.NET Core 8 Razor Pages application with its own SQL Server
database, deployed to **IIS on csm-srv-16** as its own site on its own port,
separate from the Production Audit System (RittalTLSW).

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

Copy the `publish` folder to the site root on csm-srv-16.

## IIS site

1. Create a new **Website** (not an app under an existing site) pointing at the
   published folder, e.g. `SupportAuditSystem`.
2. Give it its **own binding on its own port** (e.g. `http://*:8082`), distinct
   from the Production Audit System's site.
3. Assign it a dedicated **Application Pool** (No Managed Code — the app runs
   in-process via the ASP.NET Core Module).
4. Under **Authentication** for the site:
   - **Windows Authentication → Enabled**
   - **Anonymous Authentication → Disabled**
   The app configures no authentication in code; it reads the identity IIS
   forwards. `web.config` sets `forwardWindowsAuthToken="true"`.
5. Ensure the app-pool identity can reach CSMSVR02 and can write the Data
   Protection key folder (a sibling `SupportAudit-dataprotection-keys` folder,
   or set `DataProtection:KeyPath`). Without a writable key folder, an app-pool
   recycle invalidates open forms (HTTP 400 on save).

## Admins

Admin access is granted by display name / username / AD group in
`appsettings.json` under `Admin`. In Development, `Admin:GrantAll` opens
everything.
