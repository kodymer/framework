using CompanyName.Cqrs.Abstractions;
using CompanyName.Ddd.Domain.Common.Pagination;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Samples.Banks.AccountManagement
{
    public record class GetAllBankAccountsQuery :
        IQuery<Result<PagedResult<BankAccountDto>>>
    {

        public Guid BranchId { get; set; } = Guid.NewGuid();

        public int? Page { get; set; } = 1;

        public int? PageSize { get; set; } = 10;
    }
}