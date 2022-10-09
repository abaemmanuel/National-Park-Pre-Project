using PreProject.Models;

namespace PreProject.Repository.IRepository
{
    public interface IUserRepository
    {
        //3 methods needed
        bool IsUniqueUser(string username); //To check id user is unique
        User Authenticate(string username, string password);
        User Register(string username, string password);  

    }
}
