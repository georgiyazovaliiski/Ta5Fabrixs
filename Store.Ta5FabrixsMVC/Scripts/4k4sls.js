function ch4sls() {
    if ($("#newsletter").valid()) {
    let data = {
        mailName: $("#mailName").val()
    }
    $.ajax({
        url: "/Catalogue/AddNewspaperMember",
        dataType: "json",
        contentType: 'application/json',
        type: "POST",
        data: JSON.stringify(data),
        success: function (data) {
            alert(data)
            $("#mailName").prop("disabled", true);
            $("#mailName").remove();
            $("#mailNameSubmit").prop("disabled", true);
            $("#mailNameSubmit").val(data);
        },
        error: function (e) {
            alert("an error came along! try again later!")
        }
    })
}
}

$("#newsletter").submit(function (event) {
    event.preventDefault();
})