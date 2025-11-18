namespace UniRumboBakend.Services.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordResetEmailAsync(string email, string resetLink);
    }
}
