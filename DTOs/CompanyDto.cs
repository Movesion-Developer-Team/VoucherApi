using DTOs.MappingProfiles;
using Microsoft.AspNetCore.Mvc;

namespace DTOs
{
    public class CompanyDto : BaseDto
    {
        
        public string? Name { get; set; }
        public string? Address { get; set; }
        public int? NumberOfEmployees { get; set; }


    }
}
