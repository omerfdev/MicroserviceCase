using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Http.Json;
using System.Text;

namespace OrderAPI.RabbitMQ
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        public void SendMessage<T>(T message)
        {
            string rabbitMQHost = "localhost"; // RabbitMQ server IP adresi ya da host adı
            int rabbitMQPort = 5672; // Default RabbitMQ portu
            string rabbitMQUserName = "guest"; // RabbitMQ kullanıcı adı
            string rabbitMQPassword = "guest"; // RabbitMQ şifre
            string exchangeName = "OrderProcessingExchange";
            string queueName = "OrderQueue";

            // RabbitMQ bağlantı bilgileri
            var factory = new ConnectionFactory
            {
                HostName = rabbitMQHost,
                Port = rabbitMQPort,
                UserName = rabbitMQUserName,
                Password = rabbitMQPassword,
                // İsteğe bağlı olarak diğer bağlantı seçeneklerini de belirleyebilirsiniz.
                // Örneğin: VirtualHost, Ssl, vb.
            };
            //Create the RabbitMQ connection using connection factory details as i mentioned above
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                // Exchange oluştur
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);

                // Kuyruk oluştur
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                // Exchange ile kuyruğu bağla
                channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");
                //Serialize the message
                var json = JsonContent.Create(message);
                var body = Encoding.UTF8.GetBytes(json.ToString());
                channel.BasicPublish(exchange: "", routingKey: "OrderQueue", body: body);
                Console.WriteLine($"Exchange '{exchangeName}' ve kuyruk '{queueName}' başarıyla oluşturuldu ve birbirine bağlandı.");
         
                
                //dinleyip iş yapan worker denir.
            
               
            }
            
            //put the data on to the product queue
           
        }


    }
}
