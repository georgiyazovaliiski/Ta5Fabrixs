window.onload = function () { resizeImage(); firstChange(); }
$(window).resize(function () {
    resizeImage()
});

function resizeImage() {
    var bodyWidth = $("body").width();
    $(".carousel-image").width(bodyWidth);
}

$(".carousel-button").click(function () {
    let gotoId = $(this).attr("id").slice(-1)
    let currentId = $(".current").attr("id").slice(-1);
    var rightAmount = (gotoId - 1) * 100
    var rightAmountString = rightAmount + "%";
    $(".carousel").animate({ right: rightAmountString });
    $(".carousel-button").removeClass("current")
    $(this).addClass("current")
});
function currencyControl() {
    let currentCurrency = $("#currency-controller").val().split("-")[0];
    let multiplier = Number.parseFloat($("#currency-controller").val().split("-")[1]); // For First Func


    if ($.cookie("Currency") == undefined || $.cookie("Currency") == null) {
        $.cookie('Currency', 'EU-1', {
            expires: 7, path: '/'
        });
        $.cookie('DividingNum', '1', {
            expires: 7, path: '/'
        });
    }

    let allCurrencyPlaces = $(".currency")
    for (var i = 0; i < allCurrencyPlaces.length; i++) {
        let currencyDivider = Number.parseFloat($.cookie("DividingNum"));
        let price = Number.parseFloat($(allCurrencyPlaces[i]).text().replace(",", ".").split(" ")[0]);
        price /= currencyDivider;
        price *= multiplier;// For First Func
        $(allCurrencyPlaces[i]).text(price + " " + currentCurrency)// For First Func
    }



    if ($.cookie("Currency") != $("#currency-controller").val()) {
        let currency = $("#currency-controller").val();
        
        $.cookie('Currency', currency, {
            expires: 7, path: '/'
        });
        $.cookie('DividingNum', multiplier, {
            expires: 7, path: '/'
        });
    }
}

function firstChange() {
    if ($.cookie("Currency") == undefined || $.cookie("Currency") == null) {
        $.cookie('Currency', 'EU-1', {
            expires: 7, path: '/'
        });
        $.cookie('DividingNum', '1', {
            expires: 7, path: '/'
        });
    }
    $("#currency-controller").val($.cookie('Currency'))
    let currentCurrency = $("#currency-controller").val().split("-")[0];
    let multiplier = Number.parseFloat($.cookie('Currency').split("-")[1]); // For First Func
    let allCurrencyPlaces = $(".currency")
    for (var i = 0; i < allCurrencyPlaces.length; i++) {

        let price = Number.parseFloat($(allCurrencyPlaces[i]).text().replace(",", ".").split(" ")[0]);
        price *= multiplier;// For First Func
        $(allCurrencyPlaces[i]).text(price + " " + currentCurrency)// For First Func
    }
}