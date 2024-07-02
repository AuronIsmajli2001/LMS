namespace LMS.Service.Services
{
    public class EmailService
    {
        public Task SendEmailAsync(string toEmail, string subject, string message)
        {
            // Implement email sending logic here
            return Task.CompletedTask;
        }
    }
}
