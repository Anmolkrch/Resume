
using DemoModel.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarisBrook.Service.IUserService
{
    public interface IUserService
    {
        /// <summary>
        /// User authentication on login
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        UserViewModel LoginAuthentication(LoginViewModel loginViewModel);


        /// <summary>
        /// Update user status on logout
        /// </summary>
        /// <param name="userId"></param>
        void UserLogOff(string userId);


        /// <summary>
        /// Save Users
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logId"></param>
        /// <param name="validateOnSaveEnabled"></param>
        /// <param name="mailBodyTemplate"></param>
        /// <returns></returns>
        bool SaveUsers(UserViewModel userViewModel, long logId = 0, bool validateOnSaveEnabled = true, string mailBodyTemplate = "");

        /// <summary>
        /// Update User information
        /// </summary>
        /// <param name="user"></param>        
        /// <returns></returns>
        bool UpdateUsers(UserViewModel userViewModel);

        /// <summary>
        /// Get user detail by Id
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        UserViewModel GetUsersDetailsById(long UserId);

        /// <summary>
        /// Get user detail by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        UserViewModel GetUsersDetailsByEmail(string email);
        bool IsUserExists(string emailAddress, string id);
        List<UserTypeViewModel> GetUserTypes();
        List<UserViewModel> GetUserList(int UserId, int RoleId);

       
    }
}
