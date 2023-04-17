using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UserRoleService(IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork)
        {
            this._userRoleRepository = userRoleRepository;
            this._unitOfWork = unitOfWork;
        }

        #region IUserRoleService Members

        public IEnumerable<UserRole> GetUserRoles()
        {
            var userRoles = _userRoleRepository.GetAll().Where(i => i.IsDeleted == false);
            return userRoles;
        }

        public IEnumerable<object> GetUserRolesForDropDown()
        {
            var UserRoles = _userRoleRepository.GetAll().Where(x => x.IsActive == true && x.IsDeleted == false);
            var dropdownList = from userRoles in UserRoles
                               select new { value = userRoles.ID, text = userRoles.RoleName };
            return dropdownList;
        }

        public UserRole GetUserRole(long id)
        {
            var userRole = _userRoleRepository.GetById(id);
            return userRole;
        }

        public bool CreateUserRole(UserRole userRole, ref string message)
        {
            try
            {
                if (_userRoleRepository.GetUserRoleByName(userRole.RoleName) == null)
                {
                    userRole.IsActive = true;
                    userRole.IsDeleted = false;
                    userRole.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    _userRoleRepository.Add(userRole);
                    if (SaveUserRole())
                    {
                        message = "UserRole added successfully ...";
                        return true;

                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later...";
                        return false;
                    }
                }
                else
                {
                    message = "UserRole already exist  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool UpdateUserRole(ref UserRole userRole, ref string message)
        {
            try
            {
                if (!userRole.RoleName.Equals("Admin"))
                {
                    if (_userRoleRepository.GetUserRoleByName(userRole.RoleName, userRole.ID) == null)
                    {
                        UserRole CurrentUserRole = _userRoleRepository.GetById(userRole.ID);
                        CurrentUserRole.RoleName = userRole.RoleName;
                        userRole = null;

                        _userRoleRepository.Update(CurrentUserRole);
                        if (SaveUserRole())
                        {
                            userRole = CurrentUserRole;
                            message = "UserRole updated successfully ...";
                            return true;
                        }
                        else
                        {
                            message = "Oops! Something went wrong. Please try later...";
                            return false;
                        }
                    }
                    else
                    {
                        message = "UserRole already exist  ...";
                        return false;
                    }
                }
                else
                {
                    message = "Admin can't be updated ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool UpdateUserRole(ref UserRole userRole, ref string prevUserRole, ref string message)
        {
            try
            {
                if (!userRole.RoleName.Equals("Admin"))
                {
                    if (_userRoleRepository.GetUserRoleByName(userRole.RoleName, userRole.ID) == null)
                    {
                        UserRole CurrentUserRole = _userRoleRepository.GetById(userRole.ID);
                        prevUserRole = CurrentUserRole.RoleName;
                        CurrentUserRole.RoleName = userRole.RoleName;
                        userRole = null;

                        _userRoleRepository.Update(CurrentUserRole);
                        if (SaveUserRole())
                        {
                            userRole = CurrentUserRole;
                            message = "UserRole updated successfully ...";
                            return true;
                        }
                        else
                        {
                            message = "Oops! Something went wrong. Please try later...";
                            return false;
                        }
                    }
                    else
                    {
                        message = "UserRole already exist  ...";
                        return false;
                    }
                }
                else
                {
                    message = "Admin can't be updated ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool DeleteUserRole(long id, ref string message, ref string filePath, bool softDelete = true)
        {
            try
            {
                UserRole userRole = _userRoleRepository.GetById(id);
                if (!userRole.RoleName.Equals("Admin"))
                {
                    if (softDelete)
                    {
                        userRole.IsDeleted = true;
                        _userRoleRepository.Update(userRole);
                    }
                    else
                    {
                        filePath = string.Format("/AuthorizationProvider/Privileges/Admin/{0}.txt", userRole.RoleName);
                        _userRoleRepository.Delete(userRole);
                    }
                    if (SaveUserRole())
                    {
                        message = "UserRole deleted successfully ...";
                        return true;
                    }
                    else
                    {
                        message = "Oops! Something went wrong. Please try later...";
                        return false;
                    }
                }
                else
                {
                    message = "Admin can't be deleted ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public List<SP_GetRoutesWithUserRolePrivileges_Result> GetRoutesWithUserRolePrivileges(string type, long userRoleId)
        {
            var Privileges = _userRoleRepository.GetRoutesWithUserRolePrivileges(type, userRoleId).ToList();
            return Privileges;
        }

        public bool SaveUserRole()
        {
            try
            {
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        #endregion
    }

    public interface IUserRoleService
    {
        IEnumerable<UserRole> GetUserRoles();
        IEnumerable<object> GetUserRolesForDropDown();
        UserRole GetUserRole(long id);
        bool CreateUserRole(UserRole userRole, ref string message);
        bool UpdateUserRole(ref UserRole userRole, ref string message);
        bool UpdateUserRole(ref UserRole userRole, ref string prevUserRole, ref string message);
        bool DeleteUserRole(long id, ref string message, ref string filePath, bool softDelete = true);
        List<SP_GetRoutesWithUserRolePrivileges_Result> GetRoutesWithUserRolePrivileges(string type, long userRoleId);
        bool SaveUserRole();
    }
}
