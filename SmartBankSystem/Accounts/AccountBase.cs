using SmartBankSystem.Models;

namespace SmartBankSystem.Accounts;

// ── Delegate definition ───────────────────────────────────────────────────
// Demonstrates: Custom delegate type, used for pluggable transaction rules
public delegate bool TransactionRule(Transaction transaction);

// Demonstrates: Abstract class, Indexers (int / Index / Range),
//               Events, Delegates, Polymorphism
public abstract class AccountBase : IAccount
{
    private readonly List<Transaction> _transactions = new();
    private decimal _balance;
    private TransactionRule? _rule;

    // ── Properties ────────────────────────────────────────────────────────
    public string  AccountId { get; }
    public string  Owner     { get; }
    public decimal Balance   => _balance;
    public int     Count     => _transactions.Count;   // enables savings[^1] shorthand

    // ── Event (balance change notification) ───────────────────────────────
    // Demonstrates: Events, custom EventArgs, event chaining (multiple subscribers)
    public event EventHandler<BalanceChangedEventArgs>? BalanceChanged;

    // ── Indexers ──────────────────────────────────────────────────────────
    // Classic int indexer
    public Transaction this[int index] => _transactions[index];

    // C# 8+ Index type — supports savings[^1]
    public Transaction this[Index index]
        => _transactions[index.GetOffset(_transactions.Count)];

    // C# 8+ Range type — supports savings[0..3]
    public IReadOnlyList<Transaction> this[Range range]
    {
        get
        {
            var (offset, length) = range.GetOffsetAndLength(_transactions.Count);
            return _transactions.GetRange(offset, length);
        }
    }

    protected AccountBase(string accountId, string owner, decimal initialBalance = 0)
    {
        AccountId = accountId;
        Owner     = owner;
        _balance  = initialBalance;
    }

    // ── Delegate-based rule injection ─────────────────────────────────────
    // SetRule replaces the rule; AddRule chains with the existing rule via closure
    public void SetRule(TransactionRule rule) => _rule = rule;

    public void AddRule(TransactionRule rule)
    {
        if (_rule is null)
            _rule = rule;
        else
        {
            var existing = _rule;                  // capture in closure
            _rule = t => existing(t) && rule(t);  // composite via lambda
        }
    }

    protected bool ApplyRules(Transaction transaction) =>
        _rule?.Invoke(transaction) ?? true;

    // ── Internal transfer support ─────────────────────────────────────────
    // internal access lets sibling account types call this without exposing publicly
    internal void ReceiveDeposit(decimal amount, string description)
    {
        var transaction = new Deposit(amount, description);
        RecordTransaction(transaction, isCredit: true);
    }

    protected void RecordTransaction(Transaction transaction, bool isCredit)
    {
        var previous = _balance;
        _balance += isCredit ? transaction.Amount : -transaction.Amount;
        _transactions.Add(transaction);

        // Raise event — null-conditional operator pattern
        BalanceChanged?.Invoke(this, new BalanceChangedEventArgs(previous, _balance, transaction));
    }

    // ── Statement output ──────────────────────────────────────────────────
    public virtual void PrintStatement()
    {
        Console.WriteLine($"\n{'═',1}{'═' + new string('═', 48)}");
        Console.WriteLine($"  Account : {AccountId}  |  {GetType().Name}");
        Console.WriteLine($"  Owner   : {Owner}");
        Console.WriteLine($"  Balance : {Balance:C}");
        Console.WriteLine($"  {'─',1}{new string('─', 47)}");
        Console.WriteLine($"  Transactions ({_transactions.Count}):");
        for (int i = 0; i < _transactions.Count; i++)
            Console.WriteLine($"    [{i}] {_transactions[i]}");
        Console.WriteLine($"{'═',1}{'═' + new string('═', 48)}");
    }

    // ── Abstract interface methods (forced override) ───────────────────────
    public abstract void Deposit(decimal amount, string? description = null);
    public abstract void Withdraw(decimal amount, string? description = null);
}
