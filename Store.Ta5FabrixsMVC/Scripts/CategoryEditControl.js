$('.show-edit-category-form').click(function () {
    let id = $(this).attr("id").slice(5);
    //alert(id);
    showeditform(id);    
})
function showeditform(Id) {
    $("#edit-category-form").show();
    fillDataForItem(Id);
}

$("#close-edit-form").click(function () {
    $("#edit-category-form").hide();
    $(".emptiable-field").empty();
})

function fillDataForItem(ItemId) {
    let Id = Number.parseInt(ItemId);
    $.ajax({
        url: "/Admin/GetCategory",
        dataType: "json",
        type: "GET",
        data: { id: Id },
        success: function (data) {
            writeOnEditForm(data)
        },
        error: function (data) {
            console.log(data)
        }
    })
}

function writeOnEditForm(data) {
    //alert(data.Name)
    $("#edit-item-id").val(data.Id);
    $("#edit-item-name").val(data.Name);
    //alert("Image 1: " + data.Images[0].UrlName)
}

function removeImage(Id) {
    alert(Id)
    $.ajax({
        url: "/Admin/RemoveImage",
        dataType: "json",
        type: "GET",
        data: { Id: Id },
        success: function (data) {
            alert(data)
            removeImageFromDisplay(Id);
        },
        error: function (data) {
            console.log(data)
        }
    })
}

function removeImageFromDisplay(imageId) {
    $(`#image-${imageId}`).remove();
}