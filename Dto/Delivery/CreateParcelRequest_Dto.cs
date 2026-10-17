using System.Text.Json.Serialization;

namespace ChemiseLab.Dto.Delivery
{
    public class CreateParcelRequest_Dto
    {
        [JsonPropertyName("pickup_district_id")]
        public int? PickupDistrictId { get; set; }

        [JsonPropertyName("district_id")]
        public int DistrictId { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("amount")]
        public decimal Amount { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("phone")]
        public string Phone { get; set; } = string.Empty;

        [JsonPropertyName("comment")]
        public string? Comment { get; set; }

        [JsonPropertyName("reference")]
        public string Reference { get; set; } = string.Empty;

        [JsonPropertyName("allow_open")]
        public int AllowOpen { get; set; } = 1;

        [JsonPropertyName("allow_try")]
        public int AllowTry { get; set; } = 1;

        [JsonPropertyName("products_from_stock")]
        public int ProductsFromStock { get; set; } = 0;

        [JsonPropertyName("products")]
        public string? Products { get; set; }

        [JsonPropertyName("packaging_id")]
        public int PackagingId { get; set; } = 1;

        [JsonPropertyName("option_exchange")]
        public int OptionExchange { get; set; } = 0;

        [JsonPropertyName("delivery_exchange_id")]
        public int? DeliveryExchangeId { get; set; }
    }
}
