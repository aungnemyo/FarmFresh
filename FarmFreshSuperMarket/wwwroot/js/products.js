// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    var currentPage = 1;
    var pagesize = 3;

    //Load data for page 1 first time
    fetchData(1, pagesize);
    $(document).on('click', '.product-view', function () {
        var id = $(this).data('id');
        $.ajax({
            url: '/Home/GetProductDetail',
            type: 'GET',
            data: { id: id },
            dataType: 'json',
            success: function (output, status, xhr) {
                if (output) {
                    // generate html and Load data
                    $('#Name').html(output.name);
                    $('#description').html(output.description);
                    $('#productimage').attr('src', `data:image/png;base64,${output.image}`).addClass('img-fluid img-thumbnail rounded');
                }
            },
            error: function () {
                alert('Error! Please try again.');
            }
        }).done(function () {
            //remove loading panel after ajax call complete
            //$loading.remove();
            $('#myModal').modal('show');
        });
    });

    $("#Prev").click(function () {
        currentPage -= 1;
        fetchData(currentPage, pagesize);
    });

    $("#Next").click(function () {
        currentPage += 1;
        fetchData(currentPage, pagesize);
    });

    // Paging
    //$('#updatePanel').on('click', '.footerContent a', function (e) {
    //    e.preventDefault();
    //    var pageNo = parseInt($(this).html());
    //    currentPage = pageNo;
    //    fetchData(currentPage);
    //});

    //Fetch Data
    function fetchData(pageNo, pageSize) {
        var searchProduct = $('#searchProduct').val();
        //Create loading panel
        //var $loading = "<div class='loading'>Please wait...</div>";
        //$('#updatePanel').prepend($loading);
        //Ajax call for fetch data from WEB Api
        $.ajax({
            url: '/Home/GetProductList',
            type: 'GET',
            data: { searchProduct: searchProduct, pageNo: pageNo, pageSize: pageSize },
            dataType: 'json',
            success: function (output, status, xhr) {
                if (output) {
                    // generate html and Load data
                    $('#products').empty();
                    var $row = $('<div />').addClass('row');
                    $.each(output.products, function (i, emp) {
                        var $col = $('<div />').addClass('col-sm-6 col-md-4');
                        var $thumb = $('<div />').addClass('shop__thumb');
                        var $link = $('<a />').attr('href', '#').attr('data-id', emp.productId).addClass('product-view');
                        var $thumbimg = $('<div />').addClass('shop-thumb__img');
                        $thumbimg.html($('<img />').attr('src', `data:image/png;base64,${emp.image}`).addClass('img-responsive rounded img-thumbnail'));
                        $link.html($thumbimg);
                        $thumb.append($link);

                        //title
                        $title = $('<h5 />').addClass('shop-thumb__title').text(emp.name);
                        $thumb.append($title);

                        //desc
                        $desc = $('<div />').addClass('shop-thumb__price').text(emp.description);
                        $thumb.append($desc);
                        $col.html($thumb);
                        $row.append($col);
                    });

                    $('#products').append($row);

                    if (output.paging) {
                        if (output.paging.hasPrevious) {
                            $('#Prev').prop('disabled', false);
                        } else {
                            $('#Prev').prop('disabled', true);
                        }
                        $('#Curr').html(output.paging.currentPage + ' of ' + output.paging.totalPages);
                        if (output.paging.hasNext) {
                            $('#Next').prop('disabled', false);
                        } else {
                            $('#Next').prop('disabled', true);
                        }
                    }
                }
            },
            error: function () {
                alert('Error! Please try again.');
            }
        }).done(function () {
            //remove loading panel after ajax call complete
            //$loading.remove();
        });
    }
});