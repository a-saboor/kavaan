using MyProject.Data;
using System.Collections.Generic;

namespace MyProject.Web.Areas.Admin.ViewModels
{
	public class DashboardStatsViewModel
	{
		public int NoOfUsers { get; set; }
		public int NoOfJobs { get; set; }
		public int NoOfDepartment { get; set; }
		public int NoOfApplicant { get; set; }
		public int NoOfApproved { get; set; }
		public int NoOfRejected { get; set; }
		public IEnumerable<SP_GetAdminDashboardUnitBookingChart_Result> UnitBooking { get; set; }
		public IEnumerable<SP_GetAdminDashboardUnitEnquiryChart_Result> UnitEnquiry { get; set; }
		public IEnumerable<SP_GetAdminDashboardAppointmentChart_Result> Appointment { get; set; }
		public IEnumerable<SP_GetAdminDashboardCustomerRequestChart_Result> CustomerRequest { get; set; }
		public SP_GetAdminDashboardStatsByDateRange_Result Stats { get; set; }
		public IEnumerable<SP_GetAdminDashboardYearlyRevenueGraph_Result> RevenueGraph { get; set; }
		public IEnumerable<SP_GetAdminDashboardBookingGraphByDateRange_Result> BookingGraph { get; set; }

	}

	public class DashboardListViewModel
	{
		
	}
}