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
                // Step 1: User + BasicProfile se Name nikaalo
                var result = await (
                    from ml in _context.MasterLogin
                    join bp in _context.BasicProfile
                        on ml.EntityId equals bp.EntityId
                    where ml.UserName == model.Email && ml.Password == model.Password
                    select new
                    {
                        User = ml,
                        Name = bp.Name
                    }
                ).FirstOrDefaultAsync();

                if (result == null)
                {
                    return "Invalid Email or Password";
                }

                // Step 2: Entity table se Status aur Name nikaalo
                var result1 = await ( from en in _context.Entity
                    where en.Id == result.User.EntityId
                    select new
                    {
                        User = result.User,
                       
                        Status = en.Status
                    }
                ).FirstOrDefaultAsync();

                if (result1 == null)
                {
                    return "Invalid Email or Password";
                }

                // Step 3: Status check
                if (result1.Status == 11)
                {
                    return "Your account is pending approval. Please wait for admin verification.";
                }
                else if (result1.Status == 13)
                {
                    return "Unfortunately, your profile could not be approved. Kindly reach out to our support team for assistance.";
                }
                else if (result1.Status != 12)
                {
                    return "Invalid Email or Password";
                }

                // Step 4: Password verify
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

                // Step 5: JWT Generate
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
