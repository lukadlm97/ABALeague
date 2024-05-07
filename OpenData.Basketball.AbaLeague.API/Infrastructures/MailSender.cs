using Microsoft.AspNetCore.Identity.UI.Services;

namespace OpenData.Basketball.AbaLeague.API.Infrastructures
{
    public class MailSender : IEmailSender
    {
        public MailSender()
        {

        }
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            
        }
    }
}
