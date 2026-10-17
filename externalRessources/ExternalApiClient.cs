using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using ChemiseLab.Dto.Delivery;
using Microsoft.Extensions.Options;

namespace ChemiseLab.Clients;

public class ExternalApiClient
{
    private readonly HttpClient _http;
    private readonly ExternalApiOptions _options;

    private static string? _token;
    private static readonly SemaphoreSlim _lock = new(1, 1);

    public ExternalApiClient(HttpClient http, IOptions<ExternalApiOptions> options)
    {
        _options = options.Value;
        _http = http;
        _http.BaseAddress = new Uri(_options.BaseUrl);
        _http.Timeout = TimeSpan.FromSeconds(15);
    }

    private async Task<string> LoginAsync()
    {
        var response = await _http.PostAsJsonAsync("login", new LoginRequest
        {
            PublicKey = _options.PublicKey,
            SecretKey = _options.SecretKey
        });

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            throw new InvalidOperationException("Identifiants du transporteur invalides.");

        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<LoginResponse>();

        if (result is not { Success: true, Data: not null } || string.IsNullOrEmpty(result.Data.Token))
            throw new InvalidOperationException("Connexion au transporteur impossible.");

        return result.Data.Token;
    }

    private async Task<string> GetTokenAsync(bool forceRefresh = false)
    {
        await _lock.WaitAsync();
        try
        {
            if (forceRefresh || _token == null)
                _token = await LoginAsync();
            return _token;
        }
        finally { _lock.Release(); }
    }

    private async Task<HttpResponseMessage> SendAsync<T>(string path, T body, string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, path) { Content = JsonContent.Create(body) };
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await _http.SendAsync(request);
    }

    public async Task<TResponse?> PostAsync<TRequest, TResponse>(string path, TRequest body)
    {
        var response = await SendAsync(path, body, await GetTokenAsync());

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            response = await SendAsync(path, body, await GetTokenAsync(forceRefresh: true));

        if (!response.IsSuccessStatusCode)
        {
            var detail = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Erreur transporteur ({(int)response.StatusCode}) : {detail}");
        }

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }

    //villes
    private async Task<HttpResponseMessage> SendGetAsync(string path, string token)
    {
        var request = new HttpRequestMessage(HttpMethod.Get, path);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        return await _http.SendAsync(request);
    }

    public async Task<TResponse?> GetAsync<TResponse>(string path)
    {
        var response = await SendGetAsync(path, await GetTokenAsync());

        if (response.StatusCode == HttpStatusCode.Unauthorized)
            response = await SendGetAsync(path, await GetTokenAsync(forceRefresh: true));

        if (!response.IsSuccessStatusCode)
        {
            var detail = await response.Content.ReadAsStringAsync();
            throw new InvalidOperationException($"Erreur transporteur ({(int)response.StatusCode}) : {detail}");
        }

        return await response.Content.ReadFromJsonAsync<TResponse>();
    }
}