using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Dotz.Api
{
    public interface IUsuarioService
    {
        Usuario Authenticate(string email, string senha);
        IEnumerable<Usuario> GetAll();
        Usuario GetById(int id);
    }

    public class UsuarioService : IUsuarioService
    {
        private readonly DzContext _context;
        private List<Usuario> _users;
        private readonly AppSettings _appSettings;

        public UsuarioService(DzContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;

            _users = _context.Usuarios.ToList();

            _appSettings = appSettings.Value;
        }

        public Usuario Authenticate(string email, string senha)
        {
            var usuario = _users.SingleOrDefault(x => x.Email == email && x.Senha == senha);

            // return null if user not found
            if (usuario == null)
                return null;

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.Id.ToString()),
                    new Claim(ClaimTypes.Role, usuario.Papel)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            usuario.Token = tokenHandler.WriteToken(token);

            usuario.Senha = null;

            return usuario;
        }

        public IEnumerable<Usuario> GetAll()
        {
            // return users without passwords
            return _users.Select(x => {
                x.Senha = null;
                return x;
            });
        }

        public Usuario GetById(int id)
        {
            var user = _users.FirstOrDefault(x => x.Id == id);

            // return user without password
            if (user != null)
                user.Senha = null;

            return user;
        }
    }
}
