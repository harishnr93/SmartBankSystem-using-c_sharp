namespace SmartBankSystem.Accounts;

// Demonstrates: Interface definition, contract-based design
public interface IAccount
{
    string  AccountId { get; }
    string  Owner     { get; }
    decimal Balance   { get; }

    void Deposit(decimal amount, string? description = null);
    void Withdraw(decimal amount, string? description = null);
    void PrintStatement();
}
