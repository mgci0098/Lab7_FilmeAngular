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
                                    .Where(uur => uur.UserRoleId == id)
                                    .OrderBy(uur => uur.StartTime);

            return userUserRole.Select(uur => UserUserRoleGetModel.FromUserUserRole(uur));
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
                               .FirstOrDefault(ur => ur.Name == userUserRolePostModel.UserRoleName);

                UserUserRole curentUserUserRole = context.UserUserRoles
                    .FirstOrDefault(uur => uur.EndTime == null && uur.UserId == user.Id);

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
