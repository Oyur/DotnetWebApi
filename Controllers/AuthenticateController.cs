using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DotnetWebApi.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace DotnetWebApi.Controllers
{   //[Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticateController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;

        private readonly RoleManager<IdentityRole> roleManager;

        private readonly IConfiguration _configuration;

        public AuthenticateController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
        }

        //Login API
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model) //เขียนแบบ asynawait
         {
            var user = await userManager.FindByNameAsync(model.UserName); //ค้นหาusername
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password)) //เช็ค username ไม่เป็น null และ passwordถูกต้อง
            {
                var userRoles = await userManager.GetRolesAsync(user); //เช็ค role ของ User
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),  //compare username และ password
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), //register jwt
                };
                foreach (var userRole in userRoles)  // loopตัว role ออกมา
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));
                }

                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])); //sign ตัว JWT

                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],  //userไหน
                    audience: _configuration["JWT:ValidAudience"], //portไหน
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                // return ออกมาเป็น token
                return Ok(new 
                {
                    token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            //ไม่ผ่าน returnเป็น unauthorized
            return Unauthorized();
         }

         //Register API
         [HttpPost]
         [Route("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.UserName);

            if (userExists != null){ //เช็คว่ามี Usernameนี้รึยัง
                return StatusCode(StatusCodes.Status500InternalServerError, new Response {  
                    Status = "Error", Message = "User already exists!" 
                });
            }

            ApplicationUser user = new ApplicationUser()   //รับ Email ,Username,password
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),  //เข้า่รหัสpassword
                UserName = model.UserName
            };

            var result = await userManager.CreateAsync(user, model.Password); //สร้างusername และ password

            if (!result.Succeeded){
                return StatusCode(StatusCodes.Status500InternalServerError, new Response {
                     Status = "Error", Message = "User creation failed! Please check user details and try again." 
                });
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpPost]
        [Route("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterModel model)
        {
            var userExists = await userManager.FindByNameAsync(model.UserName);
            if (userExists != null){
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { 
                    Status = "Error", Message = "User already exists!" 
                });
            }

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.UserName
            };

            var result = await userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded){
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { 
                    Status = "Error", Message = "User creation failed! Please check user details and try again." 
                });
            }
            // เพิ่ม สิทธิ Admin เข้าไป
            if (!await roleManager.RoleExistsAsync(UserRole.Admin)){
                await roleManager.CreateAsync(new IdentityRole(UserRole.Admin));
            }

            if (!await roleManager.RoleExistsAsync(UserRole.User)){
                await roleManager.CreateAsync(new IdentityRole(UserRole.User));
            }

            if (await roleManager.RoleExistsAsync(UserRole.Admin))
            {
                await userManager.AddToRoleAsync(user, UserRole.Admin);
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }
    }
}