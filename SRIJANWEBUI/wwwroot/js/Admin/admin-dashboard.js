var cardsData, daChart, scChart;
var zone = [], zscCount = [];
$(document).ready(function () {
    $.get("/Admin/GetDashboardData", function (response) {
        //console.log(response);
        try {
            var resp = JSON.parse(response);

            // Parsing Each list to each Variable required
            resp.forEach(function (item, index) {
                switch (index) {
                    case 0:
                        cardsData = JSON.parse(item);
                        break;
                    case 1:
                        daChart = JSON.parse(item);
                        break;
                    case 2:
                        scChart = JSON.parse(item);
                        break;

                    default:
                    // fallback if needed
                }

            });


            // For Vendor Select input
            cardsData.forEach(function (data) {
                document.getElementById('count-school-t1').textContent = data.SCHOOL_COUNT;
                document.getElementById('count-user-t1').textContent = data.USER_COUNT;
                document.getElementById('count-city-t1').textContent = data.CITY_COUNT;
                document.getElementById('count-zone-t1').textContent = data.DEPARTMENT_COUNT;
                document.getElementById('count-punching-t1').textContent = data.METRIC_CARD_DATA;
            });
            // For CAtegory select input
            daChart.forEach(function (item) {
                //$('.dt-role').append(`<option value="${item.USER_ROLE_ID}">${item.ROLENAME}</option>`);
            });
            // For STate select input
            scChart.forEach(function (item) {
                zone.push(item.Zone);
                zscCount.push(item.SCHOOLCOUNT);
                //$('.dt-manager').append(`<option value="${item.EMPID}">${item.ENAME}</option>`);
            });
        }
        catch (ex) {
            cardsData = [];
            daChart = [];
            scChart = [];
        }
        if (zscCount.length == 0) {
            var element = $("#chart-pie-t1");
            noDataPlace(element);
        } else {
            schoolChart(zscCount, zone);
        }

        if (daChart.length == 0) {
            var element = $("#chart-pie-t2");
            noDataPlace(element);
        } else {
            daStatusChart([daChart[0].APPROVED, daChart[0].REJECTED ], ['Approved', 'Rejected'])
        }
    });
})

function schoolChart(list1, list2) {
    var options = {
        series: list1,
        chart: {
            width: '100%',
            
            type: 'pie',
        },
        title: {
            text: 'Zonewise Schools',
            align: 'center',
            //margin: 10,
            //offsetY: 10,
            style: {
                fontSize: '20px',
                fontWeight: '500',
                fontFamily: 'Segoe UI, Roboto, sans-serif',
                color: '#374151'
            }
        },
        labels: list2,
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 200
                },
                legend: {
                    position: 'bottom'
                }
            }
        }]
    };

    var chart = new ApexCharts(document.querySelector("#chart-pie-t1"), options);
    chart.render();
}

function noDataPlace(el) {
    el.classList.add('placeholder-container');
    const placeholderHTML = `
         <div class="apex-placeholder">
             <i class="ti ti-chart-histogram placeholder-icon"></i>
             <div class="text">No data available</div>
         </div>`;
    el.html(placeholderHTML);

}

function daStatusChart(list1, list2) {
    var options = {
        series: list1,
        chart: {
            width: '100%',
            height: 350,
            type: 'donut',
        },
        title: {
            text: 'This Week - DA Status',
            align: 'center',
            //margin: 10,
            //offsetY: 10,
            style: {
                fontSize: '20px',
                fontWeight: '500',
                fontFamily: 'Segoe UI, Roboto, sans-serif',
                color: '#374151'
            }
        },
        labels: list2,
        responsive: [{
            breakpoint: 480,
            options: {
                chart: {
                    width: 200
                },
                legend: {
                    position: 'bottom'
                }
            }
        }]
    };

    var chart = new ApexCharts(document.querySelector("#chart-pie-t2"), options);
    chart.render();
}