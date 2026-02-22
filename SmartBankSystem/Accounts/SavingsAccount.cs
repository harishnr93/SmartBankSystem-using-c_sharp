using SmartBankSystem.Models;

namespace SmartBankSystem.Accounts;

// Demonstrates: Inheritance, multiple interfaces, explicit interface implementation,
//               polymorphism, access modifiers
public class SavingsAccount : AccountBase, IAuditable
{
    public decimal InterestRate { get; }

    public SavingsAccount(string accountId, string owner,
                          decimal initialBalance = 0,
                          decimal interestRate   = 0.03m)
        : base(accountId, owner, initialBalance)
    {
        InterestRate = interestRate;
    }

    public override void Deposit(decimal amount, string? description = null)
    {
        var transaction = new Deposit(amount, description ?? "Savings deposit");

        if (!ApplyRules(transaction))
        {
            Console.WriteLine($"  [REJECTED] Deposit {amount:C} — failed rule validation.");
            return;
        }

        RecordTransaction(transaction, isCredit: true);
        Console.WriteLine($"  [OK] Deposited {amount:C} → {AccountId}");
    }

    public override void Withdraw(decimal amount, string? description = null)
    {
        if (amount > Balance)
        {
            Console.WriteLine($"  [REJECTED] Insufficient funds. Balance: {Balance:C}");
            return;
        }

        var transaction = new Withdrawal(amount, description ?? "Savings withdrawal");

        if (!ApplyRules(transaction))
        {
            Console.WriteLine($"  [REJECTED] Withdrawal {amount:C} — failed rule validation.");
            return;
        }

        RecordTransaction(transaction, isCredit: false);
        Console.WriteLine($"  [OK] Withdrew {amount:C} ← {AccountId}");
    }

    public void ApplyInterest()
    {
        var interest = Balance * InterestRate;
        Deposit(interest, $"Interest @ {InterestRate:P0}");
    }

    // ── Explicit interface implementation ─────────────────────────────────
    // savings.Log() is a compile error — must cast: ((IAuditable)savings).Log()
    void IAuditable.Log()
    {
        Console.WriteLine($"  [AUDIT] SavingsAccount {AccountId} | Owner: {Owner} | " +
                          $"Balance: {Balance:C} | Rate: {InterestRate:P1}");
    }

    string IAuditable.GenerateReport() =>
        $"Savings | {AccountId} | {Owner} | {Balance:C} | " +
        $"Rate: {InterestRate:P1} | Txns: {Count}";
}
