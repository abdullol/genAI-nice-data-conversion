using System.Text.Json.Serialization;

namespace nice_data_with_openAI.Models
{
    public class OpenAiResponse
    {
        [JsonPropertyName("text")]
        public string Text { get; set; }
    }
}
