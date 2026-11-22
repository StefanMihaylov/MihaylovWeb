$(function () {
    var $popups = $('[data-toggle="popover"]');
    $popups.popover();

    var getLastVersion = function (row, reload) {
        var $base = $('#cluster-grid');
        var url = $base.data('url');

        var $this = $(row);
        var id = $this.data('id');
        if (id) {
            $.get(url, { id: id, reload: reload }, function (data, status) {
                $this.replaceWith(data);

                $('[data-toggle="popover"]').popover();
                $('[data-toggle="version-popover"]').popover();
            });
        }
    };

    var $rows = $('#cluster-grid .lazy-load');
    $rows.each(function () {
        getLastVersion(this, false)
    });

    $('#cluster-grid').on('click', '.reload', function () {
        var $button = $(this);
        var $parent = $button.closest('.lazy-load');
        getLastVersion($parent, true)
    });
});