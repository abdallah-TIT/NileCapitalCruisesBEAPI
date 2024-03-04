

namespace NileCapitalCruisesBEAPI
{
    public interface IEmailSender
    {

        Task SendEmailAsync(string message);
    }
}
