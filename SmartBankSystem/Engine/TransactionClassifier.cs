using SmartBankSystem.Models;

namespace SmartBankSystem.Engine;

// Demonstrates: Structural pattern matching, switch expressions,
//               relational patterns, type patterns, positional patterns, when guards
public static class TransactionClassifier
{
    // ── Switch expression with type + property patterns ───────────────────
    public static string Classify(Transaction transaction) => transaction switch
    {
        Deposit   { Amount: > 10_000 } => "HIGH_VALUE_DEPOSIT",
        Deposit   { Amount: > 1_000  } => "STANDARD_DEPOSIT",
        Deposit                         => "MICRO_DEPOSIT",
        Withdrawal { Amount: > 5_000 } => "LARGE_WITHDRAWAL",
        Withdrawal                      => "STANDARD_WITHDRAWAL",
        Transfer   { Amount: > 10_000 } => "LARGE_TRANSFER",
        Transfer                        => "STANDARD_TRANSFER",
        _                               => "UNKNOWN"
    };

    // ── Relational patterns for risk scoring ──────────────────────────────
    public static string GetRiskLevel(Transaction transaction) => transaction switch
    {
        Deposit    d when d.Amount > 50_000 => "HIGH_RISK",
        Deposit    d when d.Amount > 10_000 => "MEDIUM_RISK",
        Withdrawal w when w.Amount > 20_000 => "HIGH_RISK",
        Transfer   t when t.Amount > 30_000 => "HIGH_RISK",
        _                                   => "LOW_RISK"
    };

    // ── Relational patterns on a standalone value ─────────────────────────
    public static string GetAmountBand(decimal amount) => amount switch
    {
        < 0                     => "INVALID",
        0                       => "ZERO",
        > 0      and < 100      => "MICRO",
        >= 100   and < 1_000    => "SMALL",
        >= 1_000 and < 10_000   => "MEDIUM",
        >= 10_000               => "LARGE"
    };

    // ── Full analysis combining multiple pattern types ─────────────────────
    public static void PrintAnalysis(Transaction transaction)
    {
        // Positional pattern — uses the Deconstruct method
        var (type, amount, description) = transaction;

        Console.WriteLine($"  ┌─ Transaction Analysis");
        Console.WriteLine($"  │  Type           : {type}");
        Console.WriteLine($"  │  Amount         : {amount:C}");
        Console.WriteLine($"  │  Description    : {description}");
        Console.WriteLine($"  │  Classification : {Classify(transaction)}");
        Console.WriteLine($"  │  Risk Level     : {GetRiskLevel(transaction)}");
        Console.WriteLine($"  │  Amount Band    : {GetAmountBand(amount)}");

        // Structural pattern matching — property pattern
        if (transaction is Deposit { Amount: > 10_000 })
            Console.WriteLine($"  │  *** LARGE DEPOSIT FLAG ***");

        // Type check + when guard
        if (transaction is Transfer t2 && t2.Amount > 5_000)
            Console.WriteLine($"  │  *** SIGNIFICANT TRANSFER → {t2.ToAccountId} ***");

        Console.WriteLine($"  └─────────────────────────");
    }
}
