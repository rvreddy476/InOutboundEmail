using Email.Services.InboundEmailAPI.Models;
using Email.Services.InboundEmailAPI.Models.DTO;
using RabbitMQ.Client;
using System.Text;


namespace Email.Services.InboundEmailAPI.RabbitMQ
{
    public class RabbitMQProcedure : IRabbitMQProcedure
    {
        private readonly ConnectionFactory _connectionFactory;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly ILogger<RabbitMQProcedure> _logger;
        public RabbitMQProcedure(ILogger<RabbitMQProcedure> logger)
        {
            //_logger = logger;
            //_connectionFactory = new ConnectionFactory()
            //{
            //    HostName = "localhost", // RabbitMQ host
            //    Port = 5672, // RabbitMQ port
            //    UserName = "guest", // RabbitMQ username
            //    Password = "guest", // RabbitMQ password
            //    VirtualHost = "/" // RabbitMQ virtual host
            //};
            //_connection = _connectionFactory.CreateConnection();
            //_channel = _connection.CreateModel();
        }
        public void ReveiveMessage(List<InboundEmail> mail)
        {
            try
            {
                _channel.QueueDeclare(queue: "incoming_emails", durable: true, exclusive: false, autoDelete: false, arguments: null);
                var body = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(mail));
                _channel.BasicPublish(exchange: "", routingKey: "incoming_emails", basicProperties: null, body: body);
                Console.WriteLine($"Sent email message: {mail}");
            }
            catch (Exception err)
            {
                _logger.LogError(err, err.Message);
            }
        }
    }
}
