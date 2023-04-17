using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class VendorUserRoleService : IVendorUserRoleService
	{

		private readonly IVendorUserRoleRepository _userRoleRepository;
		private readonly IUnitOfWork _unitOfWork;

		public VendorUserRoleService(IVendorUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork)
		{
			this._userRoleRepository = userRoleRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IVendorUserRoleService Members

		public IEnumerable<VendorUserRole> GetVendorUserRoles()
		{
			var userRoles = _userRoleRepository.GetAll();
			return userRoles;
		}

		public IEnumerable<VendorUserRole> GetVendorUserRolesByVendor(long vendorId)
		{
			var userRoles = _userRoleRepository.GetUserRolesByVendor(vendorId);
			return userRoles;
		}

		public IEnumerable<object> GetVendorUserRolesForDropDown(long vendorId)
		{
			var VendorUserRoles = _userRoleRepository.GetUserRolesByVendor(vendorId).Where(x=> x.IsActive == true);
			var dropdownList = from userRoles in VendorUserRoles
							   select new { value = userRoles.ID, text = userRoles.Name };
			return dropdownList;
		}

		public VendorUserRole GetVendorUserRole(long id)
		{
			var userRole = _userRoleRepository.GetById(id);
			return userRole;
		}

		public VendorUserRole GetVendorUserRole(long vendorId, string name)
		{
			var userRole = _userRoleRepository.GetRoleByName(vendorId, name);
			return userRole;
		}

		public bool CreateVendorUserRole(VendorUserRole userRole, ref string message)
		{
			try
			{
				if (!userRole.Name.Equals("Administrator"))
				{
					if (_userRoleRepository.GetRoleByName((long)userRole.VendorID, userRole.Name) == null)
					{
						userRole.IsActive = true;
						userRole.IsDeleted = false;
						userRole.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
						_userRoleRepository.Add(userRole);
						if (SaveVendorUserRole())
						{
							message = "User Role added successfully ...";
							return true;
						}
						else
						{
							message = "Oops! Something went wrong. Please try later.";
							return false;
						}
					}
					else
					{
						message = "User Role already exist  ...";
						return false;
					}
				}
				else
				{
					message = "Administrator can't be added...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateVendorUserRole(ref VendorUserRole userRole, ref string message)
		{
			try
			{
				if (!userRole.Name.Equals("Administrator"))
				{
					if (_userRoleRepository.GetRoleByName((long)userRole.VendorID, userRole.Name, userRole.ID) == null)
					{
						VendorUserRole CurrentVendorUserRole = _userRoleRepository.GetById(userRole.ID);
						CurrentVendorUserRole.Name = userRole.Name;
						userRole = null;

						_userRoleRepository.Update(CurrentVendorUserRole);
						if (SaveVendorUserRole())
						{
							userRole = CurrentVendorUserRole;
							message = "User Role updated successfully ...";
							return true;
						}
						else
						{
							message = "Oops! Something went wrong. Please try later.";
							return false;
						}
					}
					else
					{
						message = "User Role already exist  ...";
						return false;
					}
				}
				else
				{
					message = "Administrator can't be edited...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateVendorUserRole(ref VendorUserRole userRole, ref string prevVendorUserRole, ref string message)
		{
			try
			{
				if (!userRole.Name.Equals("Administrator"))
				{
					if (_userRoleRepository.GetRoleByName((long)userRole.VendorID, userRole.Name, userRole.ID) == null)
					{
						VendorUserRole CurrentVendorUserRole = _userRoleRepository.GetById(userRole.ID);
						prevVendorUserRole = CurrentVendorUserRole.Name;
						CurrentVendorUserRole.Name = userRole.Name;
						userRole = null;

						_userRoleRepository.Update(CurrentVendorUserRole);
						if (SaveVendorUserRole())
						{
							userRole = CurrentVendorUserRole;
							message = "User Role updated successfully ...";
							return true;
						}
						else
						{
							message = "Oops! Something went wrong. Please try later.";
							return false;
						}
					}
					else
					{
						message = "User Role already exist  ...";
						return false;
					}
				}
				else
				{
					message = "Administrator can't be edited...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteVendorUserRole(long id, ref string message, ref string filePath, bool softDelete = true)
		{
			try
			{
				VendorUserRole userRole = _userRoleRepository.GetById(id);
				if (!userRole.Name.Equals("Administrator"))
				{
					if (softDelete)
					{
						userRole.IsDeleted = true;
						_userRoleRepository.Update(userRole);
					}
					else
					{
						filePath = string.Format("/AuthorizationProvider/Privileges/Vendor/{VendorID}/{0}.txt", userRole.Name);
						_userRoleRepository.Delete(userRole);
					}
					if (SaveVendorUserRole())
					{
						message = "User Role deleted successfully ...";
						return true;
					}
					else
					{
						message = "Oops! Something went wrong. Please try later.";
						return false;
					}
				}
				else
				{
					message = "Administrator can't be deleted ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool SaveVendorUserRole()
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

		public VendorUserRole GetVendorUserRoleByName(string name)
		{
			var userRole = _userRoleRepository.GetvendorRoleByName(name);
			return userRole;
		}

		#endregion
	}

	public interface IVendorUserRoleService
	{
		VendorUserRole GetVendorUserRoleByName(string name);
		IEnumerable<VendorUserRole> GetVendorUserRoles();
		IEnumerable<VendorUserRole> GetVendorUserRolesByVendor(long vendorId);
		IEnumerable<object> GetVendorUserRolesForDropDown(long vendorId);
		VendorUserRole GetVendorUserRole(long id);
		VendorUserRole GetVendorUserRole(long vendorId, string name);
		bool CreateVendorUserRole(VendorUserRole userRole, ref string message);
		bool UpdateVendorUserRole(ref VendorUserRole userRole, ref string message);
		bool UpdateVendorUserRole(ref VendorUserRole userRole, ref string prevVendorUserRole, ref string message);
		bool DeleteVendorUserRole(long id, ref string message, ref string filePath, bool softDelete = true);
		bool SaveVendorUserRole();
	}
}
