$(".quantity-control").change(
    function () {
        let quantity = $(this).val();
        let productId = $(this).attr("id");
        //Send AJAX for change
        $.ajax({
            url: "/Catalogue/EditQuantity",
            dataType: "json",
            type: "POST",
            data: { quantity: quantity, productId: productId },
            success: function (data) {
                location.reload();
            },
            error: function (data) {
                console.log(data)
            }
        })
    }
)