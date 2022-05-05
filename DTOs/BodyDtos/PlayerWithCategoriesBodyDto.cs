namespace DTOs.BodyDtos
{
    public class PlayerWithCategoriesBodyDto
    {
        public PlayerBodyDto? Player { get; set; }

        public ICollection<CategoryBodyDto>? Categories { get; set; }
    }
}
