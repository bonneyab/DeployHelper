//TODO: I know I know...general of all things...
jQuery(document).ready(function ($) {
    $('a[data-toggle="tooltip"]').tooltip();

    $('.confirm').on('click', function () {
        return confirm($(this).attr("confirm-message"));
    });
});