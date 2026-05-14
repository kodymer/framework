using CompanyName.AspNetCore.Mvc;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Samples.Banks.AccountManagement;
using Samples.Banks.MoneyTransfer;
using Samples.Banks.MoneyTransfers;
using Samples.Banks.Resources;

namespace Samples.Banks.Controllers
{
    [ApiController]
    [Route("api")]
    public class BankController : CompanyNameController
    {

        private readonly IAccountManagementAppService _accountManagementAppService;
        private readonly IMoneyTransferAppService _moneyTransferAppService;

        public BankController(
            IAccountManagementAppService accountManagementAppService,
            IMoneyTransferAppService moneyTransferAppService)
        {
            LocalizationResource = typeof(BanksResource);

            _accountManagementAppService = accountManagementAppService;
            _moneyTransferAppService = moneyTransferAppService;
        }

        [HttpGet("lang")]
        [Tags("Localization")]
        public async Task<IActionResult> ViewLanguage()
        {
            var literal = L["InsufficientBalanceError"];
            return Ok(literal);
        }

        [HttpPost("bank/accounts")]
        [Tags("Bank Accounts")]
        public async Task<IActionResult> CreateBankAccountAsync([FromBody] CreateBankAccountCommand command, [FromServices] IValidator<CreateBankAccountCommand> validator, CancellationToken cancellationToken = default)
        {

            try
            {
                var validationResult = await validator.ValidateAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.ToDictionary());
                }

                await _accountManagementAppService.CreateBankAccountAsync(command);
                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpGet("bank/accounts")]
        [Tags("Bank Accounts")]
        public async Task<IActionResult> GetBankAccountsAsync(Guid branchId, CancellationToken cancellationToken = default)
        {

            try
            {

                var bankAccounts = await _accountManagementAppService.GetAllBankAccountListAsync(branchId, cancellationToken);

                return Ok(bankAccounts);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPost("bank/transfers")]
        [Tags("Bank Tranfers")]
        public async Task<IActionResult> TransferAsync([FromBody] CreateBankTransferCommand command, [FromServices] IValidator<CreateBankTransferCommand> validator, CancellationToken cancellationToken = default)
        {

            try
            {
                var validationResult = await validator.ValidateAsync(command, cancellationToken);
                if (!validationResult.IsValid)
                {
                    return BadRequest(validationResult.ToDictionary());
                }

                await _moneyTransferAppService.MakeTransferAsync(command);

                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpGet("bank/transfers")]
        [Tags("Bank Tranfers")]
        public async Task<IActionResult> GetTransferHistoryAsync()
        {

            try
            {
                var bankTransferHistory = await _moneyTransferAppService.GetTransferHistoryAsync();

                return Ok(bankTransferHistory);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}
