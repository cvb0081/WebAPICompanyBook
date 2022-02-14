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
    public class CompanyConfiguration : IEntityTypeConfiguration<Company>
    {
        public void Configure(EntityTypeBuilder<Company> builder)
        {
            builder.HasData(
                new Company
                {
                    Id = new Guid("9d2139e2-ae97-4ee5-95f4-e8769f7fd70f"),
                    Name ="IT Company",
                    Address ="Address test 123",
                    Country ="USA"
                },
                new Company
                {
                    Id=new Guid("c4952391-bbc9-47e2-94b7-e50ce255fe26"),
                    Name="Admin Company",
                    Address="Adress 2 test 123",
                    Country="EU"
                });
        }
    }
}

