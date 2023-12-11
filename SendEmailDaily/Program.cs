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
            // Burada gerekli SMTP ve diğer bilgileri girin
            var dailyEmailSender = new DailyEmailSender (
                smtpServer: "smtp.gmail.com",
                smtpPort: 587,
                smtpUsername: "noreplyhrelp@gmail.com",
                smtpPassword: "rmzqiazgoktnbcac",
                senderEmail: "noreplyhrelp@gmail.com",
                recipientEmail: "omeralmali1@gmail.com",
                subject: "Daily Database Update",
                body: "Here are the new records added in the last day:",
                connectionString: "Server=127.0.0.1;Port:18009;Database=AuditDB;User=root;Password=Omer1234;"

            );

            // Abone olunacak sınıfı oluşturun ve olaya abone olun
            var subscriber = new DailyEmailSubscriber(dailyEmailSender);

            // E-posta gönderme işlemini başlatın
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
            // E-posta gönderme işlemi veya başka işlemler gerçekleştirilebilir.
            // e.NewData içinde yeni eklenen verilere erişilebilir.
        }
    }
}