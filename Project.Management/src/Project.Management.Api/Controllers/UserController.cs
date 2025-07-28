using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Management.Api.Dtos;
using Project.Management.Domain.Services.Notificator;
using Project.Management.Domain.Services.Users;
using Project.Management.Domain.Services.Users.Models;

namespace Project.Management.Api.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController(IUserService service, IMapper mapper, INotificator notificator) : BaseController(notificator)
    {
        private readonly IUserService _service = service;
        private readonly IMapper _mapper = mapper;

        /// <summary>
        /// Get all users in the system
        /// </summary>
        /// <returns>Users</returns>
        [HttpGet("all-users")]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = _mapper.Map<IEnumerable<UserDto>>(await _service.GetAll());

            return await CustomResponse(users);
        }

        /// <summary>
        /// Get user by id
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>User</returns>
        [HttpGet("user-by-id/{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var user = _mapper.Map<UserDto>(await _service.GetById(id));

            return await CustomResponse(user);
        }

        /// <summary>
        /// Add an user to the system
        /// </summary>
        /// <param name="request">Basic user details</param>
        /// <returns>User</returns>
        [HttpPost("add")]
        public async Task<ActionResult<UserDto>> Create(UserCreationRequest request)
        {
            var created = _mapper.Map<UserDto>(await _service.Create(request));

            return await CustomResponse(created);
        }

        /// <summary>
        /// Update an user in the system
        /// </summary>
        /// <param name="request">Basic user details</param>
        /// <returns>User</returns>
        [HttpPut("update")]
        public async Task<ActionResult<UserDto>> Update(UserUpdateRequest request)
        {
            var updated = _mapper.Map<UserDto>(await _service.Update(request));

            return await CustomResponse(updated);
        }

        /// <summary>
        /// Delete an user from the system
        /// </summary>
        /// <param name="id">User id</param>
        /// <returns>True if deleted</returns>
        [HttpDelete("delete-by-id/{id}")]
        public async Task<ActionResult<bool>> Delete(Guid id)
        {
            var deleted = await _service.Delete(id);
            return await CustomResponse(deleted);
        }
    }

}
