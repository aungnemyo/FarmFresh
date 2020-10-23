// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('.carousel').carousel();

    $("#searchBtn").click(function () {
        var searchProduct = $('#searchProduct').val();
        var url = '/Home/ProductList';
        if (searchProduct) {
            let p = new URLSearchParams();
            p.set("searchProduct", searchProduct);
            url += '/?' + p.toString();
        }
        window.location.href = url;
    });
});

function urlGenerator(url, field, value) {
    var reg = new RegExp('(' + field + '=[^&]*)', "");
    return url.replace(reg, field + '=' + value);
}

//show alert message
function showAlert(title, message, reload = false) {
    bootbox.alert({
        size: "small",
        title: title,
        message: message,
        callback: function () {
            if (reload) {
                location.reload();
            }
        }
    })
}