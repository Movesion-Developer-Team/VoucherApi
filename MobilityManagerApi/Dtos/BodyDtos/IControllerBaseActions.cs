using Microsoft.AspNetCore.Mvc;

namespace MobilityManagerApi.Dtos.BodyDtos
{
    public interface IControllerBaseActions
    {
        public Task<IActionResult> GetAll();
        public Task<IActionResult> FindById([FromBody] BaseBody request);
        public Task<IActionResult> Delete([FromBody] BaseBody request);

    }
}
