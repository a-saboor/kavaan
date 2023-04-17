using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class AssetsSpecificationService : IAssetsSpecificationsService
    {
        private readonly IAssetsSpecificationsRepository _assetsSpecificationsRepository;
        private readonly IUnitOfWork _unitOfWork;
        public AssetsSpecificationService(IUnitOfWork unitOfWork, IAssetsSpecificationsRepository assetsSpecificationsRepository)
        {
            this._unitOfWork = unitOfWork;
            this._assetsSpecificationsRepository = assetsSpecificationsRepository;
        }
        public IEnumerable<AssetsSpecification> GetAssetsSpecifications()
        {
            var AssetsSpecifications = _assetsSpecificationsRepository.GetAll();
            return AssetsSpecifications;
        }
        public AssetsSpecification GetAssetsSpecification(long id)
        {
            var AssetsSpecifications = this._assetsSpecificationsRepository.GetById(id);
            return AssetsSpecifications;
        }
        public IEnumerable<AssetsSpecification> GetAssetsSpecificationByAssetID(int assetid)
        {
            var assetsSpecifications = _assetsSpecificationsRepository.GetAll().Where(x => x.AssetsID == assetid);
            return assetsSpecifications;
        }
        public bool CreateAssetSpecification(ref AssetsSpecification data, ref string message)
        {
            try
            {

                _assetsSpecificationsRepository.Add(data);
                if (SaveAssetSpecification())
                {
                    message = "Specification added successfully ...";
                    return true;

                }
                else
                {
                    message = "Oops! Something went wrong. Please try later.";
                    return false;
                }

            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later.";
                return false;
            }
        }
        public bool UpdateAssetSpecification(ref AssetsSpecification assetsSpecification, ref string message)
        {
            try
            {
                //if (_developmentDetailRepository.GetBrandByName(developmentDetail.Name, developmentDetail.ID) == null)
                //{
                AssetsSpecification CurrentDetail = _assetsSpecificationsRepository.GetById(assetsSpecification.ID);

                CurrentDetail.Name = assetsSpecification.Name;
                CurrentDetail.Value = assetsSpecification.Value;
                assetsSpecification = null;

                _assetsSpecificationsRepository.Update(CurrentDetail);
                if (SaveAssetSpecification())
                {
                    assetsSpecification = CurrentDetail;
                    message = "Specification updated successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later.";
                    return false;
                }
                //}
                //else
                //{
                //	message = "Country already exist  ...";
                //	return false;
                //}
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later.";
                return false;
            }
        }

        public bool DeleteAssetSpecification(long id, ref string message, bool softDelete = true)
        {
            try
            {
                AssetsSpecification assetsSpecification = _assetsSpecificationsRepository.GetById(id);



                _assetsSpecificationsRepository.Delete(assetsSpecification);

                if (SaveAssetSpecification())
                {
                    message = "Specification deleted successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later.";
                return false;
            }
        }

        public bool SaveAssetSpecification()
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
    public interface IAssetsSpecificationsService
    {
        AssetsSpecification GetAssetsSpecification(long id);
        IEnumerable<AssetsSpecification> GetAssetsSpecifications();
        IEnumerable<AssetsSpecification> GetAssetsSpecificationByAssetID(int assetid);
        bool DeleteAssetSpecification(long id, ref string message, bool softDelete = true);
        bool UpdateAssetSpecification(ref AssetsSpecification assetsSpecification, ref string message);
        bool CreateAssetSpecification(ref AssetsSpecification data, ref string message);
        bool SaveAssetSpecification();
    }
}
