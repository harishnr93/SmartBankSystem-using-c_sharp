using SmartBankSystem.Models;

namespace SmartBankSystem.Accounts;

// Demonstrates: Inheritance, overdraft logic, inter-account transfer
public class CheckingAccount : AccountBase
{
    public decimal OverdraftLimit { get; }

    public CheckingAccount(string accountId, string owner,
                           decimal initialBalance  = 0,
                           decimal overdraftLimit  = 500m)
        : base(accountId, owner, initialBalance)
    {
        OverdraftLimit = overdraftLimit;
    }

    public override void Deposit(decimal amount, string? description = null)
    {
        var transaction = new Deposit(amount, description ?? "Checking deposit");

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
        if (amount > Balance + OverdraftLimit)
        {
            Console.WriteLine($"  [REJECTED] Exceeds overdraft. Balance: {Balance:C}, Limit: {OverdraftLimit:C}");
            return;
        }

        var transaction = new Withdrawal(amount, description ?? "Checking withdrawal");

        if (!ApplyRules(transaction))
        {
            Console.WriteLine($"  [REJECTED] Withdrawal {amount:C} — failed rule validation.");
            return;
        }

        RecordTransaction(transaction, isCredit: false);
        Console.WriteLine($"  [OK] Withdrew {amount:C} ← {AccountId}");
    }

    // Transfer to any account — uses internal ReceiveDeposit on AccountBase
    public void TransferTo(AccountBase target, decimal amount, string? description = null)
    {
        if (amount > Balance + OverdraftLimit)
        {
            Console.WriteLine($"  [REJECTED] Transfer {amount:C} exceeds available funds.");
            return;
        }

        var outTx = new Transfer(amount, target.AccountId,
                                 description ?? $"Transfer to {target.AccountId}");
        RecordTransaction(outTx, isCredit: false);
        target.ReceiveDeposit(amount, $"Transfer from {AccountId}");

        Console.WriteLine($"  [OK] Transferred {amount:C}: {AccountId} → {target.AccountId}");
    }
}
