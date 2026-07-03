using System.Text.Json;
using Confluent.Kafka;

public class KafkaProducer : IKafkaProducer
{
    private readonly IProducer<string,string> _producer;

    public KafkaProducer(IConfiguration config)
    {
        var producerConfig =
            new ProducerConfig
            {
                BootstrapServers =
                    config["Kafka:BootstrapServers"]
            };

        _producer =
            new ProducerBuilder<string,string>(
                producerConfig)
                .Build();
    }

    public async Task PublishAsync<T>(
        string topic,
        T message)
    {
        var json =
            JsonSerializer.Serialize(message);

        await _producer.ProduceAsync(
            topic,
            new Message<string,string>
            {
                Key = Guid.NewGuid().ToString(),
                Value = json
            });
    }
}