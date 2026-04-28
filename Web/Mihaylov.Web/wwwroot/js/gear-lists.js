$(function () {

    // Handle both Edit and Add buttons
    $(document).on('click', '.btn-generic-edit, .btn-generic-add', function () {
        var btn = $(this);
        var isEdit = btn.hasClass('btn-generic-edit');

        // FIND the closest container, then FIND the modal inside it
        var container = btn.closest('.generic-grid-container');
        var modal = container.find('.generic-modal');

        // Get data from button (or defaults for Add)
        var id = isEdit ? btn.data('id') : 0;
        var name = isEdit ? btn.data('name') : '';

        // Populate the specific modal fields
        modal.find('.field-id').val(id);
        modal.find('.field-name').val(name);

        // Show the specific modal instance
        modal.modal('show');
    });
});