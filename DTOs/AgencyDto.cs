namespace DTOs
{
    public class AgencyDto
    {

        public string Name { get; set; }
        public DateTime ContactDate { get; set; }
        public List<CategoryDto> Categories { get; set; }
        public List<PlayerDto> Players { get; set; }

    }
}
