# BG Sales Management

**Status:** technical scaffold, not a release-ready Windows app.

## Included
- WinUI 3 / C# solution targeting .NET 8 and Windows 10 19041+
- SQLite data project with schema initialization, customer search, and soft archive
- Basic navigation shell and brand theme resources
- CI workflow for Windows build verification

## Not implemented or verified yet
- Complete customer CRUD UI
- Real Excel workbook import, preview, reconciliation, or transaction rollback
- Calendar/work scheduling, backup/restore, installer/portable packaging
- Build/test on Windows in this session

## Build
Open `BGSM.sln` in Visual Studio 2022 on Windows with the WinUI application development workload and a compatible Windows SDK. Restore NuGet packages, then build `BGSM.App`.

Default SQLite path: `%LOCALAPPDATA%\BG Sales Management\bgsm.db`. Do not commit production data or database files.

## Next steps
1. Run the Windows CI build and fix any compile/XAML/package issues.
2. Add automated tests for database initialization, search, archive, duplicate customer codes, and cancellation.
3. Implement approved customer CRUD flows.
4. Implement Excel import with preview, explicit mapping, duplicate policy, transaction safety, and import report.
5. Verify on Windows 10/11 before calling this production-ready.
