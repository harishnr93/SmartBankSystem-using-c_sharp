# SmartBankSystem — How to Run

## Prerequisites

Make sure .NET 8 SDK is installed:

```bash
dotnet --version
# Expected: 8.x.x
```

If not installed:

```bash
sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0
```

---

## Running the Project

All commands below must be run from inside the `SmartBankSystem/` folder.

```bash
cd SmartBankSystem
```

### Option 1 — Build + Run in one step (recommended)

```bash
dotnet run
```

### Option 2 — Build first, then run the binary

```bash
dotnet build
./bin/Debug/net8.0/SmartBankSystem
```

### Option 3 — Run with VS Code (`F5`)

1. Open the `SmartBankSystem/` folder in VS Code
2. Press `F5` — VS Code auto-generates `launch.json` if missing
3. Output appears in the **Debug Console** panel

---

## What the Output Looks Like

The program runs 13 labelled sections, one per C# topic:

```
╔══════════════════════════════════════╗
║  SmartBank System — C# Feature Demo  ║
╚══════════════════════════════════════╝

── 1. ACCOUNT CREATION ──────────────────────────────────
  Savings  | SAV-001 | Alice Johnson   | $1,000.00
  Checking | CHK-001 | Bob Smith       | $2,500.00
  Business | BUS-001 | Carol White     | $50,000.00

── 2. DELEGATES & LOGGER (MULTICAST / CHAINING) ─────────
  [LOG 10:00:00] SmartBank initialised — 3 loggers active
  [AUDIT] SmartBank initialised — 3 loggers active
  [NAMED-DELEGATE] SmartBank initialised — 3 loggers active
  [LOG 10:00:00] After removal — 2 loggers active
  ...

── 5. TRANSACTIONS ───────────────────────────────────────
  [OK] Deposited $500.00 → SAV-001
  [REJECTED] Deposit -$50.00 — failed rule validation.
  ...

── 13. ACCOUNT STATEMENTS ────────────────────────────────
  Account : SAV-001 | SavingsAccount
  Balance : $17,365.00
  Transactions (5): ...

╔═══════════════════════════╗
║  SmartBank Demo Complete  ║
╚═══════════════════════════╝
```

---

## Project Structure

```
SmartBankSystem/
├── Program.cs                       # Entry point — 13-section demo
├── SmartBankSystem.csproj           # .NET 8 project file
│
├── Models/
│   ├── Transaction.cs               # Abstract base: IComparable, Deconstruct, ToString
│   ├── Deposit.cs                   # Inherits Transaction
│   ├── Withdrawal.cs                # Inherits Transaction
│   ├── Transfer.cs                  # Inherits Transaction, adds ToAccountId
│   └── BalanceChangedEventArgs.cs   # Custom EventArgs for balance events
│
├── Accounts/
│   ├── IAccount.cs                  # Interface: Deposit, Withdraw, PrintStatement
│   ├── IAuditable.cs                # Interface: Log, GenerateReport
│   ├── AccountBase.cs               # Abstract base: indexers, event, delegate, rules
│   ├── SavingsAccount.cs            # +Interest, explicit IAuditable
│   ├── CheckingAccount.cs           # +Overdraft, TransferTo
│   └── BusinessAccount.cs           # +DailyLimit, explicit IAuditable
│
└── Engine/
    ├── Logger.cs                    # Multicast delegate (LogAction), chaining
    ├── RuleEngine.cs                # Lambda rules, anonymous delegate, composite
    └── TransactionClassifier.cs     # Pattern matching, switch expressions
```

---

## C# Topics — Where to Find Each One

| Topic | File | Key Line / Method |
|---|---|---|
| Abstract class | `Models/Transaction.cs` | `public abstract class Transaction` |
| Inheritance | `Models/Deposit.cs` | `: Transaction` |
| Interface definition | `Accounts/IAccount.cs` | `public interface IAccount` |
| Explicit interface impl. | `Accounts/SavingsAccount.cs` | `void IAuditable.Log()` |
| Class indexer (`int`) | `Accounts/AccountBase.cs` | `public Transaction this[int index]` |
| Index (`^n`) indexer | `Accounts/AccountBase.cs` | `public Transaction this[Index index]` |
| Range (`..`) indexer | `Accounts/AccountBase.cs` | `public IReadOnlyList<Transaction> this[Range range]` |
| Indexers in use | `Program.cs` §6 | `savings[0]`, `savings[^1]`, `savings[0..2]` |
| Custom delegate type | `Accounts/AccountBase.cs` | `public delegate bool TransactionRule(...)` |
| Multicast delegate | `Engine/Logger.cs` | `_loggers += logger` / `-= logger` |
| Anonymous delegate | `Program.cs` §2 | `delegate(Transaction t) { return ... }` |
| Lambda as delegate | `Program.cs` §4 | `savings.AddRule(t => t.Amount > 0)` |
| Lambda closure | `Accounts/AccountBase.cs` | `AddRule` — captures `existing` in closure |
| Delegate factory (lambda) | `Engine/RuleEngine.cs` | `MinAmount`, `MaxAmount`, `RequireDescription` |
| Event declaration | `Accounts/AccountBase.cs` | `event EventHandler<...>? BalanceChanged` |
| Custom EventArgs | `Models/BalanceChangedEventArgs.cs` | full class |
| Lambda event handler | `Program.cs` §3 | `savings.BalanceChanged += (_, e) => ...` |
| Named event handler | `Program.cs` §3 | `OnCheckingBalanceChanged` |
| Event chaining | `Program.cs` §3 | two `+=` on same event |
| Null-safe event raise | `Accounts/AccountBase.cs` | `BalanceChanged?.Invoke(...)` |
| Switch expression | `Engine/TransactionClassifier.cs` | `Classify()`, `GetRiskLevel()` |
| Type pattern | `Program.cs` §7 | `Deposit d =>` |
| Property pattern | `Engine/TransactionClassifier.cs` | `Deposit { Amount: > 10_000 }` |
| Relational pattern | `Engine/TransactionClassifier.cs` | `>= 1_000 and < 10_000` |
| `when` guard | `Engine/TransactionClassifier.cs` | `Deposit d when d.Amount > 50_000` |
| `??` null-coalescing | `Program.cs` §9 | `rawDesc ?? "No description"` |
| `??=` null-coalescing assign | `Program.cs` §9 | `cachedReport ??= ...` |
| `?.` null-conditional | `Program.cs` §9 | `ghost?.Balance ?? 0m` |
| Class deconstruction | `Models/Transaction.cs` | `void Deconstruct(out string type, ...)` |
| Extended deconstruction | `Models/Transfer.cs` | 4-param `Deconstruct` |
| Tuple deconstruction | `Program.cs` §8 | `var (id, owner, bal) = (...)` |
| `IComparable<T>` | `Models/Transaction.cs` | `CompareTo` — sort by amount desc |
| `List.Sort()` | `Program.cs` §11 | `unsorted.Sort()` |
| `ToString` override | `Models/Transaction.cs` | `public override string ToString()` |
| `Equals` override | `Models/Transaction.cs` | `public override bool Equals(...)` |
| `GetHashCode` override | `Models/Transaction.cs` | `public override int GetHashCode()` |
| Digit separators | `Program.cs` §12 | `1_000_000.00m`, `100_000_000L` |
| Binary literals | `Program.cs` §12 | `0b0001`, `0b0111` |
| Hex literals | `Program.cs` §12 | `0xFF_00`, `0x00_00_FF` |

---

## Useful `dotnet` Commands

| Command | Purpose |
|---|---|
| `dotnet run` | Build and run |
| `dotnet build` | Compile only |
| `dotnet clean` | Remove `bin/` and `obj/` |
| `dotnet run > output.txt` | Save output to a file |

---

## Checking Individual Sections

Each section is labelled in the output with `── N. SECTION NAME ──`.
To jump straight to a concept, find its section number in the table above and match it to the console output.

To save and review the full output:

```bash
dotnet run > output.txt
cat output.txt
```
