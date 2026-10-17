using System.Text.Json;
using System.Text.Json.Serialization;

public class ExternalApiOptions
{
    public string BaseUrl { get; set; } = string.Empty;
    public string PublicKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public int PickupDistrictId { get; set; }
}

public class LoginRequest
{
    [JsonPropertyName("public_key")] public string PublicKey { get; set; } = string.Empty;
    [JsonPropertyName("secret_key")] public string SecretKey { get; set; } = string.Empty;
}

public class LoginResponse
{
    [JsonPropertyName("success")] public bool Success { get; set; }
    [JsonPropertyName("message")] public string? Message { get; set; }
    [JsonPropertyName("data")] public LoginData? Data { get; set; }
}

public class LoginData
{
    [JsonPropertyName("token")] public string Token { get; set; } = string.Empty;
    [JsonPropertyName("name")] public string? Name { get; set; }
}

public class CreateParcelRequest
{
    [JsonPropertyName("pickup_district_id")] public int PickupDistrictId { get; set; }
    [JsonPropertyName("district_id")] public int DistrictId { get; set; }
    [JsonPropertyName("name")] public string Name { get; set; } = string.Empty;
    [JsonPropertyName("amount")] public decimal Amount { get; set; }
    [JsonPropertyName("address")] public string Address { get; set; } = string.Empty;
    [JsonPropertyName("phone")] public string Phone { get; set; } = string.Empty;
    [JsonPropertyName("comment")] public string? Comment { get; set; }
    [JsonPropertyName("reference")] public string Reference { get; set; } = string.Empty;
    [JsonPropertyName("allow_open")] public int AllowOpen { get; set; } = 1;
    [JsonPropertyName("allow_try")] public int AllowTry { get; set; } = 1;
    [JsonPropertyName("products_from_stock")] public int ProductsFromStock { get; set; } = 0;
    [JsonPropertyName("products")] public string? Products { get; set; }
    [JsonPropertyName("packaging_id")] public int PackagingId { get; set; } = 1;
    [JsonPropertyName("option_exchange")] public int OptionExchange { get; set; } = 0;
    [JsonPropertyName("delivery_exchange_id")] public int? DeliveryExchangeId { get; set; }
}

public class CreateParcelResponse
{
    [JsonPropertyName("data")] public ParcelData? Data { get; set; }
}

public class ParcelData
{
    [JsonPropertyName("code")] public string? Code { get; set; }   // ⚠️ à vérifier dans la doc
}

public class DistrictsResponse
{
    [JsonPropertyName("data")] public List<DistrictItem>? Data { get; set; }
    [JsonPropertyName("current_page")] public int CurrentPage { get; set; }
    [JsonPropertyName("last_page")] public int LastPage { get; set; }
}

public class DistrictItem
{
    [JsonPropertyName("id")] public JsonElement Id { get; set; }
    [JsonPropertyName("ville")] public string? Ville { get; set; }
    [JsonPropertyName("name")] public string? Name { get; set; }
    [JsonPropertyName("arabic_name")] public string? ArabicName { get; set; }
    [JsonPropertyName("delais")] public string? Delais { get; set; }
}