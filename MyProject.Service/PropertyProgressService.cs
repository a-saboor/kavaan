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
    public class PropertyProgressService: IPropertyProgressService
    {
        public readonly IPropertyProgressRepository propertyProgressRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PropertyProgressService(IPropertyProgressRepository propertyProgressRepository, IUnitOfWork unitOfWork)
        {
            this.propertyProgressRepository = propertyProgressRepository;
            this._unitOfWork = unitOfWork;
        }

        #region Property Service

        public IEnumerable<PropertyProgress> GetPropertyProgressesByProjectId(long projectId)
        {
            var progress = propertyProgressRepository.GetProgressByProject(projectId);
            return progress;
        }
        public bool Create( PropertyProgress propertyProgress, ref string message)
        {
            try
            {

                this.propertyProgressRepository.Add(propertyProgress);

                if (SaveDatabase())
                {
                    message = "Project progress added successfully...";
                    return true;

                }
                return false;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
        }

        public PropertyProgress Edit(long propertyid)
        {
            var propertyProgress = this.propertyProgressRepository.GetById(propertyid);
            return propertyProgress;
        }
        public bool Edit(PropertyProgress propertyfeature, ref string message, bool updater = true,long id = 0)
        {
            try
            {

                var db = this.propertyProgressRepository.GetById(propertyfeature.ID);
                if (db != null)
                {
                    db.ProgressesPercent = propertyfeature.ProgressesPercent;

                    this.propertyProgressRepository.Update(db);

                    if (SaveDatabase())
                    {
                        message = "Project progress updated successfully ...";
                        return true;
                    }
                }
                message = "Project feature doesn't exist...";
                return false;

            }
            catch (Exception ex)
            {
                message = ex.Message;
                return false;
            }
            return false;
        }
        public bool Delete(long id, ref string message, ref bool hasChilds, bool softDelete = true)
        {
            if (softDelete)
            {
                try
                {

                    //soft delete
                    PropertyProgress propertyfeature = this.propertyProgressRepository.GetById(id);
                    propertyfeature.IsActive = false;
                    this.propertyProgressRepository.Update(propertyfeature);

                    if (SaveDatabase())
                    {
                        message = "Project feature deleted successfully ...";
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
            return false;
          
        }

        public IEnumerable<PropertyProgress> GetAll()
        {
            var propertyfeature = this.propertyProgressRepository.GetAll();
            return propertyfeature;
        }
        public IEnumerable<PropertyProgress> GetAll(long propertyid)
        {
            var propertyfeature = this.propertyProgressRepository.GetAll().Where(x=>x.PropertyID==propertyid &&x.Progress.IsActive==true &&x.Progress.IsDeleted==false).ToList();
            return propertyfeature;
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
    public interface IPropertyProgressService
    {
        IEnumerable<PropertyProgress> GetPropertyProgressesByProjectId(long projectId);
        bool Create(PropertyProgress PropertyProgress, ref string message);
        PropertyProgress Edit(long id);
        bool Edit(PropertyProgress PropertyProgress, ref string message, bool updater = true, long id = 0);
        bool Delete(long id, ref string message, ref bool hasChilds, bool softDelete = true);
      
        IEnumerable<PropertyProgress> GetAll();
        IEnumerable<PropertyProgress> GetAll(long propertyid);

    }
}
