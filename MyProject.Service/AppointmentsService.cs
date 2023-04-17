using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
    public class AppointmentsService : IAppointmentsService
    {
        private readonly IAppointmentRepository appointmentRepository;
        private readonly INumberRangeService _numberRangeService;
        private readonly IUnitOfWork _unitOfWork;

        public AppointmentsService(IAppointmentRepository appointmentRepository, INumberRangeService numberRangeService, IUnitOfWork unitOfWork)
        {
            this.appointmentRepository = appointmentRepository;
            this._numberRangeService = numberRangeService;
            this._unitOfWork = unitOfWork;
        }

        #region IAppointmentService Members

        public IEnumerable<Appointment> GetAppointmentUser()
        {
            var appointments = this.appointmentRepository.GetAppointmentUsers();
            return appointments;
        }

        public IEnumerable<Appointment> GetAppointments(DateTime FromDate, DateTime ToDate, int id = 0)
        {
            var data = appointmentRepository.GetFilteredAppointments(FromDate, ToDate, id);
            return data;
        }

        public Appointment GetAppointment(long id)
        {
            var appointment = this.appointmentRepository.GetById(id);
            return appointment;
        }

        public IEnumerable<Appointment> GetAppointmentsByCustomer(long customerID)
        {
            var appointment = this.appointmentRepository.GetAppointmentsByCustomer(customerID);
            return appointment;
        }

        public bool Create(Appointment appointment, ref string message)
        {
            try
            {
                appointment.CreatedOn = Helpers.TimeZone.GetLocalDateTime();
                appointment.IsDeleted = false;
                appointment.IsCancelled = false;
                appointment.IsApproved = false;
                appointment.IsCompleted = false;
                appointment.AppointmentNo = _numberRangeService.GetNextValueFromNumberRangeByName("APPOINTMENT");

                this.appointmentRepository.Add(appointment);
                    if (SaveAppointment())
                    {
                        message = "Appointment created successfully";
                        return true;
                    }
                    else
                    {
                        message = "Oops! Something Went Wrong";
                        return false;
                    }
            }
            catch (Exception ex)
            {

                message = "Oops! Something Went Wrong";
                return false;

            }
        }

        public bool UpdateAppointment(ref Appointment appointment, ref string message)
        {
            try
            {
                //if (this.appointmentRepository.GetUnitPaymentPlaneByName(unitPaymentPlan.Milestones, unitPaymentPlan.ID) == null)
                //{
                Appointment appt = this.appointmentRepository.GetById(appointment.ID);

                appt.Remarks = appointment.Remarks;
                appt.IsCancelled = appointment.IsCancelled;
                appt.IsCompleted = appointment.IsCompleted;
                appt.IsApproved = appointment.IsApproved;

                this.appointmentRepository.Update(appt);
                if (SaveAppointment())
                {
                    appointment = null;
                    appointment = appt;
                    message = "Appointment updated successfully ...";
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

        public bool DeleteAppointment(long id, ref string message, bool softDelete = true)
        {
            try
            {
                Appointment appointment = this.appointmentRepository.GetById(id);

                if (softDelete)
                {
                    appointment.IsDeleted = true;
                    this.appointmentRepository.Update(appointment);
                }
                else
                {
                    this.appointmentRepository.Delete(appointment);
                }
                if (SaveAppointment())
                {
                    message = "Appointment deleted successfully ...";
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

        public IEnumerable<SP_GetFilteredAppointments_Result> GetFilteredAppointments(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, Nullable<long> customerID)
        {
            var appointments = this.appointmentRepository.GetFilteredAppointments(search, pageSize, pageNumber, sortBy, lang, customerID);
            return appointments;
        }

        public bool SaveAppointment()
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

    public interface IAppointmentsService
    {

        IEnumerable<Appointment> GetAppointmentUser();
        Appointment GetAppointment(long id);
        IEnumerable<Appointment> GetAppointmentsByCustomer(long customerID);
        bool Create(Appointment appointment, ref string message);
        //   bool CreateAppointment(ref Appointment appointment, ref string message);
        bool UpdateAppointment(ref Appointment appointment, ref string message);
         bool DeleteAppointment(long id, ref string message, bool softDelete = true);

        IEnumerable<SP_GetFilteredAppointments_Result> GetFilteredAppointments(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, Nullable<long> customerID);

        bool SaveAppointment();
        // Appointment GetAppointmentByName(string name);

        IEnumerable<Appointment> GetAppointments(DateTime FromDate, DateTime ToDate, int id = 0);
    }
}
