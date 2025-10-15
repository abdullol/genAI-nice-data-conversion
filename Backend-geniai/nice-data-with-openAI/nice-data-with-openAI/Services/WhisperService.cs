using nice_data_with_openAI.Models;
using System.Net.Http.Headers;
using System.Text.Json;

namespace nice_data_with_openAI.Services
{
    public class WhisperService : IWhisperService
    {
        private readonly HttpClient HttpClient;
        private readonly string ApiKey;
        public WhisperService(IConfiguration configuration)
        {
            HttpClient = new HttpClient();
            ApiKey = configuration["OpenAI:ApiKey"];
        }

        public async Task<string> TranscribeAudioAsync(string filePath, string outputType)
        {
            using var multiPart = new MultipartFormDataContent();
            var fileContent=new ByteArrayContent(await File.ReadAllBytesAsync(filePath));
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("audio/mpeg");
            multiPart.Add(fileContent, "file", Path.GetFileName(filePath));
            multiPart.Add(new StringContent("whisper-1"), "model");

            HttpClient.DefaultRequestHeaders.Authorization
                = new AuthenticationHeaderValue("Bearer", ApiKey);

            var response = await HttpClient.PostAsync("https://api.openai.com/v1/audio/troanscriptions", multiPart);  
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<OpenAiResponse>(json)?.Text ?? string.Empty;
        }
    }
}
