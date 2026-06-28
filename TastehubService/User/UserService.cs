using Tastehub.Core.EntityModel;
using TastehubModel.ViewModel;
using ExpressMapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;

namespace Tastehub.Service.UserService
{
    public class UserService 
    {
       
        private OnBoadTaskEntities _Context = new OnBoadTaskEntities();
        #region Public_Methods

        /// <summary>
        /// User authentication on login
        /// </summary>
        /// <param name="loginViewModel"></param>
        /// <returns></returns>
        public UserViewModel LoginAuthentication(LoginViewModel loginViewModel)
        {
            UserViewModel userviewmodel = new UserViewModel();
            try
            {
                var user = _Context.Users.Include(item => item.UserType).Where(item => item.IsDeleted != true &&
                                                     item.IsActive == true &&
                                                     item.Email.Equals(loginViewModel.Email.Trim(), StringComparison.InvariantCultureIgnoreCase) &&
                                                     item.PasswordHash == loginViewModel.PasswordHash).FirstOrDefault();

                if (user != null)
                {
                    if (user.DefaultPassword == true)
                    {
                        //  throw new CustomException("Account is not activated. Please check your email.");

                    }
                    else if (user.AccountStatus == 2) // (int)Utility.Enums.AccountStatus.Inactive)
                    {
                      //  throw new CustomException("Account is not activated. Please check your email.");

                    }
                    else if (user.AccountStatus == 3)//(int)Utility.Enums.AccountStatus.Suspended)
                    {
                      //  throw new CustomException("Account has been suspended. Please contact to administrator.");
                    }
                    else if (user.AccountStatus == 4)//(int)Utility.Enums.AccountStatus.Deactivated)
                    {
                      //  throw new CustomException("Account has been deactivated. Please contact to administrator.");

                    }
                    else
                    {
                        user.IsOnLine = true;
                        user.LastLoginDate = DateTime.Now;
                        _Context.Configuration.ValidateOnSaveEnabled = false;
                        _Context.SaveChanges();
                    }

                    Mapper.Map(user, userviewmodel);
                    return userviewmodel;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        /// <summary>
        /// Update user status on logout
        /// </summary>
        /// <param name="userId"></param>
        public void UserLogOff(string userId)
        {
            if (!string.IsNullOrWhiteSpace(userId))
            {
                try
                {
                    long id = Convert.ToInt64(userId);
                    var user = _Context.Users.Where(item => item.Id == id).FirstOrDefault();

                    if (user != null)
                    {
                        user.IsOnLine = false;
                        _Context.Configuration.ValidateOnSaveEnabled = false;
                        _Context.SaveChanges();
                    }
                }
                catch (Exception ex)
                { }

            }

        }

        /// <summary>
        /// Save Users
        /// </summary>
        /// <param name="user"></param>
        /// <param name="logId"></param>
        /// <param name="validateOnSaveEnabled"></param>
        /// <param name="mailBodyTemplate"></param>
        /// <returns></returns>
        public bool SaveUsers(UserViewModel userViewModel, long logId = 0)
        {
            bool status = false;

            User users = new User();

            Mapper.Map(userViewModel, users);

            users.IsActive = true;
            users.CreatedOn = DateTime.Now;
            users.DefaultPassword = true;
            users.AccountStatus = 1;//(int)Utility.Enums.AccountStatus.Active;
            users.IsDeleted = false;
            users.CreatedBy = logId;
            //users.UserTypeId = userViewModel.us;//UserType will be selected at the time of registration

            _Context.Users.Add(users);
            _Context.SaveChanges();

            status = true;


            return status;
            // for new users
        }

        /// <summary>
        /// Update User information
        /// </summary>
        /// <param name="user"></param>
        /// <param name="ProfileMedia"></param>
        /// <returns></returns>
        public bool UpdateUsers(UserViewModel userViewModel)
        {
            bool status = false;
            try
            {
                //var _usrsaltdetails = _Context.Users.FirstOrDefault(x => x.Id == user.Id);
                var _usrsaltdetails = _Context.Users.Include(x => x.UserDetails).Where(x => x.Id == userViewModel.Id).FirstOrDefault();
                //var _vehicledetails = _Context.Vehicles.Where(x => x.Id == userViewModel.VehicleId).FirstOrDefault();

                if (_usrsaltdetails != null)
                {
                    _usrsaltdetails.UserTypeId = userViewModel.UserTypeId;
                    _usrsaltdetails.FirstName = userViewModel.FirstName;
                    _usrsaltdetails.LastName = userViewModel.LastName;
                    _usrsaltdetails.Email = userViewModel.Email;
                    _usrsaltdetails.PhoneNumber = userViewModel.PhoneNumber;
                    _usrsaltdetails.WorkPhone = userViewModel.WorkPhone;
                    _usrsaltdetails.UserName = userViewModel.UserName;
                    _usrsaltdetails.ReportingTo = userViewModel.ReportingTo;
                    _usrsaltdetails.AssignedRegionId = userViewModel.AssignedRegionId;
                    _usrsaltdetails.ModifiedBy = userViewModel.ModifiedBy;
                    _usrsaltdetails.ModifiedOn = DateTime.Now;

                    //if (_usrsaltdetails.UserDetails == null)
                    //{
                    //    _usrsaltdetails.UserDetails = new UserDetails();
                    //}


                    //if (_usrsaltdetails.UserDetails != null)
                    //{
                    //    _usrsaltdetails.UserDetails.CurAddress1 = userViewModel.UserDetails.CurAddress1;
                    //    _usrsaltdetails.UserDetails.CostPerMile = userViewModel.UserDetails.CostPerMile;
                    //    _usrsaltdetails.UserDetails.CostPerMinute = userViewModel.UserDetails.CostPerMinute;
                    //}
                    //Mapper.Map(userViewModel.UserDetails, _usrsaltdetails.UserDetails);

                    //if (_vehicledetails != null)
                    //{
                    //    _vehicledetails.RegistrationNo = userViewModel.RegistrationNo;
                    //    //_vehicledetails.ModelName = userViewModel.ModelName;
                    //    _vehicledetails.InsuranceNo = userViewModel.InsuranceNo;
                    //    _vehicledetails.Address1 = userViewModel.Address1;
                    //    _vehicledetails.PhoneNumber = userViewModel.PhoneNumber;
                    //    _vehicledetails.Inspectation = userViewModel.Inspectation;
                    //}

                    _Context.Configuration.ValidateOnSaveEnabled = false;
                    _Context.SaveChanges();
                    status = true;

                }

            }

            catch (Exception ex)
            {
            }
            // for new users
            return status;
        }

        /// <summary>
        /// Delete user
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        
        /// <summary>
        /// Get all Users
        /// </summary>
        /// <param name="searchingParams"></param>
        /// <returns></returns>
        
        /// <summary>
        /// Get user detail by Id
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public UserViewModel GetUsersDetailsById(long UserId)
        {
            UserViewModel useViewModel = new UserViewModel();
            var user = _Context.Users.Include(item => item.UserType).Include(x=>x.UserDetails).Where(x => x.Id == UserId).FirstOrDefault();

            if (user != null)
            {
                try
                {
                    Mapper.Map(user, useViewModel);
                    useViewModel.UserDetail = new UserDetailViewModel();
                    if (user.UserDetails.Count == 1)
                    {
                        useViewModel.UserDetail.CurAddress1 = user.UserDetails.FirstOrDefault().CurAddress1;
                        useViewModel.UserDetail.CurAddress2 = user.UserDetails.FirstOrDefault().CurAddress2;
                        useViewModel.UserDetail.CurCity = user.UserDetails.FirstOrDefault().CurCity;
                        useViewModel.UserDetail.CurZipCode = user.UserDetails.FirstOrDefault().CurZipCode;
                        useViewModel.UserDetail.ProfileUrl = user.UserDetails.FirstOrDefault().ProfileUrl;
                        useViewModel.UserDetail.Id = user.UserDetails.FirstOrDefault().Id;
                        useViewModel.UserDetail.UserId = user.UserDetails.FirstOrDefault().UserId;
                    }
                    Mapper.Map(user.UserDetails, useViewModel.UserDetail);
                }
                catch (Exception ex)
                {

                }
            }
            return useViewModel;
        }

        /// <summary>
        /// Get user detail by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public UserViewModel GetUsersDetailsByEmail(string email)
        {
          var user = _Context.Users.Include(item => item.UserDetails).Where(x => x.Email == email).FirstOrDefault();

            if (user.UserDetails == null)
                user.UserDetails = new List<UserDetail>();

            return Mapper.Map(user, new UserViewModel());
        }


        /// <summary>
        /// Check for User email exists
        /// </summary>
        /// <param name="emailAddress"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool IsUserExists(string emailAddress, string id)
        {
            dynamic user="";
            if (string.IsNullOrWhiteSpace(id))
            {
                user = _Context.Users.Where(item => item.IsDeleted != true && item.Email.ToLower() == emailAddress.ToLower()).FirstOrDefault();
            }
            else
            {
                long userId = Convert.ToInt64(id);
                user = _Context.Users.Where(item => item.IsDeleted != true && item.Email.ToLower() == emailAddress.ToLower() && item.Id != userId).FirstOrDefault();
            }

            if (user == null)
                return false;
            else
                return true;
        }

       
        /// <summary>
        /// Get all user Types
        /// </summary>
        /// <returns></returns>
        public List<UserTypeViewModel> GetUserTypes()
        {
            return Mapper.Map(_Context.UserTypes.ToList(), new List<UserTypeViewModel>());
        }

        /// <summary>
        /// Get all user 
        /// </summary>
        /// <returns></returns>
        public List<UserViewModel> GetUserList(int UserId,int RoleId)
        {
            List<UserViewModel> useViewModel = new List<UserViewModel>();
            List<User> user = new List<User>();
            if ((int)Utility.Enums.UserType.SuperAdmin == RoleId)
                 user = _Context.Users.Include(item => item.UserDetails).Include(item => item.UserType).ToList();
            else
                user = _Context.Users.Include(item => item.UserDetails).Include(item => item.UserType).
                    Where(x => x.Id == UserId && x.UserTypeId==RoleId ).ToList();
            if (user != null)
            {
                Mapper.Map(user, useViewModel);
            }
            return useViewModel;
        }

        public UserViewModel RegisterUsers(UserViewModel userViewModel, long logId = 0)
        {
            User users = new User();
            try
            {
                

                Mapper.Map(userViewModel, users);
                users.IsActive = true;
                users.CreatedOn = DateTime.Now;
                users.DefaultPassword = true;
                users.AccountStatus = 1;//(int)Utility.Enums.AccountStatus.Active;
                users.IsDeleted = false;
                users.CreatedBy = logId;
                users.UserTypeId = (int)Utility.Enums.UserType.User;
                users.LastLoginDate = DateTime.Now;
                _Context.Users.Add(users);
                _Context.SaveChanges();
 
                Mapper.Map(users, userViewModel);
                userViewModel.UserType = new UserTypeViewModel();
                userViewModel.UserType.Name = Utility.Enums.UserType.User.ToString();
                userViewModel.UserType.Code = Utility.Enums.UserType.User.ToString();
                userViewModel.UserType.Id = (int)Utility.Enums.UserType.User;
            }
            catch (Exception ex)
            {

            }
            return userViewModel;
        }
        #endregion
    }
}
