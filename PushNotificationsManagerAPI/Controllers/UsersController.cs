using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PushNotificationsManagerAPI.Models;
using PushNotificationsManagerAPI.Repositories;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PushNotificationsManagerAPI.Controllers
{
    /// <summary>
    /// User Controller - provides endpoints for CRUD operations against users and login functionality.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        public UsersController(IUserRepository userRepository, IConfiguration configuration)
        {
            this._userRepository = userRepository;
            _configuration = configuration;
        }

        /// <summary>
        /// Gets all users
        /// </summary>
        /// <returns>All users</returns>
        [HttpGet]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.Get();
        }

        /// <summary>
        /// Gets user by userName.
        /// </summary>
        /// <returns>The found user for the provided userName</returns>
        [HttpGet("{userName}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<User>> GetUsers(string userName)
        {
            return await _userRepository.Get(userName);
        }

        /// <summary>
        /// Creates users.
        /// </summary>
        /// <returns>The newly created user.</returns>
        [HttpPost]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<User>> PostUsers([FromBody] User user)
        {
            try
            {
                var newUser = await _userRepository.Create(user);
                return CreatedAtAction(nameof(GetUsers), new { userName = newUser.UserName }, newUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException != null ? ex.Message + " Inner exception: " + ex.InnerException.Message : ex.Message);
            }
        }

        /// <summary>
        /// Updates an user.
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="user"></param>
        /// <returns>The newly created user.</returns>
        [HttpPut]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult<User>> PutUsers(string userName, [FromBody] User user)
        {
            try
            {
                if (!userName.Equals(user.UserName))
                {
                    return BadRequest();
                }

                var updatedUser = await _userRepository.Update(user);

                return CreatedAtAction(nameof(GetUsers), new { userName = updatedUser.UserName }, updatedUser);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException != null ? ex.Message + " Inner exception: " + ex.InnerException.Message : ex.Message);
            }
        }

        /// <summary>
        /// Deletes an user.
        /// </summary>
        /// <param name="userName"></param>
        /// <returns>The result action.</returns>
        [HttpDelete("{userName}")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<ActionResult> Delete(string userName)
        {
            try
            {
                var userToDelete = await _userRepository.Get(userName);
                if (userToDelete == null)
                    return NotFound("The user does not exist.");

                if (userToDelete.Role.Equals(UserRoles.Admin))
                {
                    return BadRequest();
                }

                await _userRepository.Delete(userToDelete.UserName);
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.InnerException != null ? ex.Message + " Inner exception: " + ex.InnerException.Message : ex.Message);
            }
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginUser"></param>
        /// <returns>The login result.</returns>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginUser loginUser)
        {
            var user = await _userRepository.Get(loginUser.UserName);

            if(user == null)
            {
                return NotFound("User does not exit.");
            }

            if (user.Password.Equals(loginUser.Password))
            {
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                authClaims.Add(new Claim(ClaimTypes.Role, user.Role));
                
                var authSiginKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSiginKey, SecurityAlgorithms.HmacSha256Signature)
                    );

                return Ok(new
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    ValidTo = token.ValidTo.ToString("yyyy-MM-ddThh:mm:ss"),
                    user = user
                });
            }

            return Unauthorized();
        }
    }
}
