using Entities.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Configuration
{
    public class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
    {
        public void Configure(EntityTypeBuilder<Employee> builder)
        {
            builder.HasData(
                new Employee
                {
                    Id = new Guid("39317070-8da4-4b8e-adfc-03a46b31d4b9"),
                    Name = "Sam Raiden",
                    Age = 26,
                    Position ="Software Developer",
                    CompanyId = new Guid("9d2139e2-ae97-4ee5-95f4-e8769f7fd70f")
                },
                new Employee
                {
                    Id=new Guid("42dc3e81-1e45-415a-8889-09fef70d08f2"),
                    Name="Jana McLeaf",
                    Age=30,
                    Position="Software Developer",
                    CompanyId=new Guid("9d2139e2-ae97-4ee5-95f4-e8769f7fd70f")
                    
                }, 
                new Employee
                {
                    Id=new Guid("6000dc78-6cc1-489d-8c18-2a82b42115e2"),
                    Name="KaneMiller",
                    Age=35,
                    Position="Software Developer",
                    CompanyId=new Guid("c4952391-bbc9-47e2-94b7-e50ce255fe26")
                }
                );
        }
    }
}
