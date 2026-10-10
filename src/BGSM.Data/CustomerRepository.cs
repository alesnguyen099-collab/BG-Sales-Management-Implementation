using Microsoft.Data.Sqlite;

namespace BGSM.Data;

/// <summary>SQLite persistence for customer records. Excel reconciliation is not implemented yet.</summary>
public sealed class CustomerRepository
{
    private readonly string _connectionString;

    public CustomerRepository(string? databasePath = null)
    {
        var builder = new SqliteConnectionStringBuilder
        {
            DataSource = databasePath ?? DatabasePath.GetDefaultPath(),
            Mode = SqliteOpenMode.ReadWriteCreate,
            ForeignKeys = true,
            Pooling = true
        };
        _connectionString = builder.ToString();
    }

    public async Task<IReadOnlyList<Customer>> SearchAsync(string? query = null, bool includeArchived = false, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        var command = connection.CreateCommand();
        command.CommandText = @"SELECT Id, ExcelCustomerCode, DisplayName, Region, Phone, IsArchived, CreatedUtc, UpdatedUtc
FROM Customers
WHERE (@includeArchived = 1 OR IsArchived = 0)
  AND (@query = '' OR DisplayName LIKE @pattern OR ExcelCustomerCode LIKE @pattern OR Phone LIKE @pattern OR Region LIKE @pattern)
ORDER BY DisplayName COLLATE NOCASE;";
        command.Parameters.AddWithValue("@includeArchived", includeArchived ? 1 : 0);
        command.Parameters.AddWithValue("@query", query?.Trim() ?? "");
        command.Parameters.AddWithValue("@pattern", $"%{query?.Trim() ?? ""}%");

        var customers = new List<Customer>();
        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        while (await reader.ReadAsync(cancellationToken))
        {
            customers.Add(new Customer(
                reader.GetInt64(0),
                reader.IsDBNull(1) ? null : reader.GetString(1),
                reader.GetString(2),
                reader.IsDBNull(3) ? null : reader.GetString(3),
                reader.IsDBNull(4) ? null : reader.GetString(4),
                reader.GetInt64(5) == 1,
                DateTime.Parse(reader.GetString(6), null, System.Globalization.DateTimeStyles.RoundtripKind),
                DateTime.Parse(reader.GetString(7), null, System.Globalization.DateTimeStyles.RoundtripKind)));
        }
        return customers;
    }

    public async Task<long> CreateAsync(string displayName, string? code = null, string? region = null, string? phone = null, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(displayName))
            throw new ArgumentException("Tên khách hàng không được để trống.", nameof(displayName));

        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        var command = connection.CreateCommand();
        command.CommandText = @"INSERT INTO Customers (ExcelCustomerCode, DisplayName, Region, Phone, IsArchived, CreatedUtc, UpdatedUtc)
VALUES ($code, $name, $region, $phone, 0, strftime('%Y-%m-%dT%H:%M:%fZ','now'), strftime('%Y-%m-%dT%H:%M:%fZ','now'));
SELECT last_insert_rowid();";
        command.Parameters.AddWithValue("$code", string.IsNullOrWhiteSpace(code) ? DBNull.Value : code.Trim());
        command.Parameters.AddWithValue("$name", displayName.Trim());
        command.Parameters.AddWithValue("$region", string.IsNullOrWhiteSpace(region) ? DBNull.Value : region.Trim());
        command.Parameters.AddWithValue("$phone", string.IsNullOrWhiteSpace(phone) ? DBNull.Value : phone.Trim());
        return (long)(await command.ExecuteScalarAsync(cancellationToken) ?? 0L);
    }

    public async Task ArchiveAsync(long id, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);
        var command = connection.CreateCommand();
        command.CommandText = "UPDATE Customers SET IsArchived = 1, UpdatedUtc = strftime('%Y-%m-%dT%H:%M:%fZ','now') WHERE Id = $id AND IsArchived = 0;";
        command.Parameters.AddWithValue("$id", id);
        await command.ExecuteNonQueryAsync(cancellationToken);
    }
}
