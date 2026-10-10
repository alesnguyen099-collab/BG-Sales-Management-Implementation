using Microsoft.Data.Sqlite;
namespace BGSM.Data;
public sealed class DatabaseInitializer
{
 public async Task InitializeAsync(string? databasePath = null, CancellationToken cancellationToken = default)
 {
  databasePath ??= DatabasePath.GetDefaultPath(); Directory.CreateDirectory(Path.GetDirectoryName(databasePath)!);
  var builder = new SqliteConnectionStringBuilder { DataSource = databasePath, Mode = SqliteOpenMode.ReadWriteCreate, ForeignKeys = true, Pooling = true };
  await using var connection = new SqliteConnection(builder.ToString()); await connection.OpenAsync(cancellationToken);
  var command = connection.CreateCommand();
  command.CommandText = @"PRAGMA journal_mode = WAL; PRAGMA foreign_keys = ON;
CREATE TABLE IF NOT EXISTS SchemaVersion (Version INTEGER NOT NULL PRIMARY KEY, AppliedUtc TEXT NOT NULL);
CREATE TABLE IF NOT EXISTS Customers (Id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, ExcelCustomerCode TEXT NULL, DisplayName TEXT NOT NULL, Region TEXT NULL, Phone TEXT NULL, SourcePayloadJson TEXT NULL, IsArchived INTEGER NOT NULL DEFAULT 0 CHECK (IsArchived IN (0,1)), CreatedUtc TEXT NOT NULL, UpdatedUtc TEXT NOT NULL);
CREATE UNIQUE INDEX IF NOT EXISTS UX_Customers_ExcelCustomerCode ON Customers(ExcelCustomerCode) WHERE ExcelCustomerCode IS NOT NULL AND trim(ExcelCustomerCode) <> '';
CREATE INDEX IF NOT EXISTS IX_Customers_DisplayName ON Customers(DisplayName);
INSERT OR IGNORE INTO SchemaVersion(Version, AppliedUtc) VALUES (1, strftime('%Y-%m-%dT%H:%M:%fZ','now'));";
  await command.ExecuteNonQueryAsync(cancellationToken);
 }
}