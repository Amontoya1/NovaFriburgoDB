using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using NovaFriburgoDB.DataAccess;
using NovaFriburgoDB.Entities;
using NovaFriburgoDB.Helpers;
using NovaFriburgoDB.Models.DataModels;

namespace NovaFriburgoDB.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly NovaFriburgoDBContext _context;
        private readonly JwtSettings _jwtSettings;
        private readonly IStringLocalizer<AccountController> _stringLocalizer;

        public AccountController(NovaFriburgoDBContext context, JwtSettings jwtSettings, IStringLocalizer<AccountController> stringLocalizer, IStringLocalizer<SharedResource> sharedResourceLocalizer)
        {
            _context = context;
            _jwtSettings = jwtSettings;
            _stringLocalizer = stringLocalizer;
            _sharedResourceLocalizer = sharedResourceLocalizer;
        }

        // Example Users
        // TODO: Change by real users in DB
        private IEnumerable<User> Logins = new List<User>()
        {
            new User()
            {
                Id = 1,
                Email = "tecnicosindependencia@gmail.com",
                Name = "Admin",
                Password = "Admin"
            },
            new User()
            {
                Id = 2,
                Email = "a.montoya1@hotmail.com",
                Name = "User1",
                Password = "andrea"
            }
        };

        [HttpPost]
        public IActionResult GetToken(UserLogins userLogin)
        {
            try
            {

                var Token = new UserTokens();

                // TODO:
                // Search a user in context with LINQ
                var searchUser = (from user in _context.Users
                                  where user.Name == userLogin.UserName && user.Password == userLogin.Password
                                  select user).FirstOrDefault();

                var hello = _stringLocalizer.GetString("Welcome").Value ?? String.Empty;


                // TODO: Change to searchUser
                // var Valid = Logins.Any(user => user.Name.Equals(userLogin.UserName, StringComparison.OrdinalIgnoreCase));

                if (searchUser != null)
                {
                    // var user = Logins.FirstOrDefault(user => user.Name.Equals(userLogin.UserName, StringComparison.OrdinalIgnoreCase));

                    Token = JwtHelpers.GenTokenKey(new UserTokens()
                    {
                        UserName = searchUser.Name,
                        EmailId = searchUser.Email,
                        Id = searchUser.Id,
                        GuidId = Guid.NewGuid(),

                    }, _jwtSettings);

                }
                else
                {
                    return BadRequest("Wrong Password");
                }

                return Ok( Token, new
                {
                    HelloName = hello,
                });
            }
            catch (Exception ex)
            {
                throw new Exception("GetToken Error", ex);
            }
        }

        private IActionResult Ok(UserTokens token, object value)
        {
            throw new NotImplementedException();
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Administrator")]
        public IActionResult GetUserList()
        {
            return Ok(Logins);
        }
    }
}
