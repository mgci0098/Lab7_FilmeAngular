using CentruMultimedia.Models;
using Lab3.Controllers;
using Lab3.Models;
using Lab3.Services;
using Lab3.Validators;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
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
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, validator, null, config);
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
                Assert.AreEqual(added.UserName, context.Users.FirstOrDefault(u => u.Id == 1).Username);
                Assert.AreEqual(1, context.UserUserRoles.FirstOrDefault(uur => uur.Id == 1).UserId);
            }
        }

        /// <summary>
        /// TODO: AAA - Arrange, Act, Assert
        /// </summary>
        [Test]
        public void InvalidRegisterShouldReturnErrorsCollection()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
                         .UseInMemoryDatabase(databaseName: nameof(InvalidRegisterShouldReturnErrorsCollection))
                         .Options;

            using (var context = new FilmeDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, validator, null, config);
                var added = new Lab3.ViewModels.RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "lastName1",
                    UserName = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111"    //invalid password should invalidate register
                };

                var result = usersService.Register(added);

                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.ErrorMessages.Count());
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
                var validator = new RegisterValidator();
                var validatorUser = new UserRoleValidator();
                var userUserRoleService = new UserUserRolesService(validatorUser, context);
                var usersService = new UsersService(context, validator, userUserRoleService, config);

                UserRole addUserRoleRegular = new UserRole
                {
                    Name = "Regular",
                    Description = "Creat pentru testare"
                };            
                context.UserRoles.Add(addUserRoleRegular);
                context.SaveChanges();

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
                //valid authentification
                var authresult = usersService.Authenticate(added.UserName, added.Password);

                Assert.IsNotNull(authresult);
                Assert.AreEqual(1, authresult.Id);
                Assert.AreEqual(authenticated.Username, authresult.UserName);

                //invalid user authentification
                var authresult1 = usersService.Authenticate("unknown", "abcdefg");
                Assert.IsNull(authresult1); 
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
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, validator, null, config);
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
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, validator, null, config);
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
                var validator = new RegisterValidator();
                var validatorUser = new UserRoleValidator();
                var userUserRoleService = new UserUserRolesService(validatorUser, context);
                var usersService = new UsersService(context, validator, userUserRoleService, config);

                UserRole addUserRoleRegular = new UserRole
                {
                    Name = "Regular",
                    Description = "Creat pentru testare"
                };
                context.UserRoles.Add(addUserRoleRegular);
                context.SaveChanges();

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

                //nu stiu sa instantiez un HttpContext
                //usersService.GetCurentUser(httpContext);

                Assert.IsNotNull(authresult);
            }
        }


        [Test]
        public void CreateShouldReturnNullIfValidUserGetModel()
        {
            var options = new DbContextOptionsBuilder<FilmeDbContext>()
            .UseInMemoryDatabase(databaseName: nameof(CreateShouldReturnNullIfValidUserGetModel))
            .Options;

            using (var context = new FilmeDbContext(options))
            {
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, validator, null, config);

                UserRole addUserRoleRegular = new UserRole
                {
                    Name = "Regular",
                    Description = "Creat pentru testare"
                };
                context.UserRoles.Add(addUserRoleRegular);
                context.SaveChanges();

                var added1 = new Lab3.ViewModels.RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "firstName1",
                    UserName = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };

                var userCreated = usersService.Create(added1);

                Assert.IsNull(userCreated);
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
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, validator, null, config);
                var added1 = new Lab3.ViewModels.RegisterPostModel
                {
                    FirstName = "firstName1",
                    LastName = "firstName1",
                    UserName = "test_userName1",
                    Email = "first@yahoo.com",
                    Password = "111111"
                };

                var userCreated = usersService.Create(added1);

                Assert.IsNull(userCreated);
                Assert.AreEqual(1, usersService.GetAll().Count());

                var userDeleted = usersService.Delete(1);

                Assert.NotNull(userDeleted);
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
                var validator = new RegisterValidator();
                var usersService = new UsersService(context, validator, null, config);
                var added22 = new Lab3.ViewModels.RegisterPostModel
                {
                    FirstName = "Nume",
                    LastName = "Prenume",
                    UserName = "userName",
                    Email = "user@yahoo.com",
                    Password = "333333"
                };

                usersService.Create(added22);

                var updated = new Lab3.ViewModels.UserPostModel
                {
                    FirstName = "Alin",
                    LastName = "Popescu",
                    UserName = "popAlin",
                    Email = "pop@yahoo.com",
                    Password = "333333"
                };

                var userUpdated = usersService.Upsert(1, updated);

                Assert.NotNull(userUpdated);
                Assert.AreEqual("Alin", userUpdated.FirstName);
                Assert.AreEqual("Popescu", userUpdated.LastName);

            }
        }




















    }
}