using AadharVerify.Helper;

namespace AadharVerify.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(Mailrequest mailrequest);
    }
}
