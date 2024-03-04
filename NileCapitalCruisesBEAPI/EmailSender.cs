using NileCapitalCruisesBEAPI;
using System.Net;
using System.Net.Mail;

public class EmailSender : IEmailSender
{
    public Task SendEmailAsync(string htmlBody)
    {
        //var senderEmail = "testtittest40@gmail.com";
        //var recipientEmail = "abdallah.abdelnasser@titegypt.com";
        ////var recipientEmail = "ahmed.taha@titegypt.com";
        //var subject = "Nile Capital Cruise Booking Mail";


        //var client = new SmtpClient("smtp.gmail.com")
        //{
        //    Port = 587,
        //    EnableSsl = true,
        //    UseDefaultCredentials = false,
        //    Credentials = new NetworkCredential(senderEmail, "rval zjjb mxth ceoe")
        //};

        var senderEmail = "do-not-reply@nilecapitalcruises.com";
        var recipientEmail = "abdallah.abdelnasser@titegypt.com";
        var subject = "Nile Capital Cruise Booking Mail";


        var client = new SmtpClient("red.specialservers.com")
        {
            Port = 587,
            EnableSsl = true,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(senderEmail, "Uhll38^86_Ak81r7q9")
        };

        return client.SendMailAsync(
         new MailMessage
         {
             From = new MailAddress(senderEmail),
             To = { new MailAddress(recipientEmail) },
             Subject = subject,
             Body = htmlBody,
             IsBodyHtml = true
         });

        //return client.SendMailAsync(
        //    new MailMessage(from: senderEmail,
        //                    to: recipientEmail,
        //                    subject,
        //                    htmlBody

        //                    ));
    }
}