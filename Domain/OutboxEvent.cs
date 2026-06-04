public class OutboxEvent
{
    public Guid Id { get; set; }

    public string EventType { get; set; } = "";

    public string Payload { get; set; } = "";

    public bool Published { get; set; }

    public DateTime CreatedAt { get; set; }
}