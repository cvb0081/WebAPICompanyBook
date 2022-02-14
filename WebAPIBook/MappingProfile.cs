using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebAPIBook
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Company, CompanyDto>()
                .ForMember(c => c.FullAddress,
                opt => opt.MapFrom(x => string.Join(' ', x.Address, x.Country)));

            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<CompanyForCreationDto, Company>();
            CreateMap<EmployeeForCreationDto,Employee>();
            CreateMap<EmployeeForUpdateDto, Employee>();
            CreateMap<CompanyForUpdateDto,Company>();
            CreateMap<EmployeeForUpdateDto,Employee>().ReverseMap();
            CreateMap<UserForRegistrationDto, User>();
        }
    }
}
