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
	public class ProductVariantService : IProductVariantService
	{
		private readonly IProductVariantRepository _productvariantRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ProductVariantService(IProductVariantRepository productvariantRepository, IUnitOfWork unitOfWork)
		{
			this._productvariantRepository = productvariantRepository;
			this._unitOfWork = unitOfWork;
		}

		#region IProductVariantService Members

		public IEnumerable<ProductVariant> GetProductVariants()
		{
			var productvariants = _productvariantRepository.GetAll();
			return productvariants;
		}

		public IEnumerable<object> GetProductVariantsForDropDown()
		{
			var ProductVariants = _productvariantRepository.GetAll();
			var dropdownList = from productvariants in ProductVariants
							   select new { value = productvariants.ID, text = productvariants.Name };
			return dropdownList;
		}

		public ProductVariant GetProductVariant(long id)
		{
			var productvariant = _productvariantRepository.GetById(id);
			return productvariant;
		}

		public bool CreateProductVariant(ProductVariant productvariant, ref string message)
		{
			try
			{
				if (_productvariantRepository.GetProductVariantBySKU((long)productvariant.ProductID, productvariant.SKU) == null)
				{
					productvariant.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_productvariantRepository.Add(productvariant);
					if (SaveProductVariant())
					{
						message = "ProductVariant added successfully ...";
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
					message = "ProductVariant already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool UpdateProductVariant(ref ProductVariant productvariant, ref string message)
		{
			try
			{
				if (_productvariantRepository.GetProductVariantBySKU((long)productvariant.ProductID, productvariant.SKU, productvariant.ID) == null)
				{
					ProductVariant CurrentProductVariant = _productvariantRepository.GetById(productvariant.ID);

					CurrentProductVariant.Name = productvariant.Name;
					CurrentProductVariant.NameAr = productvariant.NameAr;
					productvariant = null;

					_productvariantRepository.Update(CurrentProductVariant);
					if (SaveProductVariant())
					{
						productvariant = CurrentProductVariant;
						message = "ProductVariant updated successfully ...";
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
					message = "ProductVariant already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later.";
				return false;
			}
		}

		public bool DeleteProductVariant(long id, ref string message)
		{
			try
			{
				ProductVariant productvariant = _productvariantRepository.GetById(id);
				
				if (SaveProductVariant())
				{
					message = "ProductVariant deleted successfully ...";
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

		public bool SaveProductVariant()
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

	public interface IProductVariantService
	{
		IEnumerable<ProductVariant> GetProductVariants();
		IEnumerable<object> GetProductVariantsForDropDown();
		ProductVariant GetProductVariant(long id);
		bool CreateProductVariant(ProductVariant productvariant, ref string message);
		bool UpdateProductVariant(ref ProductVariant productvariant, ref string message);
		bool DeleteProductVariant(long id, ref string message);
		bool SaveProductVariant();
	}
}
