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
    public class ConsultantService : IConsultantService
    {
        private readonly IConsultantRepository _consultantRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ConsultantService(IConsultantRepository contractorRepository, IUnitOfWork unitOfWork)
        {
            this._consultantRepository = contractorRepository;
            this._unitOfWork = unitOfWork;
        }

        public bool Create(Consultant Consultant, ref string message)
        {
            try
            {
                if (this._consultantRepository.GetConsultantByName(Consultant.Name) == null)
                {
                    Consultant.IsActive = true;
                    Consultant.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    Consultant.IsDeleted = false;
                    this._consultantRepository.Add(Consultant);
                    if (SaveContractor())
                    {
                        message = "Consultant Added Succesfully";
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
                    message = "Consultant Already Exist";
                    return false;
                }
            }
            catch (Exception ex)
            {

                message = "Oops! Something Went Wrong";
                return false;

            }
        }

        public Consultant Edit(long id)
        {
            Consultant Consultant = this._consultantRepository.GetById(id);
            return Consultant;
        }
        public bool Edit(ref Consultant Consultant, ref string message)
        {
            try
            {
                var consultant = this._consultantRepository.GetConsultantByName(Consultant.Name, Consultant.ID);
                if (consultant == null)
                {

                    this._consultantRepository.Update(Consultant);
                    if (SaveContractor())
                    {
                        message = "Consultant Updated Successfully";
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
                    message = "Consultant Already Exist";
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
                Consultant Consultant = this._consultantRepository.GetById(id);
                if (softdelete)
                {
                    Consultant.IsDeleted = true;
                    this._consultantRepository.Update(Consultant);

                }
                else
                {
                    this._consultantRepository.Delete(Consultant);
                }
                if (SaveContractor())
                {
                    message = "Consultant Deleted Successfully";
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

                message = "Oops! Something Went Wrong. Please Try Again Later";
                return false;

            }
        }
        public IEnumerable<Consultant> GetAll()
        {
            try
            {
                IEnumerable<Consultant> Consultant = this._consultantRepository.GetAll().Where(x => x.IsDeleted == false);
                if (Consultant != null)
                {
                    return Consultant;
                }
                return null;
            }
            catch (Exception)
            {

                return null;
            }
        }

        public IEnumerable<SP_GetFilteredConsultants_Result> GetFilteredConsultants(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
        {
            var consultants = _consultantRepository.GetFilteredConsultants(search, pageSize, pageNumber, sortBy, lang, imageServer);
            return consultants;
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



    }
    public interface IConsultantService
    {
        bool Create(Consultant Consultant, ref string message);
        Consultant Edit(long id);
        bool Edit(ref Consultant Consultant, ref string message);
        bool Delete(long id, ref string message, bool softdelete = true);
        IEnumerable<Consultant> GetAll();

        IEnumerable<SP_GetFilteredConsultants_Result> GetFilteredConsultants(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);

    }
}
