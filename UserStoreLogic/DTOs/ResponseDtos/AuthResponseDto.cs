namespace UserStoreLogic.DTOs.ResponseDtos
{
    public class AuthResponseDto
    {
        public bool IsAuthenticated { get; set; }
        public string? Message { get; set; }
        public string? Token { get; set; }
    }
}
