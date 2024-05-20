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
        #region repository
        private readonly UserRepository _repository;
        private readonly ChatRoomRepository _crRepository;

        public UserService(UserRepository repository, ChatRoomRepository crRepository)
        {
            _repository = repository;
            _crRepository = crRepository;
        }
        #endregion

        #region private metods
        public List<UserDTO> ConvertToUsersDTO(List<UserProfile> users)
        {
            return users.Select(u => new UserDTO()
            {
                Email = u.Email,
                User = u.Username,
                Pass = u.Passwrd,
                Img = u.UsImg,
                CRCode = u.ChatRoomsCode
            }).ToList();
        }

        public UserDTO ConvertToUserDTO(UserProfile user)
        {
            return new UserDTO()
            {
                Email = user.Email,
                User = user.Username,
                Pass = user.Passwrd,
                Img = user.UsImg,
                CRCode = user.ChatRoomsCode
            };
        }

        public UserLoginDTO ConvertToUserLoginDTO(UserProfile user)
        {
            return new UserLoginDTO()
            {
                Email = user.Email,
                User = user.Username,
                Pass = user.Passwrd
            };
        }

        public UserLogin ConvertToUserLogin(UserLoginDTO userLDTO)
        {
            return new UserLogin()
            {
                Email = userLDTO.Email,
                Username = userLDTO.User,
                Passwrd = userLDTO.Pass
            };
        }

        ChatRoomDTO ConvertToChatRoomDTO(ChatRoom chatRoom)
        {
            return new ChatRoomDTO()
            {
                CRId = chatRoom.ChatRoomId,
                CRCd = chatRoom.ChatRoomCode,
                Titl = chatRoom.Title,
                Desc = chatRoom.Description,
                CRImg = chatRoom.Image,
                Usrs = chatRoom.Users.ToList()
            };
        }
        #endregion

        public bool Register(UserLoginDTO userDTO, string img)
        {
            if (CheckUserReg(userDTO))
            {
                return _repository.Register(new UserProfile()
                {
                    Email = userDTO.Email,
                    Username = userDTO.User,
                    Passwrd = BCrypt.Net.BCrypt.HashPassword(userDTO.Pass), //SHA384
                    UsImg = img
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

        public UserDTO GetByUserLog(UserLoginDTO userLoginDTO)
        {
            return ConvertToUserDTO(_repository.GetByUsername(userLoginDTO.User));
        }

        public UserDTO GetByPassword(UserDTO userDTO)
        {
            return ConvertToUserDTO(_repository.GetByPassword(userDTO.Pass));
        }

        public UserDTO? GetUser(string name)
        {
            UserProfile? u = _repository.GetByUsername(name);
            if (u != null) return ConvertToUserDTO(u);
            return null;
        }

        public bool UpdateUserPassword(UserLoginDTO userDTO, string newPassword)
        {
            UserProfile user = _repository.GetByUsername(userDTO.User);
            user.Passwrd = BCrypt.Net.BCrypt.HashPassword(newPassword);
            return _repository.UpdateUser(user);
        }
        public bool UpdateImage(UserLoginDTO userLDTO, string newImage)
        {
            if (CheckUserLog(userLDTO))
            {
                UserProfile user = _repository.GetByUsername(userLDTO.User);
                user.UsImg = newImage;
                return _repository.UpdateUser(user);
            }
            return false;
        }

        public bool CheckUserLog(UserLoginDTO userLDTO)
        {
            UserLoginDTO us = ConvertToUserDTO(_repository.GetByUsername(userLDTO.User));
            if (us.Pass == null || us.User == null) return false;
            bool verified = BCrypt.Net.BCrypt.Verify(userLDTO.Pass, us.Pass);
            if (!verified) return false;
            if (userLDTO.Email != us.Email) return false;
            return true;
        }

        public bool CheckUserReg(UserLoginDTO userLDTO)
        {
            UserLoginDTO us = ConvertToUserLoginDTO(_repository.GetByUsername(userLDTO.User));
            UserLogin userLogin = ConvertToUserLogin(us);
            return userLogin != null;
        }

        public bool CheckEmailReg(string email)
        {
            foreach (UserLoginDTO usr in GetListOfUsers())
            {
                if (usr.Email == email) return true;
            }
            return false;
        }

        public bool DeleteImage(UserLoginDTO userLDTO)
        {
            if (CheckUserLog(userLDTO))
            {
                UserProfile user = _repository.GetByUsername(userLDTO.User);
                user.UsImg = null;
                return _repository.UpdateUser(user);
            }
            return false;
        }

        public bool DeleteUser(UserLoginDTO userLDTO)
        {
            if (CheckUserLog(userLDTO))
            {

                return _crRepository.DeleteUserFromAllChatRooms(userLDTO.User) &&
                        _repository.DeleteByUser(userLDTO.User) &&
                        _crRepository.ChechEmptyChatrooms();
            }
            return false;
        }
    }
}