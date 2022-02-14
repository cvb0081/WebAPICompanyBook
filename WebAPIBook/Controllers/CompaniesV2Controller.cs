using Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIBook.Controllers
{
    [ApiExplorerSettings(GroupName = "v2")]
    [Route("api/companiesv2")]
    [ApiController]
    public class CompaniesV2Controller : ControllerBase
    {
        private readonly IRepositoryManager _repository;

        public CompaniesV2Controller(IRepositoryManager repository)
        {
            _repository=repository;
        }

        [HttpGet(Name ="GetCompaniesv2")]
        public async Task<IActionResult> GetCompaniesV2()
        {
            var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges:
                false);
            return Ok(companies);
        }
    }
}
