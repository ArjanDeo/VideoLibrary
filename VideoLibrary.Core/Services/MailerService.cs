using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using MailKit.Net.Smtp;
using MimeKit;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Diagnostics;
using System.Threading;
using IdentityServer4.Extensions;
using VideoLibrary.Core.Entities;
using VideoLibrary.Core.Helpers;
using VideoLibrary.Interfaces;

namespace VideoLibrary
{
    public class MailerService : IMailerService
    {
        private readonly SmtpSettings _smtpSettings;
        public MailerService(IOptions<SmtpSettings> smtpSettings)
        {
            _smtpSettings = smtpSettings.Value;
        }

        public async Task SendMailAsync(string Subject, string Body, IEnumerable<string> Recipients,
            IEnumerable<string> CCRecipients = null, IEnumerable<string> BCCRecipients = null,
            IEnumerable<CustomSmtpAttachment> Attachments = null, string OverridePassword = null, CancellationToken cancellationToken = default)
        {
            try
            {
                if (string.IsNullOrEmpty(Body)) Body = string.Empty;
                if (Recipients.IsNullOrEmpty()) return;

                MimeMessage Message = new MimeMessage();
                Message.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));

                foreach (string Recipient in Recipients)
                    try { if (!string.IsNullOrEmpty(Recipient)) Message.To.Add(MailboxAddress.Parse(Recipient)); }
                    catch (ParseException) { }

                if (Message.To.IsNullOrEmpty()) return;

                foreach (string CCRecipient in CCRecipients)
                    try { if (!string.IsNullOrEmpty(CCRecipient)) Message.Cc.Add(MailboxAddress.Parse(CCRecipient)); }
                    catch (ParseException) { }

                foreach (string BCCRecipient in BCCRecipients)
                    try { if (!string.IsNullOrEmpty(BCCRecipient)) Message.Bcc.Add(MailboxAddress.Parse(BCCRecipient)); }
                    catch (ParseException) { }

                Message.Subject = Subject;

                if (!Attachments.IsNullOrEmpty())
                {
                    Multipart MultipartBody = new Multipart("mixed")
                    {
                        new TextPart("html") { Text = Body }
                    };

                    foreach (CustomSmtpAttachment Attachment in Attachments)
                    {
                        MimePart BodyAttachment = new MimePart(Attachment.MediaType, Attachment.MediaSubtype)
                        {
                            Content = new MimeContent(Attachment.Content),
                            ContentDisposition = new ContentDisposition(ContentDisposition.Attachment),
                            ContentTransferEncoding = ContentEncoding.Base64,
                            FileName = Attachment.FileName
                        };

                        MultipartBody.Add(BodyAttachment);
                    }

                    Message.Body = MultipartBody;
                }
                else
                    Message.Body = new TextPart("html") { Text = Body };

                using SmtpClient Client = new SmtpClient
                {
                    ServerCertificateValidationCallback = ValidateRemoteCertificate
                };

                string pw = string.IsNullOrEmpty(OverridePassword) ? _smtpSettings.Password : SymmetricEncryption.Decrypt(OverridePassword, _smtpSettings.Key, _smtpSettings.IV);
                await Client.ConnectAsync(_smtpSettings.Server, _smtpSettings.Port, cancellationToken: cancellationToken);
                await Client.AuthenticateAsync(_smtpSettings.Username, pw, cancellationToken: cancellationToken);
                await Client.SendAsync(Message, cancellationToken: cancellationToken);
                await Client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task SendMailAsync(string Subject, string Body, string Recipient,
            string CCRecipient = null, string BCCRecipient = null,
            IEnumerable<CustomSmtpAttachment> Attachments = null, string OverridePassword = null, CancellationToken cancellationToken = default)
        {
            await SendMailAsync(Subject, Body, new string[] { Recipient }, new string[] { CCRecipient }, new string[] { BCCRecipient },
                Attachments, OverridePassword, cancellationToken);
        }

        private bool ValidateRemoteCertificate(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            Debug.WriteLine("SslPolicyErrors: {0}", sslPolicyErrors);

            return true;
        }
    }
}
