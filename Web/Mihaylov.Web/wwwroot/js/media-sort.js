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
                    url: "/Media/ReadFilesSort",
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

    // Select directory
    var $dirContainer = $("#dir-container");
    $dirContainer.off("click");

    var $dirInput = $("#move-pics-dir");
    var $dirSpan = $("#move-pics-dir-span");

    $dirContainer.on("click", ".dir-name", function () {
        var value = $(this).text();
        $dirInput.val(value);
        $dirSpan.text(value);
    });
});