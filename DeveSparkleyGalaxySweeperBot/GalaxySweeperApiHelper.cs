using DeveSparkleyGalaxySweeperBot.Logging;
using DeveSparkleyGalaxySweeperBot.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace DeveSparkleyGalaxySweeperBot
{
    public class GalaxySweeperApiHelper
    {
        private readonly ILogger logger;
        private HttpClient _httpClient;

        public GalaxySweeperApiHelper(string bearerToken, ILogger logger)
        {
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            this.logger = logger;
        }

        public async Task<GalaxySweeperGame[]> GetGames()
        {
            var data = await _httpClient.GetStringAsync("https://galaxysweeper.com/api/games");

            var deserialized = JsonConvert.DeserializeObject<GalaxySweeperGame[]>(data);

            return deserialized;
        }

        public void Sweep(string gameId, int row, int column)
        {
            //9890b1f2-b87b-4043-ba6a-62037ae921b5
            string referer = $"https://galaxysweeper.com/game/{gameId}";



            var content = new StringContent("{row: " + row + ", column: " + column + "}", Encoding.UTF8, "application/json");
            var result = _httpClient.PostAsync($"https://galaxysweeper.com/api/games/{gameId}/sweep", content).Result;
        }
    }
}
