using Dapper;
using Samples.Banks.Transfers;

namespace Samples.Banks.Dapper.Repositories
{
    public class BankTransferRepository : Repository<BankTransfer, BankTransferId>, IBankTransferDapperRepository
    {
        public BankTransferRepository(BanksDatabase database)
            : base(database)
        {

        }

        public async Task AddAsync(BankTransfer bankTransfer, CancellationToken cancellationToken = default)
        {

            //var insertSql = @$"
            //    INSERT INTO {BankTransfer.TableName} 
            //        (
            //         {nameof(BankTransfer.BankAccountFromNumber)}, 
            //         {nameof(BankTransfer.BankAccountToNumber)},
            //         {nameof(BankTransfer.Amount)},
            //         {nameof(BankTransfer.CreationTime)}
            //        )
            //    VALUES 
            //        (@BankAccountFromNumber, @BankAccountToNumber, @Amount, @CreationTime)";

            //await Database.ExecuteAsync(insertSql, bankTransfer);

            await GetTable().InsertAsync(bankTransfer);
        }

        public Task<IEnumerable<BankTransfer>> ListAsync(CancellationToken cancellationToken = default)
        {
            var sqlBuilder = new SqlBuilder();
            sqlBuilder.Select(nameof(BankTransfer.BankAccountFromNumber));
            sqlBuilder.Select(nameof(BankTransfer.BankAccountToNumber));
            sqlBuilder.Select(nameof(BankTransfer.Amount));
            sqlBuilder.Select(nameof(BankTransfer.CreationTime));

            var selectTemplate = sqlBuilder.AddTemplate(@$"
                  SELECT 
                     /**select**/ 
                  FROM 
                    {BankTransfer.TableName}");

            return Database.QueryAsync<BankTransfer>(selectTemplate.RawSql);
        }
    }
}
