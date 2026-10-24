$(document).ready(function () {
    var $container = $("#weather-container");
    $container.off("click")

    $container.on("click", ".current-add-new-button", function () {
        var $base = $("#current-add-new-row");
        var $cityInput = $("#current-new-city-imput");
        var url = $base.data('url');
        var cityValue = $cityInput.val();

        $.ajax({
            type: "Post",
            url: url,
            data: { city: cityValue },
            success: function (viewHTML) {
                $cityInput.val('');
                $(viewHTML).insertBefore($base);
            },
            error: function (errorData) {
                console.log(errorData);
            }
        });
    });

    $container.on("click", ".forecast-add-new-button", function () {
        var $base = $("#forecast-add-new-row");
        var $cityInput = $("#forecast-new-city-imput");
        var url = $base.data('url');
        var cityValue = $cityInput.val();

        $.ajax({
            type: "Post",
            url: url,
            data: { city: cityValue },
            success: function (viewHTML) {
                $cityInput.val('');
                $(viewHTML).insertBefore($base);
            },
            error: function (errorData) {
                console.log(errorData);
            }
        });
    });

});