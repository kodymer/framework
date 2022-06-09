using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Vesta.AspNetCore.Mvc;
using Vesta.Banks.Bank;
using Vesta.Banks.Bank.Dtos;
using Vesta.Banks.Domain;

namespace Vesta.Banks.Controllers
{
    [ApiController]
    [Route("api/bank")]
    public class BankController : VestaController
    {

        private readonly IBankAppService _bankAppService;

        public BankController(IBankAppService bankAppService)
        {
            _bankAppService = bankAppService;
        }

        [HttpPost("account")]
        public async Task<IActionResult> CreateBankAccountAsync([FromBody] CreateBankAccountInput input)
        {

            try
            {

                await _bankAppService.CreateBankAccountAsync(input);

                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpGet("accounts")]
        public async Task<IActionResult> GetBankAccountsAsync()
        {

            try
            {

                var bankAccounts = await _bankAppService.GetAllBankAccountList();

                return Ok(bankAccounts);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpPost("transfer")]
        public async Task<IActionResult> TransferAsync([FromBody] BankTransferInput input)
        {

            try
            {
                await _bankAppService.MakeTransferAsync(input);

                return Ok();
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }

        [HttpGet("transfers")]
        public async Task<IActionResult> GetTransferHistoryAsync()
        {

            try
            {
                var bankTransferHistory = await _bankAppService.GetAllBankTransferList();

                return Ok(bankTransferHistory);
            }
            catch (Exception)
            {

                return BadRequest();
            }
        }


    }
}
