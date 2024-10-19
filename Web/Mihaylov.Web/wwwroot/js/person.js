$(function () {
    $('[data-toggle="popover"]').popover();
});

$(function () {

    var keyFrom = 'MergeFrom';
    var keyTo = 'MergeTo';

    var $detailsContainer = $('#person-info');
    var $newPersonSearchContainer = $('#add-new-person-search');

    $('.body-content').off('click')
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
        .on('click', '.person-details-container .edit-account-button', function () {
            var $accountContainer = $('#account-info');
            $accountContainer.empty();

            var $this = $(this);
            var id = $this.data('id');
            var personId = $this.data('personid');
            var url = main.getUrl('Site/AccountView');
            var data = { id: id, personId: personId };

            $.post(url, data, function (res) {
                $accountContainer.empty().append(res);
            }).fail(function (res) {
                $accountContainer.empty().append(res);
            }).always(function () {
            });
        })
        .on('click', '.person-details-container .close-account-button', function () {
            var $accountContainer = $('#account-info');
            $accountContainer.empty();
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
        .on('click', '.person-details-container .edit-account-type-button', function () {
            var $accountTypeContainer = $('#account-type-info');
            $accountTypeContainer.empty();

            var $this = $(this);
            var id = $this.data('id');
            var url = main.getUrl('Site/AccountTypeView');
            var data = { id: id };

            $.post(url, data, function (res) {
                $accountTypeContainer.empty().append(res);
            }).fail(function (res) {
                $accountTypeContainer.empty().append(res);
            }).always(function () {
            });
        })
        .on('click', '.person-details-container .close-account-type-button', function () {
            var $accountTypeContainer = $('#account-type-info');
            $accountTypeContainer.empty();
        })
        .on('click', '.person-details-container .merge-person-button', function () {
            var fromValue = localStorage.getItem(keyFrom);
            var toValue = localStorage.getItem(keyTo);

            var $this = $(this);
            var personId = $this.data('id');

            if (fromValue === null && toValue === null) {
                localStorage.setItem(keyFrom, personId);
            }
            else if (fromValue == personId) {
                localStorage.removeItem(keyFrom);
            }
            else if (toValue == personId) {
                localStorage.removeItem(keyTo);
            }
            else if (toValue === null) {
                localStorage.setItem(keyTo, personId);
            }

            setMergeButtonColour();
        })
        .on('click', '.person-details-container .clear-merge-person-button', function () {
            var fromValue = localStorage.getItem(keyFrom);
            var toValue = localStorage.getItem(keyTo);

            if (fromValue !== null) {
                localStorage.removeItem(keyFrom);
            }

            if (toValue !== null) {
                localStorage.removeItem(keyTo);
            }

            setMergeButtonColour();
        })
        .on('click', '.person-details-container .merge-action-button', function () {
            var fromValue = localStorage.getItem(keyFrom);
            var toValue = localStorage.getItem(keyTo);

            if (fromValue !== null && toValue !== null) {
                localStorage.removeItem(keyFrom);
                localStorage.removeItem(keyTo);

                var url = main.getUrl('Site/MergeView');
                var fullUrl = url + '?from=' + fromValue + '&to=' + toValue;
                window.location = fullUrl;
            }

            setMergeButtonColour();
        })
        .on('click', '.person-details-container .add-new-person-search-button', function () {            
            $newPersonSearchContainer.empty();

            var url = main.getUrl('Site/NewPersonView');

            $.post(url, {}, function (res) {
                $newPersonSearchContainer.empty().append(res);
            }).fail(function (res) {
                $newPersonSearchContainer.empty().append(res);
            }).always(function () {
            });
        })
        .on('click', '.person-details-container .close-new-person-button', function () {
            $newPersonSearchContainer.empty();
        })
        ;

    setMergeButtonColour();

    $detailsContainer.on('change', '.AnswerTypeId-dropdown', changeButtonColour);
    $detailsContainer.on('input', '.Comments-input', changeButtonColour);

    function changeButtonColour() {
        var button = $detailsContainer.find('.submit-btn');
        button.addClass('btn-success');
    };

    function setMergeButtonColour() {
        var fromValue = localStorage.getItem(keyFrom);
        var toValue = localStorage.getItem(keyTo);

        var $baseButtons = $('.merge-base-button');
        if (fromValue !== null || toValue !== null) {
            $baseButtons.removeClass('btn-secondary').addClass('btn-info');
        }
        else {
            $baseButtons.addClass('btn-secondary').removeClass('btn-info');
        }

        var allButtons = $('.merge-person-button');
        allButtons.addClass('btn-secondary').removeClass('btn-success').removeClass('btn-info');

        if (fromValue !== null) {
            var $thisFrom = $('.merge-person-button[data-id="' + fromValue + '"]');
            $thisFrom.removeClass('btn-secondary').addClass('btn-success');
        }

        if (toValue !== null) {
            var $thisTo = $('.merge-person-button[data-id="' + toValue + '"]');
            $thisTo.removeClass('btn-secondary').addClass('btn-info');
        }
    };
});