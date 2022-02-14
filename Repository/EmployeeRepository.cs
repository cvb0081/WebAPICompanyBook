using Contracts;
using Entities;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repository.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class EmployeeRepository : RepositoryBase<Employee> , IEmployeeRepository
    {
        public EmployeeRepository(RepositoryContext repositoryContext)
            : base(repositoryContext)
        {

        }

        public IEnumerable<Employee> GetEmployees(Guid companyId,bool trackChanges)=>
            FindByCondition(e=> e.CompanyId.Equals(companyId),trackChanges)
            .OrderBy(e=>e.Name);

        public Employee GetEmployee(Guid companyId,Guid id,bool trackingChanges)=>
            FindByCondition(x=>x.CompanyId.Equals(companyId)&&x.Id.Equals(id),trackingChanges)
            .SingleOrDefault();

        public void CreateEmployeeForCompany(Guid companyId,Employee employee)
        {
            employee.CompanyId = companyId;
            Create(employee);
        }

        public void DeleteEmployee(Employee employee)=>Delete(employee);

        public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId,
            EmployeeParameters employeeParameters, bool trackChanges)
        {
            var employees = await FindByCondition(e => e.CompanyId.Equals(companyId),
                trackChanges)
                .FilterEmployees(employeeParameters.MinAge,employeeParameters.MaxAge)
                .Search(employeeParameters.SearchTerm)
                .Sort(employeeParameters.OrderBy)
                .ToListAsync();

            return PagedList<Employee>
                .ToPagedList(employees, employeeParameters.PageNumber,
                employeeParameters.PageSize);
        }

        public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid id, bool trackChanges)=>
             await FindByCondition(x => x.CompanyId.Equals(companyId)&&x.Id.Equals(id), trackChanges)
            .SingleOrDefaultAsync();

    }
}
