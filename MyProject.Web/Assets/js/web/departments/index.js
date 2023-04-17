$(document).ready(function () {
	BindCategories();
});

function BindCategories() {
	$.ajax({
		type: 'Get',
		url: '/departments',
		success: function (response) {
			if (response.success) {
				$('#department_section .main-div').empty().show();

				if (response.data.length) {
					$.each(response.data, function (k, v) {
						$('#department_section .main-div').append(`
							<a href="javascript:void(0);" class="cursor-pointer sm:w-[49%] md:w-[32%]" onclick="ViewJobs('${v.Slug}',${v.NoOfJobs})">
								<div class="bg-white p-4 shadow-xl mb-4 lg:p-6">
									<h1 class="font-Rubik text-lg font-medium uppercase">${v.Name}</h1>
									<p class="font-Rubik text-xs text-gk-purple">${v.NoOfJobs ? v.NoOfJobs : 0} ${v.NoOfJobs > 1 ? `Positions` : `Position`} ${ChangeString('Available','متوفرة')}</p>
								</div>
							</a>
						`);
					});
				}
				else {
					$('#department_section .no-more').show();
				}

			} else {
			}
		}
	});
}

function ViewJobs(slug, noOfJobs) {
	if (noOfJobs) {
		window.location = `/departments/${slug}/Jobs`;
	}
}