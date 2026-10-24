$(function () {
    // concert  
    $('.concert-grid').on('click', '.edit-concert-btn', function () {
        var $btn = $(this);

        const data = $btn.data('json');
        //console.log(data);

        $('#concert-id-input').val(data.id);
        $('#concert-date-input').val($btn.data('date'));
        $('#concert-name-input').val(data.name);
        $('#concert-type-input').val(data.concertTypeId);
        $('#concert-price-input').val(data.price);
        $('#concert-currency-input').val(data.currency);
        $('#concert-location-input').val(data.locationId);
        $('#concert-ticket-provider-input').val(data.ticketProviderId);

        for (var i = 0; i < 6; i++) {
            var band = data.bands[i];
            var bandId = band ? band.id : '';
            $('#concert-band-' + i).val(bandId);
        }

        $('#concert-name-input')[0].scrollIntoView({ behavior: 'smooth', block: 'center' });
    })
        .on('click', '.concert-cancel-btn', function () {
            $('#concert-id-input').val('');
            $('#concert-date-input').val('');
            $('#concert-name-input').val('');
            $('#concert-type-input').val('');
            $('#concert-price-input').val('');
            $('#concert-currency-input').val('');
            $('#concert-location-input').val('');
            $('#concert-ticket-provider-input').val('');

            for (var i = 0; i < 6; i++) {
                $('#concert-band-' + i).val('');
            }
        });


    // band
    $('.band-grid').on('click', '.edit-band-btn', function () {
        var $btn = $(this);

        $('#band-id-input').val($btn.data('id'));
        $('#new-band-input').val($btn.data('name'));
        $('#new-band-country-id-input').val($btn.data('country'));

        $('#new-band-input')[0].scrollIntoView({ behavior: 'smooth', block: 'center' });
    })
        .on('click', '.band-cancel-btn', function () {
            $('#band-id-input').val('');
            $('#new-band-input').val('');
            $('#new-band-country-id-input').val('');
        });

    // location
    $('.location-grid').on('click', '.edit-location-btn', function () {
        var $btn = $(this);

        $('#location-id-input').val($btn.data('id'));
        $('#new-location-input').val($btn.data('name'));

        $('#new-location-input')[0].scrollIntoView({ behavior: 'smooth', block: 'center' });
    })
        .on('click', '.location-cancel-btn', function () {
            $('#location-id-input').val('');
            $('#new-location-input').val('');
        });

    // provider
    $('.provider-grid').on('click', '.edit-provider-btn', function () {
        var $btn = $(this);

        var isActive = $btn.data('active');
        console.log(isActive)

        $('#provider-id-input').val($btn.data('id'));
        $('#new-provider-name-input').val($btn.data('name'));
        $('#new-provider-url-input').val($btn.data('url'));
        $('#new-provider-active-input').prop('checked', isActive);

        $('#new-provider-name-input')[0].scrollIntoView({ behavior: 'smooth', block: 'center' });
    })
        .on('click', '.provider-cancel-btn', function () {
            $('#provider-id-input').val('');
            $('#new-provider-name-input').val('');
            $('#new-provider-url-input').val('');
            $('#new-provider-active-input').prop('checked', false);
        });

    // countries
    $('.country-grid').on('click', '.edit-country-btn', function () {
        var $btn = $(this);

        $('#country-id-input').val($btn.data('id'));
        $('#new-country-name-input').val($btn.data('name'));
        $('#new-country-code-input').val($btn.data('code'));

        $('#new-country-name-input')[0].scrollIntoView({ behavior: 'smooth', block: 'center' });
    })
        .on('click', '.country-cancel-btn', function () {
            $('#country-id-input').val('');
            $('#new-country-name-input').val('');
            $('#new-country-code-input').val('');
        });

    // concert-type
    $('.concert-type-grid').on('click', '.edit-concert-type-btn', function () {
        var $btn = $(this);

        $('#concert-type-id-input').val($btn.data('id'));
        $('#new-concert-type-name-input').val($btn.data('name'));

        $('#new-concert-type-name-input')[0].scrollIntoView({ behavior: 'smooth', block: 'center' });
    })
        .on('click', '.concert-type-cancel-btn', function () {
            $('#concert-type-id-input').val('');
            $('#new-concert-type-name-input').val('');
        });
});