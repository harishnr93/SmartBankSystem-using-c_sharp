namespace SmartBankSystem.Models;

// Demonstrates: Inheritance, method override
public class Deposit : Transaction
{
    public override string TransactionType => "Deposit";

    public Deposit(decimal amount, string? description = null)
        : base(amount, description) { }
}
