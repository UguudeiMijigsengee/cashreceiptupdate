using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Options;
using Model.Model.Login;
using Model.Model.Log;
using Model.Model;
using System.DirectoryServices.AccountManagement;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using DataAccess.Persistence.Log;
using DataAccess.Persistence.UnitOfWork;
using System.Threading.Tasks;
using System.Linq;
using Model.Model.UserService;

namespace DataAccess.Services
{
    public class UserService : IUserService
    {
        private readonly Config _config;
        private readonly ILogRepository logRepository;
        private readonly IUnitOfWork unitOfWork;

        public UserService(IOptions<Config> config, 
                           ILogRepository logRepository,
                           IUnitOfWork unitOfWork)
        {
            _config = config.Value;
            this.logRepository = logRepository;
            this.unitOfWork = unitOfWork;
        }
        public async Task<LoginResponse> AuthenticateAsync(LoginModel loginModel)
        {
            var loginResponse = new LoginResponse();
            try
            {
                var loggedIn = false;
                var isPassWordLocked = false;
                using (var adContext = new PrincipalContext(ContextType.Domain, _config.UserServiceConfig.Domain))
                {
                    loggedIn = adContext.ValidateCredentials(loginModel.userName, loginModel.passWord);
                    UserPrincipal user = UserPrincipal.FindByIdentity(adContext, loginModel.userName);
                    if (user == null)
                        isPassWordLocked = false;
                    else 
                        isPassWordLocked = user.IsAccountLockedOut();
                }

                if (loggedIn)
                {
                    // authentication successful so generate jwt token
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(_config.UserServiceConfig.Key);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new Claim[]
                        {
                                new Claim(ClaimTypes.Name, loginModel.userName)
                        }),
                        Expires = DateTime.UtcNow.AddHours(_config.UserServiceConfig.expirationDurationHours),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                    };

                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    loginResponse.loggedIn = true;
                    loginResponse.token = tokenHandler.WriteToken(token);
                }
                else
                {
                    // User is not logged in
                    loginResponse.loggedIn = false;
                    // Response is either account is locked or User name or Password is Incorrect
                    loginResponse.token = (isPassWordLocked ? _config.UserServiceConfig.accountMessages[1] : _config.UserServiceConfig.accountMessages[0]);
                }                
            }
            catch (Exception ex)
            {
                logRepository.Add(
                    new tblProcessErrorLog
                    {
                        dtstamp = DateTime.Now,
                        ProcessName = _config.name,
                        SQLStmt = loginModel.userName,
                        ErrorString = ex.ToString()
                    }
                );
                await unitOfWork.CompleteAsync();
            }

            // not authenticated
            return loginResponse;
        }

        public async Task<List<UserRoute>> checkIfUserMemberOfAsync(string userName)
        {
            var userRoutes = new List<UserRoute>();
            //Add default home route
            userRoutes.Add(_config.UserServiceConfig.UserRoutes[0]);

            try
            {
                var routeCounter = 1;
                using (var adContext = new PrincipalContext(ContextType.Domain, _config.UserServiceConfig.Domain))
                {
                    UserPrincipal user = UserPrincipal.FindByIdentity(adContext, userName);

                    for (var i = 1; i < _config.UserServiceConfig.UserRoutes.Count; i++)
                    {
                        GroupPrincipal group = GroupPrincipal.FindByIdentity(adContext, _config.UserServiceConfig.UserRoutes[i].group);
                        if (user.IsMemberOf(group))
                        {
                            userRoutes.Add(new UserRoute
                            {
                                id = routeCounter,
                                group = "",
                                header = _config.UserServiceConfig.UserRoutes[i].header,
                                route = _config.UserServiceConfig.UserRoutes[i].route
                            });
                            routeCounter++;
                        }
                    }                                        
                }
            }
            catch (Exception ex)
            {
                logRepository.Add(
                    new tblProcessErrorLog
                    {
                        dtstamp = DateTime.Now,
                        ProcessName = _config.name,
                        SQLStmt = userName,
                        ErrorString = ex.ToString()
                    }
                );
                await unitOfWork.CompleteAsync();
            }

            return userRoutes;
        }

        public string getUserName(IEnumerable<Claim> claims)
        {
            var targetClaim = claims.FirstOrDefault(i => i.Type == _config.UserServiceConfig.user_name);
            if (targetClaim == null)
                return _config.UserServiceConfig.userNotFound;
            return targetClaim.Value;
        }

        public bool isUserMemberOfGroup(string groupName, string userName)
        {
            bool isMember = false;

            try
            {
                using (var adContext = new PrincipalContext(ContextType.Domain, _config.UserServiceConfig.Domain))
                {
                    UserPrincipal user = UserPrincipal.FindByIdentity(adContext, userName);

                    GroupPrincipal group = GroupPrincipal.FindByIdentity(adContext, groupName);

                    if (user.IsMemberOf(group))
                        isMember = true;
                }
            }
            catch (Exception ex)
            {

            }
            return isMember;
        }
    }
}
