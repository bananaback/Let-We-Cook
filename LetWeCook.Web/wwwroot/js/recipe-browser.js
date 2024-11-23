$(document).ready(function () {
    // Update Cook Time value
    $('#cookTimeRange').on('input', function () {
        var value = $(this).val();
        $('#cookTimeValue').text(value + ' minutes'); // Update the displayed value
    });

    // Update Servings value
    $('#servingsRange').on('input', function () {
        var value = $(this).val();
        $('#servingsValue').text(value + ' peoples'); // Update the displayed value
    });

});