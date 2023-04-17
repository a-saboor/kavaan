using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	public class AttributeService : IAttributeService
	{
		private readonly IAttributeRepository _attributeyRepository;
		private readonly IUnitOfWork _unitOfWork;

		public AttributeService(IAttributeRepository attributeyRepository, IUnitOfWork unitOfWork)
		{
			this._attributeyRepository = attributeyRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IAttributeService Members

		public IEnumerable<Attribute> GetAttributes()
		{
			var attributes = _attributeyRepository.GetAll();
			return attributes;
		}

		public IEnumerable<object> GetAttributesForDropDown()
		{
			var Attributes = _attributeyRepository.GetAll();
			var dropdownList = from attributes in Attributes
							   select new { value = attributes.ID, text = attributes.Name };
			return dropdownList;
		}

		public Attribute GetAttribute(long id)
		{
			var attributey = _attributeyRepository.GetById(id);
			return attributey;
		}

		public Attribute GetAttribute(string name)
		{
			var attributey = _attributeyRepository.GetAttributeByName(name);
			return attributey;
		}

		public bool CreateAttribute(ref Attribute attribute, ref string message)
		{
			try
			{
				if (_attributeyRepository.GetAttributeByName(attribute.Name) == null)
				{
					attribute.IsActive = attribute.IsActive.HasValue ? attribute.IsActive.Value : true;
					attribute.IsDeleted = attribute.IsDeleted.HasValue ? attribute.IsDeleted.Value : false;
					attribute.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_attributeyRepository.Add(attribute);
					if (SaveAttribute())
					{
						message = "Attribute added successfully ...";
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
					message = "Attribute already exist  ...";
					return false;
				}
			}
			catch (System.Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateAttribute(ref Attribute attributey, ref string message)
		{
			try
			{
				if (_attributeyRepository.GetAttributeByName(attributey.Name, attributey.ID) == null)
				{
					Attribute CurrentAttribute = _attributeyRepository.GetById(attributey.ID);

					CurrentAttribute.Name = attributey.Name;
					CurrentAttribute.NameAr = attributey.NameAr;

					attributey = null;

					_attributeyRepository.Update(CurrentAttribute);
					if (SaveAttribute())
					{
						attributey = CurrentAttribute;
						message = "Attribute updated successfully ...";
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
					message = "Attribute already exist  ...";
					return false;
				}
			}
			catch (System.Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteAttribute(long id, ref string message, bool softDelete = true)
		{
			try
			{
				Attribute attributey = _attributeyRepository.GetById(id);
				if (softDelete)
				{
					attributey.IsDeleted = true;
					_attributeyRepository.Update(attributey);
				}
				else
				{
					_attributeyRepository.Delete(attributey);
				}
				if (SaveAttribute())
				{
					message = "Attribute deleted successfully ...";
					return true;
				}
				else
				{
					message = "Oops! Something went wrong. Please try later.";
					return false;
				}
			}
			catch (System.Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool SaveAttribute()
		{
			try
			{
				_unitOfWork.Commit();
				return true;
			}
			catch (System.Exception ex)
			{
				return false;
			}
		}

		#endregion
	}

	public interface IAttributeService
	{
		IEnumerable<Attribute> GetAttributes();
		IEnumerable<object> GetAttributesForDropDown();
		Attribute GetAttribute(long id);
		Attribute GetAttribute(string name);
		bool CreateAttribute(ref Attribute attributey, ref string message);
		bool UpdateAttribute(ref Attribute attributey, ref string message);
		bool DeleteAttribute(long id, ref string message, bool softDelete = true);
		bool SaveAttribute();
	}
}
