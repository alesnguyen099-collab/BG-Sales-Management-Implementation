# Decision log

## Confirmed technical baseline
- Native desktop target: WinUI 3 and C# on .NET 8.
- Local persistence: SQLite under the current Windows user's LocalAppData folder.
- Customer archive is soft-delete in the starter repository.
- Build validation should run on Windows through GitHub Actions.

## Decisions still required
- Authoritative customer key and duplicate reconciliation policy.
- Which Excel sheets/columns are business data versus report/KPI rows.
- Whether import can update existing customers and how conflicts are presented.
- Backup retention, restore confirmation and supported Windows versions.

Do not infer unresolved business rules from sample data.