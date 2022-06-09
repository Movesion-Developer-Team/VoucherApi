using DTOs.BodyDtos;

namespace DTOs.ResponseDtos
{
    public class GetAllBatchesResponseDto : BaseResponse
    {
        public IQueryable<BatchBodyDto>? Batches { get; set; }
    }
}
