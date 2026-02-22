namespace SmartBankSystem.Models;

// Abstract base class for all transaction types
// Demonstrates: Abstraction, IComparable, custom ToString/Equals/GetHashCode
public abstract class Transaction : IComparable<Transaction>
{
    public Guid   Id          { get; } = Guid.NewGuid();
    public decimal Amount     { get; }
    public string  Description { get; }
    public DateTime Timestamp  { get; }

    // Abstract property — subclasses must define their type name
    public abstract string TransactionType { get; }

    protected Transaction(decimal amount, string? description = null)
    {
        Amount      = amount;
        Description = description ?? "No description";   // null-coalescing
        Timestamp   = DateTime.UtcNow;
    }

    // ── Deconstruction (3-param) ──────────────────────────────────────────
    public void Deconstruct(out string type, out decimal amount, out string description)
    {
        type        = TransactionType;
        amount      = Amount;
        description = Description;
    }

    // ── IComparable — sort by Amount descending ───────────────────────────
    public int CompareTo(Transaction? other)
    {
        if (other is null) return 1;
        return other.Amount.CompareTo(Amount); // descending
    }

    // ── Custom object behavior ────────────────────────────────────────────
    public override string ToString() =>
        $"[{TransactionType,-12}] {Amount,12:C} | {Description} | {Timestamp:yyyy-MM-dd HH:mm:ss}";

    public override bool Equals(object? obj) =>
        obj is Transaction t && Id == t.Id;

    public override int GetHashCode() => Id.GetHashCode();
}
