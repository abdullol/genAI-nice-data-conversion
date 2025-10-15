namespace nice_data_with_openAI.Services
{
    public interface IVideoService
    {
        Task<string> ExtractAudioAndTranscribeAsync(string videoFilePath);
    }
}
