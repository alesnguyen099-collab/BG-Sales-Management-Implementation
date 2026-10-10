namespace BGSM.Data;
public sealed record Customer(long Id, string? ExcelCustomerCode, string DisplayName, string? Region, string? Phone, bool IsArchived, DateTime CreatedUtc, DateTime UpdatedUtc);