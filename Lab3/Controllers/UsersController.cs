using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Lab3.Models;
using Lab3.Services;
using Lab3.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Lab3.Controllers
{
    // https://jasonwatmore.com/post/2018/08/14/aspnet-core-21-jwt-authentication-tutorial-with-example-api
    //https://www.c-sharpcorner.com/article/compute-sha256-hash-in-c-sharp/

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private IUsersService userService;
        private IUserUserRolesService userUserRolesService;

        public UsersController(IUsersService userService, IUserUserRolesService userUserRolesService)
        {
            this.userService = userService;
            this.userUserRolesService = userUserRolesService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody]LoginPostModel login)
        {
            var user = userService.Authenticate(login.Username, login.Password);

            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            return Ok(user);
        }


        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody]RegisterPostModel registerModel)
        {
            var errors = userService.Register(registerModel);
            if (errors != null)
            {
                return BadRequest(errors);
            }
            return Ok();
        }


        /// <summary>
        /// Get all users
        /// </summary>
        /// <returns>A list of all users</returns>
        [HttpGet]
        //[Authorize(Roles = "Admin,UserManager")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IEnumerable<UserGetModel> GetAll()
        {
            return userService.GetAll();
        }


        /// <summary>
        /// Find an user by the given id.
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /users
        ///     {  
        ///        id: 3,
        ///        firstName = "Pop",
        ///        lastName = "Andrei",
        ///        userName = "user123",
        ///        email = "Us1@yahoo.com"
        ///     }
        /// </remarks>
        /// <param name="id">The id given as parameter</param>
        /// <returns>The user with the given id</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/Users/5
        [Authorize(Roles = "Admin,UserManager")]
        //[HttpGet("{id}", Name = "GetUser")]
        [HttpGet]
        [Route("~/api/users/det/{id}")]     //suprascrie ruta prestabilita [Route("api/[controller]")]
        public IActionResult Get(int id)
        {
            var found = userService.GetById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }



        /// <summary>
        /// Find an userUserRole by the given id.
        /// </summary>
        /// <remarks>
        /// Sample response:
        ///
        ///     Get /userUserRoles
        ///     [
        ///     {  
        ///        id: 3,
        ///        userId = 2,
        ///        UserRoleId = 3,
        ///        UserRole = "Regular",
        ///        StartTime = 2019-06-05,
        ///        EndTime = null
        ///     }
        ///     ]
        /// </remarks>
        /// <param name="id">The id given as parameter</param>
        /// <returns>A list of userUserRole with the given id</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        // GET: api/UserUserRoles/5
        //[HttpGet("{id}", Name = "GetUser")]
        [HttpGet]
        [Route("~/api/users/history/{id}")]     //suprascrie ruta prestabilita [Route("api/[controller]")]
        public IActionResult GetHistoryRole(int id)
        {
            var found = userUserRolesService.GetHistoryRoleById(id);
            if (found == null)
            {
                return NotFound();
            }
            return Ok(found);
        }



        /// <summary>
        /// Add an new User
        /// </summary>
        ///   /// <remarks>
        /// Sample response:
        ///
        ///     Post /users
        ///     {
        ///        firstName = "Pop",
        ///        lastName = "Andrei",
        ///        userName = "user123",
        ///        email = "Us1@yahoo.com",
        ///        password = "feff35ffdasd"
        ///     }
        /// </remarks>
        /// <param name="userPostModel">The input user to be added</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [Authorize(Roles = "Admin,UserManager")]
        [HttpPost]
        public void Post([FromBody] RegisterPostModel userPostModel)        //pentru creare de User
        {
            userService.Create(userPostModel);
        }


        /// <summary>
        /// Add an new UserUserRole
        /// </summary>
        ///   /// <remarks>
        /// Sample response:
        ///
        ///     Put /userUserRoles
        ///     {
        ///        userId = 1,
        ///        userRoleName = "UserManager"        
        ///     }
        /// </remarks>
        /// <param name="userUserRolePostModel">The input userUserRole to be added</param>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin,UserManager")]
        [HttpPost]
        [Route("~/api/users/chrole")]     //suprascrie ruta prestabilita [Route("api/[controller]")]
        public IActionResult Post([FromBody] UserUserRolePostModel userUserRolePostModel)        //pentru creare de UserUserRole cu legatura manytomany intre User si UserRole
        {
            User curentUserLogIn = userService.GetCurentUser(HttpContext);
            string roleNameLoged = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;

            string curentUserRoleName = userUserRolesService.GetUserRoleNameById(userUserRolePostModel.UserId);

            if (roleNameLoged.Equals("UserManager"))
            {
                var anulUserRegistered = curentUserLogIn.DataRegistered;        //data inregistrarii
                var curentMonth = DateTime.Now;                                 //data curenta
                var nrLuni = curentMonth.Subtract(anulUserRegistered).Days / (365.25 / 12);   //diferenta in luni dintre datele transmise

                if (nrLuni >= 6)
                {
                    string activRoleName = userUserRolesService.GetUserRoleNameById(userUserRolePostModel.UserId);

                    if (activRoleName.Equals("Admin"))
                    {
                        return Forbid("Nu ai Rolul necesar pentru aceasta operatie !");
                    }

                    if ((activRoleName.Equals("UserManager") | activRoleName.Equals("Regular")) && userUserRolePostModel.UserRoleName.Equals("Admin"))
                    {
                        return Forbid("Nu ai Rolul necesar pentru aceasta operatie !");
                    }
                }
                else
                {
                    return Forbid("Nu ai Vechimea necesara ca UserManager pentru aceasta operatie !");
                }
            }

            userUserRolesService.Create(userUserRolePostModel);
            return Ok();
        }


        /// <summary>
        /// Modify an user if exists in dbSet , or add if not exist
        /// </summary>
        /// <param name="id">id-ul user to update</param>
        /// <param name="userPostModel">obiect userPostModel to update</param>
        /// Sample request:
        ///     <remarks>
        ///     Put /users/id
        ///     {
        ///        firstName = "Pop",
        ///        lastName = "Andrei",
        ///        userName = "user123",
        ///        email = "Us1@yahoo.com",
        ///        password = "feff35ffdasd"
        ///     }
        /// </remarks>
        /// <returns>Status 200 daca a fost modificat</returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin,UserManager")]
        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] UserPostModel userPostModel)
        {
            User curentUserLogIn = userService.GetCurentUser(HttpContext);
            string roleNameLoged = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;

            string curentUserRoleName = userUserRolesService.GetUserRoleNameById(id);
            
            if (roleNameLoged.Equals("UserManager"))
            {
                //UserGetModel userToUpdate = userService.GetById(id);

                var anulUserRegistered = curentUserLogIn.DataRegistered;        //data inregistrarii
                var curentMonth = DateTime.Now;                                 //data curenta
                var nrLuni = curentMonth.Subtract(anulUserRegistered).Days / (365.25 / 12);   //diferenta in luni dintre datele transmise

                if (nrLuni < 6)
                {                      
                    return Forbid("Nu ai Vechimea necesara ca UserManager pentru aceasta operatie !");
                }

                //UserPostModel newUserPost = new UserPostModel
                //{
                //    FirstName = userPostModel.FirstName,
                //    LastName = userPostModel.LastName,
                //    UserName = userPostModel.UserName,
                //    Email = userPostModel.Email,
                //    Password = userPostModel.Password
                //    //UserRole = activUserUserRoleName
                //};

                //var result2 = userService.Upsert(id, newUserPost);
                //return Ok(result2);
            }

            var result = userService.Upsert(id, userPostModel);
            return Ok(result);
        }



        /// <summary>
        /// Delete an user
        /// </summary>
        /// <param name="id">User id to delete</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [Authorize(Roles = "Admin,UserManager")]
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            string roleNameLoged = HttpContext.User.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role).Value;

            if (roleNameLoged.Equals("UserManager"))
            {
                UserGetModel userToDelete = userService.GetById(id);

                string activRoleName = userUserRolesService.GetUserRoleNameById(userToDelete.Id);

                if (activRoleName.Equals("Admin"))
                {
                    return Forbid("Nu ai Rolul necear pentru aceasta operatie !");
                }
            }
            var result = userService.Delete(id);
            if (result == null)
            {
                return NotFound("User with the given id not fount !");
            }
            return Ok(result);
        }


















    }
}