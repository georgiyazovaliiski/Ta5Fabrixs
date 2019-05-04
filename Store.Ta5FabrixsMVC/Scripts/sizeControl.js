$("select[name = 'ProductId']").on("change", function () {
    window.location.href = `/Catalogue/Item/${$(this).val()}`;
})

