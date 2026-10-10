# Technical architecture

- `src/BGSM.App`: WinUI 3 presentation shell, XAML resources and navigation.
- `src/BGSM.Data`: SQLite database path, schema initialization, customer record and starter repository.
- `BGSM.sln`: solution entry point.

SQLite defaults to `%LOCALAPPDATA%/BG Sales Management/bgsm.db`; database files are excluded from version control. The repository is a starter layer, not a complete data-access implementation. Excel import, complete CRUD, migrations beyond the initial schema, backup/restore and production hardening remain pending.