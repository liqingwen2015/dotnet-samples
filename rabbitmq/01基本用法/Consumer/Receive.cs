using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Configuration;
using System.Text;

namespace Consumer
{
    /// <summary>
    /// 接收消息
    /// </summary>
    public class Receive
    {
        private static readonly string appID = ConfigurationManager.AppSettings["AppID"];

        private static void Main(string[] args)
        {
            var factory = new ConnectionFactory { Uri = ConfigurationManager.AppSettings["RabbitMQUri"] };
            using (var connection = factory.CreateConnection())
            {
                using (var channel = connection.CreateModel())
                {
                    var queue = string.Format("MQ{0}.BaseStudy", appID);

                    channel.QueueDeclare(queue, false, false, false, null); //定义一个队列

                    Console.WriteLine("准备接收消息：");

                    var consumer = new EventingBasicConsumer(channel);
                    consumer.Received += (s, e) =>
                    {
                        var body = e.Body;
                        var message = Encoding.UTF8.GetString(body);
                        Console.WriteLine("接收到的消息： {0}", message);
                    };
                    channel.BasicConsume(queue, true, consumer); //开启消费者与通道、队列关联

                    Console.ReadLine();
                }
            }
        }
    }
}