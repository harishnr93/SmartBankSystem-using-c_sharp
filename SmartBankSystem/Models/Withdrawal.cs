namespace SmartBankSystem.Models;

// Demonstrates: Inheritance
public class Withdrawal : Transaction
{
    public override string TransactionType => "Withdrawal";

    public Withdrawal(decimal amount, string? description = null)
        : base(amount, description) { }
}
