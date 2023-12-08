using System;
using System.Text;
using OrderConsumer;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

class Worker
{
    static void Main()
    {
        var orderLogWorker = new OrderLogWorker();
        orderLogWorker.Start();
    }
}

