public record CreatePixRequest
(
    string SenderAccount,
    string ReceiverAccount,
    decimal Amount
);