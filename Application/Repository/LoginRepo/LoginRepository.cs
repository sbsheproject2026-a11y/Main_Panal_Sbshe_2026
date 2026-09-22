using Application.DTOs.MasterTypeDTOs;
using Application.Repository.LoginRepo;
using Domain;
using Microsoft.EntityFrameworkCore;




namespace Application.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly JwtService _jwtService;

        public LoginRepository(ApplicationDbContext context, JwtService jwtService)
        {
            _context = context;
            _jwtService = jwtService;
        }


        public async Task<string> Login(LoginDTOs model)
        {
            try
            {
                var result = await (
                    from ml in _context.MasterLogin
                    join e in _context.BasicProfile
                        on ml.EntityId equals e.EntityId
                    where ml.UserName == model.Email && ml.Password==model.Password
                    select new
                    {
                        User = ml,
                        Name = e.Name
                    }
                ).FirstOrDefaultAsync();

                if (result == null)
                {
                    return "Invalid Email or Password";
                }

                // Password verify
                if (string.IsNullOrWhiteSpace(result.User.HashPassword))
                {
                    return "Invalid Email or Password";
                }

                bool isValid = BCrypt.Net.BCrypt.Verify(
                    model.Password,
                    result.User.HashPassword
                );

                if (!isValid)
                {
                    return "Invalid Email or Password";
                }

                // JWT Generate
                var token = _jwtService.GenerateToken(result.User, result.Name);

                return token;
            }
            catch
            {
                return "Something went wrong.";
            }
        }


    }
}
