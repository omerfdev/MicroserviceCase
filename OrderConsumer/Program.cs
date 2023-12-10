using System;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using OrderConsumer;
using OrderConsumer.Models;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class Worker
{

    public static async void SaveOrderToDatabase(Order order)
    {
        try
        {
            // MySQL veritabanı bağlantı ayarlarını buraya ekleyin
            var optionsBuilder = new DbContextOptionsBuilder<AuditDbContext>();
            optionsBuilder.UseMySQL("Server=auditdb;Database=AuditDB;User=root;Password=Omer1234;Port=3306;");

            using (var dbContext = new AuditDbContext(optionsBuilder.Options))
            {
                // Order'ı OrdersLog tablosuna ekleyin
               await dbContext.OrdersLog.AddAsync(order);

                await dbContext.SaveChangesAsync();
                Console.WriteLine("Order saved to the database.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error saving order to the database: {ex.Message}");
        }
    }
    static void Main()
    {
        string rabbitMQHost = "myrabbitmq";
        int rabbitMQPort = 5672;
        string rabbitMQUserName = "guest";
        string rabbitMQPassword = "guest";
        string exchangeName = "OrderProcessingExchange";
        string queueName = "OrderQueue";

        ConnectionFactory factory = new ConnectionFactory
        {
            HostName = rabbitMQHost,
            Port = rabbitMQPort,
            UserName = rabbitMQUserName,
            Password = rabbitMQPassword,
        };

        IConnection connection = null;
        IModel channel = null;

        try
        {
            connection = factory.CreateConnection();
            channel = connection.CreateModel();

            // Declare the exchange and queue
            channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
            channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
            channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");

            // Create a consumer
            var consumer = new EventingBasicConsumer(channel);

            // Register the event handler for received messages
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body;
                var message = Encoding.UTF8.GetString(body.ToArray());

                // JSON'dan Order nesnesine dönüştür
                var order = JsonConvert.DeserializeObject<Order>(message);
                order.ReceivedAt = DateTime.Now;
                order.Message = "Ok";
                Console.WriteLine($"Received message: {order.CustomerId}, {order.Quantity}, {order.Price}, {order.Status}");
                SaveOrderToDatabase(order);
                // İşlemlerinizi burada gerçekleştirin
            };

            // Start consuming messages from the queue
            channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);
            
            Console.WriteLine("Consumer started. Press [Enter] to exit.");
            Console.ReadLine();
        }
        finally
        {
            // Manually close the channel and connection
            channel?.Close();
            connection?.Close();
        }
    }
}
