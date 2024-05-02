using API_livechat.Models;
using Microsoft.EntityFrameworkCore;

namespace API_livechat.Repositories
{
    public class UserlRepository : IUserlRepository
    {
        #region context
        private readonly loginContext _dbContext;

        public UserlRepository(loginContext dbContext)
        {
            _dbContext = dbContext;
        }
        #endregion

        #region crud login
        public bool DeleteByPassword(string pwd)
        {
            try
            {
                _dbContext.Remove(_dbContext.Users.Single(u => u.Passwrd == pwd));
                _dbContext.SaveChanges();
                return true;
            }
            catch { }

            return false;
        }

        public List<Userl> GetListOfUsers()
        {
            try
            {
                return _dbContext.Users.ToList();
            }
            catch { }

            return new List<Userl>();
        }

        public Userl GetByUsername(string username) 
        {
            try
            {
                return _dbContext.Users.Single(u => u.Username == username);
            }
            catch { }

            return new Userl();
        }

        public Userl GetByPassword(string password)
        {
            try
            {
                return _dbContext.Users.Single(u => u.Passwrd == password);
            }
            catch { }

            return new Userl();
        }

        public bool Register(Userl user)
        {
            try
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return true;
            }
            catch { }
            return false;
        }

        public bool UpdateUserPassword(Userl user)
        {
            try
            {
                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();
                return true;
            }
            catch { }
            return false;
        }
        #endregion
    }
}
