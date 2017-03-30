$(document).ready(function () {
    var $container = $('.body-content');
    var $detailsContainer = $('#person-info');

    $container.off('click').on('click', '.person-details-container .find-info-button', function () {
        $detailsContainer.empty();

        var siteUrl = $('#url').val();
        var url = main.getUrl('Site/Find');
        var data = { url: siteUrl };
        $.post(url, data, function (res) {
            $detailsContainer.empty().append(res);
            //if (res.Success === true) {
            //    toastr.success('Job ticket created');
            //}
            //else {
            //    toastr.error(res.ErrorMessage, undefined, { timeOut: 0 });
            //}
        }).fail(function (res) {
            // toastr.error(res.ErrorMessage); //???
        }).always(function () {

        });
    });
});