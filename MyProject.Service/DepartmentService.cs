using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class DepartmentService : IDepartmentService
    {
        private readonly IDepartmentsRepository _departmentRepository;
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService(IUnitOfWork unitOfWork, IDepartmentsRepository departmentRepository)
        {
            this._unitOfWork = unitOfWork;
            this._departmentRepository = departmentRepository;
        }
        public IEnumerable<Department> GetDepartments(long VendorID = 0)
        {
            var deparment = _departmentRepository.GetAll().Where(i => i.IsDeleted == false && i.VendorID == VendorID);
            return deparment;
        }
        public Department GetDepartment(long id)
        {
            var department = this._departmentRepository.GetById(id);
            return department;
        }
        public IEnumerable<object> GetDepartmentsForDropDown(string lang = "en", long VendorID = 0)
        {
            var Departments = GetDepartments(VendorID).Where(x => x.IsActive == true && x.IsDeleted == false);
            var dropdownList = from departments in Departments
                               select new { value = departments.ID, text = lang == "en" ? departments.Name : departments.Name };
            return dropdownList;
        }
        public bool CreateDepartment(Department department, ref string message)
        {
            try
            {

                department.IsActive = true;
                department.IsDeleted = false;
                department.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _departmentRepository.Add(department);
                if (SaveDepartment())
                {
                    message = "Department added successfully...";
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
        public bool UpdateDepartment(ref Department department, ref string message)
        {
            try
            {
                Department currentdepartment = _departmentRepository.GetById(department.ID);
                currentdepartment.IsActive = department.IsActive;

                _departmentRepository.Update(department);
                if (SaveDepartment())
                {
                    department = currentdepartment;
                    message = "Contractor updated successfully ...";
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
        public bool DeleteDepartment(long id, ref string message, ref bool hasChilds, bool softdelete)
        {

            if (softdelete)
            {
                try
                {
                    //soft delete
                    Department department = _departmentRepository.GetById(id);
                    //When department delete, its all refrences will be deleted
                            // delete code here.....
                    //*
                    department.IsDeleted = true;
                    _departmentRepository.Update(department);

                    if (SaveDepartment())
                    {
                        message = "Department deleted successfully ...";
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
                    Department department = _departmentRepository.GetById(id);

                    _departmentRepository.Delete(department);

                    if (SaveDepartment())
                    {
                        message = "Department deleted successfully ...";
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
        public bool SaveDepartment()
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
    public interface IDepartmentService
    {
        Department GetDepartment(long id);
        IEnumerable<Department> GetDepartments(long VendorID = 0);
        IEnumerable<object> GetDepartmentsForDropDown(string lang = "en", long VendorID = 0);
        bool CreateDepartment(Department deparment, ref string message);
        bool UpdateDepartment(ref Department department, ref string message);
        bool DeleteDepartment(long id, ref string message, ref bool hasContractorLink, bool softdelete);
        bool SaveDepartment();
    }
}
