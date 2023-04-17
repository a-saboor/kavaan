using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{
	public class VendorUserRolePrivilegeService : IVendorUserRolePrivilegeService
	{
		private readonly IVendorUserRolePrivilegeRepository _vendorUserRolePrivilegeRepository;
		private readonly IUnitOfWork _unitOfWork;

		public VendorUserRolePrivilegeService(IVendorUserRolePrivilegeRepository vendorUserRolePrivilegeRepository, IUnitOfWork unitOfWork)
		{
			this._vendorUserRolePrivilegeRepository = vendorUserRolePrivilegeRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IVendorUserRolePrivilegeService Members

		public IEnumerable<VendorUserRolePrivilege> GetVendorUserRolePrivileges()
		{
			var vendorUserRolePrivileges = _vendorUserRolePrivilegeRepository.GetAll();
			return vendorUserRolePrivileges;
		}

		public VendorUserRolePrivilege GetVendorUserRolePrivilege(long id)
		{
			var vendorUserRolePrivilege = _vendorUserRolePrivilegeRepository.GetById(id);
			return vendorUserRolePrivilege;
		}

		public bool CreateVendorUserRolePrivilege(VendorUserRolePrivilege vendorUserRolePrivilege, ref string message)
		{
			try
			{
				_vendorUserRolePrivilegeRepository.Add(vendorUserRolePrivilege);
				if (SaveVendorUserRolePrivilege())
				{
					message = "VendorUserRolePrivilege added successfully ...";
					return true;

				}
				else
				{
					message = "Oops! Something went wrong. Please try later.";
					return false;
				}

			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteVendorUserRolePrivilege(long id, ref string message)
		{
			try
			{
				VendorUserRolePrivilege vendorUserRolePrivilege = _vendorUserRolePrivilegeRepository.GetById(id);

				_vendorUserRolePrivilegeRepository.Delete(vendorUserRolePrivilege);
				if (SaveVendorUserRolePrivilege())
				{
					message = "User role privilege deleted successfully ...";
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later.";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteVendorUserRolePrivileges(long userRoleId, ref string message)
		{
			try
			{
				_vendorUserRolePrivilegeRepository.DeleteMany(userRoleId);
				if (SaveVendorUserRolePrivilege())
				{
					message = "User role privileges deleted successfully ...";
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later.";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public List<SP_GetRoutesWithVendorUserRolePrivileges_Result> GetRoutesWithVendorUserRolePrivileges(string type, long userRoleId)
		{
			var Privileges = _vendorUserRolePrivilegeRepository.GetRoutesWithVendorUserRolePrivileges(type, userRoleId);
			return Privileges;
		}

		public bool SaveVendorUserRolePrivilege()
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

	public interface IVendorUserRolePrivilegeService
	{
		IEnumerable<VendorUserRolePrivilege> GetVendorUserRolePrivileges();
		VendorUserRolePrivilege GetVendorUserRolePrivilege(long id);
		bool CreateVendorUserRolePrivilege(VendorUserRolePrivilege vendorUserRolePrivilege, ref string message);
		bool DeleteVendorUserRolePrivilege(long id, ref string message);
		bool DeleteVendorUserRolePrivileges(long userRoleId, ref string message);
		List<SP_GetRoutesWithVendorUserRolePrivileges_Result> GetRoutesWithVendorUserRolePrivileges(string type, long userRoleId);
		bool SaveVendorUserRolePrivilege();
	}
}
