﻿var city;
var minPrice;
var maxPrice;

$(document).ready(function () {
    var page = 0;
    city = $("#cities option:selected").text().toLowerCase();
    minPrice = ($("#minPrice").val());
    maxPrice = ($("#maxPrice").val());
    if (page == 0) {
        $("#previousButton").hide();
    }

    $('.cityFilter').change(function () {
        city = $("#cities option:selected").text().toLowerCase();
        var path = "/Home/Filter?city=" + city + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice + "";
        $.ajax(path).done(function (response) {
            console.log(response);
            $("#showCars").html(response);
        })
    });


    $("#filterByBudgetButton").click(function () {
        //alert(minPrice);
        if ($("#minPrice").val() == "") {
            minPrice = "";
            maxPrice = "";
            var path = "/Home/Filter?city=" + city + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice + "";
            $("#showCars").load(path);
        }
        else {
            minPrice = parseInt($("#minPrice").val());
            maxPrice = parseInt($("#maxPrice").val());


            if ((minPrice > 0 && maxPrice > 0 && minPrice <= maxPrice)) {

                var path = "/Home/Filter?city=" + city + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice + "";
                $("#showCars").load(path);
            }
                //else if (minPrice == NaN) {
                //    alert("1 1 1");
                //}
            else {
                alert("Enter Correct value");
            }
        }
    });

});


function ChangePage(buttonPress) {
    $(document).ready(function () {
        var page;
        if (buttonPress == -1)
            page = $("#previousButton1").attr("value");
        else
            page = $("#nextButton1").attr("value");
        var path = "/Home/Filter?city=" + city + "&minPrice=" + minPrice + "&maxPrice=" + maxPrice + "&page=" + page + "";
        $("#showCars").load(path);
    });
}


function ProfilePage(stockID) {
    $(document).ready(function () {
        var url = "/Home/stock/" + stockID + "";
        $(location).attr('href', url);
    });
}
