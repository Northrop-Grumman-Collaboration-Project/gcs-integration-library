using System;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Concurrent;

public class FibonacciRpcClient
{
    private readonly IConnection connection;
    private readonly IModel channel;
    private readonly string replyQueueName;
    private readonly EventingBasicConsumer consumer;
    private readonly BlockingCollection<string> respQueue = new BlockingCollection<string>();
    private readonly IBasicProperties props;

    public FibonacciRpcClient()
    {
        var factory = new ConnectionFactory() { HostName = "localhost" };
        connection = factory.CreateConnection();
        channel = connection.CreateModel();
        
        replyQueueName = channel.QueueDeclare().QueueName;
        consumer = new EventingBasicConsumer(channel);
        
        props = channel.CreateBasicProperties();
        var correlationId = Guid.NewGuid().ToString();
        props.CorrelationId = correlationId;
        props.ReplyTo = replyQueueName;

        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var response = Encoding.UTF8.GetString(body);
            if (ea.BasicProperties.CorrelationId == correlationId)
            {
                respQueue.Add(response);
            }
        };
    }

    public string Call(int message)
{
    var messageBytes = Encoding.UTF8.GetBytes(message.ToString());
    channel.BasicPublish(
        exchange: "",
        routingKey: "rpc_queue",
        basicProperties: props,
        body: messageBytes);

    channel.BasicConsume(
        consumer: consumer,
        queue: replyQueueName,
        autoAck: true);

    int flagCount = 0;
    var lastCall = DateTime.Now;

    while (true)
    {
        if (respQueue.TryTake(out string response, TimeSpan.FromSeconds(3)))
        {
            return response!;
        }
        else
        {
            if ((DateTime.Now - lastCall).TotalSeconds > 3)
            {
                flagCount++;
                Console.WriteLine($"Flag(s) raised: {flagCount}");
                lastCall = DateTime.Now;

                if (flagCount == 3)
                {
                    Console.WriteLine("[x] Vehicle is missing [x]");
                    return "Vehicle is missing";
                }
            }
        }
    }
}


    public void Close()
    {
        connection.Close();
    }
}

class Program
{
    static void Main(string[] args)
    {
        var rpcClient = new FibonacciRpcClient();

        Console.WriteLine(" [x] Sending commands to Vehicles");
        var response = rpcClient.Call(30);
        Console.WriteLine("Response: " + response);

        rpcClient.Close();
    }
}
