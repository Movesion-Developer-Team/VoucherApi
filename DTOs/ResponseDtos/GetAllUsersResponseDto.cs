namespace DTOs.ResponseDtos
{
    public class GetAllUsersResponseDto : BaseResponse
    {
        public IQueryable<UserDto>? Users { get; set; }
    }
}
