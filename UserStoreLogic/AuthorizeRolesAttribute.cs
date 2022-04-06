using Enum;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace UserStoreLogic
{
    public class AuthorizeRolesAttribute : AuthorizeAttribute
    {
        
        public AuthorizeRolesAttribute(params Role[] roles) :base()
        {
            Roles = String.Join(',', roles);

            this.AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }


    }
}
