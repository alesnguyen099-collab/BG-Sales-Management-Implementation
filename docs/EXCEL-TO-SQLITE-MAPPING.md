# Excel to SQLite mapping — draft

No production workbook has been imported. Final mappings must be approved against the actual workbook headers and representative rows before implementation.

| SQLite field | Draft source | Validation / rule |
|---|---|---|
| `ExcelCustomerCode` | Customer code column | Trim whitespace; preserve as text; define duplicate policy before import |
| `DisplayName` | Customer/company name | Required, non-empty |
| `Region` | Region/area column | Optional text; normalize only with approved rules |
| `Phone` | Phone/contact column | Optional text; preserve leading zeros |
| `SourcePayloadJson` | Optional source fields | Retain only if approved; avoid unnecessary personal data |

Import must preview changes before writing, report inserted/updated/skipped/invalid rows, and use a transaction so a failed import cannot leave a partial update.