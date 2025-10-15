using Xabe.FFmpeg;

namespace nice_data_with_openAI.Services
{
    public class VideoService : IVideoService
    {
        private readonly IWhisperService WhisperService;
        public VideoService(IWhisperService whisperService)
        {
            WhisperService = whisperService;
            FFmpeg.SetExecutablesPath("ffmpeg_bin_path");
        }

        public async Task<string> ExtractAudioAndTranscribeAsync(string videoFilePath)
        {
            var audioFilePath = Path.ChangeExtension(videoFilePath, ".mp3");
            var conversion = await FFmpeg.Conversions.FromSnippet.ExtractAudio(videoFilePath, audioFilePath);
            await conversion.Start();
            return await WhisperService.TranscribeAudioAsync(audioFilePath, "text");


        }
    }
}
