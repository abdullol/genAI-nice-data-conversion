namespace nice_data_with_openAI.Models
{
    public class UploadRequest
    {
        public IFormFile File { get; set; }
        public string OutputType { get; set; }
    }
}
