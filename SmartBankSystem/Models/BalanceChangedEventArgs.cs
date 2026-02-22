namespace SmartBankSystem.Models;

// Demonstrates: Custom EventArgs
public class BalanceChangedEventArgs : EventArgs
{
    public decimal     PreviousBalance { get; }
    public decimal     NewBalance      { get; }
    public Transaction Transaction     { get; }

    public BalanceChangedEventArgs(decimal previous, decimal newBalance, Transaction transaction)
    {
        PreviousBalance = previous;
        NewBalance      = newBalance;
        Transaction     = transaction;
    }
}
