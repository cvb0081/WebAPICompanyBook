using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPIBook.ActionFilters;
using WebAPIBook.Utility;

namespace WebAPIBook.Controllers
{
    [Route("api/companies/{companyID}/employees")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly IDataShaper<EmployeeDto> _dataShaper;
        //private readonly EmployeeLinks _employeeLinks;

        public EmployeesController(IMapper mapper, ILoggerManager loggerManager,
            IRepositoryManager repository /*,EmployeeLinks employeeLinks*/
            , IDataShaper<EmployeeDto> dataShaper)
        {
            _mapper=mapper;
            _logger=loggerManager;
            _repository=repository;
            _dataShaper=dataShaper;
            //_employeeLinks=employeeLinks;
        }

        [HttpGet]
        [HttpHead]
        [ServiceFilter(typeof(ValidateMediaTypeAttribute))]
        public async Task<IActionResult> GetEmployeesForCompany(Guid companyId,
            [FromQuery] EmployeeParameters employeeParameters)
        {
            if (!employeeParameters.ValidAgeRange)
                return BadRequest("Max age can't be less than min age.");

            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id:{companyId} does not exist in the DB");
                return NotFound();
            }

            var employeesFromDb = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges: false);

            Response.Headers.Add("X-Pagination",
                JsonConvert.SerializeObject(employeesFromDb.MetaData));  

            var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesFromDb);

            return Ok(_dataShaper.ShapeData(employeesDto, employeeParameters.Fields));
            //var links = _employeeLinks.TryGenerateLinks(employeesDto,
            //    employeeParameters.Fields, companyId, HttpContext);

            //return links.HasLinks ? Ok(links.LinkedEntities) : Ok(links.ShapedEntities);
        }

        [HttpGet("{id}", Name = "GetEmployeeForCompany")]
        public async Task<IActionResult> GetEmployeeForCompany(Guid companyId, Guid id)
        {
            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id:{companyId} does not exist in the DB");
                return NotFound();
            }
            var employeeFromDb = await _repository.Employee.GetEmployeeAsync(companyId, id,
                trackChanges: false);

            var employeeDto = _mapper.Map<EmployeeDto>(employeeFromDb);

            return Ok(employeeDto);
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateEmployeeForCompany(Guid companyId,
            [FromBody] EmployeeForCreationDto employee)
        {
            //if (employee == null)
            //{
            //    _logger.LogError("EmployeeForCreationDto object sent from client is null.");
            //    return BadRequest("EmployeeForCreationDto object is null");
            //}
            //if (!ModelState.IsValid)
            //{
            //    _logger.LogError("Invalid model state for the EmployeeForCreationDto object");
            //    return UnprocessableEntity(ModelState);
            //}

            var company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            if (company == null)
            {
                _logger.LogInfo($"Company with id:{companyId} doesn't exist in the dB.");
                return NotFound();
            }

            var employeeEntity = _mapper.Map<Employee>(employee);
            _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
            await _repository.SaveAsync();
            var employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

            return CreatedAtRoute("GetEmployeeForCompany", new { companyId, id = employeeToReturn.Id }, employeeToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteEmployeeForCompany(Guid companyId, Guid id)
        {
            //var company = _repository.Company.GetCompanyAsync(companyId, trackChanges: false);
            //if (company==null)
            //{
            //    _logger.LogInfo($"Company with id:{companyId} doesn't exist" +
            //        $"in the database.");
            //    return NotFound();
            //}
            //var employeeForCompany =await _repository.Employee.GetEmployeeAsync(companyId, id,
            //    trackChanges: false);
            //if (employeeForCompany == null)
            //{
            //    _logger.LogInfo($"Employee with id:{id} doesn't exist in the database.");
            //    return NotFound();
            //}
            var employeeForCompany = HttpContext.Items["employee"] as Employee;

            _repository.Employee.DeleteEmployee(employeeForCompany);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPost("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateEmployeeForCompany(Guid companyId, Guid id,
            [FromBody] EmployeeForUpdateDto employee)
        {
            //if (employee == null)
            //{
            //    _logger.LogError("EmployeeForUpdateDto object sent from client is null.");
            //    return BadRequest("EmployeeForUpdateDto object is null.");
            //}
            //if (!ModelState.IsValid)
            //{
            //    _logger.LogError("Invalid model state for the EmployeeForUpdateDto object");
            //    return UnprocessableEntity(ModelState);
            //}

            //var company = await _repository.Company.GetCompanyAsync(companyId,trackChanges: false);
            //if (company==null)
            //{
            //    _logger.LogInfo($"Company with id:{companyId} doesn't exist in the database.");
            //    return NotFound();
            //}

            //var employeeEntity = await _repository.Employee.GetEmployeeAsync(companyId,id,trackChanges: true); 
            //if(employeeEntity == null)
            //{
            //    _logger.LogInfo($"Employee with id:{id} doesn't exist in the databse.");
            //    return NotFound();
            //}
            var employeeEntity = HttpContext.Items["employee"] as Employee;

            _mapper.Map(employee,employeeEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public IActionResult UpdateCompany(Guid id,[FromBody] CompanyForUpdateDto company)
        {
            if(company == null)
            {
                _logger.LogError("CompanyForUpdateDto object sent from client is null.");
                return BadRequest();
            }

            var companyEntity = _repository.Company.GetCompanyAsync(id,trackChanges: true);
            if(companyEntity == null)
            {
                _logger.LogInfo($"Company with id:{id} doesn't exist in the database.");
                return NotFound();
            }

            _mapper.Map(company, companyEntity);
            _repository.SaveAsync();

            return NoContent();
        }

        //[HttpPatch("{id}")]
        //[ServiceFilter(typeof(ValidateEmployeeForCompanyExistsAttribute))]
        //public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId,Guid id,
        //    [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        //{
        //    if(patchDoc == null)
        //    {
        //        _logger.LogError("patchDoc object sent from client is null.");
        //        return BadRequest("patchDoc object is null");
        //    }

        //    //var company = _repository.Company.GetCompanyAsync(companyId,trackChanges: false);
        //    //if(company == null)
        //    //{
        //    //    _logger.LogInfo($"Company with id:{companyId} doesn't exist in the database.");
        //    //    return NotFound();
        //    //}
        //    //var employeeEntity = _repository.Employee.GetEmployeesAsync(companyId,trackChanges:true);
        //    //if(employeeEntity == null)
        //    //{
        //    //    _logger.LogInfo($"Employee with id:{id} doesn't exist in the database.");
        //    //    return NotFound();
        //    //}

        //    var employeeEntity = HttpContext.Items["employee"] as Employee;

        //    var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);
        //    patchDoc.ApplyTo(employeeToPatch);
        //    _mapper.Map(employeeToPatch,employeeEntity);
        //    _repository.SaveAsync();
        //    return NoContent();
        //}
    }
}
