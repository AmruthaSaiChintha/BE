using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


 
namespace AadharVerify.Models

{

    public class JWT

    {

        public JWT(string? Key, string? Duration)

        {

            this.Key = Key ?? "";

            this.Duration = Duration ?? "";

        }

        public string Key { get; set; }

        public string Duration { get; set; }


        public string GenerateToken(Users user)

        {

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Key));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]

            {

                new Claim("id", user.Id.ToString()),


                new Claim("email", user.Email),

                new Claim("password", user.Password),

               new Claim("userType", user.UserType)

            };

            var jwtToken = new JwtSecurityToken(

               issuer: "localhost",

               audience: "localhost",

               claims: claims,

               expires: DateTime.Now.AddMinutes(Int32.Parse(this.Duration)),

               signingCredentials: credentials

               );

            return new JwtSecurityTokenHandler().WriteToken(jwtToken);

        }

    }

}

