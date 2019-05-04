function checkout() {
    $("#checkOutForm").submit();
}
$("#agreement").on("change", function () {
    if ($(this).is(":checked")) {
        alert("Works.")
    }
    else {
        alert("Not checked.")
    }
})
let agreement = `
<h3>Almost ready! Just one step from finishing your purchase!</h3><br><hr>
<div id="agreement">
    <input type="checkbox" value="agreement" id="agreement" /> I agree with the Terms and Conditions of Ta5Fabrixs.
</div>
<hr>
<a href="#" onclick="checkout()" class="btn btn-default submitfornewspaper" id="nextpage" style="display:block; width:100%">SUBMIT ORDER</a>
`;

function nextpage() {
    let _UserDetails = {
        FirstName: $("#checkOutForm input[name='FirstName']").val(),
        LastName: $("#checkOutForm input[name='LastName']").val(),
        Country: $("#checkOutForm select[name='Country']").val(),
        City: $("#checkOutForm input[name='City']").val(),
        Region: $("#checkOutForm input[name='Region']").val(),
        Address: $("#checkOutForm input[name='Address']").val(),
        ZipCode: $("#checkOutForm input[name='ZipCode']").val(),
        Phone: $("#checkOutForm input[name='Phone']").val(),
        PromoCode: $("#checkOutForm input[name='PromoCode']").val()
    }
    if ($('#Econt').is(':checked')) {
        _UserDetails['PaymentMethod'] = 1
        $("#checkOutForm :input").attr("readonly", true)

        $("#checkOutForm a").fadeOut(500);

        setTimeout(function () {$("#checkOutForm a").remove()
    },500);
        //Send UserData to DB
        console.log(_UserDetails)
        $.ajax({
            url: "/Manage/AddUserDetails",
            data: JSON.stringify(_UserDetails),
            dataType: "json",
            contentType: 'application/json; charset=utf-8',
            type: "POST",
            success: function (result) {
                alert(result)
                $("#checkOutForm :input").attr("disabled", true)
                $("#checkOutForm a").fadeOut();

                $("#checkOutForm a").remove();
                $("#nextpage").append(agreement);
            },
            error: function (e) {
                alert(e);
                console.log(e)
            }
        })
    }
    if ($('#Paypal').is(':checked')) {
        _UserDetails['PaymentMethod'] = 0
        //AJAX here
        $.ajax({
            url: "/Manage/GetCartItemsForCheckout",
            dataType: "json",
            type: "GET",
            data: { PromoCode: _UserDetails['PromoCode'] },
            success: function (data) {
                $("#checkOutForm :input").attr("readonly", true)

                $("#checkOutForm a").fadeOut();

                $("#checkOutForm a").remove();
                //Send UserData to DB

                $.ajax({
                    url: "/Manage/AddUserDetails",
                    data: JSON.stringify(_UserDetails),
                    dataType: "json",
                    contentType: 'application/json; charset=utf-8',
                    type: "POST",
                    success: function (result) {
                        alert(result)
                    },
                    error: function (e) {
                        alert("An error came along. Maybe try again later?");
                    }
                })
                $("#checkOutForm :input").attr("disabled", true)
                //console.log(data);

                let form = `<form method="post" action="https://www.sandbox.paypal.com/cgi-bin/webscr">
                    <input type="hidden" name="upload" value="1" />
                    <input type="hidden" name="return" value="http://localhost:63714/Catalogue/FinishOrder" />
                    <input type="hidden" name="cmd" value="_cart" />
                    <input type="hidden" name="business" value="MILUSHEVA_NELI-facilitator@abv.bg" />
                    <input type="hidden" name="currency_code" value="EUR">
                    <input type="hidden" name="lc" value="EU">`

                for (let i = 1; i <= data.length; i++) {
                    let PPCartItem = `
                    <input type="hidden" name="item_name_${i}" value="${data[i-1].Name}" />
                    <input type="hidden" name="item_number_${i}" value="${data[i-1].Id}" />
                    <input type="hidden" name="amount_${i}" value="${data[i-1].PriceEU}" />
                    <input type="hidden" name="quantity_${i}" value="${data[i-1].Quantity}" />
                    `
                    form += PPCartItem
                }
                let endForm = `
<h3>Order Data submitted, after you click Finish Order you will proceed with payment.</h3><br>
<input type="submit" value="FINISH ORDER" class="btn btn-default submitfornewspaper" style="width:100%" />
                        </form>`
                form += endForm;

                $("#nextpage").append(form)
            },
            error: function (data) {
                console.log(data)
            }
        })
        
    }
    /*alert('ei ddeiba tva e ')
    $("#firstForm").fadeOut(300);
    $("#firstForm").empty();
    $("#nextpage").append(agreement);
    $("#agreement").append();*/
}
