using AutoMapper;
using Contracts;
using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebAPIBook.ActionFilters;

namespace WebAPIBook.Controllers
{
    [Route("api/authentication")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IAuthenticationManager _authenticationManager;

        public AuthenticationController(ILoggerManager logger, IMapper mapper, 
            UserManager<User> userManager, IAuthenticationManager authenticationManager)
        {
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
            _authenticationManager=authenticationManager;
        }

        [HttpPost]
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        public async  Task<IActionResult> RegisterUser([FromBody] UserForRegistrationDto
            userForRegistration)
        {
            var user = _mapper.Map<User>(userForRegistration);

            var result = await _userManager.CreateAsync(user, userForRegistration.Password);
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.TryAddModelError(error.Code, error.Description);
                }
                return BadRequest(ModelState);
            }
            await _userManager.AddToRolesAsync(user,userForRegistration.Roles);

            return StatusCode(201);
        }

        [HttpPost("login")]
        [ServiceFilter(typeof (ValidationFilterAttribute))]
        public async Task<IActionResult> Authenticate([FromBody] 
        UserForAuthenticationDto user)
        {
            if(!await _authenticationManager.ValidateUser(user))
            {
                _logger.LogWarn($"{nameof(Authenticate)} Authentication failed. Wrong" +
                    $" username or password.");
                return StatusCode(401);
            }

            return Ok(new { Token = await _authenticationManager.CreateToken() });
        }
    }
}
