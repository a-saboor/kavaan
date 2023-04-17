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
	class ContentManagementService : IContentManagementService
	{
		private readonly IContentManagementRepository _contentManagementRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ContentManagementService(IContentManagementRepository contentManagementRepository, IUnitOfWork unitOfWork)
		{
			this._contentManagementRepository = contentManagementRepository;
			this._unitOfWork = unitOfWork;
		}

		public IEnumerable<ContentManagement> GetContents()
		{
			var contents = _contentManagementRepository.GetAll();
			return contents;
		}

        //public List<ContentManagement> GetContentsByType(string type)
        //{
        //    var contents = _contentManagementRepository.GetContentsByType(type);
        //    return contents;
        //}

        public ContentManagement GetContent(long id)
		{
			var content = _contentManagementRepository.GetById(id);
			return content;
		}
		
		public ContentManagement GetEnableContent()
		{
			var content = this.GetContents().FirstOrDefault(x => x.IsEnable);
			return content;
		}

		public ContentManagement GetImageContent()
		{
			var content = this.GetContents().FirstOrDefault(x => x.Type == "Image");

			if(content == null)
			{
				content = new ContentManagement()
				{
					ID = 0,
					Path = "Assets/images/bg/main-bg.png",
					Type = "Image",
					IsEnable = true,
				};
			}

			return content;
		}

		public ContentManagement GetContentByType(string type)
        {
            var content = _contentManagementRepository.GetContentByType(type);
            return content;
        }

        public bool CreateContent(ContentManagement content, ref string message)
		{
			try
			{
				_contentManagementRepository.Add(content);
				if (SaveContent())
				{
					message = "File added successfully ...";
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

		public bool UpdateContent(ref ContentManagement ContentManagement, ref string message)
		{
			try
			{
				_contentManagementRepository.Update(ContentManagement);
				if (SaveContent())
				{
					return true;
				}
				else
				{
					return false;
				}

			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool DeleteContent(long id, bool softDelete = true)
		{
			try
			{
				ContentManagement content = _contentManagementRepository.GetById(id);
				if (softDelete)
				{
					_contentManagementRepository.Delete(content);
					SaveContent();
					return true;
				}

				return false;
			}
			catch (Exception ex)
			{

				return false;
			}
		}

		public bool SaveContent()
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
	public interface IContentManagementService
	{
		IEnumerable<ContentManagement> GetContents();
		//List<ContentManagement> GetContentsByType(string type);
		ContentManagement GetContent(long id);
		ContentManagement GetEnableContent();
		ContentManagement GetImageContent();
		ContentManagement GetContentByType(string type);

		bool CreateContent(ContentManagement content, ref string message);
		bool UpdateContent(ref ContentManagement ContentManagement, ref string message);
		bool DeleteContent(long id, bool softDelete = true);

		bool SaveContent();

	}
}
