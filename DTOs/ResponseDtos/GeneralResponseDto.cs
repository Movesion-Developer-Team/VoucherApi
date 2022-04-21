namespace MobilityManagerApi.Dtos.ResponseDtos
{
    public class GeneralResponseDto<TResponseUnit>
    {
        public string? Message { get; set; }
        public TResponseUnit? Unit { get; set; }
    }
}
