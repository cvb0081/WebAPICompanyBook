using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPIBook.ActionFilters;
using WebAPIBook.ModelBinders;

namespace WebAPIBook
{
    [ApiExplorerSettings(GroupName ="v1")]
    [Route("api/companies")]
    [ApiController]
    //[ResponseCache(CacheProfileName ="120SecondsDuration")]
    public class CompaniesController : ControllerBase
    {
        private readonly IRepositoryManager _repository;
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public CompaniesController(ILoggerManager logger, IRepositoryManager repository, IMapper mapper)
        {
            _repository = repository;
            _logger = logger;
            _mapper=mapper;
        }

        [HttpOptions]
        public IActionResult GetCompaniesOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTIONS, POST");

            return Ok();
        }

        /// <summary>
        /// Gets list of all companies
        /// </summary>
        /// <returns>Companies List</returns>
        [HttpGet(Name ="GetCompanies"),Authorize(Roles ="Manager,Administrator")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await _repository.Company.GetAllCompaniesAsync(trackChanges: false);

            var companiesDto = _mapper.Map<IEnumerable<CompanyDto>>(companies);
            
            return Ok(companiesDto);
        }

        [HttpGet("{id}",Name ="CompanyById")]
        //[ResponseCache(Duration = 60)]
        public async Task<IActionResult> GetCompany(Guid id)
        {
            var companies = await _repository.Company.GetCompanyAsync(id,trackChanges: false);

            if(companies == null)
            {
                _logger.LogInfo($"Company with id:{id} does not exist in DB");
                return NotFound();
            }
            else
            {
                var companyDto = _mapper.Map<CompanyDto>(companies);
                return Ok(companyDto);
            }
        }

        /// <summary>
        /// Creates a newly created company
        /// </summary>
        /// <param name="company"></param>
        /// <returns>A newly created company</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null</response>
        /// <response code="422">If the model is invalid</response>

        [HttpPost(Name ="CreateCompany")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        [ProducesResponseType(422)]
        public async Task<IActionResult> CreateCompany([FromBody]CompanyForCreationDto company)
        {
            //if(company == null)
            //{
            //    _logger.LogError("CompanyForCreationDto object sent from client is null.");
            //    return BadRequest();
            //}

            //if (!ModelState.IsValid)
            //{
            //    _logger.LogError("Invalid model state for the CompanyCreationDto object");
            //    return UnprocessableEntity(ModelState);
            //}

            var companyEntity = _mapper.Map<Company>(company);
            
            _repository.Company.CreateCompany(companyEntity);
            await _repository.SaveAsync();

            var companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

            return CreatedAtRoute("CompanyById", new {id=companyToReturn.Id},
                companyToReturn);
        }

        [HttpGet("collection/({ids})",Name ="CompanyCollection")]
        public async Task<IActionResult> GetCompanyCollection(
            [ModelBinder(BinderType =typeof(ArrayModelBinder))]IEnumerable<Guid> ids)
        {
            if(ids == null)
            {
                _logger.LogError("Parameter ids is null");
                return BadRequest("Parameter ids is null");
            }
            
            var companyEntities = await _repository.Company.GetByIdsAsync(ids,trackChanges:false);
            if(ids.Count()!= companyEntities.Count())
            {
                _logger.LogError("Some ids are not valid in a collection");
                return NotFound();
            }
            var companiesToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            return Ok(companiesToReturn);
        }

        [HttpPost("collection")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async Task<IActionResult> CreateCompanyCollection([FromBody]
        IEnumerable<CompanyForCreationDto> companyCollection)
        {
            //if(companyCollection == null)
            //{
            //    _logger.LogError("Company collection sent from client is null.");
            //    return BadRequest("Company collection is null");
            //}
            var companyEntities = _mapper.Map<IEnumerable<Company>>(companyCollection);
            foreach (var company in companyEntities)
            {
                _repository.Company.CreateCompany(company);
            }
            await _repository.SaveAsync();

            var companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyEntities);
            var ids = string.Join(",",
                companyCollectionToReturn.Select(c=>c.Id));
            return CreatedAtRoute("CompanyCollection", 
                new { ids }, companyCollectionToReturn);
        }

        [HttpDelete("{id}")]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> DeleteCompany(Guid id)
        {
            //var company = await _repository.Company.GetCompanyAsync(id,trackChanges:false);
            //if(company == null)
            //{
            //    _logger.LogInfo($"Company with id:{id} doesn't exist in the database.");
            //    return NotFound();
            //}
            var company = HttpContext.Items["company"] as Company;
            _repository.Company.DeleteCompany(company);
            await _repository.SaveAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [ServiceFilter(typeof(ValidateCompanyExistsAttribute))]
        public async Task<IActionResult> UpdateCompany(Guid id,[FromBody] CompanyForUpdateDto company)
        {
            //if(company == null)
            //{
            //    _logger.LogError("CompanyForUpdateDto object sent from client is null.");
            //    return BadRequest("CompanyForUpdateDto object is null");
            //}

            //var companyEntity = await _repository.Company.GetCompanyAsync(id, trackChanges:true);
            //if(companyEntity == null)
            //{
            //    _logger.LogInfo($"Company with id: {id} doesn't exist in database");
            //    return NotFound();
            //}
            var companyEntity = HttpContext.Items["company"] as Company;
            _mapper.Map(company,companyEntity);
            await _repository.SaveAsync();

            return NoContent();
        }

        //[HttpPatch("{id}")]
        //public IActionResult PartiallyUpdateEmployeeForCompany(Guid companyId,Guid id, EmployeeParameters employeeParameters,
        //    [FromBody] JsonPatchDocument<EmployeeForUpdateDto> patchDoc)
        //{
        //    if(patchDoc == null)
        //    {
        //        _logger.LogError("patchDoc object sent from client is null");
        //        return BadRequest("patchDoc object is null");
        //    }
        //    var company = _repository.Company.GetCompanyAsync(companyId,trackChanges:false);
        //    if (company==null)
        //    {
        //        _logger.LogInfo($"Company with id: {companyId} doesn't exist in the database.");
        //        return NotFound();
        //    }
        //    var employeeEntity = _repository.Employee.GetEmployeesAsync(companyId,employeeParameters,trackChanges: true);
        //    if(employeeEntity == null)
        //    {
        //        _logger.LogInfo($"Employee with id: {id} doesn't exist in the database.");
        //        return NotFound();
        //    }
        //    var employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employeeEntity);

        //    patchDoc.ApplyTo(employeeToPatch,ModelState);

        //    TryValidateModel(employeeToPatch);
        //    if (!ModelState.IsValid)
        //    {
        //        _logger.LogError("Invalid model state for the patch document");
        //        return UnprocessableEntity(ModelState);
        //    }

        //    _mapper.Map(employeeToPatch, employeeEntity);

        //    _repository.SaveAsync();

        //    return NoContent();
        //}

    }
}
