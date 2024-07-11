/* global Chart:false */

$(function () {
  'use strict'

  var ticksStyle = {
    fontColor: '#495057',
    fontStyle: 'bold'
  }

  var mode = 'index'
  var intersect = true

    document.addEventListener("DOMContentLoaded", function () {
        // Ensure the data is loaded from the Razor Page
        if (typeof labels !== 'undefined' && typeof totalFeesData !== 'undefined' && typeof platformFeesData !== 'undefined') {
            var ctx = document.getElementById('platform-fee').getContext('2d');
            var platformfee = new Chart(ctx, {
                type: 'bar',
                data: {
                    labels: labels,
                    datasets: [
                        {
                            label: 'Total Fees',
                            backgroundColor: '#007bff',
                            borderColor: '#007bff',
                            data: totalFeesData
                        },
                        {
                            label: 'Platform Fees',
                            backgroundColor: '#ced4da',
                            borderColor: '#ced4da',
                            data: platformFeesData
                        }
                    ]
                },
                options: {
                    maintainAspectRatio: false,
                    tooltips: {
                        mode: 'index',
                        intersect: false
                    },
                    hover: {
                        mode: 'nearest',
                        intersect: true
                    },
                    legend: {
                        display: false
                    },
                    scales: {
                        yAxes: [{
                            gridLines: {
                                display: true,
                                lineWidth: '4px',
                                color: 'rgba(0, 0, 0, .2)',
                                zeroLineColor: 'transparent'
                            },
                            ticks: {
                                beginAtZero: true,
                                callback: function (value) {
                                    if (value >= 1000) {
                                        value /= 1000;
                                        value += 'k';
                                    }
                                    return '$' + value;
                                }
                            }
                        }],
                        xAxes: [{
                            display: true,
                            gridLines: {
                                display: false
                            }]
                    }
                }
            }
        });
    }
})

document.addEventListener("DOMContentLoaded", function () {
    if (typeof labels !== 'undefined' && typeof thisWeekData !== 'undefined' && typeof lastWeekData !== 'undefined') {
        var $turnover = document.getElementById('turnover').getContext('2d');

        var turnover = new Chart($turnover, {
            type: 'line',
            data: {
                labels: labels,
                datasets: [
                    {
                        label: 'This Week', // Label for this week's data
                        data: thisWeekData,
                        backgroundColor: 'transparent',
                        borderColor: '#007bff', // Blue color for this week
                        pointBorderColor: '#007bff',
                        pointBackgroundColor: '#007bff',
                        fill: false
                    },
                    {
                        label: 'Last Week', // Label for last week's data
                        data: lastWeekData,
                        backgroundColor: 'transparent',
                        borderColor: '#ced4da', // Gray color for last week
                        pointBorderColor: '#ced4da',
                        pointBackgroundColor: '#ced4da',
                        fill: false
                    }
                ]
            },
            options: {
                maintainAspectRatio: false,
                tooltips: {
                    mode: 'index',
                    intersect: false
                },
                hover: {
                    mode: 'nearest',
                    intersect: true
                },
                legend: {
                    display: true
                },
                scales: {
                    yAxes: [{
                        gridLines: {
                            display: true,
                            lineWidth: '4px',
                            color: 'rgba(0, 0, 0, .2)',
                            zeroLineColor: 'transparent'
                        },
                        ticks: {
                            beginAtZero: true,
                            callback: function (value) {
                                if (value >= 1000) {
                                    value /= 1000;
                                    value += 'k';
                                }
                                return '$' + value;
                            }
                        }
                    }],
                    xAxes: [{
                        gridLines: {
                            display: false
                        }]
                }
            }
        }
        });
    }
});
// lgtm [js/unused-local-variable]


//@section Scripts {
//    <script>
//        var labels = @Html.Raw(Json.Serialize(Model.Labels));

//        var totalFeesData = @Html.Raw(Json.Serialize(Model.TotalFeesData));
//        var platformFeesData = @Html.Raw(Json.Serialize(Model.PlatformFeesData));

//        var thisWeekData = @Html.Raw(Json.Serialize(Model.ThisWeekData));
//        var lastWeekData = @Html.Raw(Json.Serialize(Model.LastWeekData));
//    </script>
//}