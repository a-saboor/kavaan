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
    public class PropertyService : IPropertyService
    {
        private readonly IPropertyRepository _propertyRepositoy;
        private readonly IPropertyFeaturesRepository propertyFeaturesRepository;
        private readonly IPropertyProgressService propertyprogressservice;
        private readonly IProgressRepository progressRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IUnitRepository _unitRepository;

        public PropertyService(IPropertyRepository propertyRepositoy, IPropertyFeaturesRepository propertyFeaturesRepository, IUnitOfWork unitOfWork, IPropertyProgressService propertyprogressservice, IProgressRepository progressRepository, IUnitRepository unitRepository)
        {
            _propertyRepositoy = propertyRepositoy;
            this.propertyFeaturesRepository = propertyFeaturesRepository;
            _unitOfWork = unitOfWork;
            this.propertyprogressservice = propertyprogressservice;
            this.progressRepository = progressRepository;
            this._unitRepository = unitRepository;
        }
        #region Property Service


        public bool Create(Property Property, ref string message)
        {
            try
            {
                Property.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                Property.IsDeleted = false;
                Property.IsActive = true;   

                _propertyRepositoy.Add(Property);
                IEnumerable<Progress> progresses = this.progressRepository.GetAll();

                if (this._propertyRepositoy.GetPropertyByTitle(Property.Title) == null)
                {
                    if (SaveProperty())
                    {
                        string propmessage = string.Empty;
                        foreach (var progress in progresses)
                        {
                            PropertyProgress propertyProgress = new PropertyProgress();
                            propertyProgress.PropertyID = Property.ID;
                            propertyProgress.ProgressesPercent = 0;
                            propertyProgress.UpdatedOn = Helpers.TimeZone.GetLocalDateTime();
                            propertyProgress.IsActive = true;
                            propertyProgress.ProgressId = progress.ID;
                            this.propertyprogressservice.Create(propertyProgress, ref propmessage);
                        }

                        message = "Project added successfully ...";
                        return true;

                    }

                }
                else
                {
                    message = "Project already exist ...";
                    return false;
                }
                message = "Oops! something went wrong ...";
                return false;


            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public IEnumerable<Property> GetProperties()
        {
            var property = _propertyRepositoy.GetAll().Where(x => x.IsDeleted == false);
            return property;
        }

        public IEnumerable<Property> GetPropertiesByDevelopmentID(long developmentID)
        {
            var property = GetProperties().Where(x=>x.DevelopmentID == developmentID);
            return property;
        }

        public Property GetProperty(long id)
        {
            Property property = _propertyRepositoy.GetById(id);
            return property;
        }
        public Property GetPropertyFeatures(long id)
        {
            Property property = new Property();
            property = _propertyRepositoy.GetById(id);
            property.PropertyFeatures = property.PropertyFeatures.Where(x => x.IsDeleted == false).ToList();



            return property;


        }

        public bool ActivateProperties(long developmentID, ref string message, bool isActive = true)
        {
            try
            {
                var properties = GetPropertiesByDevelopmentID(developmentID);
                
				foreach (var property in properties)
				{
                    property.IsActive = isActive;
                    _propertyRepositoy.Update(property);
                }

                if (SaveProperty())
                {
                    message = "Project updated successfully ...";
                    return true;
                }
                message = "Oops! something went wrong ...";
                return false;
            }
            catch (Exception ex)
            {
                message = "Oops Something Went Wrong";
                return false;
            }
        }
        public bool DeleteProperties(long developmentID, ref string message, bool softDelete = true)
        {
            try
            {
                var properties = GetPropertiesByDevelopmentID(developmentID);
				foreach (var property in properties)
				{
                    property.IsDeleted = true;
                    if (property != null && softDelete)
                    {
                        _propertyRepositoy.Update(property);

                        //setting deleting flag to units too;
                        if (property.Units.Count() > 0)
                        {
                            foreach (Unit unit in property.Units)
                            {
                                unit.IsDeleted = true;
                                this._unitRepository.Update(unit);
                            }
                        }
                        //end deleting units

                    }
                    else if (property != null && softDelete == false)
                    {
                        this.propertyFeaturesRepository.Delete(d => d.PropertyID == property.ID);
                        this._unitRepository.Delete(d => d.PropertyID == property.ID);
                        
                        _propertyRepositoy.Delete(property);
                    }
                }

                if (SaveProperty())
                {
                    message = "Project deleted successfully ...";
                    return true;
                }
                return true;
            }
            catch (Exception)
            {
                message = "Oops! Something Went Wrong";
                return false;
            }

        }

        public bool UpdateProperty(ref Property currentproperty, ref string message, bool updater = true)
        {
            try
            {

                if ((this._propertyRepositoy.GetPropertyByTitle(currentproperty.Title, currentproperty.ID)) == null)
                {
                    _propertyRepositoy.Update(currentproperty);

                    if (SaveProperty())
                    {

                        message = "Project updated successfully ...";
                        return true;

                    }
                    message = "Oops! something went wrong ...";
                    return false;

                }
                else
                {
                    message = "Project already exist ...";
                    return false;
                }

            }
            catch (Exception ex)
            {
                message = "Oops Something Went Wrong";
                return false;

            }
        }
        public bool DeleteProperty(long id, ref string message, ref bool hasChilds, bool softDelete = true)
        {

            try
            {
                var property = _propertyRepositoy.GetById(id);

                property.IsDeleted = true;
                if (property != null && softDelete)
                {

                    _propertyRepositoy.Update(property);

                    //setting deleting flag to units too;
                    if (property.Units != null)
                    {
                        foreach (Unit unit in property.Units)
                        {
                            unit.IsDeleted = true;
                            this._unitRepository.Update(unit);
                        }
                    }
                    //end deleting units

                    if (SaveProperty())
                    {
                        message = "Project deleted successfully ...";
                        return true;
                    }
                }
                else if (property != null && softDelete == false)
                {

                    this.propertyFeaturesRepository.Delete(d => d.PropertyID == id);
                    _propertyRepositoy.Delete(property);
                    message = "Project permanently deleted successfully ...";
                    return true;
                }

                message = "Oops! Something Went Wrong";
                return true;
            }
            catch (Exception)
            {
                message = "Oops! Something Went Wrong";
                return false;
            }



        }

        //public IEnumerable<Property> GetCategories()
        //{
        //    throw new NotImplementedException();
        //}

        //public int GetCategoriesForDashboard()
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<object> GetCategoriesForDropDown(long? id)
        //{
        //    throw new NotImplementedException();
        //}

        //public IEnumerable<object> GetCategoriesForDropDown(string lang = "en")
        //{
        //    throw new NotImplementedException();
        //}

        //public Property GetCommunity(long id)
        //{
        //    throw new NotImplementedException();
        //}

        //public bool SaveProperty()
        //{
        //    throw new NotImplementedException();
        //}

        //public bool UpdateProperty(ref Property Property, ref string message, bool updater = true)
        //{
        //    throw new NotImplementedException();
        //}
        public bool SaveProperty()
        {
            try
            {
                _unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                var exp = ex.Message;
                return false;
            }
        }

        public IEnumerable<object> GetPropertiesForDropDown()
        {
            var Properties = GetProperties().Where(x => x.IsActive == true);
            var dropdownList = from property in Properties
                               select new { value = property.ID, text = property.Title };
            return dropdownList;
        }

        public IEnumerable<SP_GetFilteredProperties_Result> GetFilteredProperties(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> developmentID, Nullable<bool> isfeatured, Nullable<bool> vrTour)
        {
            var properties = _propertyRepositoy.GetFilteredProperties(search, pageSize, pageNumber, sortBy, lang, imageServer, developmentID,isfeatured,vrTour);
            return properties;
        }


        #endregion
    }
    public interface IPropertyService
    {
        Property GetProperty(long id);
        Property GetPropertyFeatures(long id);
        IEnumerable<Property> GetProperties();
        IEnumerable<Property> GetPropertiesByDevelopmentID(long developmentID);
        bool Create(Property Property, ref string message);

        bool ActivateProperties(long developmentID, ref string message, bool isActive = true);
        bool DeleteProperties(long developmentID, ref string message, bool softDelete = true);

        bool UpdateProperty(ref Property Property, ref string message, bool updater = true);

        bool DeleteProperty(long id, ref string message, ref bool hasChilds, bool softDelete = true);
        IEnumerable<object> GetPropertiesForDropDown();
        IEnumerable<SP_GetFilteredProperties_Result> GetFilteredProperties(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer, Nullable<long> developmentID, Nullable<bool> isfeatured, Nullable<bool> vrTour);


    }
}
