$(function () {
    $('[data-toggle="popover"]').popover();
});

$(function () {

    var $detailsContainer = $('#person-info');

    $('.body-content').off('click')
        //.on('click', '.person-details-container .find-info-button', function () {
        //    $detailsContainer.empty();

        //    var siteUrl = $('#url').val();
        //    var url = main.getUrl('Site/Find');
        //    var data = { url: siteUrl };

        //    $.post(url, data, function (res) {
        //        $detailsContainer.empty().append(res);
        //        $('#url').val('');
        //        //if (res.Success === true) {
        //        //    toastr.success('Job ticket created');
        //        //}
        //        //else {
        //        //    toastr.error(res.ErrorMessage, undefined, { timeOut: 0 });
        //        //}
        //    }).fail(function (res) {
        //        $detailsContainer.empty().append(res);
        //    }).always(function () {
        //    });
        //})
        .on('click', '.person-details-container .edit-person-button', function () {
            $detailsContainer.empty();

            var personId = $(this).data('id');
            var url = main.getUrl('Site/PersonView');
            var data = { id: personId };

            $.post(url, data, function (res) {
                $detailsContainer.empty().append(res);
            }).fail(function (res) {
                $detailsContainer.empty().append(res);
            }).always(function () {
            });
        })
        .on('click', '.person-details-container .close-person-button', function () {
            $detailsContainer.empty();
        })
        .on('click', '.person-details-container .edit-answer-button', function () {
            var $answersContainer = $('#answers-info');
            $answersContainer.empty();

            var $this = $(this);
            var id = $this.data('id');
            var personId = $this.data('personid');
            var url = main.getUrl('Site/AnswerView');
            var data = { id: id, personId: personId };
            $.post(url, data, function (res) {
                $answersContainer.empty().append(res);
            }).fail(function (res) {
                $answersContainer.empty().append(res);
            }).always(function () {
            });
        })
        .on('click', '.person-details-container .close-answer-button', function () {
            var $answersContainer = $('#answers-info');
            $answersContainer.empty();
        })
        .on('click', '.person-details-container .edit-question-button', function () {
            var $questionContainer = $('#question-info');
            $questionContainer.empty();

            var $this = $(this);
            var id = $this.data('id');
            var url = main.getUrl('Site/QuestionView');
            var data = { id: id };

            $.post(url, data, function (res) {
                $questionContainer.empty().append(res);
            }).fail(function (res) {
                $questionContainer.empty().append(res);
            }).always(function () {
            });
        })
        .on('click', '.person-details-container .close-question-button', function () {
            var $questionContainer = $('#question-info');
            $questionContainer.empty();
        })
        ;


    $detailsContainer.on('change', '.AnswerTypeId-dropdown', changeButtonColour);
    $detailsContainer.on('input', '.Comments-input', changeButtonColour);

    function changeButtonColour() {
        var button = $detailsContainer.find('.submit-btn');
        button.addClass('btn-success');
    };
});