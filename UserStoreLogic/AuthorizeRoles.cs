using Enum;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace UserStoreLogic
{
    public class AuthorizeRoles : AuthorizeAttribute
    {
        
        public AuthorizeRoles(params Role[] roles) :base()
        {
            foreach (var role in roles)
            {
                Roles = string.Join(',', role.ToString());
            }

            this.AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }


    }
}
