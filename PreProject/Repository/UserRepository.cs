using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PreProject.Data;
using PreProject.Models;
using PreProject.Repository.IRepository;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace PreProject.Repository
{
    public class UserRepository : IUserRepository
    {
        //Dependency Injection for our AppDbContext to access the database
        private readonly  AppDbContext _db;
        private readonly AppSettings _appSettings;
        public UserRepository(AppDbContext db, IOptions<AppSettings> appsettings)
        {
            _db = db;
            _appSettings = appsettings.Value;
        }
        public User Authenticate(string username, string password)
        {
            //Retreive a user from db who's username and password matches what is passed here
            var user = _db.Users.SingleOrDefault(x => x.Username == username && x.Password == password);
            
            //if user not found
            if (user == null)
            {
                return null;
            }

            //If user was found, generate JWT token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials
                                (new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            user.Token = tokenHandler.WriteToken(token);
            user.Password = "";
            return user;
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.Users.SingleOrDefault(x => x.Username == username);

            //Return null is user not found
            if (user == null)
                return true;

            return false;
        }

        public User Register(string username, string password)
        {
            User userObj = new User()
            {
                Username = username,
                Password = password,
                Role = "Admin"
            };

            _db.Users.Add(userObj);
            _db.SaveChanges();
            userObj.Password = "";
            return userObj;
        }
    }
}
