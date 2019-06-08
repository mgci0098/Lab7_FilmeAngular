using CentruMultimedia.Models;
using Lab3.Models;
using Lab3.Validators;
using Lab3.ViewModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lab3.Services
{
    public interface IUserUserRolesService
    {
        IQueryable<UserUserRoleGetModel> GetById(int id);
        ErrorsCollection Create(UserUserRolePostModel userUserRolePostModel);

        string GetUserRoleNameById(int id);

    }

    public class UserUserRolesService : IUserUserRolesService
    {
        private FilmeDbContext context;
        private IUserRoleValidator userRoleValidator;


        public UserUserRolesService(IUserRoleValidator userRoleValidator, FilmeDbContext context)
        {
            this.context = context;
            this.userRoleValidator = userRoleValidator;
        }

        public IQueryable<UserUserRoleGetModel> GetById(int id)
        {
            IQueryable<UserUserRole> userUserRole = context.UserUserRoles
                                    .Include(u => u.UserRole)
                                    .AsNoTracking()
                                    .Where(uur => uur.UserId == id)
                                    .OrderBy(uur => uur.StartTime);

            return userUserRole.Select(uur => UserUserRoleGetModel.FromUserUserRole(uur));
        }

        public string GetUserRoleNameById(int id)
        {
            int userRoleId = context.UserUserRoles
                .AsNoTracking()
                 .FirstOrDefault(uur => uur.UserId == id && uur.EndTime == null)
                 .UserRoleId;

            string numeRol = context.UserRoles
                  .AsNoTracking()
                  .FirstOrDefault(ur => ur.Id == userRoleId)
                  .Name;

            return numeRol;
        }

        

        public ErrorsCollection Create(UserUserRolePostModel userUserRolePostModel)
        {
            var errors = userRoleValidator.Validate(userUserRolePostModel, context);
            if (errors != null)
            {
                return errors;
            }

            User user = context.Users
                .FirstOrDefault(u => u.Id == userUserRolePostModel.UserId);

            if (user != null)
            {
                UserRole userRole = context
                               .UserRoles
                               .Include(ur => ur.UserUserRoles)
                               .FirstOrDefault(ur => ur.Name == userUserRolePostModel.UserRoleName);

                UserUserRole curentUserUserRole = context.UserUserRoles
                                .Include(uur => uur.UserRole)
                                .FirstOrDefault(uur => uur.UserId == user.Id && uur.EndTime == null);

                if (curentUserUserRole == null)
                {
                    context.UserUserRoles.Add(new UserUserRole
                    {
                        User = user,
                        UserRole = userRole,
                        StartTime = DateTime.Now,
                        EndTime = null
                    });

                    context.SaveChanges();
                    return null;
                }

                //inchiderea perioadel de activare pentru un anumit rol
                if (!curentUserUserRole.UserRole.Name.Contains(userUserRolePostModel.UserRoleName))
                {
                    curentUserUserRole.EndTime = DateTime.Now;

                    context.UserUserRoles.Add(new UserUserRole
                    {
                        User = user,
                        UserRole = userRole,
                        StartTime = DateTime.Now,
                        EndTime = null
                    });

                    context.SaveChanges();
                    return null;
                }
                else
                {
                    return null;    //trebuie sa trimit eroare ca modificarea nu poate avea loc, rol nou = rol vechi
                }
            }
            return null;    //eroare Nu exista User cu Id-ul specificat

        }


    }
}
