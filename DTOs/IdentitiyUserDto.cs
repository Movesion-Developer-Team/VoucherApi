using Enum;
using Microsoft.AspNetCore.Mvc;

namespace DTOs
{
    public class IdentityUserDto
    {
        
        public string UserName { get; set; }
        public string Password { get; set; }
        public int? CompanyId { get; set; }
        public string? CompanyName { get; set; }
        

    }
}
