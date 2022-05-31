﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Vesta.AspNetCore.Mvc;
using Vesta.ProjectName.Bank;
using Vesta.ProjectName.Bank.Dtos;
using Vesta.ProjectName.Domain;

namespace Vesta.ProjectName.Controllers
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


    }
}
