


$(document).ready(function () {
    if ($('#selecProceso').val() !== null) { // en la carga inicial no ejecutamos busqueda si esta el seleccione
        GetDeclaracionesbyProcess()
    }

});

function AddZeroDate(n) {
    return (n < 10 ? '0' : '') + n;
}

//Funcion para obtener las declaraciones de los funcionarios dado el combo de procesos.https://localhost:7214/DeclaracionesFuncionario/Details/6463
function GetDeclaracionesbyProcess() {
    $("#loader").removeClass("loading")
    $("#loader").addClass("loading")
    if ($('#selecProceso').val() > 0) {
        $.getJSON('/DeclaracionesFuncionario/GetDeclaracionesbyProcess?IdProceso=' + $('#selecProceso').val()
            , function (data) {
                if ($.fn.DataTable.isDataTable('#TableDeclaraciones')) {

                } else {
                    var t = $("#TableDeclaraciones").DataTable({
                        language: {
                            url: '/js/dataTables.es-es.json'
                        }
                    });
                }
              

                try {
                    var t = $("#TableDeclaraciones").DataTable()
                    t.rows('tr').remove()
                    if (data == false) {
                    } else {
                        var arr_from_json = JSON.parse(data.result);
                       
                        $.each(arr_from_json, function (i, Declaraciones) {
                           

                       
                            var dateDEclaracion = new Date(Declaraciones.FechaDeclaracion)
                            dateDEclaracion = AddZeroDate(dateDEclaracion.getDate()) + "/" + AddZeroDate((dateDEclaracion.getMonth() + 1)) + "/" + dateDEclaracion.getFullYear()

                            $('#selecProceso option[value="' + $('#selecProceso').val() + '"]').attr('data-id')
                           
                            if (dateDEclaracion == "31/12/1969" || dateDEclaracion == "01/01/1970") {
                                dateDEclaracion = "-"
                            }   
                            var _recibidaFisico = " Si "
                            if (Declaraciones.Formulario.RecibirEnFisico == false) {
                                _recibidaFisico = " - "
                            }
                            var disableEdit = "";
                            var impresion = "";
                            if (($('#selecProceso option[value="' + $('#selecProceso').val() + '"]').attr('data-id') == "Cerrado" ||
                                $('#selecProceso option[value="' + $('#selecProceso').val() + '"]').attr('data-id') == "Cerrado anticipado"
                            ) || Declaraciones.IdEstadoDeclaracion !== 1) {
                                disableEdit = "disabled";

                            }
                            if (Declaraciones.IdEstadoDeclaracion !==1  ) {
                                impresion = "<a class='tableButton ' onclick='imprimir(" + Declaraciones.IdDeclaracion + ")'> <i class='fa fa-print'></i></a>";
                            } else {
                                impresion = "<a class='tableButton disabled ' > <i class='fa fa-print'></i></a>";
                            }
                            t.row.add([
                                
                                '<a  href="/DeclaracionesFuncionario/Edit/' + Declaraciones.IdDeclaracion + '"  class="tableButton tableButtonEdit ' +
                               // (Declaraciones.IdEstadoDeclaracion == 1 ? "" : "disabled")class="fa fa-print"
                                disableEdit
                                + ' " > <i class="fa fa-edit"></i></a > |' +
                                '<a href="/DeclaracionesFuncionario/Details/' + Declaraciones.IdDeclaracion + '"   class="tableButton tableButtonSearch ' + (Declaraciones.IdEstadoDeclaracion == 1 ? "disabled" : "") + '"><i class="fa fa-search"></i></a>',
                                Declaraciones.Formulario.Titulo,
                                Declaraciones.EstadoDeclaracion.NombreEstadoDeclaracion,
                                dateDEclaracion ,
                                _recibidaFisico
                              
                            ]).draw(false);
                        });
                    }
                }
                catch (e) {
                    console.log(e);
                    $("#loader").removeClass("loading")
                }
                $("#loader").removeClass("loading")
                t.columns.adjust().draw();
            });
    }

}



function imprimir(idDeclaracion) {

   
        $('.Mensaje').text("");
        $('.Mensaje iframe').remove();
    $('.Mensaje').append("<iframe class='iframe-Preview' src='" + window.location.origin + "/DeclaracionesFuncionario/Preview/"+idDeclaracion + "'></iframe>");
        $(".modal-Preview").modal("show");

}

