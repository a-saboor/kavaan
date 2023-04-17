using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace MyProject.Data.Repositories
{
    class AppointmentRepository : RepositoryBase<Appointment>, IAppointmentRepository
    {
        public AppointmentRepository(IDbFactory dbFactory)
            : base(dbFactory) { }

        public IEnumerable<Appointment> GetAppointmentUsers()
        {
           // var appointments = this.DbContext.Appointments.All(x=>x.IsDeleted==false);
            var appointments1 = this.DbContext.Appointments.Where(x=>x.IsDeleted==false).ToList();

            return appointments1;
        }

        public IEnumerable<Appointment> GetAppointmentsByCustomer(long customerId)
        {
            var appointment = this.DbContext.Appointments.Where(mod => mod.CustomerID == customerId && mod.IsDeleted == false).ToList();
            return appointment;
        }

        public IEnumerable<SP_GetFilteredAppointments_Result> GetFilteredAppointments(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, Nullable<long> customerID)
        {
            var appointmemnts = this.DbContext.SP_GetFilteredAppointments(search, pageSize, pageNumber, sortBy, lang, customerID).ToList();
            return appointmemnts;
        }

        public List<Appointment> GetFilteredAppointments(DateTime FromDate, DateTime ToDate, int id = 0)
        {
            List<Appointment> appointments = new List<Appointment>();
            appointments = this.DbContext.Appointments.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate && c.ID == id).ToList();
            if (id == 0)
            {
                appointments = this.DbContext.Appointments.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate).ToList();
            }
            else if (id == 1) /*isCancelled == "FALSE" && isApproved == "FALSE"*/
            {
                appointments = this.DbContext.Appointments.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate && c.IsCancelled == false && c.IsApproved == false).ToList();
            }
            else if (id == 2)
            {
                appointments = this.DbContext.Appointments.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate && c.IsCompleted == true).ToList();
            }
            else if (id == 3)
            {
                appointments = this.DbContext.Appointments.Where(c => c.CreatedOn >= FromDate && c.CreatedOn <= ToDate && c.IsCancelled == true).ToList();
            }
            return appointments;
        }
    }

    public interface IAppointmentRepository : IRepository<Appointment>
    {
        IEnumerable<Appointment> GetAppointmentUsers();

        IEnumerable<Appointment> GetAppointmentsByCustomer(long customerId);

        IEnumerable<SP_GetFilteredAppointments_Result> GetFilteredAppointments(string search, Nullable<int> pageSize, Nullable<int> pageNumber, Nullable<int> sortBy, string lang, Nullable<long> customerID);

        List<Appointment> GetFilteredAppointments(DateTime FromDate, DateTime ToDate, int id = 0);

    }

}
