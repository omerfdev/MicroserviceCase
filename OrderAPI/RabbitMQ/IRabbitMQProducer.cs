namespace OrderAPI.RabbitMQ
{
    public interface IRabbitMQProducer
    {
        public void SendMessage<T>(T message);
    }
}
