namespace Application.Repository.LoginRepo
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.IdentityModel.Tokens;
    using Model;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;


    public class JwtService
    {

        private readonly IConfiguration _configuration;


        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public string GenerateToken(MasterLogin user, string name)
        {
            var claims = new[]
            {
        new Claim("UserId", user.EntityId.ToString()),

        new Claim("Email", user.UserName ?? ""),

        new Claim("Name", name ?? ""),

        new Claim(
            ClaimTypes.Role,
            user.RoleId?.ToString() ?? ""
        )
    };

            var key = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(
                    _configuration["Jwt:Key"]!
                )
            );

            var credential = new SigningCredentials(
                key,
                SecurityAlgorithms.HmacSha256
            );

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credential
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
