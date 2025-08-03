using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Data;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using WebModuleTeko.Configuration;
using WebModuleTeko.Models.Authentication;

namespace WebModuleTeko.Services.Authentication;

public class KeycloakService
{
    private readonly int Retries = 5;

    private readonly HttpClient _httpClient;
    private readonly ApiConfiguration _apiConfiguration;

    private string? _token;

    public KeycloakService(HttpClient httpClient, IOptions<ApiConfiguration> apiConfiguration)
    {
        _httpClient = httpClient;
        _apiConfiguration = apiConfiguration.Value;
        _httpClient.BaseAddress = new Uri(_apiConfiguration.KeycloakApiUrl);
    }

    public async Task<bool> EnsureReady()
    {
        var tryCount = 0;
        var connected = false;

        while(!connected || tryCount >= Retries)
        {
            try
            {
                connected = await IsReady();
            }catch(Exception ex) {
                await Task.Delay(500);
            }

            tryCount++;
        }

        return connected;
    }

    public async Task<string> LoginUser(string username, string password)
    {
        if (_token == null)
        {
            await AuthenticateClient();
        }

        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "client_id", _apiConfiguration.ClientId },
            { "client_secret", _apiConfiguration.ClientSecret },
            { "grant_type", "password" },
            { "username", username },
            { "password", password }
        });
        var response = await _httpClient.PostAsync("realms/web-module/protocol/openid-connect/token", content);
        response.EnsureSuccessStatusCode();

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

        if (tokenResponse == null)
        {
            throw new NoNullAllowedException();
        }

        return tokenResponse.AccessToken;
    }

    public async Task CreateNewUser(RegisterUserModel registerUser)
    {
        if(_token == null)
        {
            await AuthenticateClient();
        }

        var json = JsonConvert.SerializeObject(new CreateUser
        {
            Email = registerUser.Email,
            Username = registerUser.Username,
            EmailVerified = true,
            Enabled = true,
            Credentials = [
                new Credentials {
                    Temporary = false,
                    Type = "password",
                    Value = registerUser.Password,
                }]
        });
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync("admin/realms/web-module/users", content);
        response.EnsureSuccessStatusCode();
    }

    public async Task AuthenticateClient()
    {
        var content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "client_id", _apiConfiguration.ClientId },
            { "client_secret", _apiConfiguration.ClientSecret },
            { "grant_type", "client_credentials" }
        });
        var response = await _httpClient.PostAsync("realms/web-module/protocol/openid-connect/token", content);
        response.EnsureSuccessStatusCode();

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

        if(tokenResponse == null)
        {
            return;
        }

        _token = tokenResponse.AccessToken;
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _token);
    }

    private async Task<bool> IsReady()
    {
        var response = await _httpClient.GetAsync("/health/ready");

        return response.IsSuccessStatusCode;
    }

    private class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = null!;
    }

    private class CreateUser
    {
        [JsonProperty("username")]
        public required string Username { get; set; }
        [JsonProperty("email")]
        public required string Email { get; set; }
        [JsonProperty("emailVerified")]
        public required bool EmailVerified { get; set; }
        [JsonProperty("enabled")]
        public required bool Enabled { get; set; }
        [JsonProperty("credentials")]
        public required List<Credentials> Credentials { get; set; }
    }

    private class Credentials
    {
        [JsonProperty("type")]
        public required string Type { get; set; }
        [JsonProperty("value")]
        public required string Value { get; set; }
        [JsonProperty("temporary")]
        public required bool Temporary { get; set; }
    }

}
