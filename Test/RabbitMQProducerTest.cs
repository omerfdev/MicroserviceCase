
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OrderAPI.RabbitMQ;
using RabbitMQ.Client;

namespace Test
{
    [TestClass]
    public class RabbitMQProducerTest
    {
        [TestMethod]
        public static void TestSendMessage()
        {
            // Arrange
            var mockConnectionFactory = new Mock<IConnectionFactory>();
            var mockConnection = new Mock<IConnection>();
            var mockModel = new Mock<IModel>();

            mockConnectionFactory.Setup(factory => factory.CreateConnection()).Returns(mockConnection.Object);
            mockConnection.Setup(connection => connection.CreateModel()).Returns(mockModel.Object);

            var rabbitMQProducer = new RabbitMQProducer();

            // Act
            rabbitMQProducer.SendMessage("TestMessage");

            // Assert
            // Verify that the methods are called as expected
            mockModel.Verify(
                model => model.ExchangeDeclare(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>()),
                Times.Once
            );
            mockModel.Verify(
                model => model.QueueDeclare(It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<bool>(), It.IsAny<IDictionary<string, object>>()),
                Times.Once
            );
            mockModel.Verify(
                model => model.QueueBind(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IDictionary<string, object>>()),
                Times.Once
            );
            mockModel.Verify(
                model => model.BasicPublish(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<IBasicProperties>(), It.IsAny<byte[]>()),
                Times.Once
            );
        }
    }
}
