$(function () {
    var $popups = $('[data-toggle="popover"]');
    $popups.popover();

    var getLastVersion = function (row, reload) {
        var $this = $(row);
        var id = $this.data('id');
        if (id) {
            var url = '/cluster/getLastVersion?id=' + id + '&reload=' + reload;
            $.get(url, function (data, status) {
                $this.replaceWith(data);

                $('[data-toggle="popover"]').popover();
                $('[data-toggle="version-popover"]').popover();
            });
        }
    };

    var $rows = $('#cluster-grid-tab .lazy-load');
    $rows.each(function () {
        getLastVersion(this, false)
    });

    $('#cluster-grid-tab').on('click', '.reload', function () {
        var $button = $(this);
        var $parent = $button.closest('.lazy-load');
        getLastVersion($parent, true)
    });
});