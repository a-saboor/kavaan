using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;


namespace MyProject.Service
{
    class CustomerDocumentTypeService : ICustomerDocumentTypeService
    {
        private readonly ICustomerDocumentTypeRepository _customerDocumentTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerDocumentTypeService(IUnitOfWork unitOfWork, ICustomerDocumentTypeRepository customerDocumentTypeRepository)
        {
            this._unitOfWork = unitOfWork;
            this._customerDocumentTypeRepository = customerDocumentTypeRepository;
        }
        public IEnumerable<object> GetDocumentTypesForDropDown()
        {
            var documents = _customerDocumentTypeRepository.GetAll().Where(x => x.IsDeleted == false && x.IsActive==true);
            var dropdownList = from docx in documents
                               select new { value = docx.ID, text = docx.Title };
            return dropdownList;
        }
        public IEnumerable<CustomerDocumentType> GetCustomerDocumentType()
        {
            var customerDocumentType = this._customerDocumentTypeRepository.GetAll().Where(i => i.IsDeleted == false);
            return customerDocumentType;
        }
        public CustomerDocumentType GetCustomerDocumentType(long id)
        {
            var customerDocumentType = this._customerDocumentTypeRepository.GetById(id);
            return customerDocumentType;
        }
        public bool CreateCustomerDocumentType(ref CustomerDocumentType customerDocumentType, ref string message)
        {
            try
            {
                customerDocumentType.IsActive = true;
                customerDocumentType.IsDeleted = false;
                customerDocumentType.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                this._customerDocumentTypeRepository.Add(customerDocumentType);
                if (SaveData())
                {
                    message = "Document created successfully...";
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
        public bool UpdateCustomerDocumentType(ref CustomerDocumentType customerDocumentType, ref string message)
        {
            try
            {
                CustomerDocumentType CurrentCustomerDocumentType = this._customerDocumentTypeRepository.GetById(customerDocumentType.ID);

                CurrentCustomerDocumentType.Title = customerDocumentType.Title;
                CurrentCustomerDocumentType.TitleAr = customerDocumentType.TitleAr;

                customerDocumentType = null;

                this._customerDocumentTypeRepository.Update(CurrentCustomerDocumentType);
                if (SaveData())
                {
                    customerDocumentType = CurrentCustomerDocumentType;
                    message = "Document updated successfully ...";
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
        public bool DeleteCustomerDocumentType(long id, ref string message, bool softDelete = true)
        {
            try
            {
                CustomerDocumentType customerDocumentType = this._customerDocumentTypeRepository.GetById(id);
                if (softDelete)
                {
                    customerDocumentType.IsDeleted = true;
                    this._customerDocumentTypeRepository.Update(customerDocumentType);
                }
                else
                {
                    this._customerDocumentTypeRepository.Delete(customerDocumentType);
                }
                if (SaveData())
                {
                    message = "Document deleted successfully ...";
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

        public bool SaveData()
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
    public interface ICustomerDocumentTypeService
    {
        IEnumerable<CustomerDocumentType> GetCustomerDocumentType();
        CustomerDocumentType GetCustomerDocumentType(long id);
        bool CreateCustomerDocumentType(ref CustomerDocumentType customerDocumentType, ref string message);
        bool UpdateCustomerDocumentType(ref CustomerDocumentType customerDocumentType, ref string message);
        bool DeleteCustomerDocumentType(long id, ref string message, bool softDelete = true);
        IEnumerable<object> GetDocumentTypesForDropDown();
        bool SaveData();
    }
}

