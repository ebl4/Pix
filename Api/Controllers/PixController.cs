using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Pix.Domain.Entities;

public class PixController (AppDbContext db) : ControllerBase
{
    [HttpPost("/pix")]
    public async Task<IActionResult> CreatePix(CreatePixRequest request)
    {
        var pix = new PixTransaction(
            request.SenderAccount,
            request.ReceiverAccount,
            request.Amount);

        var evt = new PixRequestedEvent(
            pix.Id,
            pix.SenderAccount,
            pix.ReceiverAccount,
            pix.Amount);

        var outbox = new OutboxEvent
        {
            Id = Guid.NewGuid(),

            EventType = nameof(PixRequestedEvent),

            Payload =
                JsonSerializer.Serialize(evt),

            CreatedAt = DateTime.UtcNow,

            Published = false
        };

        db.PixTransactions.Add(pix);

        db.OutboxEvents.Add(outbox);

        await db.SaveChangesAsync();

        return Accepted(
            $"/api/pix/{pix.Id}",
            new
            {
                pix.Id
            });
    }
}