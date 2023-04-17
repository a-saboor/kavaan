using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;

namespace MyProject.Service
{
    class ServiceBookingService : IServiceBookingService
    {

        private readonly IServiceBookingRepository _serviceBookingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceBookingService(IServiceBookingRepository _serviceBookingRepository, IUnitOfWork unitOfWork)
        {
            this._serviceBookingRepository = _serviceBookingRepository;
            this._unitOfWork = unitOfWork;

        }

        public IEnumerable<ServiceBooking> GetServiceBookings()
        {
            var booking = _serviceBookingRepository.GetAll();
            return booking;
        }

        public IEnumerable<ServiceBooking> GetPendingServiceBookings(long vendorId)
        {
            var bookings = _serviceBookingRepository.GetAllPendingBookings(vendorId);
            return bookings;
        }
        public IEnumerable<ServiceBooking> GetAllServiceUpcomingBookingsbyCustomerID(long customerid)
        {
            var bookings = _serviceBookingRepository.GetAllCustomerUpcomingBookings(customerid);
            return bookings;
        }
        public IEnumerable<ServiceBooking> GetAllServicePastBookingsbyCustomerID(long customerid)
        {
            var bookings = _serviceBookingRepository.GetAllCustomerPastBookings(customerid);
            return bookings;
        }
        public IEnumerable<ServiceBooking> GetAllServiceCompletedBookingsbyStaffID(long staffId)
        {
            var bookings = _serviceBookingRepository.GetAllStaffCompletedBookings(staffId);
            return bookings;
        }
        public IEnumerable<ServiceBooking> GetAllServiceOngoingBookingsbyStaffID(long staffId)
        {
            var bookings = _serviceBookingRepository.GetAllStaffOngoingBookings(staffId);
            return bookings;
        }
        public IEnumerable<ServiceBooking> GetPendingServiceBookingsDateWise(DateTime startDate, DateTime endDate, long vendorId)
        {
            var bookings = _serviceBookingRepository.GetAllPendingBookingsDateWise(startDate, endDate, vendorId);
            return bookings;
        }

        public IEnumerable<ServiceBooking> GetCompletedServiceBookings(long vendorId)
        {
            var bookings = _serviceBookingRepository.GetAllCompletedBookings(vendorId);
            return bookings;
        }

        public IEnumerable<ServiceBooking> GetCompletedServiceBookingsDateWise(DateTime startDate, DateTime endDate, long vendorId)
        {
            var bookings = _serviceBookingRepository.GetAllCompletedBookingsDateWise(startDate, endDate, vendorId);
            return bookings;
        }

        public IEnumerable<ServiceBooking> GetFilteredBookings(DateTime startDate, DateTime endDate, string orderStatus, long vendorId)
        {
            var bookings = _serviceBookingRepository.GetAllByStatus(startDate, endDate, orderStatus, vendorId);
            return bookings;
        }

        public ServiceBooking GetServiceBooking(long id)
        {
            var booking = _serviceBookingRepository.GetById(id);
            return booking;
        }

        public bool CreateServiceBooking(ref ServiceBooking serviceBooking, ref string message)
        {
            try
            {

                //ServiceBooking.IsActive = true;
                //ServiceBooking.IsDeleted = false;
                serviceBooking.CreatedOn = MyProject.Service.Helpers.TimeZone.GetLocalDateTime();
                _serviceBookingRepository.Add(serviceBooking);
                if (SaveServiceBooking())
                {
                    message = "Service Booking added successfully ...";
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

        public bool UpdateServiceBooking(ref ServiceBooking serviceBooking, ref string message)
        {
            try
            {

                ServiceBooking CurrentServiceBooking = _serviceBookingRepository.GetById(serviceBooking.ID);

                CurrentServiceBooking.Status = serviceBooking.Status;

                if (serviceBooking.VendorID != null)
                {
                    CurrentServiceBooking.VendorID = serviceBooking.VendorID;
                }

                serviceBooking = null;

                _serviceBookingRepository.Update(CurrentServiceBooking);
                if (SaveServiceBooking())
                {
                    serviceBooking = CurrentServiceBooking;
                    message = "Service Booking updated successfully ...";
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

        public List<SP_GetFilteredServiceBookings_Result> GetFilteredServiceBookings(string search, DateTime? startDate, DateTime? endDate, long? vendorId, string status, int? pageNumber = 1, int? pageSize = 20, int sortBy = 1)
        {
            var Orders = _serviceBookingRepository.GetFilteredServiceBookings(search, startDate, endDate, vendorId, status, pageNumber, pageSize, sortBy);
            return Orders;
        }

        public List<SP_GetFVendorServiceBookings_Result> GetFilteredVendorServiceBookings(string search, DateTime? startDate, DateTime? endDate, long? vendorId, string status, int? pageNumber = 1, int? pageSize = 20, int sortBy = 1)
        {
            var bookings = _serviceBookingRepository.GetFilteredVendorServiceBookings(search, startDate, endDate, vendorId, status, pageNumber, pageSize, sortBy);
            return bookings;
        }
        public bool DeleteServiceBooking(long id, ref string message, bool softDelete = true)
        {
            try
            {
                ServiceBooking serviceBooking = _serviceBookingRepository.GetById(id);

                if (softDelete)
                {
                    //ServiceBooking.IsDeleted = true;
                    _serviceBookingRepository.Update(serviceBooking);
                }
                else
                {
                    _serviceBookingRepository.Delete(serviceBooking);
                }
                if (SaveServiceBooking())
                {
                    message = "Service Booking deleted successfully ...";
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

        public bool UpdateStatus(ref ServiceBooking serviceBooking, ref string message)
        {
            try
            {
                ServiceBooking currentBooking = _serviceBookingRepository.GetById(serviceBooking.ID);
                if (!string.IsNullOrEmpty(serviceBooking.CancellationReason))
                {
                    currentBooking.CancellationReason = serviceBooking.CancellationReason;
                }
                currentBooking.Status = serviceBooking.Status;


                serviceBooking = null;

                _serviceBookingRepository.Update(currentBooking);
                if (SaveServiceBooking())
                {
                    serviceBooking = currentBooking;
                    message = "Service booking updated successfully ...";
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

        public bool SaveServiceBooking()
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

        public IEnumerable<SP_GetCustomerServiceBookings_Result> GetCustomerServiceBookings(long customerID, string status, int? pageSize, int? pageNumber, int? sortBy, string lang, string imageServer)
        {
            var bookings = _serviceBookingRepository.GetCustomerServiceBookings(customerID, status, pageSize, pageNumber, sortBy, lang, imageServer);
            return bookings;
        }
    }
    public interface IServiceBookingService
    {

        IEnumerable<ServiceBooking> GetServiceBookings();
        IEnumerable<ServiceBooking> GetPendingServiceBookings(long vendorId);
        IEnumerable<ServiceBooking> GetAllServiceUpcomingBookingsbyCustomerID(long customerid);
        IEnumerable<ServiceBooking> GetAllServicePastBookingsbyCustomerID(long customerid);
        List<SP_GetFilteredServiceBookings_Result> GetFilteredServiceBookings(string search, DateTime? startDate, DateTime? endDate, long? vendorId, string status, int? pageNumber = 1, int? pageSize = 20, int sortBy = 1);
        IEnumerable<ServiceBooking> GetPendingServiceBookingsDateWise(DateTime startDate, DateTime endDate, long vendorId);
        IEnumerable<ServiceBooking> GetCompletedServiceBookings(long vendorId);
        IEnumerable<ServiceBooking> GetCompletedServiceBookingsDateWise(DateTime startDate, DateTime endDate, long vendorId);
        IEnumerable<ServiceBooking> GetFilteredBookings(DateTime startDate, DateTime endDate, string orderStatus, long vendorId);
        ServiceBooking GetServiceBooking(long id);
        bool CreateServiceBooking(ref ServiceBooking serviceBooking, ref string message);
        bool UpdateServiceBooking(ref ServiceBooking serviceBooking, ref string message);
        bool UpdateStatus(ref ServiceBooking serviceBooking, ref string message);
        bool DeleteServiceBooking(long id, ref string message, bool softDelete = true);
        bool SaveServiceBooking();
        List<SP_GetFVendorServiceBookings_Result> GetFilteredVendorServiceBookings(string search, DateTime? startDate, DateTime? endDate, long? vendorId, string status, int? pageNumber = 1, int? pageSize = 20, int sortBy = 1);

        IEnumerable<SP_GetCustomerServiceBookings_Result> GetCustomerServiceBookings(long customerID, string status, int? pageSize, int? pageNumber, int? sortBy, string lang, string imageServer);
        IEnumerable<ServiceBooking> GetAllServiceCompletedBookingsbyStaffID(long staffId);
        IEnumerable<ServiceBooking> GetAllServiceOngoingBookingsbyStaffID(long staffId);
    }
}
