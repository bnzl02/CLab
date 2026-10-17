using ChemiseLab.Clients;
using ChemiseLab.Dto.Delivery;
using ChemiseLab.IServices;
using ChemiseLab.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace ChemiseLab.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly ExternalApiClient _client;
        private readonly ExternalApiOptions _options;
        IMemoryCache _cache;

        public DeliveryService(ExternalApiClient client, IOptions<ExternalApiOptions> options, IMemoryCache cache)
        {
            _client = client;
            _options = options.Value;
            _cache = cache;
        }

        public async Task<string> CreateParcelAsync(Order order, string products)
        {
            var request = new CreateParcelRequest
            {
                PickupDistrictId = _options.PickupDistrictId,
                DistrictId = order.IdDistrict ?? 0,
                Name = $"{order.PrenomClient} {order.NomClient}",
                Amount = order.TotalOrder,
                Address = order.AdresseClient,
                Phone = order.TelephoneClient,
                Comment = "",
                //Reference = order.IdOrder.ToString(),
                AllowOpen = 0,
                AllowTry = 0,
                ProductsFromStock = 0,
                PackagingId = 0,
                OptionExchange = 0,
                DeliveryExchangeId = 0,
                Products = products
            };

            var response = await _client.PostAsync<CreateParcelRequest, CreateParcelResponse>("deliveries", request);

            var code = response?.Data?.Code;
            if (string.IsNullOrEmpty(code))
                throw new InvalidOperationException("Le transporteur n'a pas renvoyé de code de suivi.");

            return code;
        }

        public async Task<List<DistrictsList_Dto>> GetDistrictsAsync()
        {
            if (_cache.TryGetValue("districts", out List<DistrictsList_Dto>? cached) && cached != null)
                return cached;

            var result = new List<DistrictsList_Dto>();
            var page = 1;
            var lastPage = 1;

            do
            {
                var response = await _client.GetAsync<DistrictsResponse>(
                    $"districts?page={page}&pickup-district={_options.PickupDistrictId}");

                if (response?.Data == null)
                    break;

                foreach (var d in response.Data)
                {
                    var id = d.Id.ValueKind == JsonValueKind.String
                        ? int.Parse(d.Id.GetString()!)
                        : d.Id.GetInt32();

                    result.Add(new DistrictsList_Dto
                    {
                        IdDistrict = id,
                        Ville = d.Ville ?? string.Empty,
                        Name = d.Name ?? string.Empty,
                        ArabicName = d.ArabicName,
                        Delai = d.Delais
                    });
                }

                lastPage = response.LastPage;
                page++;
            } while (page <= lastPage);

            _cache.Set("districts", result, TimeSpan.FromHours(12));
            return result;
        }
    }
}
