using LetWeCook.Web.Models.Configs;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net;
using System.Net.Mail;

namespace LetWeCook.Services.EmailSenders
{
	public class EmailSender : IEmailSender
	{
		private readonly SmtpSettings _smtpSettings;

		public EmailSender(SmtpSettings smtpSettings)
		{
			_smtpSettings = smtpSettings;
		}

		public async Task SendEmailAsync(string email, string subject, string htmlMessage)
		{
			var smtpClient = new SmtpClient(_smtpSettings.Server)
			{
				Port = _smtpSettings.Port,
				Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
				EnableSsl = true,
			};

			var mailMessage = new MailMessage
			{
				From = new MailAddress(_smtpSettings.SenderEmail, _smtpSettings.SenderName),
				Subject = subject,
				Body = htmlMessage,
				IsBodyHtml = true,
			};

			mailMessage.To.Add(email);

			await smtpClient.SendMailAsync(mailMessage);

		}
	}
}
