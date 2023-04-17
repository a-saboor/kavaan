$(document).ready(function () {
    BindJobs();
});

function BindJobs() {
    $.ajax({
        type: 'Get',
        url: `/departments/${departmentId}/JobOpenings?lang=en`,
        success: function (response) {
            if (response.success) {
                $('#jobs_section .main-div').empty().show();

                if (response.data.length) {
                    $.each(response.data, function (k, v) {
                        var template = `
                                    <div class="w-[80%] mx-auto bg-white shadow-2xl p-5 mt-4 sm:w-[45%] lg:w-[30%] lg:m-3 xl:flex xl:justify-center xl:items-baseline xl:flex-col">
                                         <h1 class ="font-Rubik font-medium text-sm ">${v.Title}</h1>
                                         <p class ="font-Rubik text-xs my-4 font-light cut-text">${v.Requirements}</p>
                                         <button class ="border-2 border-gk-blue flex items-center rounded-lg py-2 pl-2" onclick="ApplyNow(${v.ID})">
                                             <p class="font-Rubik font-medium text-xs mr-3">${ChangeString('Apply Now','قدم الآن')}</p>
                                             <svg viewBox="0 0 192 512" class="w-1 mx-2 flip-horizontal-rtl"><path fill="#F9B35E" d="M0 384.662V127.338c0-17.818 21.543-26.741 34.142-14.142l128.662 128.662c7.81 7.81 7.81 20.474 0 28.284L34.142 398.804C21.543 411.404 0 402.48 0 384.662z" class=""></path></svg>
                                         </button>
                                     </div>
                                    `;

                        $('#jobs_section .main-div').append(template);
                    });
                }
                else {
                    $('#jobs_section .no-more').show();
                }
                
            } else {
            }
        }
    });
}

function ApplyNow(id) {
    window.location = `/Jobs/${id}/Apply`;
}