using Enum;

namespace DTOs.BodyDtos
{
    public class ChangeRoleBodyDto
    {
        public string? UserName { get; set; }
        public string? UserId { get; set; }
        
        public BasicRole? Role { get; set; }
    }
}
