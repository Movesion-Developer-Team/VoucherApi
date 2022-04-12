namespace UserStoreLogic.DTOs.BodyDtos
{
    public class LoginBodyDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public LoginBodyDto(string userName, string password)
        {
            UserName = userName;
            Password = password;
        }
    }
}
