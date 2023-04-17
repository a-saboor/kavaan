using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class TagService : ITagService
	{
		private readonly ITagRepository _tagRepository;
		private readonly IUnitOfWork _unitOfWork;

		public TagService(ITagRepository tagRepository, IUnitOfWork unitOfWork)
		{
			this._tagRepository = tagRepository;
			this._unitOfWork = unitOfWork;
		}

		#region ITagService Members

		public IEnumerable<Tag> GetTags()
		{
			var tags = _tagRepository.GetAll();
			var filtertags = _tagRepository.GetAll().Where(x => x.IsDeleted == false);
			return filtertags;
		}

		public IEnumerable<object> GetTagsForDropDown()
		{
			var Tags = _tagRepository.GetAll();
			var dropdownList = from tags in Tags
							   select new { value = tags.ID, text = tags.Name };
			return dropdownList;
		}

		public Tag GetTag(long id)
		{
			var tag = _tagRepository.GetById(id);
			return tag;
		}

		public bool CreateTag(ref Tag tag, ref string message)
		{
			try
			{
				if (_tagRepository.GetTagByName(tag.Name) == null)
				{
					tag.IsActive = true;
					tag.IsDeleted = false;
					tag.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_tagRepository.Add(tag);
					if (SaveTag())
					{

						message = "Tag added successfully ...";
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
					message = "Tag already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateTag(ref Tag tag, ref string message)
		{
			try
			{
				if (_tagRepository.GetTagByName(tag.Name, tag.ID) == null)
				{
					Tag CurrentTag = _tagRepository.GetById(tag.ID);

					CurrentTag.Name = tag.Name;
					CurrentTag.NameAr = tag.NameAr;

					tag = null;

					_tagRepository.Update(CurrentTag);
					if (SaveTag())
					{
						tag = CurrentTag;
						message = "Tag updated successfully ...";
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
					message = "Tag already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteTag(long id, ref string message, bool softDelete = true)
		{
			try
			{
				Tag tag = _tagRepository.GetById(id);
				if (softDelete)
				{
					tag.IsDeleted = true;
					_tagRepository.Update(tag);
				}
				else
				{
					_tagRepository.Delete(tag);
				}
				if (SaveTag())
				{
					message = "Tag deleted successfully ...";
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

		public bool SaveTag()
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

	public interface ITagService
	{
		IEnumerable<Tag> GetTags();
		IEnumerable<object> GetTagsForDropDown();
		Tag GetTag(long id);
		bool CreateTag(ref Tag tag, ref string message);
		bool UpdateTag(ref Tag tag, ref string message);
		bool DeleteTag(long id, ref string message, bool softDelete = true);
		bool SaveTag();
	}
}
