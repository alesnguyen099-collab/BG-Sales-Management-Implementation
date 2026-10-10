# Application flows — intended behavior

## Startup
Initialize app resources and database; show a recoverable error if initialization fails. A loading indicator must not imply success before readiness is confirmed.

## Customers
Open list -> search/filter -> open or create customer -> validate -> save -> refresh list. Archive requires confirmation and must be reversible only if a restore policy is approved.

## Excel import
Choose workbook -> inspect sheets/headers -> map columns -> preview validation and duplicate outcomes -> user confirms -> transaction writes -> show import summary. On failure, rollback and preserve the pre-import database state.

## Backup/restore
Create backup to a user-selected location; validate restore candidate and warn before replacing data. These are requirements only and are not implemented.