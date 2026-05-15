using Microsoft.Extensions.Logging;
using Samples.Banks.Accounts;


namespace Samples.Banks
{
    public static partial class BankAppServiceLog
    {
        [LoggerMessage(EventId = BanksLogEventConsts.GetBankTransferHistory, Level = LogLevel.Information ,Message = "Getting all bank transfers.")]
        public static partial void GettingAllBankTransferHistory(this ILogger logger);


        [LoggerMessage(EventId = BanksLogEventConsts.GetBankTransferHistory, Level = LogLevel.Debug, Message = "{Count} bank transfers have been obtained.")]
        public static partial void BankTransferHistoryCountObtained(this ILogger logger, int count);


        [LoggerMessage(EventId = BanksLogEventConsts.GetBankTransferHistory, Level = LogLevel.Error, Message = "Could not get the bank transfers.")]
        public static partial void CouldNotGetBankTransfers(this ILogger logger, Exception ex);


        [LoggerMessage(EventId = BanksLogEventConsts.GetBankAccounts, Level = LogLevel.Information, Message = "Getting all bank accounts.")]
        public static partial void GettingAllBankAccounts(this ILogger logger);


        [LoggerMessage(EventId = BanksLogEventConsts.GetBankAccounts, Level = LogLevel.Debug, Message = "{Count} bank accounts have been obtained.")]
        public static partial void BankAccountCountObtained(this ILogger logger, int count);


        [LoggerMessage(EventId = BanksLogEventConsts.GetBankAccounts, Level = LogLevel.Error, Message = "Could not get the bank accounts.")]
        public static partial void CouldNotGetBankAccounts(this ILogger logger, Exception ex);


        [LoggerMessage(EventId = BanksLogEventConsts.GenerateNewBankAccount, Level = LogLevel.Information, Message = "Creating a new bank account with balance {Balance}.")]
        public static partial void GeneratingNewBankAccount(this ILogger logger, decimal balance);


        [LoggerMessage(EventId = BanksLogEventConsts.GenerateNewBankAccount, Level = LogLevel.Debug, Message = "Bank account: {Data}")]
        public static partial void AddingBankAccountDetail(this ILogger logger, BankAccount data);


        [LoggerMessage(EventId = BanksLogEventConsts.GenerateNewBankAccount, Level = LogLevel.Information, Message = "Bank account created!")]
        public static partial void BankAccountCreated(this ILogger logger);


        [LoggerMessage(EventId = BanksLogEventConsts.GenerateNewBankAccount, Level = LogLevel.Error, Message = "Could not create the bank account.")]
        public static partial void CouldNotCreateBankAccount(this ILogger logger, Exception ex);


        [LoggerMessage(EventId = BanksLogEventConsts.TransfersBetweenBankAccounts, Level = LogLevel.Information, Message = "Making a transfer between {FromId} and {ToId} bank accounts by €{Amount}.")]
        public static partial void MakingBankTransfersBetweenBankAccounts(this ILogger logger, Guid fromId, Guid toId, decimal amount);


        [LoggerMessage(EventId = BanksLogEventConsts.TransfersBetweenBankAccounts, Level = LogLevel.Debug, Message = "Getting bank account from by ID: {Id}.")]
        public static partial void GettingBankAccountFrom(this ILogger logger, Guid id);


        [LoggerMessage(EventId = BanksLogEventConsts.TransfersBetweenBankAccounts, Level = LogLevel.Debug, Message = "Getting bank account to by ID: {Id}.")]
        public static partial void GettingBankAccountTo(this ILogger logger, Guid id);


        [LoggerMessage(EventId = BanksLogEventConsts.TransfersBetweenBankAccounts, Level = LogLevel.Debug, Message = "Making transfer by €{Amount}.")]
        public static partial void AddingTransferDetail(this ILogger logger, decimal amount);


        [LoggerMessage(EventId = BanksLogEventConsts.TransfersBetweenBankAccounts, Level = LogLevel.Information, Message = "Successful transfer correctly.")]
        public static partial void BankTransferCreated(this ILogger logger);


        [LoggerMessage(EventId = BanksLogEventConsts.TransfersBetweenBankAccounts, Level = LogLevel.Error, Message = "An unexpected error occurred during the bank transfer.")]
        public static partial void CouldNotCreateBankTransfer(this ILogger logger, Exception ex);


        [LoggerMessage(EventId = BanksLogEventConsts.TransfersBetweenBankAccounts, Level = LogLevel.Error, Message = "Could not found entity.")]
        public static partial void EntityNotFound(this ILogger logger, Exception ex);
    }
}

