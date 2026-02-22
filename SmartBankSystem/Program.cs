// ============================================================
//  SmartBank System — Comprehensive C# Feature Demonstration
// ============================================================
// Topics covered:
//   OOP (classes, inheritance, polymorphism, abstraction)
//   Delegates (named, anonymous, composite / multicast, chaining)
//   Events (custom EventArgs, multiple subscribers, event chaining)
//   Lambda expressions (inline rules, closures, event handlers)
//   Interfaces (definition, implementation, casting, multiple, explicit)
//   Structural pattern matching (switch expr, type/property/relational patterns)
//   Indexers (int, Index ^n, Range 0..n)
//   Deconstruction (class + tuple)
//   Null-coalescing (??, ??=, ?.)
//   Custom comparisons (IComparable<T>)
//   Custom type behaviour (ToString, Equals, GetHashCode)
//   Literal number improvements (digit separators, binary, hex)
// ============================================================

using SmartBankSystem.Accounts;
using SmartBankSystem.Engine;
using SmartBankSystem.Models;

Banner("SmartBank System — C# Feature Demo");

// ─────────────────────────────────────────────────────────────────────────────
// 1. ACCOUNT CREATION — OOP: Constructors, Inheritance, Polymorphism
// ─────────────────────────────────────────────────────────────────────────────
Section("1. ACCOUNT CREATION");

var savings  = new SavingsAccount ("SAV-001", "Alice Johnson", 1_000.00m, 0.05m);
var checking = new CheckingAccount("CHK-001", "Bob Smith",    2_500.00m, overdraftLimit: 500m);
var business = new BusinessAccount("BUS-001", "Carol White",  "ACME Corp", 50_000.00m, 100_000m);

Console.WriteLine($"  Savings  | {savings.AccountId}  | {savings.Owner,-15} | {savings.Balance:C}");
Console.WriteLine($"  Checking | {checking.AccountId} | {checking.Owner,-15} | {checking.Balance:C}");
Console.WriteLine($"  Business | {business.AccountId} | {business.Owner,-15} | {business.Balance:C}");

// ─────────────────────────────────────────────────────────────────────────────
// 2. DELEGATES — Named, anonymous, multicast (composite), chaining
// ─────────────────────────────────────────────────────────────────────────────
Section("2. DELEGATES & LOGGER (MULTICAST / CHAINING)");

// Chain multiple loggers with +=  (composite / multicast delegate)
Logger.AddLogger(Logger.ConsoleLogger);
Logger.AddLogger(Logger.CreatePrefixLogger("AUDIT"));

// Named method stored as a delegate, then chained in
Logger.AddLogger(NamedAuditLog);
Logger.Log("SmartBank initialised — 3 loggers active");

// Remove one logger with -= (delegate removal)
Logger.RemoveLogger(NamedAuditLog);
Logger.Log("After removal — 2 loggers active");

// Anonymous delegate syntax (pre-lambda style)
TransactionRule positiveAmountRule = delegate(Transaction t) { return t.Amount > 0; };
Console.WriteLine($"  Anonymous delegate test (100): {positiveAmountRule(new Deposit(100m))}");

// ─────────────────────────────────────────────────────────────────────────────
// 3. EVENTS — Custom EventArgs, multiple subscribers, event chaining
// ─────────────────────────────────────────────────────────────────────────────
Section("3. EVENTS");

// Lambda event handler (subscriber 1)
savings.BalanceChanged += (_, e) =>
    Logger.Log($"[SAV] {e.PreviousBalance:C} → {e.NewBalance:C} | {e.Transaction.TransactionType}");

// Named method as event handler on checking (subscriber 1)
checking.BalanceChanged += OnCheckingBalanceChanged;

// Second lambda subscriber on savings — demonstrates event chaining
savings.BalanceChanged += (_, e) =>
{
    if (e.NewBalance < 50m)
        Console.WriteLine("  [ALERT] Savings critically low!");
};

Console.WriteLine("  Events subscribed (savings: 2 handlers, checking: 1 handler).");

// ─────────────────────────────────────────────────────────────────────────────
// 4. LAMBDA EXPRESSIONS — Inline rules, closures, rule composition
// ─────────────────────────────────────────────────────────────────────────────
Section("4. LAMBDA EXPRESSIONS & RULE ENGINE");

// Lambda rules added directly to account (closure-based chaining in AccountBase)
savings.AddRule(t => t.Amount > 0);
savings.AddRule(t => t.Amount <= 1_000_000m);

// RuleEngine collects lambda rules; injected into business account via delegate
var engine = new RuleEngine();
engine.AddRule(RuleEngine.MinAmount(0.01m));
engine.AddRule(RuleEngine.MaxAmount(100_000m));
engine.AddRule(RuleEngine.RequireDescription());

// Inject the engine's evaluation as a single delegate into business account
business.SetRule(t => engine.Evaluate(t));

// Build a composite delegate from rule list (non-void return: last result wins)
var composite = engine.BuildCompositeRule();
Console.WriteLine($"  Composite rule on $500 deposit: {composite(new Deposit(500m, "Test"))}");
Console.WriteLine("  Rules configured on all accounts.");

// ─────────────────────────────────────────────────────────────────────────────
// 5. TRANSACTIONS — Polymorphism, Inheritance, Events fired
// ─────────────────────────────────────────────────────────────────────────────
Section("5. TRANSACTIONS");

savings.Deposit(500m,     "Monthly paycheck");
savings.Deposit(15_000m,  "Annual bonus");
savings.Withdraw(200m,    "ATM withdrawal");
savings.ApplyInterest();                           // interest = Balance * 5%

checking.Deposit(1_000m,  "Direct deposit");
checking.Withdraw(300m,   "Grocery store");
checking.TransferTo(savings, 250m, "Savings top-up");

business.Deposit(25_000m, "Client payment - INV-2025-042");
business.Withdraw(5_000m, "Office supplies - PO-0192");

// Rule rejection demos
Console.WriteLine("\n  -- Rule Rejection Tests --");
savings.Deposit(-50m,       "Negative amount");      // rejected: amount > 0
savings.Deposit(2_000_000m, "Way over the limit");   // rejected: amount <= 1_000_000

// ─────────────────────────────────────────────────────────────────────────────
// 6. INDEXERS & RANGES — int, C# 8+ Index (^), Range (..)
// ─────────────────────────────────────────────────────────────────────────────
Section("6. INDEXERS & RANGES (C# 8+)");

if (savings.Count > 0)
{
    // Classic int indexer
    Console.WriteLine($"  savings[0]  (first) : {savings[0]}");

    // Index type — ^1 means "one from the end"
    Console.WriteLine($"  savings[^1] (last)  : {savings[^1]}");

    // Range type — first two transactions
    if (savings.Count >= 2)
    {
        Console.WriteLine("  savings[0..2] (slice):");
        foreach (var tx in savings[0..2])
            Console.WriteLine($"    {tx}");
    }
}

// ─────────────────────────────────────────────────────────────────────────────
// 7. PATTERN MATCHING — switch expressions, type/property/relational patterns
// ─────────────────────────────────────────────────────────────────────────────
Section("7. STRUCTURAL PATTERN MATCHING");

var samples = new List<Transaction>
{
    new Deposit(15_000m,              "Large deposit"),
    new Withdrawal(500m,              "Regular withdrawal"),
    new Transfer(8_000m, "CHK-001",   "Inter-account move"),
    new Deposit(50m,                  "Micro top-up"),
};

// Full analysis: type + property + relational + when guard + positional patterns
foreach (var tx in samples)
    TransactionClassifier.PrintAnalysis(tx);

// Switch expression with mixed pattern types
Console.WriteLine("\n  -- Switch Expression --");
foreach (var tx in samples)
{
    string label = tx switch
    {
        Deposit   { Amount: > 10_000 } d => $"HIGH-VALUE DEPOSIT      : {d.Amount:C}",
        Deposit                        d => $"Deposit                 : {d.Amount:C}",
        Withdrawal { Amount: > 1_000 } w => $"Large Withdrawal        : {w.Amount:C}",
        Withdrawal                     w => $"Withdrawal              : {w.Amount:C}",
        Transfer                       t => $"Transfer → {t.ToAccountId,-8}: {t.Amount:C}",
        _                                => "Unknown"
    };
    Console.WriteLine($"  {label}");
}

// ─────────────────────────────────────────────────────────────────────────────
// 8. DECONSTRUCTION — class Deconstruct() + tuples
// ─────────────────────────────────────────────────────────────────────────────
Section("8. DECONSTRUCTION");

// 3-param Deconstruct defined in Transaction base class
var deposit3 = new Deposit(1_500m, "Test deposit");
var (txType, txAmount, txDesc) = deposit3;
Console.WriteLine($"  Deposit (3-param) : Type={txType}, Amount={txAmount:C}, Desc={txDesc}");

// 4-param Deconstruct defined in Transfer (extended version)
var transfer4 = new Transfer(3_000m, "SAV-001", "Salary allocation");
var (tfType, tfAmount, tfDesc, tfTarget) = transfer4;
Console.WriteLine($"  Transfer (4-param): {tfType}, {tfAmount:C} → {tfTarget}, Note: {tfDesc}");

// Tuple deconstruction (value tuple, no class needed)
var (acctId, acctOwner, acctBal) = (savings.AccountId, savings.Owner, savings.Balance);
Console.WriteLine($"  Tuple             : {acctId} | {acctOwner} | {acctBal:C}");

// ─────────────────────────────────────────────────────────────────────────────
// 9. NULL COALESCING — ??, ??=, ?.
// ─────────────────────────────────────────────────────────────────────────────
Section("9. NULL COALESCING");

// ?? — provide a default when left-hand side is null
string? rawDesc = null;
string  display = rawDesc ?? "No description provided";
Console.WriteLine($"  ?? operator          : '{display}'");

// ??= — assign only if the variable is currently null
string? cachedReport = null;
cachedReport ??= (savings as IAuditable)?.GenerateReport();
Console.WriteLine($"  ??= assignment       : {cachedReport ?? "N/A"}");

// ?. null-conditional chained with ?? null-coalescing
SavingsAccount? ghost = null;
decimal safeBalance   = ghost?.Balance ?? 0m;
Console.WriteLine($"  Null account ?.??    : {safeBalance:C}  (no NullReferenceException)");

// ?? inside constructor — Transaction.Description defaults via null-coalescing
var noDesc = new Deposit(100m);    // description omitted → null → "No description"
Console.WriteLine($"  Default desc (ctor)  : '{noDesc.Description}'");

// ─────────────────────────────────────────────────────────────────────────────
// 10. INTERFACES — Casting, multiple interfaces, explicit implementation
// ─────────────────────────────────────────────────────────────────────────────
Section("10. INTERFACES");

// Reference via interface type (polymorphism)
IAccount iAccount = savings;
Console.WriteLine($"  IAccount.Balance     : {iAccount.Balance:C}");

// Explicit interface implementation — Log() is only accessible via IAuditable cast
// savings.Log();   ← compile error: method not visible without interface cast
IAuditable iAuditSav = savings;
iAuditSav.Log();
Console.WriteLine($"  {iAuditSav.GenerateReport()}");

IAuditable iAuditBiz = business;
iAuditBiz.Log();
Console.WriteLine($"  {iAuditBiz.GenerateReport()}");

// Interface-based polymorphic collection (multiple interface types at runtime)
var allAccounts = new List<IAccount> { savings, checking, business };
Console.WriteLine("\n  All accounts via IAccount interface:");
foreach (var acc in allAccounts)
{
    Console.WriteLine($"    {acc.AccountId} | {acc.Owner,-15} | {acc.Balance:C}");

    // 'is' pattern — check and cast simultaneously
    if (acc is IAuditable auditable)
        Console.WriteLine($"      [Auditable] {auditable.GenerateReport()}");
}

// ─────────────────────────────────────────────────────────────────────────────
// 11. CUSTOM COMPARISONS — IComparable<T> drives List.Sort()
// ─────────────────────────────────────────────────────────────────────────────
Section("11. CUSTOM COMPARISONS (IComparable<T>)");

var unsorted = new List<Transaction>
{
    new Deposit   (500m,               "A"),
    new Withdrawal(2_000m,             "B"),
    new Deposit   (150m,               "C"),
    new Transfer  (10_000m, "SAV-001", "D"),
    new Deposit   (75m,                "E"),
};

unsorted.Sort();   // calls Transaction.CompareTo — sorts by Amount descending

Console.WriteLine("  Sorted by Amount descending (IComparable<Transaction>):");
foreach (var tx in unsorted)
    Console.WriteLine($"    {tx.TransactionType,-12} | {tx.Amount,12:C} | {tx.Description}");

// ─────────────────────────────────────────────────────────────────────────────
// 12. LITERAL NUMBER IMPROVEMENTS — digit separators, binary, hex literals
// ─────────────────────────────────────────────────────────────────────────────
Section("12. LITERAL NUMBER IMPROVEMENTS");

decimal maxTransferLimit = 1_000_000.00m;   // _ digit separator in decimal
decimal fdInsuranceLimit =   250_000.00m;
int     batchSize        =        10_000;
long    totalLedger      = 100_000_000L;

// Binary literals — readable permission flags
int readPerm  = 0b0001;
int writePerm = 0b0010;
int execPerm  = 0b0100;
int allPerms  = 0b0111;

// Hex literals with optional _ separators
int flagMask  = 0xFF_00;
int colorBlue = 0x00_00_FF;

Console.WriteLine($"  Max Transfer Limit : {maxTransferLimit:C}");
Console.WriteLine($"  FD Insurance Limit : {fdInsuranceLimit:C}");
Console.WriteLine($"  Batch Size         : {batchSize:N0}");
Console.WriteLine($"  Total Ledger       : {totalLedger:N0}");
Console.WriteLine($"  Permissions (bin)  : READ={readPerm:b4}  WRITE={writePerm:b4}  ALL={allPerms:b4}");
Console.WriteLine($"  Flag Mask (hex)    : 0x{flagMask:X4}");
Console.WriteLine($"  Blue (hex)         : 0x{colorBlue:X6}");
_ = execPerm; // suppress unused warning (value used in concept demonstration)

// ─────────────────────────────────────────────────────────────────────────────
// 13. FINAL STATEMENTS — full account printouts
// ─────────────────────────────────────────────────────────────────────────────
Section("13. ACCOUNT STATEMENTS");
savings.PrintStatement();
checking.PrintStatement();
business.PrintStatement();

Banner("SmartBank Demo Complete");

// ─────────────────────────────────────────────────────────────────────────────
// Local functions — helpers, named delegates, named event handlers
// ─────────────────────────────────────────────────────────────────────────────
static void Banner(string title)
{
    string bar = new('═', title.Length + 4);
    Console.WriteLine($"\n╔{bar}╗");
    Console.WriteLine($"║  {title}  ║");
    Console.WriteLine($"╚{bar}╝");
}

static void Section(string title)
{
    int pad = Math.Max(0, 58 - title.Length);
    Console.WriteLine($"\n── {title} {new string('─', pad)}");
}

// Named method stored and used as LogAction delegate
static void NamedAuditLog(string message) =>
    Console.WriteLine($"  [NAMED-DELEGATE] {message}");

// Named event handler method for BalanceChanged on checking account
static void OnCheckingBalanceChanged(object? sender, BalanceChangedEventArgs e) =>
    Console.WriteLine($"  [CHK EVENT] Balance updated → {e.NewBalance:C}");
