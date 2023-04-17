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
    public class PropertyFeaturesService : IPropertyFeaturesService
    {
        private readonly IPropertyFeaturesRepository propertyFeaturesRepository;
        private readonly IPropertyRepository propertyRepository;
        private readonly IUnitOfWork _unitOfWork;
        public PropertyFeaturesService(IPropertyFeaturesRepository propertyFeaturesRepository, IUnitOfWork unitOfWork, IPropertyRepository propertyRepository)
        {
            this.propertyFeaturesRepository = propertyFeaturesRepository;
            this.propertyRepository = propertyRepository;
            this._unitOfWork = unitOfWork;
        }
        #region Community Service


        public bool Create(PropertyFeature propertyFeature, ref string message)
        {
            try
            {

                this.propertyFeaturesRepository.Add(propertyFeature);
                if (this.propertyFeaturesRepository.GetPropertyFeatureByID((long)propertyFeature.PropertyID,(long)propertyFeature.FeatureID, (int)propertyFeature.Position) == null)
                {
                    if (SaveDatabase())
                    {
                        message = "Project feature added successfully ...";
                        return true;

                    }
                    message = "Oops something went wrong ...";
                    return false;
                }
                else
                {
                    message = "Feature or position already selected for this project ...";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public bool DeletePropertyFeatures(long id, ref string message, ref bool hasChilds, bool softDelete = true)
        {
            if (softDelete)
            {
                try
                {

                    //soft delete
                    PropertyFeature propertyfeature = this.propertyFeaturesRepository.GetById(id);
                    propertyfeature.IsDeleted = true;
                    this.propertyFeaturesRepository.Update(propertyfeature);

                    if (SaveDatabase())
                    {
                        message = "Project feature deleted successfully ...";
                        return true;
                    }
                    else
                    {
                        message = "Oops! something went wrong ...";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    message = "Oops! something went wrong ...";
                    return false;
                }
            }
            return false;
            //else
            //{

            //    ////hard delete
            //    //try
            //    //{
            //    //    Blog Blog = _newsFeedRepository.GetById(id);

            //    //    _newsFeedRepository.Delete(Blog);

            //    //    if (SaveNewsfeed())
            //    //    {
            //    //        message = "Blog deleted successfully ...";
            //    //        return true;
            //    //    }
            //    //    else
            //    //    {
            //    //        message = "Oops! Something went wrong. Please try later...";
            //    //        return false;
            //    //    }
            //    //}
            //    //catch (Exception ex)
            //    //{
            //    //    message = "Oops! Something went wrong. Please try later...";
            //    //    return false;
            //    //}
            //}
        }

        public IEnumerable<PropertyFeature> GetPropertyFeatures()
        {
            var propertyfeature = this.propertyFeaturesRepository.GetAll().Where(x => x.IsDeleted == false);
            return propertyfeature;
        }

        public IEnumerable<PropertyFeature> GetPropertyFeaturesByPropertyID(long projectId)
        {
            var projectfeature = this.propertyFeaturesRepository.GetPropertyFeaturesByPropertyID(projectId);

            return projectfeature;
        }
        public PropertyFeature GetPropertyFeatures(long propertyid)
        {
            var propertyfeatures = this.propertyFeaturesRepository.GetAll().Where(x => x.PropertyID == propertyid && x.IsDeleted == false).FirstOrDefault();
            return propertyfeatures;
        }
        public PropertyFeature GetPropertyFeaturesByID(long id)
        {
            var propertyfeature = this.propertyFeaturesRepository.GetById(id);
            return propertyfeature;
        }

        public bool UpdateProperyFeatures(ref PropertyFeature propertyfeature, ref string message, bool updater = true, long id = 0)
        {
            try
            {




                if (this.propertyFeaturesRepository.GetPropertyFeatureByID((long)propertyfeature.PropertyID,(long)propertyfeature.FeatureID, (int)propertyfeature.Position, propertyfeature.ID) == null)
                {
                    this.propertyFeaturesRepository.Update(propertyfeature);

                    if (SaveDatabase())
                    {
                        message = "Project features updated successfully ...";
                        return true;
                    }
                    message = "Oops something went wrong ...";
                    return false;
                }
                else
                {
                    message = "Feature or position already selected for this project ...";
                    return false;
                }


            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }

        }
        public bool SaveDatabase()
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
    public interface IPropertyFeaturesService
    {
        PropertyFeature GetPropertyFeatures(long propertyid);
        PropertyFeature GetPropertyFeaturesByID(long id);

        IEnumerable<PropertyFeature> GetPropertyFeaturesByPropertyID(long projectId);

        IEnumerable<PropertyFeature> GetPropertyFeatures();
        bool Create(PropertyFeature propertyfeature, ref string message);
        bool UpdateProperyFeatures(ref PropertyFeature propertyfeature, ref string message, bool updater = true, long id = 0);

        bool DeletePropertyFeatures(long id, ref string message, ref bool hasChilds, bool softDelete = true);

    }
}
