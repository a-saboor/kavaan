using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class UnitBookingService : IUnitBookingService
    {
        private readonly IUnitBookingRepository unitBookingRepository;
        private readonly IUnitOfWork unitOfWork;

        public UnitBookingService(IUnitBookingRepository unitBookingRepository, IUnitOfWork unitOfWork)
        {
            this.unitBookingRepository = unitBookingRepository;
            this.unitOfWork = unitOfWork;
        }

        #region IUnitBookingService Members
        public IEnumerable<UnitBooking> GetUnitBookings()
        {
            var unitbooking = this.unitBookingRepository.GetAll();
            return unitbooking;
        }
        public IEnumerable<UnitBooking> GetUnitBookings(DateTime FromDate, DateTime ToDate, string status)
        {
            var data = this.unitBookingRepository.GetFilteredUnitBookings(FromDate, ToDate, status);
            return data;
        }
        public IEnumerable<UnitBooking> GetUnitBookings(string status)
        {
            var data = this.GetUnitBookings().Where(b => b.BookingStatus == status);
            return data;
        }

        public IEnumerable<UnitBooking> GetUnitBookingByCustomerId(long customerId)
        {
            var unitbooking = this.unitBookingRepository.GetUnitBookingsByCustomer(customerId);
            return unitbooking;
        }

        public UnitBooking GetUnitBooking(long id)
        {
            var country = this.unitBookingRepository.GetById(id);
            return country;
        }

        //public UnitBooking GetUnitBookingByName(string name)
        //{
        //    var booking = this.unitBookingRepository.getuni(name);
        //    return booking;
        //}

        public bool CreateUnitBooking(ref UnitBooking booking, ref string message)
        {
            try
            {
                booking.CreatedOn = Helpers.TimeZone.GetLocalDateTime();

                this.unitBookingRepository.Add(booking);

                if (SaveUnitBooking())
                {
                    message = "Unit Payment Plan added successfully ...";
                    return true;
                }

                else
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }
                //}
                //else
                //{
                //    message = "Unit Payment Plan MileStone already exist  ...";
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool UpdateUnitBooking(ref UnitBooking UnitBooking, ref string message)
        {
            try
            {
                //if (this.unitBookingRepository.GetUnitBookingeByName(UnitBooking.Milestones, UnitBooking.ID) == null)
                //{
                UnitBooking CurrentUnitBooking = this.unitBookingRepository.GetById(UnitBooking.ID);

                CurrentUnitBooking.ID = UnitBooking.ID;
                CurrentUnitBooking.BookingNo = UnitBooking.BookingNo;
                //CurrentUnitBooking.FirstName = UnitBooking.FirstName;
                //CurrentUnitBooking.LastName = UnitBooking.LastName;
                //CurrentUnitBooking.Email = UnitBooking.Email;
                //CurrentUnitBooking.Contact = UnitBooking.Contact;
                //CurrentUnitBooking.Address = UnitBooking.Address;
                //CurrentUnitBooking.CityID = UnitBooking.CityID;
                //CurrentUnitBooking.PoBox = UnitBooking.PoBox;
                //CurrentUnitBooking.ZipCode = UnitBooking.ZipCode;
                //CurrentUnitBooking.Status = UnitBooking.Status;
                //CurrentUnitBooking.CNICNo = UnitBooking.CNICNo;
                //CurrentUnitBooking.CNICExpiry = UnitBooking.CNICExpiry;
                //CurrentUnitBooking.PassportNo = UnitBooking.PassportNo;
                //CurrentUnitBooking.PassportExpiry = UnitBooking.PassportExpiry;
                CurrentUnitBooking.Status = UnitBooking.Status;


                this.unitBookingRepository.Update(CurrentUnitBooking);
                if (SaveUnitBooking())
                {
                    UnitBooking = null;
                    UnitBooking = CurrentUnitBooking;
                    message = "Unit Payment Plan updated successfully ...";
                    return true;
                }
                else
                {
                    message = "Oops! Something went wrong. Please try later...";
                    return false;
                }
                // }
                //else
                //{
                //    message = "Unit Payment Plan already exist  ...";
                //    return false;
                //}
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool DeleteUnitBooking(long id, ref string message, bool softDelete = true)
        {
            try
            {
                UnitBooking unitBookingPlan = this.unitBookingRepository.GetById(id);

                //if (softDelete)
                //{
                //    unitBookingPlan.IsDeleted = true;
                //    this.unitBookingRepository.Update(country);
                //}
                //else
                //{
                //    this.unitBookingRepository.Delete(country);
                //}
                //if (SaveUnitBookingPlan())
                //{
                //    message = "UnitBookingPlan deleted successfully ...";
                //    return true;
                //}
                //else
                //{
                //    message = "Oops! Something went wrong. Please try later...";
                //    return false;
                //}
                return false;
            }
            catch (Exception ex)
            {
                message = "Oops! Something went wrong. Please try later...";
                return false;
            }
        }

        public bool SaveUnitBooking()
        {
            try
            {
                this.unitOfWork.Commit();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        #endregion
    }

    public interface IUnitBookingService
    {

        IEnumerable<UnitBooking> GetUnitBookings();
        IEnumerable<UnitBooking> GetUnitBookings(DateTime FromDate, DateTime ToDate, string status);
        IEnumerable<UnitBooking> GetUnitBookings(string status);

        UnitBooking GetUnitBooking(long id);
        bool CreateUnitBooking(ref UnitBooking unitbooking, ref string message);
        bool UpdateUnitBooking(ref UnitBooking unitbooking, ref string message);
        bool DeleteUnitBooking(long id, ref string message, bool softDelete = true);
        bool SaveUnitBooking();
        IEnumerable<UnitBooking> GetUnitBookingByCustomerId(long customerId);

        //   UnitBooking GetUnitBookingPlanByName(string name);
    }
}
