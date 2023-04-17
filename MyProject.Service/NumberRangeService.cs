using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{

	public class NumberRangeService : INumberRangeService
	{
		private readonly INumberRangeRepository _numberRangeRepository;
		private readonly IUnitOfWork _unitOfWork;

		public NumberRangeService(INumberRangeRepository numberRangeRepository, IUnitOfWork unitOfWork)
		{
			this._numberRangeRepository = numberRangeRepository;
			this._unitOfWork = unitOfWork;
		}

		#region INumberRangeService Members

		public IEnumerable<NumberRange> GetNumberRanges()
		{
			var numberRanges = _numberRangeRepository.GetAll();
			return numberRanges;
		}

		public NumberRange GetNumberRange(long id)
		{
			var numberRange = _numberRangeRepository.GetById(id);
			return numberRange;
		}

		public string GetNextValueFromNumberRangeByName(string name)
		{
			var numberRange = _numberRangeRepository.GetNumberRangeByName(name);
			if (numberRange != null)
			{
				string code = numberRange.Prefix + numberRange.CurrentValue.Value.ToString().PadLeft(numberRange.PaddingZero.Value, '0');

				numberRange.CurrentValue += 1;
				_numberRangeRepository.Update(numberRange);
				if (SaveNumberRange())
					return code;
				else
					return string.Empty;
			}
			return string.Empty;
		}

		public bool CreateNumberRange(NumberRange numberRange, ref string message)
		{
			try
			{
				if (_numberRangeRepository.GetNumberRangeByName(numberRange.Name) == null)
				{
					numberRange.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
					_numberRangeRepository.Add(numberRange);
					if (SaveNumberRange())
					{
						message = "NumberRange added successfully ...";
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
					message = "NumberRange already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool UpdateNumberRange(ref NumberRange numberRange, ref string message)
		{
			try
			{
				if (_numberRangeRepository.GetNumberRangeByName(numberRange.Name, numberRange.Id) == null)
				{
					NumberRange CurrentNumberRange = _numberRangeRepository.GetById(numberRange.Id);

					CurrentNumberRange.Name = numberRange.Name;
					CurrentNumberRange.Prefix = numberRange.Prefix;
					CurrentNumberRange.InitialValue = numberRange.InitialValue;
					CurrentNumberRange.CurrentValue = numberRange.CurrentValue;
					CurrentNumberRange.IncreamentValue = numberRange.IncreamentValue;
					CurrentNumberRange.PaddingZero = numberRange.PaddingZero;
					numberRange = null;

					_numberRangeRepository.Update(CurrentNumberRange);
					if (SaveNumberRange())
					{
						numberRange = CurrentNumberRange;
						message = "NumberRange updated successfully ...";
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
					message = "NumberRange already exist  ...";
					return false;
				}
			}
			catch (Exception ex)
			{
				message = "Oops! Something went wrong. Please try later...";
				return false;
			}
		}

		public bool DeleteNumberRange(long id, ref string message)
		{
			try
			{
				NumberRange numberRange = _numberRangeRepository.GetById(id);

				_numberRangeRepository.Delete(numberRange);

				if (SaveNumberRange())
				{
					message = "NumberRange deleted successfully ...";
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

		public bool SaveNumberRange()
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

	public interface INumberRangeService
	{
		IEnumerable<NumberRange> GetNumberRanges();
		NumberRange GetNumberRange(long id);
		string GetNextValueFromNumberRangeByName(string name);
		bool CreateNumberRange(NumberRange numberRange, ref string message);
		bool UpdateNumberRange(ref NumberRange numberRange, ref string message);
		bool DeleteNumberRange(long id, ref string message);
		bool SaveNumberRange();
	}
}
