using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ReversiMvcApp.Helpers;
using ReversiMvcApp.Schemas;

namespace ReversiMvcApp.Services
{
    public class ApiService : IApiService
    {
        private HttpClient _client;
        public ApiService(string BaseUrl)
        {
            // Maak een nieuwe HttpClientHandler
            var httpClientHandler = new HttpClientHandler();
            // Omdat de SSL certificaat self-signed is, geeft de HTTPClient standaard een error.
            // Daarom overwriten we de functie die de certificaten checkt zodat die altijd true returned
            httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, sslPolicyErrors) =>
            {
                return true;
            };
            // Vervolgens maken we een nieuwe HttpClient met deze handler.
            _client = new HttpClient(httpClientHandler)
            {
                // Ook stellen we hier een BaseAddress in,
                // zodat we niet elke keer de 'https://localhost:5001/' voor onze URL's hoeven te zetten
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public async Task<Result<List<PendingGame>>> GetPendingGames()
        {
            var response = await _client.GetAsync("api/Spel");
            var body = await response.Content.ReadAsStringAsync();
            if (response.StatusCode != HttpStatusCode.OK)
            {
                return Result.Fail<List<PendingGame>>($"Failed to get descriptions ({response.StatusCode}): {body}");
            }
            return Result.Ok(JsonConvert.DeserializeObject<List<PendingGame>>(body));
        }

        public async Task<Result<string>> CreateNew(string playerToken, string description)
        {
            var newGame = new NewGame()
            {
                PlayerToken = playerToken,
                Description = description
            };
            
            var response = await _client.PostAsync("api/Spel", 
                new StringContent(JsonConvert.SerializeObject(newGame), 
                    Encoding.UTF8, 
                    "application/json"
                ));
            
            return response.IsSuccessStatusCode ? Result.Ok(await response.Content.ReadAsStringAsync()) : Result.Fail<string>(await response.Content.ReadAsStringAsync());
        }

        public async Task<Result<Game>> GetGame(string gameToken)
        {
            var response = await _client.GetAsync($"api/Spel/{gameToken}");
            var body = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
                return Result.Fail<Game>(body);

            var game = JsonConvert.DeserializeObject<Game>(body);
            return Result.Ok(game);
        }
    }
}