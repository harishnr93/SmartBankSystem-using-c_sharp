using SmartBankSystem.Models;

namespace SmartBankSystem.Accounts;

// Demonstrates: Inheritance, multiple interfaces, explicit interface implementation,
//               daily limit enforcement
public class BusinessAccount : AccountBase, IAuditable
{
    public string  BusinessName { get; }
    public decimal DailyLimit   { get; }

    public BusinessAccount(string accountId, string owner, string businessName,
                           decimal initialBalance = 0,
                           decimal dailyLimit     = 100_000m)
        : base(accountId, owner, initialBalance)
    {
        BusinessName = businessName;
        DailyLimit   = dailyLimit;
    }

    public override void Deposit(decimal amount, string? description = null)
    {
        var transaction = new Deposit(amount, description ?? "Business deposit");

        if (!ApplyRules(transaction))
        {
            Console.WriteLine($"  [REJECTED] Business deposit {amount:C} — failed rule.");
            return;
        }

        RecordTransaction(transaction, isCredit: true);
        Console.WriteLine($"  [OK] Business deposit {amount:C} → {AccountId} ({BusinessName})");
    }

    public override void Withdraw(decimal amount, string? description = null)
    {
        if (amount > Balance)
        {
            Console.WriteLine($"  [REJECTED] Insufficient business funds. Balance: {Balance:C}");
            return;
        }

        if (amount > DailyLimit)
        {
            Console.WriteLine($"  [REJECTED] Exceeds daily limit of {DailyLimit:C}");
            return;
        }

        var transaction = new Withdrawal(amount, description ?? "Business withdrawal");

        if (!ApplyRules(transaction))
        {
            Console.WriteLine($"  [REJECTED] Business withdrawal {amount:C} — failed rule.");
            return;
        }

        RecordTransaction(transaction, isCredit: false);
        Console.WriteLine($"  [OK] Business withdrawal {amount:C} ← {AccountId}");
    }

    // ── Explicit interface implementation ─────────────────────────────────
    void IAuditable.Log()
    {
        Console.WriteLine($"  [AUDIT] BusinessAccount {AccountId} | {BusinessName} | " +
                          $"Owner: {Owner} | Balance: {Balance:C} | Daily Limit: {DailyLimit:C}");
    }

    string IAuditable.GenerateReport() =>
        $"Business | {AccountId} | {BusinessName} | {Owner} | " +
        $"{Balance:C} | Daily Limit: {DailyLimit:C} | Txns: {Count}";
}
