namespace SmartBankSystem.Engine;

// ── Delegate definition ───────────────────────────────────────────────────
// Demonstrates: Custom delegate, multicast (composite) delegates, delegate chaining
public delegate void LogAction(string message);

public static class Logger
{
    // Multicast delegate field — multiple LogAction functions can be chained with +=
    private static LogAction? _loggers;

    // Delegate chaining: += adds a subscriber; -= removes one
    public static void AddLogger(LogAction    logger) => _loggers += logger;
    public static void RemoveLogger(LogAction logger) => _loggers -= logger;

    // Invoke all chained loggers in registration order
    public static void Log(string message) => _loggers?.Invoke(message);

    // ── Pre-built logger implementations ─────────────────────────────────

    // Named method stored as delegate value
    public static readonly LogAction ConsoleLogger =
        msg => Console.WriteLine($"  [LOG {DateTime.Now:HH:mm:ss}] {msg}");

    // Simulated file logger (writes to console with FILE tag for demo)
    public static readonly LogAction FileLogger =
        msg => Console.WriteLine($"  [FILE] {DateTime.Now:HH:mm:ss} | {msg}");

    // Factory: creates a logger with a custom prefix (closure over 'prefix')
    public static LogAction CreatePrefixLogger(string prefix) =>
        msg => Console.WriteLine($"  [{prefix}] {msg}");
}
