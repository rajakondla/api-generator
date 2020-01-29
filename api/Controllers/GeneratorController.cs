using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Faker;
using Faker.Extensions;
using MimeKit;
using MailKit.Security;
using MailKit.Net.Smtp;

namespace api.Controllers
{
    // Just use action name as route
    [Route("[action]")]
    public class GenerateController : Controller
    {
        [HttpGet]
        public IEnumerable<string> Names(Range range)
            => range.Of(Name.FullName);

        [HttpGet]
        public IEnumerable<string> PhoneNumbers(Range range)
            => range.Of(Phone.Number);

        [HttpGet]
        public IEnumerable<int> Numbers(Range range)
            => range.Of(RandomNumber.Next);

        [HttpGet]
        public IEnumerable<string> Companies(Range range)
            => range.Of(Company.Name);

        [HttpGet]
        public IEnumerable<string> Paragraphs(Range range)
            => range.Of(() => Lorem.Paragraph(3));

        [HttpGet]
        public IEnumerable<string> CatchPhrases(Range range)
            => range.Of(Company.CatchPhrase);

        [HttpGet]
        public IEnumerable<string> Marketing(Range range)
            => range.Of(Company.BS);

        [HttpGet]
        public IEnumerable<string> Emails(Range range)
            => range.Of(Internet.Email);

        [HttpGet]
        public IEnumerable<string> Domains(Range range)
            => range.Of(Internet.DomainName);

        [HttpPost]
        public async Task<string> EmaiRandomNames(Range range, string email = "test@fake.com")
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("Generator", "generator@generate.com"));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = "Here are your random names!";

                message.Body = new TextPart("plain")
                {
                    Text = "Hi, this is Raja!" // string.Join(Environment.NewLine, range.Of(Name.FullName))
                };

                using (var mailClient = new SmtpClient())
                {
                    await mailClient.ConnectAsync("mail", 1025,SecureSocketOptions.None);
//                    await mailClient.AuthenticateAsync("raja.kondla@xinthe.com","Vicky@201904");
                    await mailClient.SendAsync(message);
                    await mailClient.DisconnectAsync(true);
                    return "Success";
                }
            }
            catch(Exception e)
            {
                return e.Message;
            }
        }
    }
}
