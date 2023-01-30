namespace APIClientes.Repository
{
    using APIClientes.Data;
    using APIClientes.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;
    using System.IdentityModel.Tokens.Jwt;

    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDBContext db;

        private readonly IConfiguration _configuration;

        public UserRepository(ApplicationDBContext db, IConfiguration configuration)
        {
            this.db = db;
            this._configuration = configuration;
        }

        public async Task<string> Login(string user, string password)
        {
            var userDB = await db.Users.FirstOrDefaultAsync(x => x.UserName.ToLower().Equals(user.ToLower()));
            if (userDB == null) return "nouser";
            else if (!ConfirmPassword(password, userDB.PasswordHash, userDB.PasswordSalt)) return "wpassword";
            else return CrearToken(userDB);
        }

        public async Task<int> Register(User user, string password)
        {
            try
            {
                if (await UserExist(user.UserName)) return -1;

                CreateHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
                return user.Id;
            }
            catch (Exception ex)
            {
                return -500;
            }
        }

        public async Task<bool> UserExist(string username)
        {
            if (await db.Users.AnyAsync(x => x.UserName.ToLower().Equals(username.ToLower()))) return true;
            return false;
        }

        private void CreateHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool ConfirmPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++) if (computedHash[i] != passwordHash[i]) return false;
                return true;
            }
        }

        private string CrearToken (User user)
        {
            var claim = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName)
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claim),
                Expires = System.DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
