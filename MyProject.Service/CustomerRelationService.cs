using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
namespace MyProject.Service
{
    class CustomerRelationService : ICustomerRelationService
    {
        private readonly ICustomerRelationRepository _customerRelationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CustomerRelationService(IUnitOfWork unitOfWork, ICustomerRelationRepository customerRelationRepository)
        {

            this._unitOfWork = unitOfWork;
            this._customerRelationRepository = customerRelationRepository;
        }
        public IEnumerable<CustomerRelation> GetCustomerRelations()
        {
            var customerRelation = this._customerRelationRepository.GetAll().Where(i => i.IsDeleted == false);
            return customerRelation;
        }
        public CustomerRelation GetCustomerRelation(long id)
        {
            var customerRelation = this._customerRelationRepository.GetById(id);
            return customerRelation;
        }
        public IEnumerable<object> GetCustomerRelationForDropDown(string lang = "en", bool isFamily = false)
        {
            var awards = GetCustomerRelations().Where(x => x.IsDeleted == false);
			if (isFamily)
			{
                awards = GetCustomerRelations().Where(x => x.IsDeleted == false && x.Relation != "Personal");
            }
            var dropdownList = from customerRelation in awards
                               select new { value = customerRelation.ID, text = lang == "en" ? customerRelation.Relation : customerRelation.RelationAr };
            return dropdownList;
        }
    }
    public interface ICustomerRelationService
    {
        
        IEnumerable<object> GetCustomerRelationForDropDown(string lang = "en", bool isFamily = false);
        IEnumerable<CustomerRelation> GetCustomerRelations();
        CustomerRelation GetCustomerRelation(long id);
        //CustomerDocument GetCustomerDocument(long id);
        //bool CreateCustomerDocument(ref CustomerDocument customerDocument, ref string message);
        //bool UpdateCustomerDocument(ref CustomerDocument customerDocument, ref string message);
        //bool DeleteCustomerDocument(long id, ref string message, ref string filepath, bool softDelete = true);
        //bool SaveData();
    }
}
