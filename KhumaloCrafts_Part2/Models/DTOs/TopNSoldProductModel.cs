namespace KhumaloCrafts_Part2.Models.DTOs;

public record TopNSoldProductModel(string ProductName, string ArtisanName, int TotalUnitSold);
public record TopNSoldProductsVm(DateTime StartDate, DateTime EndDate, IEnumerable<TopNSoldProductModel> TopNSoldProducts);
