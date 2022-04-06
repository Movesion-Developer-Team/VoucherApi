using System.Security.Cryptography.X509Certificates;
using AutoMapper;
using Core.Domain;
using DTOs;
using Enum;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Npgsql.EntityFrameworkCore.PostgreSQL.Storage.Internal.Mapping;
using Persistence;
using UserStoreLogic;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    [EnableCors]
    public class CategoryController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;

        public CategoryController(IMapper mapper, UnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateNewCategory([FromQuery]CategoryDto category)
        {
            var newCategory = _mapper.Map<CategoryDto, Category>(category);
            await _unitOfWork.Category.AddAsync(newCategory);
            return Ok();
        }


    }
}
