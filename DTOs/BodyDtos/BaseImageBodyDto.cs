namespace DTOs.BodyDtos
{
    public class BaseImageBodyDto : BaseBody
    {
        public byte[]? Content { get; set; }
        public int? CategoryId { get; set; }
        public int? PlayerId { get; set; }
    }
}
