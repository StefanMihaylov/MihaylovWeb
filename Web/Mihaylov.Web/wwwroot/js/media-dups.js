$(function () {
    var $container = $("#scan-container");
    $container.off("click");

    var $fileProgressBar = $("#files-progress-bar");
    var $hashProgressBar = $("#hash-progress-bar");

    var connection = new signalR.HubConnectionBuilder()
        .withUrl("/ScanProgressHub")
        .build();

    connection.on("updateLoadingBar",
        (data) => {
            $fileProgressBar.attr("style", `width:${data.files}%; transition:none;`);
            $fileProgressBar.attr("aria-valuenow", data.files);
            $fileProgressBar.text(`${data.files}% `);

            $hashProgressBar.attr("style", `width:${data.processes}%; transition:none;`);
            $hashProgressBar.attr("aria-valuenow", data.processes);
            $hashProgressBar.text(`${data.processes}% `);
        });

    connection
        .start()
        .then(function () {
            const connectionId = connection.connectionId;
            $container.on("click", ".start-new-scan", function () {
                $.ajax({
                    type: "Get",
                    url: "/Media/ReadFiles",
                    data: { connectionId: connectionId },
                    success: function (viewHTML) {
                        location.reload();
                    },
                    error: function (errorData) {
                        console.log(errorData);
                    }
                });
            });
        });
});