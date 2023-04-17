using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class CustomerDocumentService : ICustomerDocumentService
    {
        private readonly ICustomerDocumentRepository _customerDocumentRepository;
        private readonly ICustomerDocumentTypeRepository _customerDocumenttypeRepository;
        private readonly ICustomerRelationRepository _customerRelationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerDocumentService(IUnitOfWork unitOfWork, ICustomerRelationRepository customerRelationRepository, ICustomerDocumentTypeRepository customerDocumenttypeRepository, ICustomerDocumentRepository customerDocumentRepository)
        {

            this._unitOfWork = unitOfWork;
            this._customerDocumentRepository = customerDocumentRepository;
            this._customerDocumenttypeRepository = customerDocumenttypeRepository;
            this._customerRelationRepository = customerRelationRepository;
        }
        #region ICustomerDocumentService Members

        public IEnumerable<CustomerDocument> GetCustomerDocuments()
        {
            var customerDocument = this._customerDocumentRepository.GetAll().Where(i => i.IsDeleted == false);
            return customerDocument;
        }

        public IEnumerable<CustomerDocument> GetCustomerDocumentsByCustomerID(long customerid)
        {
            var customerDocument = this._customerDocumentRepository.GetAll().Where(i => i.CustomerID == customerid && i.IsDeleted == false);
            return customerDocument;
        }

        public IEnumerable<object> GetCustomerDocumentForDropDown(string lang = "en")
        {
            var awards = GetCustomerDocuments().Where(x => x.IsFamily == true);
            var dropdownList = from customerDocument in awards
                               select new { value = customerDocument.ID, text = lang == "en" ? customerDocument.DocumentNo : customerDocument.DocumentNo };
            return dropdownList;
        }
        public CustomerDocument GetCustomerDocument(long id)
        {
            var customerDocument = this._customerDocumentRepository.GetById(id);
            return customerDocument;
        }

        public bool isDocumentAlreadyExist(ref CustomerDocument customerDocument, ref string message)
        {
            if (this._customerDocumentRepository.GetCustomerDocumentByType(customerDocument.Relation, (long)customerDocument.CustomerDocumentTypeID) == null)
            {
                message = "Document not exist ...";
                return true;
            }
            else
            {
                message = "Document already exist with same type  ...";
                return false;
            }
        }

        public bool CreateCustomerDocument(ref CustomerDocument customerDocument, ref string message)
        {
            try
            {
                CustomerRelation relationtype = _customerRelationRepository.GetById((long)customerDocument.CustomerRelationID);
                if (this._customerDocumentRepository.CheckCustomerDocumentIsExist((long)customerDocument.CustomerRelationID, (long)customerDocument.CustomerID, (long)customerDocument.CustomerDocumentTypeID) == null)
                {

                    if (_customerRelationRepository.GetById((long)customerDocument.CustomerRelationID).Relation == "Personal")
                    {
                        customerDocument.IsFamily = false;
                    }
                    else
                    {
                        customerDocument.IsFamily = true;
                    }

                    customerDocument.IsDeleted = false;
                    customerDocument.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                    this._customerDocumentRepository.Add(customerDocument);
                    if (SaveData())
                    {
                        message = "Document added successfully ...";
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
                    message = "Document already exist with same type  ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool UpdateCustomerDocument(ref CustomerDocument customerDocument, ref string message)
        {
            try
            {
                CustomerDocument CurrentCustomerDocument = this._customerDocumentRepository.GetById(customerDocument.ID);
                if (CurrentCustomerDocument.CustomerRelationID == customerDocument.CustomerRelationID)
                {
                    CurrentCustomerDocument.CustomerDocumentTypeID = customerDocument.CustomerDocumentTypeID;
                    CurrentCustomerDocument.CustomerRelationID = customerDocument.CustomerRelationID;
                    CurrentCustomerDocument.ExpiryDate = customerDocument.ExpiryDate;
                    CurrentCustomerDocument.Path = customerDocument.Path;
                }
                else
                {
                    if (this._customerDocumentRepository.CheckCustomerDocumentIsExist((long)customerDocument.CustomerRelationID, (long)customerDocument.CustomerID, (long)customerDocument.CustomerDocumentTypeID) == null)
                    {

                        CurrentCustomerDocument.CustomerDocumentTypeID = customerDocument.CustomerDocumentTypeID;
                        CurrentCustomerDocument.CustomerRelationID = customerDocument.CustomerRelationID;
                        CurrentCustomerDocument.ExpiryDate = customerDocument.ExpiryDate;
                        CurrentCustomerDocument.Path = customerDocument.Path;
                    }
                    else
                    {
                        message = "CustomerDocument already exist  ...";
                        return false;
                    }
                }
                customerDocument = null;

                this._customerDocumentRepository.Update(CurrentCustomerDocument);
                if (SaveData())
                {
                    customerDocument = CurrentCustomerDocument;
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

        public bool DeleteCustomerDocument(long id, ref string message, ref string filepath, bool softDelete = true,bool IsCustomer = false)
        {
            try
            {
                CustomerDocument customerDocument = this._customerDocumentRepository.GetById(id);
                if ((bool)customerDocument.IsFamily || IsCustomer)
                { 
                filepath = customerDocument.Path;
                    if (softDelete)
                    {
                        customerDocument.IsDeleted = true;
                        this._customerDocumentRepository.Update(customerDocument);
                    }
                    else
                    {
                        this._customerDocumentRepository.Delete(customerDocument);
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
                else
                {
                    message = "Personal document Not allowed to delete...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool DeleteCustomerDocumentForCustomerApp(long id, ref string message, ref string filepath, bool softDelete = true)
        {
            try
            {
                CustomerDocument customerDocument = this._customerDocumentRepository.GetById(id);

                filepath = customerDocument.Path;
                if (softDelete)
                {
                    customerDocument.IsDeleted = true;
                    this._customerDocumentRepository.Update(customerDocument);
                }
                else
                {
                    this._customerDocumentRepository.Delete(customerDocument);
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
        #endregion

    }
    public interface ICustomerDocumentService
    {
        IEnumerable<CustomerDocument> GetCustomerDocuments();
        IEnumerable<CustomerDocument> GetCustomerDocumentsByCustomerID(long customerid);
        IEnumerable<object> GetCustomerDocumentForDropDown(string lang = "en");
        CustomerDocument GetCustomerDocument(long id);
        bool isDocumentAlreadyExist(ref CustomerDocument customerDocument, ref string message);
        bool CreateCustomerDocument(ref CustomerDocument customerDocument, ref string message);
        bool UpdateCustomerDocument(ref CustomerDocument customerDocument, ref string message);
        bool DeleteCustomerDocument(long id, ref string message, ref string filepath, bool softDelete = true, bool IsCustomer = false);
        bool DeleteCustomerDocumentForCustomerApp(long id, ref string message, ref string filepath, bool softDelete = true);
        bool SaveData();
    }
}
