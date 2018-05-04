$(function () {
    let menuIsVisible = true;

    if (window.matchMedia('(max-width: 992px)').matches) {
        menuIsVisible = false;
    }

    $('.btn-menu-slide').on('click', function () {
        if (menuIsVisible) {
            $(this).parent().animate({ right: '-250px' }, 400);
            $(this).animate({ right: '0' }, 400);
            $content = $('.content article');
            $content.addClass('col-md-12');
            $content.removeClass('col-md-10');
            menuIsVisible = false;
        }
        else {
            $(this).parent().animate({ right: '0' }, 400);
            $(this).animate({ right: '250px' }, 400);
            $content = $('.content article');
            $content.addClass('col-md-10');
            $content.removeClass('col-md-12');
            menuIsVisible = true;
        }
    });

    $('.dropdown').on('click', function () {
        $(this).next().slideToggle();
    })
})