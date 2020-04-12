using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace integrations
{
    public class EmailTests
    {
        public const string GeneratorApiRoot = "http://generator:80";
        public const string MailHogApiV2Root = "http://mail:8025/api/v2";

        [Fact]
        public async Task SendEmailWithNames_IsFromGenerator()
        {
            // send email
            var client = new HttpClient();
            Console.WriteLine($"{GeneratorApiRoot}/EmailRandomNames");
            var sendEmail = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri($"{GeneratorApiRoot}/EmailRandomNames")
            };
            Console.WriteLine($"Sending email to: {sendEmail.RequestUri}");

            //var resp= await client.SendAsync(sendEmail);
            //Console.WriteLine("Status code: "+ resp.StatusCode);


            using (var response = await client.SendAsync(sendEmail))
            {
                Console.WriteLine("status code:"+ response.StatusCode);
                response.EnsureSuccessStatusCode();
            }

            // check if email
            var checkEmails = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri($"{MailHogApiV2Root}/messages")
            };
            Console.WriteLine($"Checking emails: {checkEmails.RequestUri}");
            using (var response = await client.SendAsync(checkEmails))
            {
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                var messages = JObject.Parse(content);
                
                messages.Should().HaveElement("total").Which.Should().BeEquivalentTo(1);
                messages.Should().HaveElement("items")
                    .Which.Should().BeOfType<JArray>()
                    .Which.First.Should().HaveElement("Raw")
                    .Which.Should().HaveElement("From")
                    .Which.Should().HaveValue("generator@generate.com");
            }
        }
    }
}
