// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

$(function () {
    $('#AICheckbox').click(function () {
        var cb1 = $('#AICheckbox').is(':checked');
        $('#PlayerTwo').prop('disabled', cb1);
    });
});

$(function () {
    $('#custom-ships').click(function () {
        var cb2 = $('#custom-ships').is(':checked');
        $('#no-of-ships').prop('disabled', cb2);
    });
});