# C# Programming
---

## VS Code Setup for C# Development

### Prerequisites

1. **Install .NET SDK**

   Download and install the [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0 or later recommended).

   On Ubuntu/Debian (WSL or native):
   ```bash
   sudo apt-get update && sudo apt-get install -y dotnet-sdk-8.0
   ```

   Verify installation:
   ```bash
   dotnet --version
   ```

2. **Install VS Code**

   Download from [https://code.visualstudio.com](https://code.visualstudio.com) if not already installed.

---

### Required VS Code Extensions

Install the following extensions from the VS Code Extensions panel (`Ctrl+Shift+X`):

| Extension | Publisher | Purpose |
|-----------|-----------|---------|
| **C# Dev Kit** | Microsoft | Full C# language support, IntelliSense, debugging |
| **C#** | Microsoft | Core C# language extension (installed automatically with C# Dev Kit) |
| **.NET Install Tool** | Microsoft | Manages .NET runtime for extensions |

To install via terminal:
```bash
code --install-extension ms-dotnettools.csdevkit
```

---

### Creating a New C# Project

```bash
# Console application
dotnet new console -n MyProject
cd MyProject

# Open in VS Code
code .
```

---

### Running and Debugging

#### Run from Terminal
```bash
dotnet run
```

#### Run with Debugging in VS Code

1. Open the project folder in VS Code
2. Press `F5` to start debugging (VS Code will auto-generate `launch.json` if missing)
3. Or use **Run > Start Debugging** from the menu

#### Build Only
```bash
dotnet build
```

---

### Recommended VS Code Settings

Add to your workspace `.vscode/settings.json`:

```json
{
  "editor.formatOnSave": true,
  "[csharp]": {
    "editor.defaultFormatter": "ms-dotnettools.csharp"
  },
  "dotnet.defaultSolution": "disable"
}
```

---

### Project Structure (typical console app)

```
MyProject/
├── .vscode/
│   ├── launch.json      # Debug configuration
│   └── tasks.json       # Build tasks
├── MyProject.csproj     # Project file
├── Program.cs           # Entry point
└── bin/                 # Build output (generated)
```

---

### Useful `dotnet` CLI Commands

| Command | Description |
|---------|-------------|
| `dotnet new console -n <name>` | Create a new console project |
| `dotnet run` | Build and run the project |
| `dotnet build` | Compile the project |
| `dotnet test` | Run unit tests |
| `dotnet add package <pkg>` | Add a NuGet package |
| `dotnet clean` | Remove build artifacts |

---

### Keyboard Shortcuts (VS Code)

| Shortcut | Action |
|----------|--------|
| `F5` | Start debugging |
| `Ctrl+F5` | Run without debugging |
| `F9` | Toggle breakpoint |
| `Ctrl+Shift+B` | Run build task |
| `Ctrl+.` | Quick fix / suggestions |
| `F12` | Go to definition |
| `Shift+F12` | Find all references |

---

## Projects

### SmartBankSystem — Modular Event-Driven Banking Engine

> A single project designed to exercise **every major C# language feature** in a realistic, enterprise-style context.

**Folder:** `SmartBankSystem/`

```
SmartBankSystem/
├── Program.cs                         # Entry point — 13-section feature demo
├── Models/
│   ├── Transaction.cs                 # Abstract base: IComparable, Deconstruct, ToString/Equals/GetHashCode
│   ├── Deposit.cs                     # Concrete transaction type
│   ├── Withdrawal.cs                  # Concrete transaction type
│   ├── Transfer.cs                    # Extended Deconstruct (4-param)
│   └── BalanceChangedEventArgs.cs     # Custom EventArgs for balance events
├── Accounts/
│   ├── IAccount.cs                    # Interface: Deposit, Withdraw, PrintStatement
│   ├── IAuditable.cs                  # Interface: Log, GenerateReport
│   ├── AccountBase.cs                 # Abstract base: delegate, int/Index/Range indexers, event
│   ├── SavingsAccount.cs              # Explicit IAuditable implementation, interest
│   ├── CheckingAccount.cs             # Overdraft logic, inter-account transfer
│   └── BusinessAccount.cs             # Daily limit enforcement, explicit IAuditable
└── Engine/
    ├── Logger.cs                      # Multicast delegate (LogAction), delegate chaining
    ├── RuleEngine.cs                  # Lambda rules, anonymous delegates, composite delegate
    └── TransactionClassifier.cs       # Structural pattern matching, switch expressions
```

#### Run

```bash
cd SmartBankSystem
dotnet run
```

---

#### C# Topics Covered

##### C# Basics
| Topic | Where |
|-------|-------|
| Variables & Data Types | `AccountBase`, `Transaction` fields |
| Type Casting | `acc is IAuditable auditable` |
| Operators & Math | Balance arithmetic, interest calculation |
| Strings & Interpolation | Throughout `ToString()`, `Console.WriteLine` |
| Booleans & If/Else | Rule validation (`ApplyRules`) |
| Switch | `TransactionClassifier` switch expressions |
| Loops & Arrays | Statement printing, sorting |
| Enums | `DayOfWeek` in `RuleEngine.NotOnWeekend()` |
| Exceptions | Graceful rejection instead of throwing (extension point) |

##### C# Methods & OOP
| Topic | Where |
|-------|-------|
| Methods & Parameters | All account operations |
| Method Overloading | `Deconstruct` (3-param in `Transaction`, 4-param in `Transfer`) |
| Classes / Objects | `Transaction`, `SavingsAccount`, etc. |
| Constructors | Every class; `SavingsAccount` passes to `base(...)` |
| Access Modifiers | `private`, `protected`, `internal`, `public` used throughout |
| Properties | `Balance`, `Owner`, `InterestRate` — get-only |
| Inheritance | `SavingsAccount : AccountBase`, `Deposit : Transaction` |
| Polymorphism | `List<IAccount>` holds all three account types |
| Abstraction | `abstract class AccountBase`, `abstract Transaction` |
| Interfaces | `IAccount`, `IAuditable` — defined, implemented, cast |
| Explicit Interfaces | `void IAuditable.Log()` in `SavingsAccount`, `BusinessAccount` |

##### Advanced Language Features
| Topic | Where |
|-------|-------|
| Class Indexers | `AccountBase`: `this[int]`, `this[Index]`, `this[Range]` |
| Indexes & Ranges (C# 8+) | `savings[0]`, `savings[^1]`, `savings[0..2]` in `Program.cs` |
| Null-Coalescing `??` | `Transaction` constructor, `Program.cs` §9 |
| Null-Coalescing Assignment `??=` | `cachedReport ??= ...` in `Program.cs` §9 |
| Null-Conditional `?.` | `ghost?.Balance ?? 0m`, `BalanceChanged?.Invoke(...)` |
| Deconstruction — Classes | `var (type, amount, desc) = transaction` |
| Deconstruction — Tuples | `var (id, owner, bal) = (...)` |
| Literal Number Improvements | `1_000_000m`, `0b0001`, `0xFF_00` in `Program.cs` §12 |

##### Structural Pattern Matching
| Topic | Where |
|-------|-------|
| Type Patterns | `transaction is Deposit d` |
| Property Patterns | `Deposit { Amount: > 10_000 }` |
| Relational Patterns | `>= 1_000 and < 10_000` in `GetAmountBand` |
| Switch Expressions | `Classify()`, `GetRiskLevel()`, `GetAmountBand()` |
| `when` Guards | `Deposit d when d.Amount > 50_000` |
| Positional Patterns | `var (type, amount, desc) = transaction` (via `Deconstruct`) |

##### Delegates
| Topic | Where |
|-------|-------|
| Custom Delegate Type | `delegate bool TransactionRule(...)` in `AccountBase.cs` |
| Named Delegate | `NamedAuditLog` stored as `LogAction` in `Program.cs` |
| Anonymous Delegate | `delegate(Transaction t) { return t.Amount > 0; }` in `Program.cs` §2 |
| Composite Delegate | `Logger._loggers += logger` (multicast) |
| Delegate Chaining | `AddRule` closure chain; `Logger` += / -= |
| Delegate as Parameter | `SetRule(TransactionRule rule)` |

##### Events
| Topic | Where |
|-------|-------|
| Event Declaration | `event EventHandler<BalanceChangedEventArgs>? BalanceChanged` |
| Custom EventArgs | `BalanceChangedEventArgs` (previous/new balance + transaction) |
| Lambda Event Handler | `savings.BalanceChanged += (_, e) => ...` |
| Named Event Handler | `checking.BalanceChanged += OnCheckingBalanceChanged` |
| Event Chaining | Two subscribers on `savings.BalanceChanged` |
| Null-safe Raise | `BalanceChanged?.Invoke(this, args)` |

##### Lambda Expressions
| Topic | Where |
|-------|-------|
| Lambda as Delegate | `t => t.Amount > 0` passed to `AddRule` |
| Inline Lambda | `savings.AddRule(t => t.Amount <= 1_000_000m)` |
| Closure | `AddRule` captures `existing` rule; `CreatePrefixLogger` captures `prefix` |
| Lambda Factory | `RuleEngine.MinAmount(decimal)` returns a `TransactionRule` lambda |

##### Interfaces
| Topic | Where |
|-------|-------|
| Interface Definition | `IAccount`, `IAuditable` |
| Implementation | `SavingsAccount`, `CheckingAccount`, `BusinessAccount` |
| Casting | `IAccount iAccount = savings` |
| Multiple Interfaces | `SavingsAccount : AccountBase, IAuditable` |
| Explicit Implementation | `void IAuditable.Log()` — only accessible via interface reference |
| Interface as Collection Type | `List<IAccount> allAccounts` |
| `is` Pattern Check | `if (acc is IAuditable auditable)` |
