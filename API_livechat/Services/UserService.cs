using API_livechat.Models;
using API_livechat.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;
using API_livechat.DTO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;

namespace API_livechat.Services
{
    public class UserService
    {
        #region service
        private readonly UserRepository _repository;

        public UserService(UserRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region private metods
        List<UserDTO> ConvertToUsersDTO(List<UserProfile> users)
        {
            return users.Select(u => new UserDTO()
            {
                User = u.Username,
                Pass = u.Passwrd
            }).ToList();
        }

        UserDTO ConvertToUserDTO(UserProfile user)
        {
            return new UserDTO()
            {
                User = user.Username,
                Pass = user.Passwrd
            };
        }

        public UserLogin ConvertToUserLogin(UserDTO userDTO)
        {
            return new UserLogin()
            {
                Username = userDTO.User,
                Passwrd = userDTO.Pass
            };
        }
        #endregion

        /*
        public bool UpdateUserPassword(Userl user);
        bool DeleteByPassword(string pwd);
         */

        public bool Register(UserDTO userDTO)
        {
            if(CheckUserReg(userDTO)) {
                return _repository.Register(new UserProfile()
                {
                    Username = userDTO.User,
                    Passwrd = BCrypt.Net.BCrypt.HashPassword(userDTO.Pass) //SHA384
                });
            }
            return false;            
        }

        public List<UserDTO> GetListOfUsers()
        {
            return ConvertToUsersDTO(_repository.GetListOfUsers());
        }

        public UserDTO GetByUsername(UserDTO userDTO)
        {

            return ConvertToUserDTO(_repository.GetByUsername(userDTO.User));
        }

        public UserDTO GetByPassword(UserDTO userDTO)
        {
            return ConvertToUserDTO(_repository.GetByPassword(userDTO.Pass));
        }

        public UserDTO? GetUser(string name)
        {
            UserProfile? u = _repository.GetByUsername(name);
            
            if (u != null)
            {
                return ConvertToUserDTO(u);
            }

            return null;
        }

        public bool UpdateUserPassword(UserDTO userDTO, string newPassword)
        {
            UserProfile user = _repository.GetByUsername(userDTO.User);

            user.Passwrd = BCrypt.Net.BCrypt.HashPassword(newPassword);

            return _repository.UpdateUser(user);
        }

        public bool CheckUserLog(UserDTO userDTO)
        {
            UserDTO us = ConvertToUserDTO(_repository.GetByUsername(userDTO.User));
            
            if(us.Pass == null || us.User == null) return false;

            bool verified = BCrypt.Net.BCrypt.Verify(userDTO.Pass, us.Pass);

            if (!verified) return false;

            UserLogin userLogin = ConvertToUserLogin(us);
            return true;
        }

        public bool CheckUserReg(UserDTO userDTO)
        {
            UserDTO us = ConvertToUserDTO(_repository.GetByUsername(userDTO.User));
            UserLogin userLogin = ConvertToUserLogin(us);
            return userLogin != null;
        }

        public static bool VerifySha384Hash(string inputData, string storedHash)
        {
            // Hash the input.
            string hashOfInput = BCrypt.Net.BCrypt.HashPassword(inputData);

            // Create a StringComparer and compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            return comparer.Compare(hashOfInput, storedHash) == 0;
        }
    }
}
