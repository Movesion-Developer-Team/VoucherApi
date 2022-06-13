using System.Reflection;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BenefitsApi.Controllers;
using Core.Domain;
using DTOs.BodyDtos;
using DTOs.MethodDto;
using DTOs.ResponseDtos;
using Enum;
using Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Persistence;
using UserStoreLogic;

namespace MobilityManagerApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]/")]
    [EnableCors]
    public class CompanyController : PadreController, IControllerBaseActions
    {

        

        public CompanyController(IMapper mapper, VoucherContext vContext) : base(mapper, vContext)
        {
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
                var players = await _unitOfWork.Company.GetAllPlayersForOneCompany(companyId);

                response.Players = _mapper.ProjectTo<PlayerOnlyBodyDto>(players);
                response.Message = "Success";
                response.StatusCode = StatusCodes.Status200OK;
                return Ok(response);
            }
            catch (NullReferenceException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (ArgumentNullException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (InvalidOperationException ex)
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
            var response = new DeleteResponseDto();

            try
            {
                response.Message = "Deleted";
                response.IsDeleted = await _unitOfWork.Company.RemoveAsync(id);
                
                if (!response.IsDeleted)
                {
                    response.Message = "Company not found";
                    return BadRequest(response);
                }

                await _unitOfWork.Complete();
                response.Message = "Deleted";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Unexpected server error: {ex.Message}";
                response.IsDeleted = false;
                return BadRequest(response);
            }
            
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllCompaniesWithPlayersResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllCompaniesWithPlayersResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllCompaniesWithPlayers()
        {
            var response = new GetAllCompaniesWithPlayersResponseDto();
            try
            {
                var companiesWithPlayersList = await Task.Run(() => _unitOfWork.Company.GetAllCompaniesWithPlayers());
                var companiesWithPlayersDto = _mapper.ProjectTo<CompaniesWithPlayersBodyDto>(companiesWithPlayersList.AsQueryable());
                response.Companies = companiesWithPlayersDto;
                response.Message = "Done";
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllUsersForCompanyResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllUsersForCompanyResponseDto), StatusCodes.Status400BadRequest)]
        [Route("{companyId}")]
        public async Task<IActionResult> GetAllUsersOfCompany([FromRoute]int companyId)
        {
            var response = new GetAllUsersForCompanyResponseDto();
            try
            {
                response.Users = await _unitOfWork.Company.GetAllUsersOfCompany(companyId).ToListAsync();
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
            
        }

        

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(GenerateInvitationCodeResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GenerateInvitationCodeResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GenerateInvitationCodeForEmployees([FromBody] GenerateInvitationCodeForEmployeesBodyDto body)
        {

            var response = new GenerateInvitationCodeResponseDto();
            try
            {
                response.InvitationCodeId = await _unitOfWork.InvitationCode.GenerateInvitationCodeForEmployees(
                    body.StartDate, body.EndDate,
                    body.CompanyId);
                var codeEntity = await _unitOfWork.InvitationCode
                    .Find(ic => ic.Id == response.InvitationCodeId)
                    .FirstAsync();
                response.Message = "Code generated successfully";
                response.InvitationCode = codeEntity.InviteCode;
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
            
        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllJoinRequestsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllJoinRequestsResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetAllJoinRequests(int companyId)
        {
            var response = new GetAllJoinRequestsResponseDto();
            try
            {
                var requests = await _unitOfWork.Company.GetAllJoinRequests(companyId);
                response.JoinRequests = await _mapper.ProjectTo<JoinRequestBodyDto>(requests).ToListAsync();
                response.Message = "Done";
                
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AcceptJoinRequest([FromBody] AcceptJoinRequestBodyDto body)
        {
            var response = new BaseResponse();
            try
            {
                await _unitOfWork.Company.AcceptRequest(body.RequestId, body.UserId);
                response.Message = "Request sent successfully";
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
        }

        [AuthorizeRoles(Role.SuperAdmin, Role.Admin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeclineJoinRequest([FromBody] DeleteJoinRequestBodyDto body)
        {
            var response = new BaseResponse();
            try
            {
                await _unitOfWork.Company.DeclineRequest(body.RequestId);
                response.Message = "Done";
                return Ok(response);
            }
            catch (InvalidOperationException ex)
            {
                response.Message = ex.Message;
                return BadRequest(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
            
        }

        [AuthorizeRoles(Role.SuperAdmin)]
        [HttpPost]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(BaseResponse), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AssignUserToCompany([FromBody] AssignUserToCompanyBodyDto body)
        {
            var userId = body.UserId;
            var companyId = body.CompanyId;

            var response = new BaseResponse();
            try
            {
                
                var user = await _unitOfWork.User.Find(u => u.IdentityUserId == userId).SingleOrDefaultAsync();
                if (user == null)
                {
                    response.Message = "User not found";
                    return BadRequest(response);
                }

                if (user.CompanyId != null && user.CompanyId != companyId)
                {
                    response.Message = "User is already assigned to another company";
                    return BadRequest(response);
                }

                if (user.CompanyId == companyId)
                {
                    response.Message = "User is already assigned to this company";
                    return BadRequest(response);
                }

                _unitOfWork.User.Update(user);
                user.CompanyId = companyId;
                await _unitOfWork.Complete();
                response.Message = "Done";
                return Ok(response);
            }
            catch(Exception ex)
            {
                response.Message = $"Internal server error: {ex.Message}";
                return BadRequest(response);
            }
            
        }

        [Authorize]
        [HttpGet]
        [ProducesResponseType(typeof(GetAllPlayersForCurrentCompanyResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(GetAllPlayersForCurrentCompanyResponseDto), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> SearchForPlayerOfCompany([FromQuery] string? playerName)
        {
            var response = new GetAllPlayersForCurrentCompanyResponseDto();
            var userInfo = await GetCurrentUserInfo();
            var companyId = userInfo.CompanyId;
            if (companyId == null)
            {
                response.Message = "Go and check yourself buddy, you are not belonging to any company";
                return BadRequest(response);
            }
            try
            {
                var players = await _unitOfWork.Company.SearchForPlayerOfCompany(playerName, (int)companyId);
                response.Players = _mapper.ProjectTo<PlayerOnlyBodyDto>(players);
                response.Message = "Done";
                response.StatusCode = StatusCodes.Status200OK;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.Message = $"Unexpected internal server error: {ex.Message}";
                return BadRequest(response);
            }
        }

    } 
}
