$(function () {
    var $container = $("#update-person-container");
    $container.off("click");

    var $singleProgressBar = $("#single-progress-bar");
    var $totalProgressBar = $("#total-progress-bar");
    var $updateCount = $("span.update-count");

    const url = $container.data('url');
    const tokenName = $container.data('token');

    var connection = new signalR.HubConnectionBuilder()
        .withUrl(url + "/UpdateProgressHub")
        .build();

    connection.on("updateAccountsLoadingBar",
        (data) => {
            $singleProgressBar.attr("style", `width:${data.single}%; transition:none;`);
            $singleProgressBar.attr("aria-valuenow", data.single);
            $singleProgressBar.text(`${data.single}% `);

            $totalProgressBar.attr("style", `width:${data.total}%; transition:none;`);
            $totalProgressBar.attr("aria-valuenow", data.total);
            $totalProgressBar.text(`${data.total}% `);

            $updateCount.text(data.count);
        });

    connection
        .start()
        .then(function () {
            const connectionId = connection.connectionId;
            const fullUrl = url + "/api/person/accounts";

            $container.on("click", ".start-new-update", function () {
                $.ajax({
                    type: "Post",
                    url: fullUrl,
                    headers: {
                        Authorization: "Bearer " + $.cookie(tokenName)
                    },
                    data: { connectionId: connectionId },
                    success: function (viewHTML) {
                        location.reload();
                    },
                    error: function (xhr, ajaxOptions, thrownError) {
                        console.log(xhr.status);
                        console.log(thrownError);
                    }
                });
            });
        });
});