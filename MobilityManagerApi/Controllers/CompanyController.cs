﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using Core.Domain;
using DTOs.BodyDtos;
using DTOs.ResponseDtos;
using Enum;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using UserStoreLogic;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [EnableCors]
    public class CompanyController : ControllerBase, IControllerBaseActions
    {

        private readonly IMapper _mapper;
        private readonly UnitOfWork _unitOfWork;




        public CompanyController(IMapper mapper, VoucherContext vContext)
        {

                _mapper = mapper;
                _unitOfWork = new UnitOfWork(vContext);

        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(CreateNewEntityResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateNewEntityResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateNewCompany([FromBody] CreateNewCompanyBodyDto modelDto)
        {

            var response = new CreateNewEntityResponseDto();

            if (modelDto.Name.IsNullOrEmpty())
            {
                response.Message = "Please, provide company name";
                return BadRequest(response);
            }

            var newAgency = _mapper.Map<Company>(modelDto);
            if (newAgency == null)
            {
                response.Message = "Server side error: Object is not mapped, check mapping profile";
                return BadRequest(response);
            }
            int? id;
            try
            {
                id = await _unitOfWork.Company.AddAsync(newAgency);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }

            await _unitOfWork.Complete();
            response.Message = "New entity created";
            response.Id = id;
            return Ok(response);
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(CompanyMainResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CompanyMainResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Change([FromBody] CompanyBodyDto body)
        {
            Company company = new();
            var response = new CompanyMainResponseDto();

            try
            {
                company = await _unitOfWork.Company.Find(c => c.Id == body.Id).FirstAsync();
                _unitOfWork.Company.Update(company);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
            }


            if (response.Message != null)
            {
                return BadRequest(response);
            }



            var listDtoProp = body.GetType().GetProperties();
            foreach (var property in listDtoProp)
            {
                if (property.GetValue(body) != null)
                {
                    _mapper.Map(body, company);
                }

            }

            var idValue = listDtoProp.First(p => p.Name == "Id").GetValue(body);
            response.Message = idValue != null ? "Warning: changes applied, but new Id is not assigned, because it is forbidden on server side" 
                : "Changes applied";

            await _unitOfWork.Complete();
            
            response.Company = _mapper.Map<Company, CompanyBodyDto>(company);
            return Ok(response);
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddPlayerToCompany([FromBody] AddPlayerToCompanyBodyDto body)
        {
            var response = new BaseResponse();
            if (body.CompanyId == null || body.PlayerId == null)
            {
                return BadRequest("Please, provide all required parameters");
            }

            try
            {
                await _unitOfWork.Company.AddPlayerToCompany((int)body.PlayerId, (int)body.CompanyId);
                await _unitOfWork.Complete();
                response.Message = "Player added to the Company";
                return Ok(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllCompaniesResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllCompaniesResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAll()

        {
            var response = new GetAllCompaniesResponseDto();

            try
            {
                var companies = await _unitOfWork.Company.GetAll();
                response.Companies = companies.ProjectTo<CompanyBodyDto>(_mapper.ConfigurationProvider);
                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }

        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        [ProducesResponseType(typeof(CompanyMainResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CompanyMainResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindById(int id)
        {
            var response = new CompanyMainResponseDto();
            try
            {
                var companies = _unitOfWork.Company.Find(c => c.Id == id);
                response.Company = await companies.ProjectTo<CompanyBodyDto>(_mapper.ConfigurationProvider).FirstAsync();
                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }

        }


        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        [ProducesResponseType(typeof(CompanyFindByNameResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CompanyFindByNameResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> FindByName(string name)
        {
            var response = new CompanyFindByNameResponseDto();
            try
            {
                var companies = _unitOfWork.Company.Find(c => c.Name == name);
                response.Companies = await Task.Run(() => companies.ProjectTo<CompanyBodyDto>(_mapper.ConfigurationProvider));
                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllPlayersForCurrentCompanyResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllPlayersForCurrentCompanyResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllPlayersForOneCompany([FromQuery] int companyId)
        {
            var response = new GetAllPlayersForCurrentCompanyResponseDto();
            try
            {
                var company = await _unitOfWork.Company.Find(c => c.Id == companyId).FirstAsync();
                response.Players = company.Players.ToList();
                response.Message = "Success";
                return Ok(response);
            }
            catch(NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpDelete]
        [ProducesResponseType(typeof(DeleteResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(DeleteResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var response = new DeleteResponseDto
            {
                IsDeleted = await _unitOfWork.Company.RemoveAsync(id)
            };
            if (!response.IsDeleted)
            {
                response.Message = "Company not found";
                return BadRequest(response);
            }
            await _unitOfWork.Complete();
            response.Message = "Deleted";
            return Ok(response);
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllCompaniesWithPlayersResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllCompaniesWithPlayersResponseDto), StatusCodes.Status400BadRequest)]
        public IActionResult GetAllCompaniesWithPlayers()
        {
            var response = new GetAllCompaniesWithPlayersResponseDto();
            try
            {
                var companiesWithPlayers = _unitOfWork.Company.GetAllCompaniesWithPlayers();
                response.Companies = _mapper.ProjectTo<CompanyWithPlayersBodyDto>(companiesWithPlayers).ToList();
                response.Message = "Done";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
        }
    } 
}
