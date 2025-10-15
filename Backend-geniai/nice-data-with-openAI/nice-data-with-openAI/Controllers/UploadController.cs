using Microsoft.AspNetCore.Mvc;
using nice_data_with_openAI.Services;

namespace nice_data_with_openAI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IWhisperService WhisperService;
        private readonly IVideoService VideoService;
        private readonly IOpenAiService OpenAiService;

        public UploadController(
            IWhisperService whisperService,
            IVideoService videoService,
            IOpenAiService openAiService)
        {
            WhisperService = whisperService;
            VideoService = videoService;
            OpenAiService = openAiService;
        }

        [HttpPost("UploadFiles")]
        public async Task<IActionResult> UploadFiles(IFormFile file, [FromForm] string outputType)
        {

            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            var tempFilePath = Path.GetTempFileName();
            string transcription;
            try
            {
                using (var stream = System.IO.File.Create(tempFilePath))
                {
                    await file.CopyToAsync(stream);
                }
                var fileExtension = Path.GetExtension(file.FileName).ToLower();
                if (fileExtension == ".mp4" || fileExtension == ".mov" || fileExtension == ".avi")
                {
                    transcription = await VideoService.ExtractAudioAndTranscribeAsync(tempFilePath);
                }
                else if (fileExtension == ".mp3" || fileExtension == ".wav" || fileExtension == ".m4a")
                {
                    transcription = await WhisperService.TranscribeAudioAsync(tempFilePath, outputType);
                }
                else if (fileExtension == ".txt")
                {
                    transcription = await System.IO.File.ReadAllTextAsync(tempFilePath);
                }
                else
                {
                    System.IO.File.Delete(tempFilePath);
                    return BadRequest("Unsupported file type.");
                }
                var formattedContent = await OpenAiService.GenerateFormattedContentAsync(transcription, outputType);
                System.IO.File.Delete(tempFilePath);
                return Ok(formattedContent);
            }
            catch (Exception ex)
            {
                System.IO.File.Delete(tempFilePath);
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
