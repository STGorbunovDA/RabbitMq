using RabbitMQ.Client;
using System.Text;

namespace Producer
{
    class Program
    {
        static void Main(string[] args)
        {

            var random = new Random();

            do
            {
                int timeToSleep = new Random().Next(1000, 3000);
                Thread.Sleep(timeToSleep);

                var factory = new ConnectionFactory() { HostName = "localhost" };
                using (var connection = factory.CreateConnection())
                using (var channel = connection.CreateModel())
                {
                    channel.ExchangeDeclare(exchange: "notifier",
                                            type: ExchangeType.Fanout);

                    var maneyCount = random.Next(1000, 10_000);

                    string message = $"Payment received for the amount of {maneyCount}";

                    var body = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(exchange: "notifier",
                                         routingKey: string.Empty,
                                         basicProperties: null,
                                         body: body);

                    Console.WriteLine($"Payment received for amount of {maneyCount}.\nNotifying by 'notifier' Exchange");
                }
            } while (true);
        }
    }
}