$(document).ready(function () {
    $('.btn-menu-slide').on('click', function () {
     $('.menu').slideToggle();
    });

    $('.dropdown').on('click', function () {
        $(this).next().slideToggle();
    });

    $('#datetimepicker1').datetimepicker();
})