namespace TravelCapstone.BackEnd.Application.IServices;

public interface IEmailService
{
    public void SendEmail(string recipient, string subject, string body);
}