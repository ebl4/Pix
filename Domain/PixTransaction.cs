namespace Pix.Domain.Entities;

public class PixTransaction
{
    public Guid Id { get; private set; }

    public string SenderAccount { get; private set; }

    public string ReceiverAccount { get; private set; }

    public decimal Amount { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public PixTransaction(
        string sender,
        string receiver,
        decimal amount)
    {
        Id = Guid.NewGuid();

        SenderAccount = sender;

        ReceiverAccount = receiver;

        Amount = amount;

        CreatedAt = DateTime.UtcNow;
    }
}

public record PixRequestedEvent
(
    Guid TransactionId,
    string SenderAccount,
    string ReceiverAccount,
    decimal Amount
);