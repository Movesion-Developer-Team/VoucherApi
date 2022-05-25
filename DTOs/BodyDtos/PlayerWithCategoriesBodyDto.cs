namespace DTOs.BodyDtos
{
    public class PlayerWithCategoriesBodyDto
    {
        public PlayerWithCategoriesAndDiscountTypesBodyDto? Player { get; set; }

        public ICollection<CategoryBodyDto>? Categories { get; set; }
    }
}
