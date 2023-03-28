using System.IO;

namespace VideoLibrary.Core.Entities
{
    /// A custom SMTP attachment class for use with MailerService.
    /// </summary>
    public class CustomSmtpAttachment
    {
        public string FileName { get; set; }
        public string MediaType { get; set; }
        public string MediaSubtype { get; set; }
        public MemoryStream Content { get; set; }
    }
}
