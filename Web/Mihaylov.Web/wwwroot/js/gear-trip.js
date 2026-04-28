$(function () {

    function updateSelectorVisibility($select) {

        const type = $select.val();
        const $modal = $select.closest('.modal');

        var t = parseInt(type, 10);
        // Hide all selector groups and clear dropdowns
        $modal.find('.gearnode-selector-group').addClass('d-none').find('.gearnode-dropdown').val('');

        var selected = '';
        if (t === 1) {
            selected = '.gearnode-field-groupid';
        } else if (t === 2) {
            selected = '.gearnode-field-categoryId';
        } else if (t === 3) {
            selected = '.gearnode-field-itemid';
        }

        if (selected) {
            $modal.find(selected).closest('.gearnode-selector-group').removeClass('d-none');
        }
    }

    // Toggle fields based on NodeType selection
    $(document).on('change', '.gearnode-field-type', function () {

        updateSelectorVisibility($(this));
    });

    // Handle Add (setting ParentId automatically)
    $(document).on('click', '.gearnode-btn-add', function () {
        const parentId = $(this).data('parent-id');
        const modal = $('.gearnode-modal');

        modal.find('form')[0].reset();
        modal.find('.gearnode-field-id').val(0);
        modal.find('.gearnode-field-parentid').val(parentId);
        modal.find('.gearnode-field-qty').val(1);

        // Ensure selector visibility matches the current NodeType after reset
        updateSelectorVisibility($('.gearnode-field-type'));

        modal.modal('show');
    });

    // Handle Edit
    $(document).on('click', '.gearnode-btn-edit', function () {
        const data = $(this).data('json');
        const modal = $('.gearnode-modal');

        modal.find('.gearnode-field-id').val(data.id);
        modal.find('.gearnode-field-parentid').val(data.parentId);
        modal.find('.gearnode-field-type').val(data.nodeType);

        // Ensure selector visibility matches the set NodeType
        updateSelectorVisibility($('.gearnode-field-type'));

        modal.find('.gearnode-field-groupid').val(data.groupId);
        modal.find('.gearnode-field-categoryId').val(data.categoryId);
        modal.find('.gearnode-field-itemid').val(data.inventoryItemId).trigger('change');

        modal.find('.gearnode-field-qty').val(data.quantity);
        modal.find('.gearnode-field-packed').prop('checked', data.isPacked);
        modal.find('.gearnode-field-excluded').prop('checked', data.isExcluded);
        modal.find('.gearnode-field-required').prop('checked', data.isRequired);

        modal.modal('show');
    });

    // toggle
    $('input[name="gearFilter"]').on('change', function () {
        const $button = $('#filterUnpacked');
        const showUnpackedOnly = $button.is(':checked');
        const id = $button.data('trip-id');

        var nonPacked = "";
        if (showUnpackedOnly) {
            nonPacked = "?nonPacked=true"
        }

        window.location.href = `/Gear/Trip/` + id + nonPacked;
    });

    $('.select2-searchable').select2({
        placeholder: "Search items...",
        allowClear: true,
        dropdownParent: $('.gearnode-modal') // CRITICAL: Fixes focus issues inside Modals
    });
});