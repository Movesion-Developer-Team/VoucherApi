using System.Drawing;

namespace DTOs.BodyDtos
{
    public class BaseImageBodyDto : BaseBody
    {
        public BaseImageBodyDto? Image { get; set; }
        public int? CategoryId { get; set; }
        public int? PlayerId { get; set; }
    }
}
