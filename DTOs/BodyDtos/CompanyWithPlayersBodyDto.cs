namespace DTOs.BodyDtos
{
    public class CompanyWithPlayersBodyDto
    {
        public CompanyBodyDto? Company { get; set; }

        public ICollection<PlayerWithCategoriesBodyDto>? Players { get; set; }



    }
}
