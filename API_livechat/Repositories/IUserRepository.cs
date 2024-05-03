namespace API_livechat.Repositories{
    
    using Models;
    public interface IUserRepository
    {
        public List<UserProfile> GetListOfUsers();
        public UserProfile GetByCode(string code);
        public bool UpdateUser(UserProfile user);
        public bool Register(UserProfile user);
        bool DeleteByCode(string pwd);
        public bool DeleteByPassword(string pwd);

    }
}
