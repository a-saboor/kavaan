using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    class BankService : IBankService
    {
        private readonly IBankRepository _bankRepository;
        private readonly IUnitOfWork _unitOfWork;
        public BankService(IUnitOfWork unitOfWork, IBankRepository bankRepository)
        {
            this._unitOfWork = unitOfWork;
            this._bankRepository = bankRepository;
        }
        public IEnumerable<Bank> GetBanks()
        {
            var bank = _bankRepository.GetAll().Where(i => i.IsDeleted == false);
            return bank;
        }
        public Bank GetBank(long id)
        {
            var bank = this._bankRepository.GetById(id);
            return bank;
        }
        public IEnumerable<object> GetBankForDropDown(string lang = "en")
        {
            var Banks = GetBanks().Where(x => x.IsActive == true && x.IsDeleted == false);
            var dropdownList = from banks in Banks
                               select new { value = banks.ID, text = lang == "en" ? banks.Name : banks.Name };
            return dropdownList;
        }
        public bool CreateBank(Bank bank, ref string message)
        {
            try
            {

                bank.IsActive = true;
                bank.IsDeleted = false;
                bank.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                _bankRepository.Add(bank);
                if (SaveBank())
                {
                    message = "Bank added successfully...";
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
        public bool UpdateBank(ref Bank bank, ref string message)
        {
            try
            {
                Bank currentbank = _bankRepository.GetById(bank.ID);
                currentbank.IsActive = bank.IsActive;

                _bankRepository.Update(bank);
                if (SaveBank())
                {
                    bank = currentbank;
                    message = "bank updated successfully ...";
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
        public bool DeleteBank(long id, ref string message, ref bool hasChilds, bool softdelete)
        {

            if (softdelete)
            {
                try
                {
                    //soft delete
                    Bank bank = _bankRepository.GetById(id);
                    //When department delete, its all refrences will be deleted
                    // delete code here.....
                    //*
                    bank.IsDeleted = true;
                    _bankRepository.Update(bank);

                    if (SaveBank())
                    {
                        message = "Bank deleted successfully ...";
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
                    Bank bank = _bankRepository.GetById(id);

                    _bankRepository.Delete(bank);

                    if (SaveBank())
                    {
                        message = "Bank deleted successfully ...";
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
        public bool SaveBank()
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
    public interface IBankService
    {
        Bank GetBank(long id);
        IEnumerable<Bank> GetBanks();
        IEnumerable<object> GetBankForDropDown(string lang = "en");
        bool CreateBank(Bank bank, ref string message);
        bool UpdateBank(ref Bank bank, ref string message);
        bool DeleteBank(long id, ref string message, ref bool hasContractorLink, bool softdelete);
        bool SaveBank();
    }
}
