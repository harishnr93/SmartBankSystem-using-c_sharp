namespace SmartBankSystem.Models;

// Demonstrates: Inheritance, extended Deconstruction (4-param)
public class Transfer : Transaction
{
    public override string TransactionType => "Transfer";
    public string ToAccountId { get; }

    public Transfer(decimal amount, string toAccountId, string? description = null)
        : base(amount, description)
    {
        ToAccountId = toAccountId;
    }

    // Extended Deconstruction — 4-param version (does not shadow base 3-param)
    public void Deconstruct(out string type, out decimal amount, out string description, out string toAccountId)
    {
        type        = TransactionType;
        amount      = Amount;
        description = Description;
        toAccountId = ToAccountId;
    }
}
