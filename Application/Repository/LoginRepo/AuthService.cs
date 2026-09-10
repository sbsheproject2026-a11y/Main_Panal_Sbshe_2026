using Application.DTOs.MasterTypeDTOs;
using Domain;
using Microsoft.EntityFrameworkCore;

namespace Application.Repository.LoginRepo
{
    public class AuthService
    {

        private readonly ApplicationDbContext _context;

        private readonly JwtService _jwtService;


        public AuthService(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }


        public async Task<string> Login(LoginDTOs model)
        {
            var result = await (  from ml in _context.MasterLogin
                join bp in _context.BasicProfile   on ml.EntityId equals bp.EntityId
                where ml.UserName == model.Email
                select new
                {
                    User = ml,
                    Name = bp.Name
                }
            ).FirstOrDefaultAsync();

            if (result == null)
            {
                return "User not found";
            }

            if (result.User.Password != model.Password)
            {
                return "Invalid password";
            }

            var token = _jwtService.GenerateToken(
                result.User,
                result.Name
            );

            return token;
        }


    }
}
