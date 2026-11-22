$(function () {
    $('[data-toggle="popover"]').popover();
});

$(function () {
    var keyFrom = 'MergeFrom';
    var keyTo = 'MergeTo';

    var $detailsContainer = $('#person-info');
    var $accountContainer = $('#account-info');
    var $answersContainer = $('#answers-info');
    var $questionContainer = $('#question-info');
    var $accountTypeContainer = $('#account-type-info');

    var $newPersonSearchContainer = $('#add-new-person-search');
    var $defaultFilterContainer = $('#default-filter-info');

    $('.body-content').off('click')

        // _AddPerson
        .on('click', '.person-details-container .edit-person-button', function () {
            $detailsContainer.empty();

            var methodUrl = $detailsContainer.data("url");

            var personId = $(this).data('id');
            var url = main.getUrl(methodUrl);
            var data = { id: personId };

            $.get(url, data, function (res) {
                $detailsContainer.empty().append(res);
            }).fail(function (res) {
                $detailsContainer.empty().append(res);
            }).always(function () {
            });
        })
        .on('click', '.person-details-container .close-person-button', function () {
            $detailsContainer.empty();
        })

        // _AccountsGrid
        .on('click', '.person-details-container .edit-account-button', function () {            
            $accountContainer.empty();
            var methodUrl = $accountContainer.data("url");

            var $this = $(this);
            var id = $this.data('id');
            var personId = $this.data('personid');
            var url = main.getUrl(methodUrl);
            var data = { id: id, personId: personId };

            $.get(url, data, function (res) {
                $accountContainer.empty().append(res);
            }).fail(function (res) {
                $accountContainer.empty().append(res);
            }).always(function () {
            });
        })
        .on('click', '.person-details-container .close-account-button', function () {
            $accountContainer.empty();
        })

        // _AnswersGrid
        .on('click', '.person-details-container .edit-answer-button', function () {
            $answersContainer.empty();
            var methodUrl = $answersContainer.data("url");

            var $this = $(this);
            var id = $this.data('id');
            var personId = $this.data('personid');
            var url = main.getUrl(methodUrl);
            var data = { id: id, personId: personId };

            $.get(url, data, function (res) {
                $answersContainer.empty().append(res);
            }).fail(function (res) {
                $answersContainer.empty().append(res);
            }).always(function () {
            });
        })
        .on('click', '.person-details-container .close-answer-button', function () {            
            $answersContainer.empty();
        })
        .on('click', '.person-details-container .delete-answer-button', function () {
            var methodUrl = $answersContainer.data("delete-url");

            var id = $(this).data('id');
            var url = main.getUrl(methodUrl + id);

            $.ajax({
                url: url,
                type: 'DELETE',
                success: function (result) {
                    location.reload();
                }
            });
        })

        // _AddQuestion
        .on('click', '.person-details-container .edit-question-button', function () {            
            $questionContainer.empty();

            var methodUrl = $questionContainer.data("url");
            var $this = $(this);
            var id = $this.data('id');
            var url = main.getUrl(methodUrl);
            var data = { id: id };

            $.get(url, data, function (res) {
                $questionContainer.empty().append(res);
            }).fail(function (res) {
                $questionContainer.empty().append(res);
            }).always(function () {
            });
        })
        .on('click', '.person-details-container .close-question-button', function () {
            $questionContainer.empty();
        })

        // _AddAccountType
        .on('click', '.person-details-container .edit-account-type-button', function () {
            $accountTypeContainer.empty();

            var methodUrl = $accountTypeContainer.data("url");
            var $this = $(this);
            var id = $this.data('id');
            var url = main.getUrl(methodUrl);
            var data = { id: id };

            $.get(url, data, function (res) {
                $accountTypeContainer.empty().append(res);
            }).fail(function (res) {
                $accountTypeContainer.empty().append(res);
            }).always(function () {
            });
        })
        .on('click', '.person-details-container .close-account-type-button', function () {            
            $accountTypeContainer.empty();
        })

        // MergePerson
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

                var methodUrl = $(this).data("url");
                var url = main.getUrl(methodUrl);
                var fullUrl = url + '?from=' + fromValue + '&to=' + toValue;
                window.location = fullUrl;
            }

            setMergeButtonColour();
        })

        // _AddNewPerson
        .on('click', '.person-details-container .add-new-person-search-button', function () {            
            $newPersonSearchContainer.empty();

            var methodUrl = $newPersonSearchContainer.data("url");
            var url = main.getUrl(methodUrl);

            var $dropdown = $('.person-details-container .search-form #AccountTypeId');
            var accountTypeId = $dropdown.val();
            var data = { accountTypeId: accountTypeId };

            $.get(url, data, function (res) {
                $newPersonSearchContainer.empty().append(res);
            }).fail(function (res) {
                $newPersonSearchContainer.empty().append(res);
            }).always(function () {
            });
        })
        .on('click', '.person-details-container .close-new-person-button', function () {
            $newPersonSearchContainer.empty();
        })

        // _AddDefaultFilter
        .on('click', '.person-details-container .edit-default-filter-button', function () {            
            $defaultFilterContainer.empty();

            var methodUrl = $defaultFilterContainer.data("url");
            var $this = $(this);
            var id = $this.data('id');
            var url = main.getUrl(methodUrl);
            var data = { id: id };

            $.get(url, data, function (res) {
                $defaultFilterContainer.empty().append(res);
            }).fail(function (res) {
                $defaultFilterContainer.empty().append(res);
            }).always(function () {
            });
        })
        .on('click', '.person-details-container .close-default-filter-button', function () {            
            $defaultFilterContainer.empty();
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