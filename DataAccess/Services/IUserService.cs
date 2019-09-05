using System;
using System.Collections.Generic;
using System.Text;
using Model.Model.Login;
using System.Threading.Tasks;
using System.Security.Claims;
using Model.Model.UserService;

namespace DataAccess.Services
{
    public interface IUserService
    {
        Task <LoginResponse> AuthenticateAsync(LoginModel login);
        Task <List<UserRoute>> checkIfUserMemberOfAsync(string userName);
        string getUserName(IEnumerable<Claim> claims);
        bool isUserMemberOfGroup(string groupName, string userName);
    }
}
