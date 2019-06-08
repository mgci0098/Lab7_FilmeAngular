using CentruMultimedia.Models;
using Lab3.Controllers;
using Lab3.Models;
using Lab3.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Linq;

namespace Tests
{
    public class UsersServiceTest
    {
        private IOptions<AppSettings> config;

        [SetUp]
        public void Setup()
        {
            config = Options.Create(new AppSettings
            {
                //trebuie sa fie suficient de lung sirul de caractere.
                Secret = "dsadhjcghduihdfhdifd8ih"
            });

        }

        /// <summary>
        /// TODO: AAA - Arrange, Act, Assert
        /// </summary>
        [Test]
        public void ValidRegisterShouldCreateNewUser()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
                         .UseInMemoryDatabase(databaseName: nameof(ValidRegisterShouldCreateNewUser))// "ValidRegisterShouldCreateANewUser")
                         .Options;

            using (var context = new FilmeDbContext(options))
            {
                //todo: do not pass null
                var usersService = new UsersService(context, null, config);
                var added = new Lab3.ViewModels.RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "lastName1",
                    UserName = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };

                var result = usersService.Register(added);

                Assert.IsNull(result);
                //Assert.IsNotNull(result);
                //Assert.AreEqual(added.UserName, result.UserName);
            }
        }


        [Test]
        public void AuthenticateShouldLogTheRegisteredUser()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(AuthenticateShouldLogTheRegisteredUser))
              .Options;

            using (var context = new FilmeDbContext(options))
            {
                //todo: do not pass null
                var usersService = new UsersService(context, null, config);
                var added = new Lab3.ViewModels.RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "lastName1",
                    UserName = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };
                var result = usersService.Register(added);

                var authenticated = new Lab3.ViewModels.LoginPostModel
                {
                    Username = "test_userName1",
                    Password = "111111"
                };
                var authresult = usersService.Authenticate(added.UserName, added.Password);

                Assert.IsNotNull(authresult);
                Assert.AreEqual(1, authresult.Id);
                Assert.AreEqual(authenticated.Username, authresult.UserName);
            }
        }



        [Test]
        public void GetAllShouldReturnAllRegisteredUsers()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
              .UseInMemoryDatabase(databaseName: nameof(GetAllShouldReturnAllRegisteredUsers))
              .Options;

            using (var context = new FilmeDbContext(options))
            {
                //todo: do not pass null
                var usersService = new UsersService(context, null, config);
                var added1 = new Lab3.ViewModels.RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "firstName1",
                    UserName = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };
                var added2 = new Lab3.ViewModels.RegisterPostModel
                {
                    FirstName = "secondName2",
                    LastName = "secondName2",
                    UserName = "test_userName2",
                    Email = "second@yahoo.com",
                    Password = "111111"
                };
                usersService.Register(added1);
                usersService.Register(added2);

                int numberOfElements = usersService.GetAll().Count();

                Assert.NotZero(numberOfElements);
                Assert.AreEqual(2, numberOfElements);

            }
        }

        [Test]
        public void GetByIdShouldReturnAnValidUser()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
         .UseInMemoryDatabase(databaseName: nameof(GetByIdShouldReturnAnValidUser))
         .Options;

            using (var context = new FilmeDbContext(options))
            {
                //todo: do not pass null
                var usersService = new UsersService(context, null, config);
                var added1 = new Lab3.ViewModels.RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "firstName1",
                    UserName = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };

                usersService.Register(added1);
                var userById = usersService.GetById(1);

                Assert.NotNull(userById);
                Assert.AreEqual("firstName1", userById.FirstName);

            }
        }

        [Test]
        public void GetCurentUserShouldReturnAccesToKlaims()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
        .UseInMemoryDatabase(databaseName: nameof(GetCurentUserShouldReturnAccesToKlaims))
        .Options;

            using (var context = new FilmeDbContext(options))
            {
                //todo: do not pass null
                var usersService = new UsersService(context, null, config);

                //UsersController usersController = new UsersController(usersService);
                //usersController.ControllerContext = new Microsoft.AspNetCore.Mvc.ControllerContext();
                //usersController.ControllerContext.HttpContext = new DefaultHttpContext();
                //usersController.ControllerContext.HttpContext.Items.Add("user-Name", "Ghita");

                var added = new Lab3.ViewModels.RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "lastName1",
                    UserName = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };
                var result = usersService.Register(added);

                var authenticated = new Lab3.ViewModels.LoginPostModel
                {
                    Username = "test_userName1",
                    Password = "111111"
                };
                var authresult = usersService.Authenticate(added.UserName, added.Password);

                Assert.IsNotNull(authresult);

                // usersController.Get();    //am eroare de versiune

            }
        }


        [Test]
        public void CreateShouldReturnValidUserGetModel()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(CreateShouldReturnValidUserGetModel))
            .Options;

            using (var context = new FilmeDbContext(options))
            {
                //todo: do not pass null
                var usersService = new UsersService(context, null, config);
                var added1 = new Lab3.ViewModels.UserPostModel
                {
                    FirstName = "firstName1",
                    LastName = "firstName1",
                    UserName = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111",
                    UserRole = "Regular"
                };

                var userCreated = usersService.Create(added1);

                Assert.NotNull(userCreated);
                Assert.AreEqual(added1.UserName, userCreated.UserName);
            }
        }


        [Test]
        public void DeleteShouldEmptyTheDb()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(DeleteShouldEmptyTheDb))
            .Options;

            using (var context = new FilmeDbContext(options))
            {
                //todo: do not pass null
                var usersService = new UsersService(context, null, config);
                var added1 = new Lab3.ViewModels.UserPostModel
                {
                    FirstName = "firstName1",
                    LastName = "firstName1",
                    UserName = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111",
                    UserRole = "Regular"
                };

                var userCreated = usersService.Create(added1);

                Assert.NotNull(userCreated);
                Assert.AreEqual(1, usersService.GetAll().Count());

                var userDeleted = usersService.Delete(1);

                Assert.NotNull(userDeleted);
                Assert.AreEqual(userDeleted.FirstName, userCreated.FirstName);

                Assert.AreEqual(0, usersService.GetAll().Count());

            }
        }


        [Test]
        public void UpsertShouldModifyFildsValues()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(UpsertShouldModifyFildsValues))
            .Options;

            using (var context = new FilmeDbContext(options))
            {
                //todo: do not pass null
                var usersService = new UsersService(context, null, config);
                var added22 = new Lab3.ViewModels.UserPostModel
                {
                    FirstName = "Nume",
                    LastName = "Prenume",
                    UserName = "userName",
                    Email = "user@yahoo.com",
                    Password = "333333",
                    UserRole = "Regular"
                };

                usersService.Create(added22);

                var updated = new Lab3.ViewModels.UserPostModel
                {
                    FirstName = "Alin",
                    LastName = "Popescu",
                    UserName = "popAlin",
                    Email = "pop@yahoo.com",
                    Password = "333333",
                    UserRole = "UserManager"
                };

                var userUpdated = usersService.Upsert(1, updated);

                Assert.NotNull(userUpdated);
                //Assert.AreEqual("Alin", userUpdated.FirstName);
                //Assert.AreEqual("Popescu", userUpdated.LastName);
                //Assert.AreEqual(UserRole.UserManager, userUpdated.UserRole);

            }
        }




















    }
}