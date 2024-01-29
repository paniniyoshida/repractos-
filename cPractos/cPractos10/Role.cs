namespace CarShowroomApp
{
    enum Role
    {
        Administrator,
        Cashier,
        PersonnelManager,
        WarehouseManager,
        Accountant
    }

    enum TransactionType
    {
        Income,
        Expense
    }

    class FinancialTransaction
    {
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }

        public FinancialTransaction(TransactionType type, decimal amount)
        {
            Type = type;
            Amount = amount;
        }
    }


}