using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Project.Management.Api.Dtos;
using Project.Management.Domain.Entities;
using Project.Management.Domain.Services.Users;

namespace Project.Management.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService service, IMapper mapper) : ControllerBase
    {
        private readonly IUserService _service = service;
        private readonly IMapper _mapper = mapper;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _service.GetAllUsers();

            return Ok(_mapper.Map<IEnumerable<UserDto>>(users));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(Guid id)
        {
            var user = await _service.GetUserById(id);

            if (user == null) 
                return NotFound();

            return Ok(_mapper.Map<UserDto>(user));
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(UserDto dto)
        {
            var user = _mapper.Map<User>(dto);
            var created = await _service.Create(user);

            return CreatedAtAction(nameof(GetById), new { id = created.Id }, _mapper.Map<UserDto>(created));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, UserDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var user = _mapper.Map<User>(dto);
            var updated = await _service.Update(user);

            return Ok(_mapper.Map<UserDto>(updated));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _service.Delete(id);
            return deleted ? NoContent() : NotFound();
        }
    }

}
