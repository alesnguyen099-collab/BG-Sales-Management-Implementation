# Prototype flow test matrix

| ID | Flow | Expected behavior | Status |
|---|---|---|---|
| PF-01 | Navigation | Active section and title remain in sync | Basic shell only |
| PF-02 | Search customers | Search matches name/code and supports clearing query | SQLite method exists; UI not connected |
| PF-03 | Archive customer | Soft archive and hide archived records by default | Data method exists; no UI/test yet |
| PF-04 | Excel import | Preview, validate, confirm, transaction, import report | Not implemented |
| PF-05 | Backup/restore | Validate backup and protect existing data on failure | Not implemented |
| PF-06 | Theme/accessibility | Keyboard navigation, focus, contrast and reduced motion | Not verified |

This is a test plan, not evidence that tests passed. Record Windows CI results and test logs before changing status to passed.