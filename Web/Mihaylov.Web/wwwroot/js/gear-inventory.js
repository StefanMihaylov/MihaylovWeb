$(function () {

    // Handle Add
    $(document).on('click', '.inventory-btn-add', function () {
        const container = $(this).closest('.inventory-grid-container');
        const modal = container.find('.inventory-modal');

        modal.find('form')[0].reset(); // Clear all fields
        modal.find('.inventory-field-id').val(0);
        modal.find('.inventory-modal-title').text('Добави');
        modal.modal('show');
    });

    // Handle Edit
    $(document).on('click', '.inventory-btn-edit', function () {
        const btn = $(this);
        const container = btn.closest('.inventory-grid-container');
        const modal = container.find('.inventory-modal');

        // Parse the full object from the data-json attribute
        const data = btn.data('json');

        // console.log(data);

        // Map data to fields using the inventory-field- prefix
        modal.find('.inventory-field-id').val(data.id);
        modal.find('.inventory-field-name').val(data.name);
        modal.find('.inventory-field-description').val(data.description);
        modal.find('.inventory-field-category').val(data.categoryId);
        modal.find('.inventory-field-shop').val(data.shopId);
        modal.find('.inventory-field-brand').val(data.brandId);
        modal.find('.inventory-field-price').val(data.price);
        modal.find('.inventory-field-currency').val(data.currencyId);
        modal.find('.inventory-field-item-status').val(data.itemStatus);

        // Fix for Date inputs (needs yyyy-MM-dd format)
        if (data.purchaseDate) {
            modal.find('.inventory-field-purchasedate').val(data.purchaseDate.split('T')[0]);
        }
        else {
            modal.find('.inventory-field-purchasedate').val('');
        }

        $('#inventory-kit-body').empty();

        if (data.kitContents && data.kitContents.length > 0) {
            data.kitContents.forEach(function (item) {
                addKitRow(item.name);
            });
        }

        modal.find('.inventory-modal-title').text('Edit: ' + data.name);
        modal.modal('show');
    });

    function addKitRow(value) {
        var template = $('#kit-row-template').clone().removeAttr('id');
        template.find('.kit-item-input').val(value);
        $('#inventory-kit-body').append(template);
        renumberKitIndices();
    }

    function renumberKitIndices() {
        $('#inventory-kit-body tr').each(function (index, row) {
            $(row).find('.kit-item-input').attr('name', 'KitContents[' + index + '].Name');
        });
    }

    $('#inventory-btn-add-kit').on('click', function () {
        addKitRow('');
    });

    $(document).on('click', '.remove-kit-row', function () {
        $(this).closest('tr').remove();
        renumberKitIndices();
    });

    $('input[name="inventoryFilter"]').on('change', function () {
        const $button = $('#inventory-filter-active');
        const showUnpackedOnly = $button.is(':checked');

        var nonPacked = "";
        if (showUnpackedOnly) {
            nonPacked = "?isActive=true"
        }

        window.location.href = `/Gear/Inventory/` + nonPacked;
    });
});