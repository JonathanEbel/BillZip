using Core;
using Core.Pagination;
using Identity.Models;
using System;
using System.Linq;
using Core.Cryptography;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Repos
{
    public interface IApplicationUserRepository : IRepository<ApplicationUser, Guid>
    {
        PagedResult<ApplicationUser> FindAll(QueryConstraints<ApplicationUser> constraints);
        PagedResult<ApplicationUser> FindByUserName(string text, QueryConstraints<ApplicationUser> constraints);
        bool UserAuthenticates(string userName, string password);
        ApplicationUser Get(string userName);
    }


    public class ApplicationUserRepository : IApplicationUserRepository, IDisposable
    {
        private readonly IdentityContext _dbContext;

        public ApplicationUserRepository(IdentityContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Delete(ApplicationUser entity)
        {
            _dbContext.ApplicationUsers.Remove(entity);
        }

        public PagedResult<ApplicationUser> FindByUserName(string text, QueryConstraints<ApplicationUser> constraints)
        {
            var query = _dbContext.ApplicationUsers.Where(x => x.UserName.StartsWith(text));
            var count = query.Count();
            var items = constraints.ApplyTo(query).ToList();

            return new PagedResult<ApplicationUser>(items, count);
        }

        public PagedResult<ApplicationUser> FindAll(QueryConstraints<ApplicationUser> constraints)
        {
            var items = constraints.ApplyTo(_dbContext.ApplicationUsers).ToList();
            var count = items.Count;

            return new PagedResult<ApplicationUser>(items, count);
        }

        public void Add(ApplicationUser entity)
        {
            _dbContext.ApplicationUsers.Add(entity);
        }

        public ApplicationUser Get(Guid id)
        {
            return _dbContext.ApplicationUsers.Where(x => x.Id == id).FirstOrDefault();
        }

        public ApplicationUser Get(string userName)
        {
            return _dbContext.ApplicationUsers.Include(x => x.Claims).Where(x => x.UserName == userName).FirstOrDefault();
        }

        public bool UserAuthenticates(string userName, string password)
        {
            var user = _dbContext.ApplicationUsers.Where(x => x.UserName == userName).FirstOrDefault();

            //this username is no good...
            if (user == null)
                return false;

            //if the username exists check the password...
            if (Crypto.getHash(password + user.Salt) == user.Password)
                return true;

            //user must have wrong password
            return false;
        }

        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public void Dispose()
        {
            _dbContext.Dispose();
        }
    }
}
