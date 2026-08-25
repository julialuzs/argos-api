using ArgosApi.Domain.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ArgosApi.Infrastructure.Authentication
{
    /// <summary>
    /// Serviço responsável por gerar tokens JWT
    /// </summary>
    public class JwtService
    {
        private readonly JwtOptions _options;

        /// <summary>
        /// Inicializa o serviço com as opções de JWT
        /// </summary>
        public JwtService(IOptions<JwtOptions> options)
        {
            _options = options.Value;
        }

        /// <summary>
        /// Gera o token JWT para o usuário informado
        /// </summary>
        public JwtToken GenerateToken(Usuario usuario)
        {
            var expiration = DateTime.UtcNow.AddMinutes(_options.ExpirationMinutes);

            var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, usuario.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, usuario.Email),
            new(JwtRegisteredClaimNames.Name, usuario.Nome),

            new(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new(ClaimTypes.Name, usuario.Nome),
            new(ClaimTypes.Email, usuario.Email),

            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(_options.Key));

            var credentials = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _options.Issuer,
                audience: _options.Audience,
                claims: claims,
                expires: expiration,
                signingCredentials: credentials);

            return new JwtToken
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }
    }
}
