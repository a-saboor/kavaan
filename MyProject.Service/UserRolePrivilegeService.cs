using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{

	public class UserRolePrivilegeService : IUserRolePrivilegeService
	{
		private readonly IUserRolePrivilegeRepository _userRolePrivilegeRepository;
		private readonly IUnitOfWork _unitOfWork;

		public UserRolePrivilegeService(IUserRolePrivilegeRepository userRolePrivilegeRepository, IUnitOfWork unitOfWork)
		{
			this._userRolePrivilegeRepository = userRolePrivilegeRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IUserRolePrivilegeService Members

		public IEnumerable<UserRolePrivilege> GetUserRolePrivileges()
		{
			var userRolePrivileges = _userRolePrivilegeRepository.GetAll();
			return userRolePrivileges;
		}

		public UserRolePrivilege GetUserRolePrivilege(long id)
		{
			var userRolePrivilege = _userRolePrivilegeRepository.GetById(id);
			return userRolePrivilege;
		}

		public bool CreateUserRolePrivilege(UserRolePrivilege userRolePrivilege, ref string message)
		{
			try
			{
				_userRolePrivilegeRepository.Add(userRolePrivilege);
				if (SaveUserRolePrivilege())
				{
					message = "UserRolePrivilege added successfully ...";
					return true;

				}
				else
				{
					message = "Oops! Something went wrong. Please try later...";
					return false;
				}

			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool DeleteUserRolePrivilege(long id, ref string message)
		{
			try
			{
				UserRolePrivilege userRolePrivilege = _userRolePrivilegeRepository.GetById(id);

				_userRolePrivilegeRepository.Delete(userRolePrivilege);
				if (SaveUserRolePrivilege())
				{
					message = "UserRolePrivilege deleted successfully ...";
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool DeleteUserRolePrivileges(long userRoleId, ref string message)
		{
			try
			{
				_userRolePrivilegeRepository.DeleteMany(userRoleId);
				if (SaveUserRolePrivilege())
				{
					message = "User Role Privileges deleted successfully ...";
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool SaveUserRolePrivilege()
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

	public interface IUserRolePrivilegeService
	{
		IEnumerable<UserRolePrivilege> GetUserRolePrivileges();
		UserRolePrivilege GetUserRolePrivilege(long id);
		bool CreateUserRolePrivilege(UserRolePrivilege userRolePrivilege, ref string message);
		bool DeleteUserRolePrivilege(long id, ref string message);
		bool DeleteUserRolePrivileges(long userRoleId, ref string message);
		
		bool SaveUserRolePrivilege();
	}
}
