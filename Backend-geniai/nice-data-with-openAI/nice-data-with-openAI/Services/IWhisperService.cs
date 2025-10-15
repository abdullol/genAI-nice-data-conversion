namespace nice_data_with_openAI.Services
{
    public interface IWhisperService
    {
        Task<string> TranscribeAudioAsync(string filePath, string outputType);
    }
}
