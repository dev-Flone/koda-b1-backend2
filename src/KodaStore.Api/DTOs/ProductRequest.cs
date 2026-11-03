namespace KodaStore.Api.DTOs;

public record ProductRequest(string ProductCode, string Name, long Price, int Stock, string Category);