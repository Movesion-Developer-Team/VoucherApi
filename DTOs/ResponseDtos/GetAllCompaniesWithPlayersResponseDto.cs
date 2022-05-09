using System.Reflection.Metadata;
using DTOs.BodyDtos;
using DTOs.MethodDto;

namespace DTOs.ResponseDtos
{
    public class GetAllCompaniesWithPlayersResponseDto : BaseResponse
    {

        public IQueryable<CompaniesWithPlayersBodyDto>? Companies { get; set; }
    }
}
