namespace DTOs
{
    public class CategoryDto
    {
        public string Name { get; set; }
        public string? Description { get; set; }

        public CategoryDto(string name)
        {
            Name = name;
        }

    }
}
