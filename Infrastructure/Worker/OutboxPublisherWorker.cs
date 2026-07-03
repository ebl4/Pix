using Microsoft.EntityFrameworkCore;

public class OutboxPublisherWorker
    : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;

    public OutboxPublisherWorker(
        IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(
        CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            using var scope =
                _scopeFactory.CreateScope();

            var db =
                scope.ServiceProvider
                    .GetRequiredService<AppDbContext>();

            var producer =
                scope.ServiceProvider
                    .GetRequiredService<IKafkaProducer>();

            var events =
                await db.OutboxEvents
                    .Where(x => !x.Published)
                    .Take(100)
                    .ToListAsync();

            foreach(var evt in events)
            {
                await producer.PublishAsync(
                    "pix-requested",
                    evt.Payload);

                evt.Published = true;
            }

            await db.SaveChangesAsync();

            await Task.Delay(
                TimeSpan.FromSeconds(5),
                stoppingToken);
        }
    }
}