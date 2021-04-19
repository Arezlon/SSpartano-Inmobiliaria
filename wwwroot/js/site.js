$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

/*
$(document).ready(function () {
    $('#FechaInicio').change(function () {
        var inicio = new Date($('#FechaInicio').val());
        inicio.setDate(inicio.getDate() + 1);
        var mesInicio = ("0" + (inicio.getMonth() + 1)).slice(-2)
        var anioInicio = inicio.getFullYear();

        var fin = new Date($('#FechaFin').val());
        fin.setDate(fin.getDate() + 1);
        var mesFin = ("0" + (inicio.getMonth() + 1)).slice(-2)
        var anioFin = inicio.getFullYear();

        var mesMinimo = ("0" + (fechaDesde.getMonth() + 2)).slice(-2);
        var anioMinimo = anioInicio;
        if (mesInicio == 13) {
            anioMinimo = anioInicio + 1;
            mesMinimo = 1;
        }

        $('#FechaFin').attr({
            "min": [anioMinimo, mesMinimo].join('-')
        });

        if (anioInicio > anioFin || (anioInicio == anioFin && mesInicio >= mesFin)) {
            $('#FechaFin').val([anioMinimo, mesMinimo].join('-'));
        }
        
    });
});*/
