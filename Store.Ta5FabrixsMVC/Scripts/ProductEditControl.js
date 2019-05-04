$('.show-edit-form').click(function () {
    let id = $(this).attr("id").slice(5);
    //alert(id);
    showeditform(id);    
})
function showeditform(Id) {
    $("#edit-form").show();
    fillDataForItem(Id);
}

$("#close-edit-form").click(function () {
    $("#edit-form").hide();
    $(".emptiable-field").empty();
})

function fillDataForItem(ItemId) {
    let Id = Number.parseInt(ItemId);
    $.ajax({
        url: "/Admin/GetItem",
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
    $("#edit-item-catid").val(data.CategoryId);
    $("#edit-item-tagsreal").val(data.Tags);
    $("#edit-item-name").val(data.Name);
    $("#edit-item-description").val(data.Description);
    $("#edit-item-quantity").val(data.Quantity);
    $("#edit-item-priceeu").val(data.PriceEU);
    $(`#edit-item-size option:eq(${data.Size + 1})`).attr('selected', true);
    $("#edit-item-tags").val(data.TagsText);
    //alert("Image 1: " + data.Images[0].UrlName)

    for (var i of data.Images) {
        $('#current-images').append(`<div class="imageclass" id="image-${i.Id}"><img class="thumbnail" src="../../Content/Images/ProductImages/${i.UrlName}" id="image-${i.Id}"/> <a href="javascript:removeImage(${i.Id})"><i class="fa fa-close"></i></a></div>`)
    }
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