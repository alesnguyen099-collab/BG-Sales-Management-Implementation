using Microsoft.Data.Sqlite;
namespace BGSM.Data;
/// <summary>Starter repository; import reconciliation and full CRUD are not implemented yet.</summary>
public sealed class CustomerRepository
{
 private readonly string _connectionString;
 public CustomerRepository(string? databasePath = null) { var b = new SqliteConnectionStringBuilder { DataSource = databasePath ?? DatabasePath.GetDefaultPath(), Mode = SqliteOpenMode.ReadWriteCreate, ForeignKeys = true, Pooling = true }; _connectionString = b.ToString(); }
 public async Task<IReadOnlyList<Customer>> SearchAsync(string? query = null, bool includeArchived = false, CancellationToken cancellationToken = default)
 {
  await using var c = new SqliteConnection(_connectionString); await c.OpenAsync(cancellationToken); var cmd = c.CreateCommand();
  cmd.CommandText = @"SELECT Id, ExcelCustomerCode, DisplayName, Region, Phone, IsArchived, CreatedUtc, UpdatedUtc FROM Customers WHERE (@includeArchived = 1 OR IsArchived = 0) AND (@query = '' OR DisplayName LIKE @pattern OR ExcelCustomerCode LIKE @pattern) ORDER BY DisplayName COLLATE NOCASE;";
  cmd.Parameters.AddWithValue("@includeArchived", includeArchived ? 1 : 0); cmd.Parameters.AddWithValue("@query", query ?? ""); cmd.Parameters.AddWithValue("@pattern", $"%{query ?? ""}%");
  var result = new List<Customer>(); await using var r = await cmd.ExecuteReaderAsync(cancellationToken);
  while (await r.ReadAsync(cancellationToken)) result.Add(new Customer(r.GetInt64(0), r.IsDBNull(1)?null:r.GetString(1), r.GetString(2), r.IsDBNull(3)?null:r.GetString(3), r.IsDBNull(4)?null:r.GetString(4), r.GetInt64(5)==1, DateTime.Parse(r.GetString(6), null, System.Globalization.DateTimeStyles.RoundtripKind), DateTime.Parse(r.GetString(7), null, System.Globalization.DateTimeStyles.RoundtripKind)));
  return result;
 }
 public async Task ArchiveAsync(long id, CancellationToken cancellationToken = default)
 {
  await using var c = new SqliteConnection(_connectionString); await c.OpenAsync(cancellationToken); var cmd = c.CreateCommand();
  cmd.CommandText = "UPDATE Customers SET IsArchived = 1, UpdatedUtc = strftime('%Y-%m-%dT%H:%M:%fZ','now') WHERE Id = $id AND IsArchived = 0;"; cmd.Parameters.AddWithValue("$id", id); await cmd.ExecuteNonQueryAsync(cancellationToken);
 }
}