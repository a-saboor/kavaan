using MyProject.Data.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Data.Repositories
{
	class SPRepository : RepositoryBase<SP_GetAdminDashboardAppointmentChart_Result>, ISPRepository
	{
		public SPRepository(IDbFactory dbFactory)
		: base(dbFactory) { }

		public IEnumerable<SP_GetAdminDashboardAppointmentChart_Result> GetAppointmentsChart(DateTime startDate, DateTime endDate)
		{
			var stats = this.DbContext.SP_GetAdminDashboardAppointmentChart(startDate, endDate);
			return stats;
		}
		public IEnumerable<SP_GetAdminDashboardUnitBookingChart_Result> GetUnitBookingsChart(DateTime startDate, DateTime endDate)
		{
			var stats = this.DbContext.SP_GetAdminDashboardUnitBookingChart(startDate, endDate);
			return stats;
		}
		public IEnumerable<SP_GetAdminDashboardUnitEnquiryChart_Result> GetUnitEquiryChart(DateTime startDate, DateTime endDate)
		{
			var stats = this.DbContext.SP_GetAdminDashboardUnitEnquiryChart(startDate, endDate);
			return stats;
		}
		public IEnumerable<SP_GetAdminDashboardCustomerRequestChart_Result> GetCustomerRequestChart(DateTime startDate, DateTime endDate)
		{
			var stats = this.DbContext.SP_GetAdminDashboardCustomerRequestChart(startDate, endDate);
			return stats;
		}
		public SP_GetAdminDashboardStatsByDateRange_Result GetAdmingDashboardStats(DateTime startDate, DateTime endDate)
		{
			var stats = this.DbContext.SP_GetAdminDashboardStatsByDateRange(startDate, endDate).SingleOrDefault();
			return stats;
		}
		public SP_GetVendorDashboardStatsByDateRange_Result GetVendorDashboardStats(DateTime startDate, DateTime endDate,long VendorID = 0)
		{
			var stats = this.DbContext.SP_GetVendorDashboardStatsByDateRange(startDate, endDate, VendorID).SingleOrDefault();
			return stats;
		}
		public IEnumerable<SP_GetAdminDashboardYearlyRevenueGraph_Result> GetAdmingDashboardYearlyRevenueGraph()
		{
			var stats = this.DbContext.SP_GetAdminDashboardYearlyRevenueGraph(DateTime.Now.ToString("yyyy")).ToList();
			return stats;
		}
		public IEnumerable<SP_GetVendorDashboardYearlyRevenueGraph_Result> GetVendorDashboardYearlyRevenueGraph(long VendorID = 0)
		{
			var stats = this.DbContext.SP_GetVendorDashboardYearlyRevenueGraph(DateTime.Now.ToString("yyyy"), VendorID).ToList();
			return stats;
		}
		public IEnumerable<SP_GetAdminDashboardBookingGraphByDateRange_Result> GetAdmingDashboardBookingGraphByDateRange(DateTime startDate, DateTime endDate)
		{
			var stats = this.DbContext.SP_GetAdminDashboardBookingGraphByDateRange(startDate, endDate).ToList();
			return stats;
		}

		public IEnumerable<SP_GetVendorDashboardBookingGraphByDateRange_Result> GetVendorDashboardBookingGraphByDateRange(DateTime startDate, DateTime endDate, long VendorID = 0)
		{
			var stats = this.DbContext.SP_GetVendorDashboardBookingGraphByDateRange(startDate, endDate, VendorID).ToList();
			return stats;
		}

		public SP_GetVendorDashboardStatsByDateRange_Result GetVendorDashboardStatusByDateRange(DateTime startDate, DateTime endDate, long vendorId)
		{
			var stats = this.DbContext.SP_GetVendorDashboardStatsByDateRange(startDate, endDate, vendorId).SingleOrDefault();
			return stats;
		}
		public SP_GetAppDashboardStats_Result GetAppDashboardStats(long vendorId)
		{
			var stats = this.DbContext.SP_GetAppDashboardStats(vendorId).SingleOrDefault();
			return stats;
		}
		public SP_GetStaffDashboardStats_Result GetStaffDashboardStats(long staffID)
		{
			var stats = this.DbContext.SP_GetStaffDashboardStats(staffID).SingleOrDefault();
			return stats;
		}
	}
	public interface ISPRepository : IRepository<SP_GetAdminDashboardAppointmentChart_Result>
	{
		IEnumerable<SP_GetAdminDashboardAppointmentChart_Result> GetAppointmentsChart(DateTime startDate, DateTime endDate);
		IEnumerable<SP_GetAdminDashboardUnitBookingChart_Result> GetUnitBookingsChart(DateTime startDate, DateTime endDate);
		IEnumerable<SP_GetAdminDashboardUnitEnquiryChart_Result> GetUnitEquiryChart(DateTime startDate, DateTime endDate);
		IEnumerable<SP_GetAdminDashboardCustomerRequestChart_Result> GetCustomerRequestChart(DateTime startDate, DateTime endDate);
		SP_GetAdminDashboardStatsByDateRange_Result GetAdmingDashboardStats(DateTime startDate, DateTime endDate);
		SP_GetVendorDashboardStatsByDateRange_Result GetVendorDashboardStats(DateTime startDate, DateTime endDate, long VendorID = 0);
		IEnumerable<SP_GetAdminDashboardYearlyRevenueGraph_Result> GetAdmingDashboardYearlyRevenueGraph();
		IEnumerable<SP_GetVendorDashboardYearlyRevenueGraph_Result> GetVendorDashboardYearlyRevenueGraph(long VendorID = 0);
		IEnumerable<SP_GetAdminDashboardBookingGraphByDateRange_Result> GetAdmingDashboardBookingGraphByDateRange(DateTime startDate, DateTime endDate);
		IEnumerable<SP_GetVendorDashboardBookingGraphByDateRange_Result> GetVendorDashboardBookingGraphByDateRange(DateTime startDate, DateTime endDate, long VendorID = 0);
		SP_GetVendorDashboardStatsByDateRange_Result GetVendorDashboardStatusByDateRange(DateTime startDate, DateTime endDate, long vendorId);
		SP_GetAppDashboardStats_Result GetAppDashboardStats(long vendorId);
		SP_GetStaffDashboardStats_Result GetStaffDashboardStats(long staffID);
	}
}
