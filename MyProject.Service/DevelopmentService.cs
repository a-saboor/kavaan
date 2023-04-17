using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Service
{
    public class DevelopmentService : IDevelopmentService
    {
        private readonly IDevelopmentRepository developmentRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DevelopmentService(IDevelopmentRepository developmentRepository, IUnitOfWork unitOfWork)
        {
            this.developmentRepository = developmentRepository;
            this._unitOfWork = unitOfWork;
        }

        public bool Create(Development development, ref string message)
        {
            try
            {
                if (this.developmentRepository.GerDevelopmentByName(development.Name) == null)
                {
                   
                    development.IsActive = true;
                    development.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    development.IsDeleted = false;
                    this.developmentRepository.Add(development);
                    if (SaveContractor())
                    {
                        message = "Development Added Successfully";
                        return true;
                    }
                    else
                    {
                        message = "Oops! Something Went Wrong";
                        return false;
                    }
                }
                else
                {
                    message = "Development Already Exist";
                    return false;
                }
            }
            catch (Exception ex)
            {

                message = "Oops! Something Went Wrong";
                return false;

            }
        }

        public Development Edit(long id)
        {
            Development Development = this.developmentRepository.GetById(id);
            return Development;
        }

        public Development GetDevelopmentById(long id)
        {
            Development Development = this.developmentRepository.GetById(id);
            return Development;
        }
        public bool Edit(ref Development developmentcurrent, ref string message)
        {
            try
            {
                var development = this.developmentRepository.GerDevelopmentByName(developmentcurrent.Name, developmentcurrent.ID);
                if (development == null)
                {

                    this.developmentRepository.Update(developmentcurrent);

                    if (SaveContractor())
                    {
                        message = "Development Updated Successfully";
                        return true;
                    }
                    else
                    {
                        message = "Oops! Something Went Wrong";
                        return false;
                    }
                }
                else
                {
                    message = "Development Already Exist";
                    return false;
                }
            }
            catch (Exception)
            {
                message = "Oops! Something Went Wrong";
                return false;

            }
        }
        public bool Delete(long id, ref string message, bool softdelete = true)
        {
            try
            {
                Development Development = this.developmentRepository.GetById(id);
                if (softdelete)
                {
                    Development.IsDeleted = true;
                    this.developmentRepository.Update(Development);

                }
                else
                {
                    this.developmentRepository.Delete(Development);
                }
                if (SaveContractor())
                {
                    message = "Development Deleted Succesfully";
                    return true;
                }
                else
                {
                    message = "Oops! Something Went Wrong";
                    return false;


                }

            }
            catch (Exception)
            {

                message = "Oops! Something Went Wrong. Pleae Try Again Later";
                return false;

            }
        }


        public IEnumerable<Development> GetAll()
        {
            var Developments = this.developmentRepository.GetAll().Where(x => x.IsActive == true && x.IsDeleted == false);
            return Developments;
        }
        public bool SaveContractor()
        {
            try
            {
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return false;
            }
        }
        public IEnumerable<object> GetDevelopmentForDropDown(string lang = "en")
        {
            var Developments = GetAll().Where(x => x.IsActive == true && x.IsDeleted == false);
            var dropdownList = from Development in Developments
                               select new { value = Development.ID, text = lang == "en" ? Development.Name : Development.NameAr };
            return dropdownList;
        }

        public IEnumerable<SP_GetFilteredDevelopment_Result> GetFilteredDevelopment(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
        {
            var development = developmentRepository.GetFilteredDevelopment(search, pageSize, pageNumber, sortBy, lang, imageServer);

            return development;
        }

    }
    public interface IDevelopmentService
    {
        bool Create(Development Development, ref string message);
        Development Edit(long id);
        bool Edit(ref Development Development, ref string message);
        bool Delete(long id, ref string message, bool softdelete = true);
        IEnumerable<Development> GetAll();
        IEnumerable<object> GetDevelopmentForDropDown(string lang = "en");
        Development GetDevelopmentById(long id);

        IEnumerable<SP_GetFilteredDevelopment_Result> GetFilteredDevelopment(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);

    }
}
