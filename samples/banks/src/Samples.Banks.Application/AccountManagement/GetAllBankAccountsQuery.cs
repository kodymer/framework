namespace Samples.Banks.AccountManagement
{
    public record class GetAllBankAccountsQuery
    {

        public Guid BranchId { get; set; } = Guid.NewGuid();

        public int? Page { get; set; } = 1;

        public int? PageSize { get; set; } = 10;
    }
}