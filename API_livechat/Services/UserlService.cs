using API_livechat.Models;
using API_livechat.Repositories;
using Microsoft.EntityFrameworkCore.Metadata;
using API_livechat.DTO;

namespace API_livechat.Services
{
    public class UserlService
    {
        #region service
        private readonly UserlRepository _repository;

        public UserlService(UserlRepository repository)
        {
            _repository = repository;
        }
        #endregion

        #region private metods
        List<UserlDTO> ConvertToUsersDTO(List<Userl> users)
        {
            return users.Select(u => new UserlDTO()
            {
                User = u.Username,
                Pass = u.Passwrd
            }).ToList();
        }

        UserlDTO ConvertToUserDTO(Userl user)
        {
            return new UserlDTO()
            {
                User = user.Username,
                Pass = user.Passwrd
            };
        }
        #endregion

        /*
        public bool UpdateUserPassword(Userl user);
        bool DeleteByPassword(string pwd);
         */

        public bool Register(UserlDTO userDTO)
        {
            return _repository.Register(new Userl()
            {
                Username = userDTO.User,
                Passwrd = BCrypt.Net.BCrypt.HashPassword(userDTO.Pass) //SHA384
            });
        }

        public List<UserlDTO> GetListOfUsers()
        {
            return ConvertToUsersDTO(_repository.GetListOfUsers());
        }

        public UserlDTO GetByUsername(UserlDTO userDTO)
        {
            return ConvertToUserDTO(_repository.GetByUsername(userDTO.User));
        }

        public UserlDTO GetByPassword(UserlDTO userDTO)
        {
            return ConvertToUserDTO(_repository.GetByPassword(userDTO.Pass));
        }

        public bool UpdateUserPassword(UserlDTO userDTO, string newPassword)
        {
            Userl user = _repository.GetByUsername(userDTO.User);

            user.Passwrd = BCrypt.Net.BCrypt.HashPassword(newPassword);

            return _repository.UpdateUserPassword(user);


        }

        public bool CheckUser(UserlDTO userDTO)
        {
            return (GetByUsername(userDTO) == null || GetByPassword(userDTO) == null) ? false : true;
        }
    }
}
