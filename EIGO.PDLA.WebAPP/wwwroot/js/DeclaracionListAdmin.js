

var iRowCnt = 0; // conteo de elementos agregados a la tabla. Para no repeter ID
var selectorCiudad = "#IdCiudad";//Lista desplegable de ciudad
var IdParentescoFuncionario = 0;
var ParentescoOptions = "";
var EstadoDeclaracionOptions = "";
var EstadoDeclaracionOptionsArray = [];
var idDeclaracionSeleccionada = 0;
var idEstadoInicial;
var rowCambiandoEstado;
var tiempoParaAlerta = 3000;

var NombresOptions = "";
$(document).ready(function () {
    GetEstadoDeclaraciones()
    if ($('#selecProceso').val() !== 0) {
        GetDeclaracionesbyProcess()
    }

});
function AddZeroDate(n) {
    return (n < 10 ? '0' : '') + n;
}
$(window).on("load", function () {

    setTimeout(function () {
        $('.TableHistory ').DataTable().destroy();
  
    }, 2000);

})

function GetDeclaracionesbyProcess() {

    if ($('#selecProceso').val() > 0) {
        $("#loader").removeClass("loading")
        $("#loader").addClass("loading")
        if ($('#Idfuncionario').val() == 0) {
            var t = $("#formularioTable").DataTable();
            t.rows('tr').remove()
         
            t.columns.adjust().draw();
            $("#loader").removeClass("loading")
        }
        else {

      
        $.getJSON('/Administracion/Declaraciones/GetDeclaracionesbyProcess?IdProceso=' + $('#selecProceso').val() + "&IdFuncionario=" + $('#Idfuncionario').val()
            , function (data) {
                var t = $("#formularioTable").DataTable();
                t.rows('tr').remove()
                try {
                    if (data == false) {
                    } else if(data=="salir"){

                        window.location.href = "/MicrosoftIdentity/Account/SignOut";
                    }
                    else {
                        var arr_from_json = JSON.parse(data.result);
   
                        for (var i = 0; i < arr_from_json.length;i++) {
                            // do stuff

                            var _recibidaFisico = "";
                            if (arr_from_json[i].RecibidaEnFisico == false) {
                                _recibidaFisico = " <select onchange='ChangeRecibidoDeclaracion(" + arr_from_json[i].IdDeclaracion + "," + i + ")' class='form-control SelectRecibidaEnFisico" + i + "'><option value='0'>Seleccione</option><option selected value='false'>Electronica</option><option value='true'>Fisica</option></select> " + '<div class="spinner-border hiddenElement spinnerRecibida' + i + '" role="status"><span class="visually-hidden">Loading...</span> </div><div class="mensajeActualizarRecibida' + i + ' hiddenElement"></div>'
                            } else if (arr_from_json[i].RecibidaEnFisico == null) {
                                _recibidaFisico = " <select  onchange='ChangeRecibidoDeclaracion(" + arr_from_json[i].IdDeclaracion + "," + i + ")' class='form-control SelectRecibidaEnFisico" + i + "'><option selected value='0'>Seleccione</option><option value='false'>Electronica</option><option  value='true'>Fisica</option></select> " + '<div class="spinner-border hiddenElement spinnerRecibida' + i + '" role="status"><span class="visually-hidden">Loading...</span> </div><div class="mensajeActualizarRecibida' + i + ' hiddenElement"></div>'
                            }
                                else {
                                _recibidaFisico = " <select  onchange='ChangeRecibidoDeclaracion(" + arr_from_json[i].IdDeclaracion + "," + i + ")' class='form-control SelectRecibidaEnFisico" + i + "'><option value='0'>Seleccione</option><option value='false'>Electronica</option><option selected value='true'>Fisica</option></select> " + '<div class="spinner-border hiddenElement spinnerRecibida' + i + '" role="status"><span class="visually-hidden">Loading...</span> </div><div class="mensajeActualizarRecibida' + i + ' hiddenElement"></div>'
                            }
                            var _recibirFisico = "Si"
                            if (arr_from_json[i].Formulario.RecibirEnFisico == false) {
                                _recibirFisico = "No"
                            }
                            if (arr_from_json[i].Formulario.RecibirEnFisico == null) {
                                _recibirFisico = ""
                            }

                            asignarEstadoDEclaracion(arr_from_json[i].IdEstadoDeclaracion, arr_from_json[i].Formulario.RecibirEnFisico)
                            var disableselect = "";
                            if ($('#selecProceso option:selected').text().indexOf("Cerrado") > 0) {

                                disableselect = " disabled='disabled '"
                            }
                            var dateDEclaracion = new Date(arr_from_json[i].FechaDeclaracion)
                            dateDEclaracion = AddZeroDate(dateDEclaracion.getDate())+ "/" + AddZeroDate((dateDEclaracion.getMonth() + 1)) + "/" + dateDEclaracion.getFullYear()

                            if (dateDEclaracion == "31/12/1969" || dateDEclaracion == "01/01/1970") {
                                dateDEclaracion = "-"
                            }
                            t.row.add([
                                '<a    class="tableButton tableButtonEdit "' + ' " onclick="InitComments(' + arr_from_json[i].IdDeclaracion + ')" > <i class="fa fa-comment"></i></a > |' +
                                '<a href="/Administracion/Declaraciones/Details/' + arr_from_json[i].IdDeclaracion + '"   class="tableButton tableButtonSearch ' + (arr_from_json[i].IdEstadoDeclaracion == 1 ? "disabled" : "") + '"><i class="fa fa-search"></i></a>',
                                arr_from_json[i].Funcionario.Nombres + " " + arr_from_json[i].Funcionario.Apellidos,
                           
                                arr_from_json[i].Funcionario.Email,
                                arr_from_json[i].Cargo,
                                arr_from_json[i].Funcionario.UnidadOrganizacional,
                                arr_from_json[i].Formulario.Titulo,
                                "<div class='filterTable'>" + arr_from_json[i].EstadoDeclaracion.NombreEstadoDeclaracion + "</div>" +
                                "<select " + disableselect+" onchange='ChangeEstadoDeclaracion(" + arr_from_json[i].IdDeclaracion + "," + arr_from_json[i].IdEstadoDeclaracion + "," + i + ")' class='form-control' id='EstadoSolicitud" + i + "'>" +
                                EstadoDeclaracionOptions
                                + "</select>" +
                                '<div class="spinner-border hiddenElement spinnerES' + i + '" role="status"><span class="visually-hidden">Loading...</span> </div><div class="mensajeActualizarEstado' + i + ' hiddenElement"></div>'
                                ,
                                dateDEclaracion,
                                _recibirFisico,
                                _recibidaFisico
                            ])
                         
                            $('#EstadoSolicitud' + i).val(arr_from_json[i].IdEstadoDeclaracion)
                     
                          
                        }
                    }
                }
                catch (e) {
                    console.log(e);
                }
        
                t.columns.adjust().draw();
                $("#loader").removeClass("loading")
            });
        }
  
    }

}


function SearchFuncionariosByProceso() {
    var t = $("#formularioTable").DataTable();
    t.rows('tr').remove().draw()
    if ($('#selecProceso').val() > 0) {
        $("#loader").removeClass("loading")
        $("#loader").addClass("loading")
        $('#Idfuncionario option').remove()
        $('#Idfuncionario').append('<option value="0"> Seleccione</option>')
        $('#Idfuncionario').append('<option value="-1">Todos</option>')
        $.getJSON('/Administracion/Declaraciones/GetFuncionariosbyProcess?IdProceso=' + $('#selecProceso').val()
            , function (data) {
             
              
                if (data !== false) {
                    try {
                        $.each(JSON.parse(data.result), function (i, Funcionario) {
                            $('#Idfuncionario').append('<option value="' + Funcionario.IdFuncionarioNavigation.IdFuncionario + '">' + Funcionario.IdFuncionarioNavigation.Nombres + " " + Funcionario.IdFuncionarioNavigation.Apellidos + " - " + Funcionario.IdFuncionarioNavigation.Email + '</option>')
                        });
                    }
                    catch (e) {
                        console.log(e);
                    }
                }
                $("#loader").removeClass("loading")
            });
    }

}


/*
 Script para obtener  los paises dada la ciudad
 */
function GetEstadoDeclaraciones() {
    var dfr = $.Deferred();
    $.getJSON('/Administracion/Declaraciones/GetEstadoDeclaraciones', function (data) {
        $.each(data, function (i, EstadoDeclaracion) {
        
            EstadoDeclaracionOptionsArray.push($('<option>', { value: 0, text: "Seleccione" }))
            //EstadoDeclaracionOptions += '<option value="0">Seleccione</option>';
            if (EstadoDeclaracion.length > 0) {
                for (var i = 0; i < EstadoDeclaracion.length; i++) {
                    //EstadoDeclaracionOptions += '<option value="' + EstadoDeclaracion[i].idEstado + '">' + EstadoDeclaracion[i].nombreEstadoDeclaracion + '</option>';
                    EstadoDeclaracionOptionsArray.push($('<option>', { value: EstadoDeclaracion[i].idEstado, text: EstadoDeclaracion[i].nombreEstadoDeclaracion }))
                }
            }
        });
    });
    return dfr.promise();
}


function asignarEstadoDEclaracion(idEstado, recibirEnFisicoFormulario) {
    _arrayTemporal = EstadoDeclaracionOptionsArray;
    _arrayTemporal[idEstado].selected = true;
    EstadoDeclaracionOptions = ""
    
    $.each(EstadoDeclaracionOptionsArray, function (i, key) {
        var disabled = "";
        if (key[0].text =="Diligenciada"  ) {
            disabled = "disabled"
        }
        if (key[0].text == "Recibida" && recibirEnFisicoFormulario==false ) {
            disabled = "disabled"
        }

       

        if (idEstado !== 1 && key[0].text == "Dispensada") {
            disabled = "disabled"
        }

        if (i == idEstado) {
            EstadoDeclaracionOptions += '<option ' + disabled+' selected value="' + key[0].value + '">' + key[0].text + '</option>'
        } else {
            EstadoDeclaracionOptions += '<option ' + disabled +' value="' + key[0].value + '">' + key[0].text + '</option>'
        }
    });
    return EstadoDeclaracionOptions;

}

function ChangeEstadoDeclaracion(IdDeclaracion, IdEstadoDeclaracion, row) {
    if (IdEstadoDeclaracion !== 1 && $('#EstadoSolicitud' + row).val() == 4) {
        alert("Solo puede dispersar una declaración que se encuentre en el estado Por diligenciar.")
        CancelarCambioEstadoDeclaracion()
    } else {

        


        rowCambiandoEstado = row;
        idEstadoInicial = IdEstadoDeclaracion;
        idDeclaracionSeleccionada = IdDeclaracion;
        $('.spinnerES' + row).removeClass('hiddenElement')
        $(".aceptarModalEstadoDeclaracion").modal("show");
        $('.mensajeActualizarEstado' + row).removeClass("hiddenElement").text('Actualizando')
    }

}
var idDeclaracionObservacion;
function InitComments(IdDeclaracion) {
    $("#loader").removeClass("loading")
    $("#loader").addClass("loading")
    idDeclaracionObservacion = IdDeclaracion;
    GetComentariosDeclaracion();
    $(".ModalObservaciones").modal("show");
}
function CancelarComentario() {
    $(".ModalObservaciones").modal("hide");
    CleanComentarioControls()
}

function GuardarComentario() {
    var data = { "Observaciones": $("#NewComment").val(), "idDeclaracion": idDeclaracionObservacion };

    if ($("#NewComment").val() !== "") {
        $.ajax({
            url: '/Administracion/Declaraciones/CreateNewCommentDeclaracion?idDeclaracion=' + idDeclaracionObservacion,
            method: 'POST',
            headers: { 'Accept': 'application/json', "Content-Type": "application/json" },
            contentType: 'application/json',
            dataType: 'json',
            data: JSON.stringify(data),
            success: function (response) {
                if (response.result == true) {
                    CleanComentarioControls()
                    $(".ModalObservaciones").modal("hide");
                } else {
                    alert('Existen problemas para registrar su observación. ');
                }
            },
            failure: function (response) {
                alert('Existen problemas para registrar su observación. ');
            },
            error: function (response) {
                alert('Existen problemas para registrar su observación. ');
            }
        });
    } else {

        alert("Debe incluir una observación")
        $("#loader").removeClass("loading")
    }
   


  
}

function GetComentariosDeclaracion() {
    $(".TableHistory tbody tr").remove()
        $.ajax({
            url: '/Administracion/Declaraciones/GetComentariosDeclaracion?idDeclaracion=' + idDeclaracionObservacion,
            method: 'POST',
            headers: { 'Accept': 'application/json', "Content-Type": "application/json" },
            contentType: 'application/json',
            dataType: 'json',
            success: function (response) {
                $("#loader").removeClass("loading")
                console.log(response.result)

                if (response.result !== false) {

                    if (response.result == null) {

                    } else {
                        var ComentariosSplit = response.result.split("#L#")

                        for (var i = 1; i < ComentariosSplit.length; i++) {
                            var item = ComentariosSplit[i].split("#C#");
                            $(".TableHistory tbody ").append(" <tr><td>" + item[0] + "</td><td>" + item[1] + "</td></tr>")
                        }
                    } 
                   
                }
               
            },
            failure: function (response) {
                alert('Existen problemas para registrar su observación. ');
                $("#loader").removeClass("loading")
            },
            error: function (response) {
                alert('Existen problemas para registrar su observación. ');
                $("#loader").removeClass("loading")
            }
        });
   



}

function CleanComentarioControls() {

    $("#NewComment").val('');
    $('.TableHistory tbody tr').remove();
}

function ajustarEstadosOption(idSelectEstado) {

    $('#EstadoSolicitud' + rowCambiandoEstado + ' option').each(function (index) {
        var disabled = "";
        if ($('#EstadoSolicitud' + rowCambiandoEstado).val() != 1 && $(this).text() == "Dispensada") {
            $(this).attr("disabled", "disabled")
        }

        if ($(this).text() == "Diligenciada") {
            $(this).attr("disabled", "disabled")
        }

        if ($(this).text() == "Recibida" && $('#SelectRecibidaEnFisico0' + rowCambiandoEstado).val() == false) {
            $(this).attr("disabled", "disabled")
        }

        if ($('#EstadoSolicitud' + rowCambiandoEstado).val() == 1 && $(this).text() == "Dispensada") {
            $(this).removeAttr("disabled")
        }
        console.log($(this).attr("disabled") + ":" + $(this).val() + ": " + $(this).text());
    });
}
function AceptarCambioEstadoDeclaracion() {
    $(".aceptarModalEstadoDeclaracion").modal("hide");
    $.ajax({
        url: '/Administracion/Declaraciones/ActualizarEstadoDeclaracion?idDeclaracion=' + idDeclaracionSeleccionada + "&idEstadoDeclaracion=" + $('#EstadoSolicitud' + rowCambiandoEstado).val(),
        method: 'POST',
        headers: { 'Accept': 'application/json', "Content-Type": "application/json" },
        contentType: 'application/json',
        dataType: 'json',
        success: function (response) {
            if (response.result == true) {
                $('#EstadoSolicitud' + rowCambiandoEstado).attr("onchange", "ChangeEstadoDeclaracion(" + idDeclaracionSeleccionada + ", " + $('#EstadoSolicitud' + rowCambiandoEstado).val() + "," + rowCambiandoEstado + ")")
                OcultarMensaje(rowCambiandoEstado, 'Actualizado', 'spinnerES', 'mensajeActualizarEstado')


                ajustarEstadosOption(rowCambiandoEstado)

            } else {
                OcultarMensaje(rowCambiandoEstado, 'Cancelado', 'spinnerES', 'mensajeActualizarEstado')
            }
        },
        failure: function (response) {
            OcultarMensaje(rowCambiandoEstado, 'Cancelado', 'spinnerES', 'mensajeActualizarEstado')
        },
        error: function (response) {
            OcultarMensaje(rowCambiandoEstado, 'Cancelado', 'spinnerES', 'mensajeActualizarEstado')
        }
    });

}

function CancelarCambioEstadoDeclaracion() {
    $('#EstadoSolicitud' + rowCambiandoEstado).val(idEstadoInicial);
    $(".aceptarModalEstadoDeclaracion").modal("hide");
    OcultarMensaje(rowCambiandoEstado, 'Cancelado', 'spinnerES', 'mensajeActualizarEstado')
}


function OcultarMensaje(rowI, Mensaje, claseSpinner, ClaseMensaje) {
    $('.' + claseSpinner + rowI).addClass('hiddenElement')
    $('.' + ClaseMensaje + rowI).text(Mensaje)
    setTimeout(function () { $('.' + ClaseMensaje + rowI).addClass("hiddenElement") }, tiempoParaAlerta);

}


function ChangeRecibidoDeclaracion(IdDeclaracion, row) {
    $('.spinnerRecibida' + row).removeClass('hiddenElement')
    $('.mensajeActualizarRecibida' + row).removeClass("hiddenElement").text('Actualizando')
    rowCambiandoEstado = row;
    idDeclaracionSeleccionada = IdDeclaracion;
    ActualizarRecibidaEnFisicoDeclaracion(IdDeclaracion, rowCambiandoEstado)

}



function ActualizarRecibidaEnFisicoDeclaracion(IdDeclaracion, rowCambiandoRecibida) {
    $.ajax({
        url: '/Administracion/Declaraciones/ActualizarRecibidaEnFisicoDeclaracion?idDeclaracion=' + idDeclaracionSeleccionada + "&recibidaEnFisico=" + $('.SelectRecibidaEnFisico' + rowCambiandoRecibida).val(),
        method: 'POST',
        headers: { 'Accept': 'application/json', "Content-Type": "application/json" },
        contentType: 'application/json',
        dataType: 'json',
        success: function (response) {
            if (response.result == true) {
                OcultarMensaje(rowCambiandoRecibida, 'Actualizado', 'spinnerRecibida', 'mensajeActualizarRecibida')
            } else {
                OcultarMensaje(rowCambiandoRecibida, 'Cancelado', 'spinnerRecibida', 'mensajeActualizarRecibida')
            }
        },
        failure: function (response) {
            OcultarMensaje(rowCambiandoRecibida, 'Cancelado', 'spinnerRecibida', 'mensajeActualizarRecibida')
        },
        error: function (response) {
            OcultarMensaje(rowCambiandoRecibida, 'Cancelado', 'spinnerRecibida', 'mensajeActualizarRecibida')
        }
    });


}





