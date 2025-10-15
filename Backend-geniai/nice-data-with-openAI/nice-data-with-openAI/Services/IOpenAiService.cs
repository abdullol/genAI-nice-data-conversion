namespace nice_data_with_openAI.Services
{
    public interface IOpenAiService
    {
        Task<string> GenerateFormattedContentAsync(string input, string outputType);
    }
}
