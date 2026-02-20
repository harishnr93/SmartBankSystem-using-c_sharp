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
