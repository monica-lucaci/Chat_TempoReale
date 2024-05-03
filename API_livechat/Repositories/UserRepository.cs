using API_livechat.Models;
using Microsoft.EntityFrameworkCore;

namespace API_livechat.Repositories
{
    public class UserRepository : IUserRepository
    {
        #region context, logger
        private readonly ILogger _logger;
        private readonly loginContext _dbContext;

        public UserRepository(loginContext dbContext, ILogger<UserRepository> logger)
        {
            _logger = logger;
            _dbContext = dbContext;
        }
        #endregion

        #region crud login
        public List<UserProfile> GetListOfUsers()
        {
            try
            {
                return _dbContext.Users.ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new List<UserProfile>();
        }

        public UserProfile GetByCode(string code)
        {
            try
            {
                return _dbContext.Users.Single(u => u.Code == code);
            }
            catch { }

            return new UserProfile();
        }

        public bool UpdateUser(UserProfile user)
        {
            try
            {
                _dbContext.Users.Update(user);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return false;
        }

        public bool Register(UserProfile user)
        {
            try
            {
                _dbContext.Users.Add(user);
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return false;
        }

        public bool DeleteByCode(string code)
        {
            try
            {
                _dbContext.Remove(_dbContext.Users.Single(u => u.Code == code));
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return false;
        }

        public bool DeleteByPassword(string pwd)
        {
            try
            {
                _dbContext.Remove(_dbContext.Users.Single(u => u.Passwrd == pwd));
                _dbContext.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return false;
        }

        public UserProfile GetByUsername(string username) 
        {
            try
            {
                return _dbContext.Users.Single(u => u.Username == username);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }

            return new UserProfile();
        }

        public UserProfile GetByPassword(string password)
        {
            try
            {
                return _dbContext.Users.Single(u => u.Passwrd == password);
            }
            catch { }

            return new UserProfile();
        }

        public bool CheckLoginUser(UserLogin usL)
        {
            return (_dbContext.Users.FirstOrDefault(u => u.Username == usL.Username && u.Passwrd == usL.Passwrd)) != null;
        }
        #endregion
    }
}
