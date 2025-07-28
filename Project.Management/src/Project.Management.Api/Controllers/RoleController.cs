using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Management.Api.Dtos;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Roles;
using Project.Management.Domain.Services.Roles.Models;

namespace Project.Management.Api.Controllers
{
    [ApiController]
    [Route("api/role")]
    public class RoleController(IRoleService service, IMapper mapper, INotificator notificator) : BaseController(notificator)
    {
        private readonly IRoleService _service = service;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Get all roles in the system
        /// </summary>
        /// <returns>Roles</returns>
        [HttpGet("all-roles")]
        public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll()
        {
            var roles = _mapper.Map<IEnumerable<RoleDto>>(await _service.GetAll());

            return await CustomResponse(roles);
        }

        /// <summary>
        /// Get role by id
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>Role</returns>
        [HttpGet("role-by-id/{id}")]
        public async Task<ActionResult<RoleDto>> GetById(Guid id)
        {
            var role = _mapper.Map<RoleDto>(await _service.GetById(id));

            return await CustomResponse(role);
        }

        /// <summary>
        /// Add a role to the system
        /// </summary>
        /// <param name="request">Basic role details</param>
        /// <returns>Role</returns>
        [HttpPost("add")]
        public async Task<ActionResult<RoleDto>> Create(RoleCreateRequest request)
        {
            var created = _mapper.Map<RoleDto>(await _service.Create(request));

            return await CustomResponse(created);
        }

        /// <summary>
        /// Update a role in the system
        /// </summary>
        /// <param name="request">Basic user details</param>
        /// <returns>User</returns>
        [HttpPut("update")]
        public async Task<ActionResult<RoleDto>> Update(RoleUpdateRequest request)
        {
            var updated = await _service.Update(request);

            return Ok(_mapper.Map<RoleDto>(updated));
        }

        /// <summary>
        /// Delete a role from the system
        /// </summary>
        /// <param name="id">Role id</param>
        /// <returns>True if deleted</returns>
        [HttpDelete("delete-by-id/{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var deleted = await _service.Delete(id);

            return await CustomResponse(deleted);
        }
    }

}
