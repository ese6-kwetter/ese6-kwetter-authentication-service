﻿using System;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using UserMicroservice.Exceptions;
using UserMicroservice.Models;
using UserMicroservice.Services;

namespace UserMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Consumes(MediaTypeNames.Application.Json)]
    [Produces(MediaTypeNames.Application.Json)]
    public class LoginController : ControllerBase
    {
        private readonly ILoginService _service;

        public LoginController(ILoginService service)
        {
            _service = service;
        }

        /// <summary>
        ///     Login a User with password.
        /// </summary>
        /// <param name="model">Login credentials</param>
        /// <returns>User without sensible data</returns>
        [HttpPost("password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LoginPasswordAsync([FromBody] LoginPasswordModel model)
        {
            try
            {
                return Ok(await _service.LoginPasswordAsync(model.Email, model.Password));
            }
            catch (EmailNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///     Login a User with a connected Google account.
        /// </summary>
        /// <param name="model">Login credentials</param>
        /// <returns>User without sensible data</returns>
        [HttpPost("google")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LoginGoogleAsync([FromBody] LoginGoogleModel model)
        {
            try
            {
                return Ok(await _service.LoginGoogleAsync(model.Token));
            }
            catch (AccountNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///     Login a User with a connected Apple account.
        /// </summary>
        /// <param name="model">Login credentials</param>
        /// <returns>User without sensible data</returns>
        [HttpPost("apple")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> LoginAppleAsync([FromBody] LoginAppleModel model)
        {
            try
            {
                return Ok(await _service.LoginAppleAsync(model.Token));
            }
            catch (AccountNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
