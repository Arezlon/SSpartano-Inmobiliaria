$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

$(document).ready(function () {
    $('#FechaInicio').change(function () {
        var date = new Date($('#FechaInicio').val());
        var month = ("0" + (date.getMonth() + 2)).slice(-2)
        var year = date.getFullYear();

        $('#FechaFin').val([year, month].join('-'));
        $('#FechaFin').attr({
            "min": [year, month].join('-')
        });
    });
});
