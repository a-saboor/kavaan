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
    public class FeatureService : IFeatureService
    {
        private readonly IFeatureRepository featureRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FeatureService(IFeatureRepository featureRepository, IUnitOfWork unitOfWork)
        {
            this.featureRepository = featureRepository;
            this._unitOfWork = unitOfWork;
        }

        public IEnumerable<Feature> GetFeatures()
        {
            var features = featureRepository.GetAll().Where(d => d.IsDeleted == false);
            return features;
        }

        public Feature GetFeatureByID(long id)
        {
            var Feature = featureRepository.GetById(id);
            return Feature;
        }



        public bool CreateFeature(Feature feature, ref string message)
        {
            try
            {
                feature.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                feature.IsDeleted = false;
                feature.IsActive = true;
                feature.IsApproved = true;
                if (this.featureRepository.GetFeatureByTitle(feature.Name) == null)
                {

                    this.featureRepository.Add(feature);
                    if (SaveFeature())
                    {
                        message = "Feature added successfully ...";
                        return true;
                    }
                    else
                    {
                        message = "Oops something went wrong ...";
                        return false;
                    }
                }
                else
                {
                    message = "Feature already exist ...";
                    return false;
                }



            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public bool UpdateFeature(ref Feature feature, ref string message)
        {
            try
            {

              

              
                if (this.featureRepository.GetFeatureByTitle(feature.Name, feature.ID) == null)
                {
                    featureRepository.Update(feature);
                    if (SaveFeature())
                    {
                        message = "Feature updated successfully ...";
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
                    message = "Feature already exist ...";
                    return false;
                }


            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool DeleteFeature(long id, ref string message, bool softdelete)
        {
            if (softdelete)
            {
                try
                {

                    //soft delete
                    Feature Feature = featureRepository.GetById(id);
                    Feature.IsDeleted = true;
                    featureRepository.Update(Feature);

                    if (SaveFeature())
                    {
                        message = "Feature deleted successfully ...";
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
                    Feature Feature = featureRepository.GetById(id);

                    featureRepository.Delete(Feature);

                    if (SaveFeature())
                    {
                        message = "Feature deleted successfully ...";
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

        public bool SaveFeature()
        {
            try
            {
                this._unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<object> GetFeatureForDropDown()
        {
            var Feature = featureRepository.GetAll().Where(x => x.IsActive == true && x.IsDeleted == false);
            var FeatureDropDown = from feature in Feature
                                  select new { value = feature.ID, text = feature.Name };

            return FeatureDropDown;
        }
    }
    public interface IFeatureService
    {
        IEnumerable<Feature> GetFeatures();
        Feature GetFeatureByID(long id);
        IEnumerable<object> GetFeatureForDropDown();
        bool CreateFeature(Feature newsFeed, ref string message);
        bool UpdateFeature(ref Feature newsFeed, ref string message);
        bool DeleteFeature(long id, ref string message, bool softdelete);
        bool SaveFeature();


    }
}
