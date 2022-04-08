using System.Reflection.Metadata;

namespace UserStoreLogic.DTOs
{
    public class AuthResponseDto
    {
        public bool IsAuthenticated { get; set; }
        public string ErrorMessage { get; set; }
        public string Token { get; set; }
    }
}
