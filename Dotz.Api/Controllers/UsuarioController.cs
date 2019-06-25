using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Dotz.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        private IUsuarioService _userService;

        public UsuarioController(IUsuarioService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]JObject usrJson)
        {
            Usuario userParam = JsonConvert.DeserializeObject<Usuario>(usrJson.ToString());
            var user = _userService.Authenticate(userParam.Email, userParam.Senha);

            if (user == null)
                return BadRequest(new { message = "Email ou senha incorreta!" });

            return Ok(user);
        }

        [Authorize(Roles = Papel.Admin)]
        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _userService.GetAll();
            return Ok(users);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var user = _userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            var currentUserId = int.Parse(User.Identity.Name);
            if (id != currentUserId && !User.IsInRole(Papel.Admin))
            {
                return Forbid();
            }

            return Ok(user);
        }
    }
}