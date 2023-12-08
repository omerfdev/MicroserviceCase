using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrderConsumer.Models;

namespace OrderConsumer
{
    public class OrderLogWorker
    {
        public void Start()
        {
            string rabbitMQHost = "localhost";
            int rabbitMQPort = 5672;
            string rabbitMQUserName = "guest";
            string rabbitMQPassword = "guest";
            string exchangeName = "OrderProcessingExchange";
            string queueName = "OrderQueue";

            var factory = new ConnectionFactory
            {
                HostName = rabbitMQHost,
                Port = rabbitMQPort,
                UserName = rabbitMQUserName,
                Password = rabbitMQPassword,
            };

            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.ExchangeDeclare(exchange: exchangeName, type: ExchangeType.Direct);
                channel.QueueDeclare(queue: queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);
                channel.QueueBind(queue: queueName, exchange: exchangeName, routingKey: "");

                var consumer = new EventingBasicConsumer(channel);

                consumer.Received += (model, ea) =>
                {
                    try
                    {
                        var body = ea.Body.ToArray();
                        var message = Encoding.UTF8.GetString(body);

                        Console.WriteLine($"Received message: {message}");

                        // Mesajı MySQL veritabanına kaydet
                        using (var dbContext = new AuditDbContext(new DbContextOptionsBuilder<AuditDbContext>()
                            .UseMySQL("Server=localhost;port=3306;Database=AuditDB;User Id=root;Password=Omer1234;", options => options.EnableRetryOnFailure())
                            .Options))
                        {
                            var order = new Order { Message = message, ReceivedAt = DateTime.Now };
                            dbContext.OrdersLog.Add(order);
                            dbContext.SaveChanges();
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error: {ex.Message}");
                    }
                };

                channel.BasicConsume(queue: queueName, autoAck: true, consumer: consumer);

                Console.WriteLine("Worker started. Press any key to exit.");
                Console.ReadKey();
            }
        }
    }
}
