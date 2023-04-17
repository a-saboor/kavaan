using MyProject.Data;
using MyProject.Data.Infrastructure;
using MyProject.Data.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MyProject.Service
{
	class SPService : ISPService
	{
		private readonly ISPRepository _spRepository;
		private readonly IUnitOfWork _unitOfWork;

		public SPService(ISPRepository spRepository, IUnitOfWork unitOfWork)
		{
			this._spRepository = spRepository;
			this._unitOfWork = unitOfWork;
		}

		public IEnumerable<SP_GetAdminDashboardAppointmentChart_Result> GetAppointmentChart(DateTime startDate, DateTime endDate)
		{
			var status = _spRepository.GetAppointmentsChart(startDate, endDate);
			return status;
		}
		public IEnumerable<SP_GetAdminDashboardCustomerRequestChart_Result> GetCustomerRequestChart(DateTime startDate, DateTime endDate)
		{
			var status = _spRepository.GetCustomerRequestChart(startDate, endDate);
			return status;
		}
		public IEnumerable<SP_GetAdminDashboardUnitBookingChart_Result> GetUnitBookingChart(DateTime startDate, DateTime endDate)
		{
			var status = _spRepository.GetUnitBookingsChart(startDate, endDate);
			return status;
		}
		public IEnumerable<SP_GetAdminDashboardUnitEnquiryChart_Result> GetUnitEnquiryChart(DateTime startDate, DateTime endDate)
		{
			var status = _spRepository.GetUnitEquiryChart(startDate, endDate);
			return status;
		}
		public IEnumerable<SP_GetAdminDashboardBookingGraphByDateRange_Result> GetBookingGraphByDateRange(DateTime startDate, DateTime endDate)
		{
			var status = _spRepository.GetAdmingDashboardBookingGraphByDateRange(startDate, endDate).ToList();
			return status;
		}
		public IEnumerable<SP_GetVendorDashboardBookingGraphByDateRange_Result> GetVendorBookingGraphByDateRange(DateTime startDate, DateTime endDate,long VendorID = 0)
		{
			var status = _spRepository.GetVendorDashboardBookingGraphByDateRange(startDate, endDate, VendorID).ToList();
			return status;
		}
		public IEnumerable<SP_GetAdminDashboardYearlyRevenueGraph_Result> GetYearlyRevenueGraphChart()
		{
			var status = _spRepository.GetAdmingDashboardYearlyRevenueGraph().ToList();
			return status;
		}
		public IEnumerable<SP_GetVendorDashboardYearlyRevenueGraph_Result> GetVendorYearlyRevenueGraphChart(long VendorID = 0)
		{
			var status = _spRepository.GetVendorDashboardYearlyRevenueGraph(VendorID).ToList();
			return status;
		}
		public SP_GetAdminDashboardStatsByDateRange_Result GetAdmingDashboardStats(DateTime startDate, DateTime endDate)
		{
			var stats = _spRepository.GetAdmingDashboardStats(startDate, endDate);
			return stats;
		}
		public SP_GetVendorDashboardStatsByDateRange_Result GetVendorDashboardStats(DateTime startDate, DateTime endDate, long VendorID = 0)
		{
			var stats = _spRepository.GetVendorDashboardStats(startDate, endDate, VendorID);
			return stats;
		}
		public SP_GetVendorDashboardStatsByDateRange_Result GetVendorDashboardStatusByRange(DateTime startDate, DateTime endTime, long vendorId)
		{
			var status = _spRepository.GetVendorDashboardStatusByDateRange(startDate, endTime, vendorId);
			return status;
		}
		public SP_GetAppDashboardStats_Result GetAppDashboardStats( long vendorId)
		{
			var status = _spRepository.GetAppDashboardStats(vendorId);
			return status;
		}
		public SP_GetStaffDashboardStats_Result GetStaffDashboardStats(long staffID)
		{
			var status = _spRepository.GetStaffDashboardStats(staffID);
			return status;
		}
	}
	public interface ISPService
	{
		IEnumerable<SP_GetAdminDashboardAppointmentChart_Result> GetAppointmentChart(DateTime startDate, DateTime endDate);
		IEnumerable<SP_GetAdminDashboardCustomerRequestChart_Result> GetCustomerRequestChart(DateTime startDate, DateTime endDate);
		IEnumerable<SP_GetAdminDashboardUnitBookingChart_Result> GetUnitBookingChart(DateTime startDate, DateTime endDate);
		IEnumerable<SP_GetAdminDashboardUnitEnquiryChart_Result> GetUnitEnquiryChart(DateTime startDate, DateTime endDate);
		IEnumerable<SP_GetAdminDashboardBookingGraphByDateRange_Result> GetBookingGraphByDateRange(DateTime startDate, DateTime endDate);
		IEnumerable<SP_GetVendorDashboardBookingGraphByDateRange_Result> GetVendorBookingGraphByDateRange(DateTime startDate, DateTime endDate, long VendorID = 0);
		IEnumerable<SP_GetAdminDashboardYearlyRevenueGraph_Result> GetYearlyRevenueGraphChart();
		IEnumerable<SP_GetVendorDashboardYearlyRevenueGraph_Result> GetVendorYearlyRevenueGraphChart(long VendorID = 0);
		SP_GetAdminDashboardStatsByDateRange_Result GetAdmingDashboardStats(DateTime startDate, DateTime endDate);
		SP_GetVendorDashboardStatsByDateRange_Result GetVendorDashboardStats(DateTime startDate, DateTime endDate, long VendorID = 0);
		SP_GetVendorDashboardStatsByDateRange_Result GetVendorDashboardStatusByRange(DateTime startDate, DateTime endTime, long vendorId);
		SP_GetAppDashboardStats_Result GetAppDashboardStats(long vendorId);
		SP_GetStaffDashboardStats_Result GetStaffDashboardStats(long staffID);
	}
}
