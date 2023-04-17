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
	public class ContractorService : IContractorService
	{
		private readonly IContractorRepository _contractorRepository;
		private readonly IUnitOfWork _unitOfWork;

		public ContractorService(IContractorRepository contractorRepository, IUnitOfWork unitOfWork)
		{
			this._contractorRepository = contractorRepository;
			this._unitOfWork = unitOfWork;
		}

		public bool Create(Contractor contractor, ref string message)
		{
			try
			{
			
				if (this._contractorRepository.GetCountractorByName(contractor.Name) == null)
				{
					contractor.IsActive = true;
					contractor.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					contractor.IsDeleted = false;
					this._contractorRepository.Add(contractor);
					if (SaveContractor())
					{
						message = "Contractor Added Succesfully";
						return true;
					}
					else
					{
						message = "Oops! Something Went Wrong";
						return false;
					}
				}
				else
				{
					message = "Contractor Already Exist";
					return false;
				}
			}
			catch (Exception ex)
			{

				message = "Oops! Something Went Wrong";
				return false;

			}
		}

		public Contractor Edit(long id)
		{
			Contractor contractor = this._contractorRepository.GetById(id);
			return contractor;
		}
		public bool Edit(ref Contractor contractor, ref string message)
		{
			try
			{
				var contractordb = this._contractorRepository.GetCountractorByName(contractor.Name, contractor.ID);
				if (contractordb == null)
				{ this._contractorRepository.Update(contractor);


					if (SaveContractor())
					{
						message = "Contractor Added Succesfully";
						return true;
					}
					else
					{
						message = "Oops! Something Went Wrong";
						return false;
					}
				}
				else
				{
					message = "Contractor Already Exist";
					return false;
				}
			}
			catch (Exception)
			{
				message = "Oops! Something Went Wrong";
				return false;

			}
		}
		public bool Delete(long id, ref string message, bool softdelete = true)
		{
			try
			{
				Contractor contractor = this._contractorRepository.GetById(id);
				if (softdelete)
				{
					contractor.IsDeleted = true;
					this._contractorRepository.Update(contractor);

				}
				else
				{
					this._contractorRepository.Delete(contractor);
				}
				if (SaveContractor())
				{
					message = "Contracter Deleted Succesfully";
					return true;
				}
				else
				{
					message = "Oops! Something Went Wrong";
					return false;


				}

			}
			catch (Exception)
			{

				message = "Oops! Something Went Wrong. Pleae Try Again Later";
				return false;

			}
		}

		public IEnumerable<SP_GetFilteredContractors_Result> GetFilteredContractors(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer)
		{
			var consultants = _contractorRepository.GetFilteredContractors(search, pageSize, pageNumber, sortBy, lang, imageServer);
			return consultants;
		}

		public IEnumerable<Contractor> GetAll()
		{
			try
			{
				IEnumerable<Contractor> contractor = this._contractorRepository.GetAll().Where(x => x.IsDeleted == false);
				if (contractor != null)
				{
					return contractor;
				}
				return null;
			}
			catch (Exception)
			{

				return null;
			}
		}
		public bool SaveContractor()
		{
			try
			{
				_unitOfWork.Commit();
				return true;
			}
			catch (Exception ex)
			{
				string message = ex.Message;
				return false;
			}
		}


	}
	public interface IContractorService
	{
		bool Create(Contractor contractor, ref string message);
		Contractor Edit(long id);
		bool Edit(ref Contractor contractor, ref string message);
		bool Delete(long id, ref string message, bool softdelete = true);
		IEnumerable<Contractor> GetAll();

		IEnumerable<SP_GetFilteredContractors_Result> GetFilteredContractors(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, string imageServer);

	}
}
