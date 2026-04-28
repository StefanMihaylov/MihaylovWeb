$(function () {

    $(document).on('click', '.trip-btn-add', function () {
        const container = $(this).closest('.trip-grid-container');

        const modal = container.find('.trip-modal');
        modal.find('.trip-modal-title').text('Ново пътешествие');

        modal.find('form')[0].reset();
        modal.find('.trip-field-id').val(0);
        modal.find('.trip-field-year').val(new Date().getFullYear());

        //modal.find('.trip-created-info').addClass('d-none');
        modal.modal('show');
    });

    $(document).on('click', '.trip-btn-edit', function () {
        const btn = $(this);
        const container = btn.closest('.trip-grid-container');
        const modal = container.find('.trip-modal');
        modal.find('.trip-modal-title').text('Редактиране');

        const data = btn.data('json');
        console.log(data);

        // Map DTO properties to prefixed fields
        modal.find('.trip-field-id').val(data.id);
        modal.find('.trip-field-title').val(data.title);
        modal.find('.trip-field-year').val(data.year);
        modal.find('.trip-field-notes').val(data.notes);

        // Show the creation date read-only
        if (data.createdAt) {
            modal.find('.trip-field-created-text').val(data.createdAt.split('T')[0]);
            // modal.find('.trip-created-info').removeClass('d-none');
        }

        modal.modal('show');
    });
});