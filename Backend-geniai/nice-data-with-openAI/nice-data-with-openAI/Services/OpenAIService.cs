using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace nice_data_with_openAI.Services
{
    public class OpenAIService : IOpenAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;

        public OpenAIService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _apiKey = configuration["OpenAI:ApiKey"];

            // Set authorization header
            _httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", _apiKey);
        }

        public async Task<string> GenerateFormattedContentAsync(string prompt)
        {
            var requestBody = new
            {
                model = "gpt-3.5-turbo",
                messages = new[]
                {
                    new { role = "user", content = "Hello world" }
                }
            };

            var json = JsonConvert.SerializeObject(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://api.openai.com/v1/chat/completions", content);

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            return result;
        }

        // Add the missing method implementation for the interface
        public async Task<string> GenerateFormattedContentAsync(string input, string outputType)
        {
            // You can use the 'outputType' parameter to modify the prompt or behavior as needed.
            // For now, simply append outputType to the prompt for demonstration.
            var prompt = $"{input}\nOutput type: {outputType}";
            return await GenerateFormattedContentAsync(prompt);
        }
    }
}
