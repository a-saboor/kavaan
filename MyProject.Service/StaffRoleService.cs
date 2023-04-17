using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class StaffRoleService : IStaffRoleService
    {
        private readonly IStaffRolesRepository _staffRolesRepository;
        private readonly IDepartmentService _departmentService;
        private readonly IUnitOfWork _unitOfWork;
        public StaffRoleService(IUnitOfWork unitOfWork, IStaffRolesRepository staffRolesRepository,IDepartmentService departmentService)
        {
            this._unitOfWork = unitOfWork;
            this._staffRolesRepository = staffRolesRepository;
            this._departmentService = departmentService;
        }
        public IEnumerable<StaffRole> GetStaffRoles(long VendorID = 0)
        {
            var staffRoles = _staffRolesRepository.GetAll().Where(i => i.IsDeleted == false && i.VendorID == VendorID);
            return staffRoles;
        }
        public StaffRole GetStaffRole(long id)
        {
            var staffRoles = this._staffRolesRepository.GetById(id);
            return staffRoles;
        }
        public IEnumerable<object> GetStaffRoleForDropDown(string lang = "en", long VendorID = 0)
        {
            var StaffRoles = GetStaffRoles(VendorID).Where(x => x.IsActive == true && x.IsDeleted == false && x.VendorID == VendorID);
            var dropdownList = from staffroles in StaffRoles
                               select new { value = staffroles.ID, text = lang == "en" ? staffroles.RoleName : staffroles.RoleName };
            return dropdownList;
        }
        public IEnumerable<object> GetStaffRoleForDropDown(long departmentID, string lang = "en", long VendorID = 0)
        {
            var StaffRoles = GetStaffRoles(VendorID).Where(x => x.IsActive == true && departmentID == x.DepartmentID && x.IsDeleted == false);
            var dropdownList = from staffroles in StaffRoles
                               select new { value = staffroles.ID, text = lang == "en" ? staffroles.RoleName : staffroles.RoleName };
            return dropdownList;
        }
        public IEnumerable<object> GetCitiesForDropDown(long departmentID, string lang = "en")
        {
            var Roles = _staffRolesRepository.GetAllByDepartmentID(departmentID).Where(x => x.IsActive == true && x.IsDeleted == false);
            var dropdownList = from roles in Roles
                               select new { value = roles.ID, text = lang == "en" ? roles.RoleName : roles.RoleName };
            return dropdownList;
        }

        public bool CreateStaffRole(StaffRole staffRole, ref string message)
        {
            try
            {

                staffRole.IsActive = true;
                staffRole.IsDeleted = false;
                staffRole.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _staffRolesRepository.Add(staffRole);
                if (SaveStaffRole())
                {
                    message = "Staff Role added successfully...";
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
        public bool UpdateStaffRole(ref StaffRole staffRole, ref string message)
        {
            try
            {
                StaffRole currentstaffRole = _staffRolesRepository.GetById(staffRole.ID);
                currentstaffRole.IsActive = staffRole.IsActive;

                _staffRolesRepository.Update(staffRole);
                if (SaveStaffRole())
                {
                    staffRole = currentstaffRole;
                    message = "Staff Role updated successfully ...";
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
        public bool DeleteStaffRole(long id, ref string message, ref bool hasChilds, bool softdelete)
        {

            if (softdelete)
            {
                try
                {
                    //soft delete
                    StaffRole staffRole = _staffRolesRepository.GetById(id);
                    //When category delete, its Contractor will be deleted
                    //*
                    staffRole.IsDeleted = true;
                    _staffRolesRepository.Update(staffRole);

                    if (SaveStaffRole())
                    {
                        message = "Staff Role deleted successfully ...";
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
            else
            {

                //hard delete
                try
                {
                    StaffRole staffRole = _staffRolesRepository.GetById(id);

                    _staffRolesRepository.Delete(staffRole);

                    if (SaveStaffRole())
                    {
                        message = "Staff Role deleted successfully ...";
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
        }
        public bool SaveStaffRole()
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
    }
    public interface IStaffRoleService
    {
        StaffRole GetStaffRole(long id);
        IEnumerable<StaffRole> GetStaffRoles(long VendorID = 0);
        IEnumerable<object> GetStaffRoleForDropDown(string lang = "en", long VendorID = 0);
        IEnumerable<object> GetStaffRoleForDropDown(long departmentID, string lang = "en", long VendorID = 0);
        bool CreateStaffRole(StaffRole staffRole, ref string message);
        bool UpdateStaffRole(ref StaffRole staffRole, ref string message);
        bool DeleteStaffRole(long id, ref string message, ref bool hasContractorLink, bool softdelete);
        bool SaveStaffRole();
    }
}
