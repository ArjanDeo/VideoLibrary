using VideoLibrary.Core.Entities;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VideoLibrary.Interfaces
{
    public interface IMailerService
    {
        /// <summary>
        /// Sends an email with a subject and body to the provided list of recipients with optional attachments.
        /// </summary>
        /// <param name="Subject">The email subject</param>
        /// <param name="Body">The email body</param>
        /// <param name="Recipients">The email recipients</param>
        /// <param name="CCRecipients">The optional email carbon copy recipients</param>
        /// <param name="BCCRecipients">The optional email blind carbon copy recipients</param>
        /// <param name="Attachments">Optional attachments for the email</param>
        /// <param name="OverridePassword">Optional password for email authentication</param>
        /// <param name="cancellationToken">A token to cancel the email request</param>
        /// <returns></returns>
        Task SendMailAsync(string Subject, string Body, IEnumerable<string> Recipients,
            IEnumerable<string> CCRecipients = null, IEnumerable<string> BCCRecipients = null,
            IEnumerable<CustomSmtpAttachment> Attachments = null, string OverridePassword = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// Sends an email with a subject and body to the provided list of recipients with optional attachments.
        /// </summary>
        /// <param name="Subject">The email subject</param>
        /// <param name="Body">The email body</param>
        /// <param name="Recipient">The email recipient</param>
        /// <param name="CCRecipient">The optional email carbon copy recipient</param>
        /// <param name="BCCRecipient">The optional email blind carbon copy recipient</param>
        /// <param name="Attachments">Optional attachments for the email</param>
        /// <param name="OverridePassword">Optional password for email authentication</param>
        /// <param name="cancellationToken">A token to cancel the email request</param>
        /// <returns></returns>
        Task SendMailAsync(string Subject, string Body, string Recipient,
            string CCRecipient = null, string BCCRecipient = null,
            IEnumerable<CustomSmtpAttachment> Attachments = null, string OverridePassword = null, CancellationToken cancellationToken = default);
    }
}
