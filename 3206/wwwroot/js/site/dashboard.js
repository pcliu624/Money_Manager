$(function () {
    //demo
    //var options = {
    //    chart: {
    //        type: 'bar'
    //    },
    //    series: [{
    //        name: 'sales',
    //        data: [30, 40, 45, 50, 49, 60, 70, 91, 125]
    //    }],
    //    xaxis: {
    //        categories: [1991, 1992, 1993, 1994, 1995, 1996, 1997, 1998, 1999]
    //    }
    //}    
    
    $('#partialChart').load("/Home/ChartPartial");
    var interval = setInterval(function () {
        if ($('#typedata').val() != undefined) {
            renderchart();
            clearInterval(interval);
        }
    }, 150); 

    $('input[name="daterange"]').daterangepicker({
        opens: 'right',
        "startDate": moment().startOf('month'),
        "endDate": moment().endOf('month'),
    }, function (start, end, label) {
    });
    $('input[name="daterange"]').daterangepicker().on('apply.daterangepicker', function (e, picker) {
        var startDate = picker.startDate.format('YYYY-MM-DD');
        var endDate = picker.endDate.format('YYYY-MM-DD');
        console.log(`startDate = ${startDate}, endDate = ${endDate}`);
        var url = "../Home/ChartPartial?range=" + startDate + "|" + endDate;
        $('#partialChart').load(url);
        var interval = setInterval(function () { // keep try until data loaded
            if ($('#typedata').val() != undefined) {
                renderchart();
                clearInterval(interval);
            }
        }, 150); 
    });
   
})
function renderchart() {
    //var typedata = {
    //    "chart": {
    //        "type": "bar"
    //    },
    //    "series": [
    //        {
    //            "name": "CAD",
    //            "data": [
    //                439.57,
    //                31.59,
    //                77.68
    //            ]
    //        }
    //    ],
    //    "xaxis": {
    //        "categories": [
    //            "Grocery",
    //            "House stuff",
    //            "Dining"
    //        ]
    //    },     
    //}  
        var typedata = JSON.parse($('#typedata').val());
        var typechart = new ApexCharts(document.querySelector("#typechart"), typedata);
        typechart.render();

        var storedata = JSON.parse($('#storedata').val());
        var storechart = new ApexCharts(document.querySelector("#storechart"), storedata);
        storechart.render();

        var piedata = JSON.parse($('#piedata').val());
        var piechart = new ApexCharts(document.querySelector("#piechart"), piedata);
        piechart.render();

        var visitdata = JSON.parse($('#visitdata').val());
        var visitchart = new ApexCharts(document.querySelector("#visitchart"), visitdata);
        visitchart.render();
   
}
