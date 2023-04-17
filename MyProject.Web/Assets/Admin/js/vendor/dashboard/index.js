"use strict";
const colors = { primary: "#6993FF", brown: "#bc9359", blue: "#58b1d0", pink: "#c52683", grey: "gray", light: "gray-500", white: "#ffffff" };
// Shared Colors Definition
const primary = '#6993FF';
const success = '#1BC5BD';
const info = '#8950FC';
const warning = '#FFA800';
const danger = '#F64E60';





$(document).ready(function () {
	//salesChart();
	//bookingsChart();
	//customerSatisfactionChart();

	$("#btnSearch").trigger('click');
	salesChartData();
});

function salesChartData() {

	$.ajax({
		type: 'Get',
		url: '/Vendor/Dashboard/RevenueGraph',
		success: function (response) {


			salesChart(response.data);
		}
	});
}

//#region Charts js
function salesChart(data) {

	let MonthArray = [];
	let TotalAmountArray = [];
	let TotalAmountArraySum = 0;
	let TotalAmountArrayMax = 0;
	let NetAmountArray = [];
	let NetAmountArraySum = 0;

	data.forEach(function (value) {
		MonthArray.push(value.Month);
		TotalAmountArray.push(value.TotalAmount);
		NetAmountArray.push(value.NetAmount);
	});

	TotalAmountArraySum = TotalAmountArray.reduce(function (a, b) {
		return a + b;
	}, 0);
	NetAmountArraySum = NetAmountArray.reduce(function (a, b) {
		return a + b;
	}, 0);
	TotalAmountArrayMax = Math.max.apply(Math, TotalAmountArray);

	$('#total_amount_sum').text(TotalAmountArraySum);
	$('#net_amount_sum').text(NetAmountArraySum);

	var element = document.getElementById("sales_chart");
	var height = parseInt(KTUtil.css(element, 'height'));

	if (!element) {
		return;
	}

	var options = {
		series: [{
			name: 'Net Amout',
			data: NetAmountArray
		}, {
			name: 'Total Amount',
			data: TotalAmountArray
		}],
		chart: {
			type: 'bar',
			height: height,
			toolbar: {
				show: false
			},
			sparkline: {
				enabled: true
			},
		},
		plotOptions: {
			bar: {
				horizontal: false,
				columnWidth: ['30%'],
				endingShape: 'rounded'
			},
		},
		legend: {
			show: false
		},
		dataLabels: {
			enabled: false
		},
		stroke: {
			show: true,
			width: 1,
			colors: ['transparent']
		},
		xaxis: {
			categories: MonthArray,
			axisBorder: {
				show: false,
			},
			axisTicks: {
				show: false
			},
			labels: {
				style: {
					colors: KTApp.getSettings()['colors']['gray']['gray-500'],
					fontSize: '12px',
					fontFamily: KTApp.getSettings()['font-family']
				}
			}
		},
		yaxis: {
			min: 0,
			max: TotalAmountArrayMax,
			labels: {
				style: {
					colors: KTApp.getSettings()['colors']['gray']['gray-500'],
					fontSize: '12px',
					fontFamily: KTApp.getSettings()['font-family']
				}
			}
		},
		fill: {
			type: ['solid', 'solid'],
			opacity: [0.8, 0.8]
		},
		states: {
			normal: {
				filter: {
					type: 'none',
					value: 0
				}
			},
			hover: {
				filter: {
					type: 'none',
					value: 0
				}
			},
			active: {
				allowMultipleDataPointsSelection: false,
				filter: {
					type: 'none',
					value: 0
				}
			}
		},
		tooltip: {
			style: {
				fontSize: '12px',
				fontFamily: KTApp.getSettings()['font-family']
			},
			y: {
				formatter: function (val, { series, seriesIndex, dataPointIndex, w }) {
					if (seriesIndex == 1) {
						/*Revenue*/
						return "AED " + val;
					}
					else {
						/*Requests*/
					}
					return val;
					//return "$" + val + " thousands"
				}
			},
			marker: {
				show: false
			}
		},
		colors: [colors.brown, colors.blue],
		grid: {
			borderColor: KTApp.getSettings()['colors']['gray']['gray-200'],
			strokeDashArray: 4,
			yaxis: {
				lines: {
					show: true
				}
			},
			padding: {
				left: 20,
				right: 20
			}
		}
	};

	var chart = new ApexCharts(element, options);
	chart.render();
}
function bookingsChart(data) {

	let DateArray = [];
	let PendingArray = [];
	let CompletedArray = [];
	let CanceledArray = [];
	let MaxOfPCC = 0;

	data.forEach(function (value) {
		DateArray.push(value.PendingBookingsDate);
		PendingArray.push(value.PendingBookingsCount);
		CompletedArray.push(value.CompletedBookingsCount);
		CanceledArray.push(value.CanceledBookingsCount);
	});
	MaxOfPCC = Math.max.apply(Math, PendingArray, CompletedArray, CanceledArray);

	var element = document.getElementById("bookings_chart");
	var height = parseInt(KTUtil.css(element, 'height'));

	if (!element) {
		return;
	}

	var options = {
		series:
			[
				{
					name: 'Completed',
					data: CompletedArray
				},
				{
					name: 'Pending',
					data: PendingArray
				},
				{
					name: 'Canceled',
					data: CanceledArray
				},
			],
		chart:
		{
			type: 'bar',
			height: height,
			toolbar:
			{
				show: false
			},
			sparkline: {
				enabled: true
			},
		},
		plotOptions: {
			bar: {
				horizontal: false,
				columnWidth: ['40%'],
				endingShape: 'rounded'
			},
		},
		legend: {
			show: false
		},
		dataLabels: {
			enabled: false
		},
		stroke: {
			show: true,
			width: 1,
			colors: ['transparent']
		},
		xaxis:
		{
			categories: DateArray,
			axisBorder: {
				show: false,
			},
			axisTicks: {
				show: false
			},
			labels: {
				style: {
					colors: KTApp.getSettings()['colors']['gray']['gray-500'],
					fontSize: '12px',
					fontFamily: KTApp.getSettings()['font-family']
				}
			}
		},
		yaxis: {
			min: 0,
			max: MaxOfPCC,
			labels: {
				style: {
					colors: KTApp.getSettings()['colors']['gray']['gray-500'],
					fontSize: '12px',
					fontFamily: KTApp.getSettings()['font-family']
				}
			}
		},
		fill: {
			type: ['solid', 'solid', 'solid'],
			opacity: [0.7, 0.9, 0.7]
		},
		states: {
			normal:
			{
				filter:
				{
					type: 'none',
					value: 0
				}
			},
			hover:
			{
				filter:
				{
					type: 'none',
					value: 0
				}
			},
			active:
			{
				allowMultipleDataPointsSelection: false,
				filter:
				{
					type: 'none',
					value: 0
				}
			}
		},
		tooltip:
		{
			style:
			{
				fontSize: '12px',
				fontFamily: KTApp.getSettings()['font-family']
			},
			y:
			{
				formatter: function (val, { series, seriesIndex, dataPointIndex, w }) {
					return val;
				}
			},
			marker:
			{
				show: false
			}
		},
		colors: [colors.blue, colors.brown, colors.pink],
		grid:
		{
			borderColor: KTApp.getSettings()['colors']['gray']['gray-200'],
			strokeDashArray: 4,
			yaxis:
			{
				lines:
				{
					show: true
				}
			},
			padding:
			{
				left: 20,
				right: 20
			}
		}
	};

	var chart = new ApexCharts(element, options);
	chart.render();
}
function customerSatisfactionChart(data) {
	const apexChart = "#customer_satisfy";
	/* get pending/comleted/canceld bookings value by ID */


	let completedVal = data.CompletedBookings;
	let pendingVal = data.PendingBookings;
	let canceledVal = data.CanceledBookings;
	/* end*/

	var options = {
		series: [pendingVal, completedVal, canceledVal],
		labels: ['Pending Bookings', 'Completed Bookings', 'Canceled Bookings'],
		chart: {
			width: 390,
			type: 'donut',
		},
		responsive: [{
			breakpoint: 480,
			options: {
				chart: {
					width: 300,
				},
				legend: {
					position: 'bottom'
				}
			}
		}],
		colors: [colors.brown, colors.blue, colors.pink],
		dataLabels: {
			enabled: true,
			formatter: function (val) {
				return val.toFixed(2) + " %"
			},
			dropShadow: {
			},
		},
		plotOptions: {
			pie: {
				donut: {
					labels: {
						show: true,
						name: {
							fontSize: '10px',
							fontWeight: 10,
						},
						value: {
							fontSize: '10px',
						}
					}
				}
			}
		}
	};

	var chart = new ApexCharts(document.querySelector(apexChart), options);
	chart.render();
}
//#endregion

//#region callback functions
function DateFilterCallBack(data) {
	if (data) {
		//			$("#TopList").html(data);
		//			//KTDatatablesBasicScrollableCustomer.init();
		//			//KTDatatablesBasicScrollableCategory.init();
		//			//KTDatatablesBasicScrollableCar.init();
		//			//KTDatatablesBasicScrollableer.init();

		//			$('.FromDate').val(fromDate);
		//			$('.ToDate').val(toDate);

		//			KTDatatablesBasicScrollableer.init();

		//			$("#btnSearch").removeClass('spinner spinner-left spinner-sm').attr('disabled', false);
		//			$("#btnSearch").find('i').show();

		//			var hdnOrderCounter = Number($('#hdnOrderCounter').val());
		//			$('#OrderCounter').text(hdnOrderCounter > 1 ? hdnOrderCounter + " Bookings" : hdnOrderCounter + " Booking");


		/* charts reinitialization */

		//$('#sales_chart').empty(); salesChart();//send data after search button clicked
		$('#bookings_chart').empty(); bookingsChart(data.booking);//send data after search button clicked
		
		$('#customer_satisfy').empty(); customerSatisfactionChart(data.stats);//send data after search button clicked

		/* cards widgets data insertion */

		$('.t-users').html(data.stats.TotalUsers);
		$('.t-products').html(data.stats.TotalProducts);
		$('.t-productsapr').html(data.stats.TotalProductApprovals);


		$('.t-customers').html(data.stats.TotalCustomers);
		$('.t-vendors').html(data.stats.TotalVisitors);
		$('.t-vendorapr').html(data.stats.TotalVendorApprovals);
		$('.t-category').html(data.stats.TotalServiceCategories);
		$('.t-service').html(data.stats.TotalServices);
		$('.t-products').html(data.stats.TotalProducts);
		$('.t-productapr').html(data.stats.TotalProductApprovals);

		//BOOKINGS STATS
		$('.t-pb').html(data.stats.PendingBookings);
		$('.t-cb').html(data.stats.CompletedBookings);
		$('.t-canceledbookings').html(data.stats.CanceledBookings);
		$('.t-bookings').html(data.stats.TotalBookings);
	}
	else {
		//salesChart(0);	
		bookingsChart(0);
		customerSatisfactionChart(0);

		$('.t-users').html(0);
		$('.t-customers').html(0);
	}
}
//#endregion


//function float2AED(value) {
//	return "AED " + (value).toFixed(2).replace(/\d(?=(\d{3})+\.)/g, '$&,');
//}
//function nsChart(data) {
//	var dataa = [];
//	var dates = [];
//	var bgColor = [];
//	var borderColor = [];
//	data.forEach(function (value) {
//		dataa.push(value.NetSale);
//		dates.push(value.Date);
//		bgColor.push('rgba(233, 126, 1, 1)');
//		bgColor.push('rgba(0, 0, 0, 1)');
//		borderColor.push('rgba(233, 126, 1, 1)');
//		borderColor.push('rgba(0, 0, 0, 1)');
//	});
//	$('#nsChart').remove();
//	$('.nscanvas').append('<canvas id="nsChart" width="400" height="400"></canvas>');
//	var ctx = document.getElementById('nsChart').getContext('2d');
//	var myChart = new Chart(ctx, {
//		type: 'bar',
//		data: {
//			labels: dates,
//			datasets: [{
//				label: 'Net Sale',
//				data: dataa,
//				backgroundColor: bgColor,
//				borderColor: borderColor,
//				borderWidth: 1
//			}]
//		},
//		options: {
//			scales: {
//				xAxes: [{
//					gridLines: {
//						color: "rgba(0, 0, 0, 0)",
//					}
//				}],
//				yAxes: [{
//					ticks: {
//						beginAtZero: true,
//						callback: function (value, index, values) {
//							return float2AED(value);
//						}
//					}
//				}]
//			}
//		}
//	});
//}
//function UnitBookingChart(data) {
//	var dataa = [];
//	var dates = [];
//	var bgColor = [];
//	var borderColor = [];
//	data.forEach(function (value) {
//		dataa.push(value.Bookings);
//		dates.push(value.Date);
//		bgColor.push('rgba(83, 169, 198, 1)');
//		/*bgColor.push('rgba(199, 57, 146, 1)');*/
//		borderColor.push('rgba(83, 169, 198, 1)');
//		/*borderColor.push('rgba(199, 57, 146, 1)');*/
//	});
//	$('#ubChart').remove();
//	$('.ubcanvas').append('<canvas id="ubChart" width="400" height="400"></canvas>');
//	var ctx = document.getElementById('ubChart').getContext('2d');
//	var myChart = new Chart(ctx, {
//		type: 'bar',
//		data: {
//			labels: dates,
//			datasets: [{
//				label: 'Bookings',
//				data: dataa,
//				backgroundColor: bgColor,
//				borderColor: borderColor,
//				borderWidth: 1
//			}]
//		},
//		options: {
//			scales: {
//				xAxes: [{
//					gridLines: {
//						color: "rgba(0, 0, 0, 0)",
//					}
//				}],
//				yAxes: [{
//					ticks: {
//						beginAtZero: true
//					}
//				}]
//			}
//		}
//	});
//}

//function CustomerRequestChart(data) {
//	var dataa = [];
//	var dates = [];
//	var bgColor = [];
//	var borderColor = [];
//	data.forEach(function (value) {
//		dataa.push(value.Request);
//		dates.push(value.Date);
//		bgColor.push('rgba(83, 169, 198, 1)');
//		/*bgColor.push('rgba(199, 57, 146, 1)');*/
//		borderColor.push('rgba(83, 169, 198, 1)');
//		/*borderColor.push('rgba(199, 57, 146, 1)');*/
//	});
//	$('#crChart').remove();
//	$('.crcanvas').append('<canvas id="crChart" width="400" height="400"></canvas>');
//	var ctx = document.getElementById('crChart').getContext('2d');
//	var myChart = new Chart(ctx, {
//		type: 'bar',
//		data: {
//			labels: dates,
//			datasets: [{
//				label: 'Customer Request',
//				data: dataa,
//				backgroundColor: bgColor,
//				borderColor: borderColor,
//				borderWidth: 1
//			}]
//		},
//		options: {
//			scales: {
//				xAxes: [{
//					gridLines: {
//						color: "rgba(0, 0, 0, 0)",
//					}
//				}],
//				yAxes: [{
//					ticks: {
//						beginAtZero: true
//					}
//				}]
//			}
//		}
//	});
//}
//function AppointmentChart(data) {
//	var dataa = [];
//	var dates = [];
//	var bgColor = [];
//	var borderColor = [];
//	data.forEach(function (value) {
//		dataa.push(value.Appointment);
//		dates.push(value.Date);
//		bgColor.push('rgba(83, 169, 198, 1)');
//		/*bgColor.push('rgba(199, 57, 146, 1)');*/
//		borderColor.push('rgba(83, 169, 198, 1)');
//		/*borderColor.push('rgba(199, 57, 146, 1)');*/
//	});
//	$('#aChart').remove();
//	$('.acanvas').append('<canvas id="aChart" width="400" height="400"></canvas>');
//	var ctx = document.getElementById('aChart').getContext('2d');
//	var myChart = new Chart(ctx, {
//		type: 'bar',
//		data: {
//			labels: dates,
//			datasets: [{
//				label: 'Appontments',
//				data: dataa,
//				backgroundColor: bgColor,
//				borderColor: borderColor,
//				borderWidth: 1
//			}]
//		},
//		options: {
//			scales: {
//				xAxes: [{
//					gridLines: {
//						color: "rgba(0, 0, 0, 0)",
//					}
//				}],
//				yAxes: [{
//					ticks: {
//						beginAtZero: true
//					}
//				}]
//			}
//		}
//	});
//}
//function UnitEnquiryChart(data) {
//	var dataa = [];
//	var dates = [];
//	var bgColor = [];
//	var borderColor = [];
//	data.forEach(function (value) {
//		dataa.push(value.Appointment);
//		dates.push(value.Date);
//		bgColor.push('rgba(83, 169, 198, 1)');
//		/*bgColor.push('rgba(199, 57, 146, 1)');*/
//		borderColor.push('rgba(83, 169, 198, 1)');
//		/*borderColor.push('rgba(199, 57, 146, 1)');*/
//	});
//	$('#ueChart').remove();
//	$('.uecanvas').append('<canvas id="ueChart" width="400" height="400"></canvas>');
//	var ctx = document.getElementById('ueChart').getContext('2d');
//	var myChart = new Chart(ctx, {
//		type: 'bar',
//		data: {
//			labels: dates,
//			datasets: [{
//				label: 'Unit Enquiries',
//				data: dataa,
//				backgroundColor: bgColor,
//				borderColor: borderColor,
//				borderWidth: 1
//			}]
//		},
//		options: {
//			scales: {
//				xAxes: [{
//					gridLines: {
//						color: "rgba(0, 0, 0, 0)",
//					}
//				}],
//				yAxes: [{
//					ticks: {
//						beginAtZero: true
//					}
//				}]
//			}
//		}
//	});
//}
//function aovChart(data) {
//	var dataa = [];
//	var dates = [];
//	var bgColor = [];
//	var borderColor = [];
//	data.forEach(function (value) {
//		dataa.push(value.AverageOrderValue);
//		dates.push(value.Date);
//		bgColor.push('rgba(233, 126, 1, 1)');
//		bgColor.push('rgba(0, 0, 0, 1)');
//		borderColor.push('rgba(233, 126, 1, 1)');
//		borderColor.push('rgba(0, 0, 0, 1)');
//	});
//	$('#aovChart').remove();
//	$('.aovcanvas').append('<canvas id="aovChart" width="400" height="400"></canvas>');
//	var ctx = document.getElementById('aovChart').getContext('2d');
//	var myChart = new Chart(ctx, {
//		type: 'bar',
//		data: {
//			labels: dates,
//			datasets: [{
//				label: 'Average Order Values',
//				data: dataa,
//				backgroundColor: bgColor,
//				borderColor: borderColor,
//				borderWidth: 1
//			}]
//		},
//		options: {
//			scales: {
//				xAxes: [{
//					gridLines: {
//						color: "rgba(0, 0, 0, 0)",
//					}
//				}],
//				yAxes: [{
//					ticks: {
//						beginAtZero: true,
//						callback: function (value, index, values) {
//							return float2AED(value);
//						}
//					}
//				}]
//			}
//		}
//	});
//}
//function isChart(data) {
//	var dataa = [];
//	var dates = [];
//	var bgColor = [];
//	var borderColor = [];
//	data.forEach(function (value) {
//		dataa.push(value.ItemsSold);
//		dates.push(value.Date);
//		bgColor.push('rgba(233, 126, 1, 1)');
//		bgColor.push('rgba(0, 0, 0, 1)');
//		borderColor.push('rgba(233, 126, 1, 1)');
//		borderColor.push('rgba(0, 0, 0, 1)');
//	});
//	$('#isChart').remove();
//	$('.iscanvas').append('<canvas id="isChart" width="400" height="400"></canvas>');
//	var ctx = document.getElementById('isChart').getContext('2d');
//	var myChart = new Chart(ctx, {
//		type: 'bar',
//		data: {
//			labels: dates,
//			datasets: [{
//				label: 'Items Sold',
//				data: dataa,
//				backgroundColor: bgColor,
//				borderColor: borderColor,
//				borderWidth: 1
//			}]
//		},
//		options: {
//			scales: {
//				xAxes: [{
//					gridLines: {
//						color: "rgba(0, 0, 0, 0)",
//					}
//				}],
//				yAxes: [{
//					ticks: {
//						beginAtZero: true
//					}
//				}]
//			}
//		}
//	});
//}
//function retChart(data) {
//	var dataa = [];
//	var dates = [];
//	var bgColor = [];
//	var borderColor = [];
//	data.forEach(function (value) {
//		dataa.push(value.Returnss);
//		dates.push(value.Date);
//		bgColor.push('rgba(233, 126, 1, 1)');
//		bgColor.push('rgba(0, 0, 0, 1)');
//		borderColor.push('rgba(233, 126, 1, 1)');
//		borderColor.push('rgba(0, 0, 0, 1)');
//	});
//	$('#retChart').remove();
//	$('.retcanvas').append('<canvas id="retChart" width="400" height="400"></canvas>');
//	var ctx = document.getElementById('retChart').getContext('2d');
//	var myChart = new Chart(ctx, {
//		type: 'bar',
//		data: {
//			labels: dates,
//			datasets: [{
//				label: 'Returns',
//				data: dataa,
//				backgroundColor: bgColor,
//				borderColor: borderColor,
//				borderWidth: 1
//			}]
//		},
//		options: {
//			scales: {
//				xAxes: [{
//					gridLines: {
//						color: "rgba(0, 0, 0, 0)",
//					}
//				}],
//				yAxes: [{
//					ticks: {
//						beginAtZero: true,
//						callback: function (value, index, values) {
//							return float2AED(value);
//						}
//					}
//				}]
//			}
//		}
//	});
//}
