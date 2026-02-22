namespace SmartBankSystem.Accounts;

// Demonstrates: Multiple interfaces, explicit interface implementation target
public interface IAuditable
{
    void   Log();
    string GenerateReport();
}
