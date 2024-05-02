namespace API_livechat.Repositories{
    
    using Models;
    public interface IUserlRepository
    {
        public List<Userl> GetListOfUsers();
        public bool UpdateUserPassword(Userl user);
        public bool Register(Userl user);
        bool DeleteByPassword(string pwd);
        
    }
}
