using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{
	public class ProjectTagService : IProjectTagService
	{
		private readonly IProjectTagRepository _projectTagRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProjectTagService(IProjectTagRepository projectTagRepository, IUnitOfWork unitOfWork)
		{
			this._projectTagRepository = projectTagRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProjectTagService Members

		public IEnumerable<ProjectTag> GetProjectTags()
		{
			var projectCategories = _projectTagRepository.GetAll();
			return projectCategories;
		}

		public IEnumerable<ProjectTag> GetProjectTags(long projectId)
		{
			var projectCategories = _projectTagRepository.GetProjectTags(projectId);
			return projectCategories;
		}

		public ProjectTag GetProjectTag(long id)
		{
			var projectTag = _projectTagRepository.GetById(id);
			return projectTag;
		}

		public bool CreateProjectTag(ref ProjectTag projectTag, ref string message)
		{
			try
			{
				if (_projectTagRepository.GetProjectTag((long)projectTag.ProjectID, (long)projectTag.TagID) == null)
				{
					_projectTagRepository.Add(projectTag);
					if (SaveProjectTag())
					{

						message = "Project tag added successfully ...";
						return true;

					}
					else
					{
						message = "Oops! Something went wrong. Please try later.";
						return false;
					}
				}
				else
				{
					message = "Project tag already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateProjectTag(ref ProjectTag projectTag, ref string message)
		{
			try
			{
				if (_projectTagRepository.GetProjectTag((long)projectTag.ProjectID, (long)projectTag.TagID, projectTag.ID) == null)
				{
					_projectTagRepository.Update(projectTag);
					if (SaveProjectTag())
					{
						message = "Project tag updated successfully ...";
						return true;
					}
					else
					{
						message = "Oops! Something went wrong. Please try later.";
						return false;
					}
				}
				else
				{
					message = "Project tag already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteProjectTag(long id, ref string message)
		{
			try
			{
				ProjectTag projectTag = _projectTagRepository.GetById(id);
				_projectTagRepository.Delete(projectTag);
				if (SaveProjectTag())
				{
					message = "Project tag deleted successfully ...";
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

		public bool DeleteProjectTag(ProjectTag projectTag, ref string message)
		{
			try
			{
				ProjectTag currentProjectTag = _projectTagRepository.GetProjectTag((long)projectTag.ProjectID, (long)projectTag.TagID);
				projectTag = null;
				_projectTagRepository.Delete(currentProjectTag);
				if (SaveProjectTag())
				{
					message = "Project tag deleted successfully ...";
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

		public bool SaveProjectTag()
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
        public bool PostExcelData(long ProjectID,string ProjectTags)
        {
            try
            {
                DateTime CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _projectTagRepository.InsertProjectTags(ProjectID, ProjectTags);
                //SaveProjectTag();
                return true;
            }
            catch (Exception ex)
            {
                //log.Error("Error", ex);
                //log.Error("Error", ex);
                return false;
            }
        }

        #endregion
    }

	public interface IProjectTagService
	{
        bool PostExcelData(long ProjectID, string ProjectTags);

        IEnumerable<ProjectTag> GetProjectTags();
		IEnumerable<ProjectTag> GetProjectTags(long projectId);
		ProjectTag GetProjectTag(long id);
		bool CreateProjectTag(ref ProjectTag projectTag, ref string message);
		bool UpdateProjectTag(ref ProjectTag projectTag, ref string message);
		bool DeleteProjectTag(long id, ref string message);
		bool DeleteProjectTag(ProjectTag projectTag, ref string message);
		bool SaveProjectTag();
	}
}
