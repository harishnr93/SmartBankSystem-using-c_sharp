using SmartBankSystem.Accounts;
using SmartBankSystem.Models;

namespace SmartBankSystem.Engine;

// Demonstrates: Lambda expressions as delegates, delegate composition,
//               anonymous delegates, factory methods
public class RuleEngine
{
    private readonly List<TransactionRule> _rules = new();

    // Add any lambda/delegate as a rule
    public void AddRule(TransactionRule rule) => _rules.Add(rule);

    // Evaluate all rules — returns true only if ALL pass (short-circuit)
    public bool Evaluate(Transaction transaction)
    {
        foreach (var rule in _rules)
            if (!rule(transaction)) return false;
        return true;
    }

    // Build a single composite delegate from all rules
    // Demonstrates: multicast delegate with non-void return (last result wins)
    public TransactionRule BuildCompositeRule()
    {
        TransactionRule? composite = null;
        foreach (var rule in _rules)
            composite += rule;
        return composite ?? (_ => true);
    }

    // ── Rule factories (lambda expressions) ───────────────────────────────
    public static TransactionRule MinAmount(decimal min) =>
        t => t.Amount >= min;

    public static TransactionRule MaxAmount(decimal max) =>
        t => t.Amount <= max;

    public static TransactionRule NotOnWeekend() =>
        t => t.Timestamp.DayOfWeek is not DayOfWeek.Saturday and not DayOfWeek.Sunday;

    public static TransactionRule RequireDescription() =>
        t => !string.IsNullOrWhiteSpace(t.Description) && t.Description != "No description";

    // Anonymous delegate syntax (pre-lambda style, for demonstration)
    public static TransactionRule PositiveAmountAnonymous() =>
        delegate(Transaction t) { return t.Amount > 0; };
}
