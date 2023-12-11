using MySqlX.XDevAPI;
using SendEmailDailyLibrary;
using System.Net.Sockets;
using System.Xml.Linq;

namespace SendEmailDaily
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var dailyEmailSender = new DailyEmailSender (
                smtpServer: "smtp.gmail.com",
                smtpPort: 587,
                smtpUsername: "noreplyhrelp@gmail.com",
                smtpPassword: "rmzqiazgoktnbcac",
                senderEmail: "noreplyhrelp@gmail.com",
                recipientEmail: "omeralmali1@gmail.com",
                subject: "Daily Database Update",
                body: "Here are the new records added in the last day:",
                connectionString: "Server=auditdb;Port:18009;Database=AuditDB;User=root;Password=Omer1234;"
            );

            var subscriber = new DailyEmailSubscriber(dailyEmailSender);
            await dailyEmailSender.SendDailyEmail();
            Console.WriteLine("Press any key to exit...");
        }
    }

    public class DailyEmailSubscriber
    {
        public DailyEmailSubscriber(DailyEmailSender sender)
        {
            sender.DailyEmailEvent += HandleDailyEmailEvent;
        }

        private void HandleDailyEmailEvent(object sender, DailyEmailEventArgs e)
        {
            Console.WriteLine("Daily email event received. Sending email...");
        }
    }
}