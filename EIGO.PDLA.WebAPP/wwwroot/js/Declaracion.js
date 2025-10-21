var iRowCnt = 0; // conteo de elementos agregados a la tabla. Para no repeter ID
var selectorCiudad = "#IdCiudad";//Lista desplegable de ciudad
var IdParentescoFuncionario = 0;
var ParentescoOptions = "";
var PaisesOptions = "";
var NombresOptions = "";
var MesesOptions = "";
MesesOptions += '<option value="01">Enero</option>';
MesesOptions += '<option value="02">Febrero</option>';
MesesOptions += '<option value="03">Marzo</option>';
MesesOptions += '<option value="04">Abril</option>';
MesesOptions += '<option value="05">Mayo</option>';
MesesOptions += '<option value="06">Junio</option>';
MesesOptions += '<option value="07">Julio</option>';
MesesOptions += '<option value="08">Agosto</option>';
MesesOptions += '<option value="09">Septiembre</option>';
MesesOptions += '<option value="10">Octubre</option>';
MesesOptions += '<option value="11">Noviembre</option>';
MesesOptions += '<option value="12">Diciembre</option>';
var CatalogoCargosOptions = "";
var CatalogoAniosOptions = "";

$(document).ready(function () {

    bloquearAgregar()
    $("#loader").removeClass("loading")
    $("#loader").addClass("loading")
    reiniciarSelect(selectorCiudad);
    reiniciarDropdownNacionalidad()
    $('.tablaBlue ').on('click', '.remove', function () {
        $(this).parents('tr').remove();
        ValidarDisclaimer();
    });





    $('button[data-bs-toggle="tab"]').on('shown.bs.tab', function (e) {
        var target = $(e.target).attr("data-bs-target") // activated tab
        alert(target);
    });

    


    $('#DilegenciarDeclaracion').on('click', function () {
        //window.open("/Administracion/Declaraciones/DetailsPreview/" + $('#IdFormulario').val(), '_blank')
        $("#loader").removeClass("loading")
        $("#loader").addClass("loading")
        $(".ModalMensaje").modal("hide");

    });


    if ($("#bConflictoInteres").val() == "true") {
        document.getElementById('respuestaSi').checked = true
        $(".PorqueConflicto").show()
    } else {
        document.getElementById('respuestaNo').checked = true
        $(".PorqueConflicto").hide()
        $("#sJustificacion").val("")
    }
});
function mostrarModal() {


        $(".ModalMensaje").modal("show");
    
}

function cancelarModal() {


    $(".ModalMensaje").modal("hide");

}
$(window).on("load",async function () {
    var cambio = false;
    setTimeout(function () {
        $('.tablaBlue ').DataTable().destroy();
        $('.tablaBlue ').DataTable().destroy();
        GetCiudadByPais()
        GetParentesco()
    }, 3000);



            do {
        

                    if (IdParentescoFuncionario !== 0) {
            
                        GetFamiliaresByFuncionario("init");
                        GetPaises("init");
                        GetCatalogoCargos("init");
                        GetCatalogoAnios("init");
                        desbloquearAgregar()
                        cambio = true
                }
                console.log("esperando")
                await delay(2000);
            } while (cambio == false);

     


    

  
})

const delay = ms => new Promise(res => setTimeout(res, ms));
function imprimir() {
   /* */window.print()
}
// validar si en inversiones las tablas tienen todos los campos correctos.
function ValideTableInversiones() {
    var _validateTable = true;
    if ($('.NewItem ').length > 0) {
        for (var i = 0; i <= iRowCnt; i++) {
            if ($('.NewItem #NombreCompleto' + i + ' option:selected').val() == '0' ||
                ($('.NewItem #NombreCompleto' + i + ' option:selected').val() == '-1' && $('.NewItem #NuevaParticipacionNombre' + i).val()=="") ||
                $('.NewItem #ParentescoSelect' + i + ' option:selected').val() == '0' ||
                $('.NewItem #PaisSelect' + i + ' option:selected').val() == '0' ||
                $('.NewItem #NombreEmpresa' + i).val() == '' ||
                ($('.NewItem #PorcentajeParticipacion' + i).val() == '' && $('.NewItem #PorcentajeParticipacion' + i).parent().parent().parent().parent().attr("id") == 'DataTables_Table_0') ||
                ($('.NewItem #CargoParticipacion' + i).val() == '' && $('.NewItem #CargoParticipacion' + i).parent().parent().parent().parent().attr("id") == 'DataTables_Table_2')) {
                _validateTable = false;
            }
        }
    }
    return _validateTable;
}


function eliminarNacionalidadFormulario(row, idpais) {
    $(" .bodyNacionalidad tr.item" + row + " ").addClass("hiddenElement");
    $(" .bodyNacionalidad tr.item" + row + " input").addClass("deleteElemen");
    $('#selecNacionalidad option[value="' + idpais + '"]').removeClass('hiddenElement')

    ValidarDisclaimer()
}
/*
 Script para agregar las secciones a la tabla de disclaimer del formulario
 */
function AddRowNacionalidad() {
    //Add your html fields using iRowCnt as the index
    if ($('#selecNacionalidad option:selected').val() == 0) {
        alert("Debe seleccionar una Nacionalidad");
    } else {
        //var t = $(".TableNacionalidad").DataTable();
        //t.row.add([
        //    '<a class="btn btn-link remove" onclick="eliminarNacionalidadFormulario(' + iRowCnt + ',' + $('#selecNacionalidad option:selected').val() + ')"> Eliminar</a>',
        //    $('#selecNacionalidad option:selected').text(),
        //    ' <input type="hidden" class="text-box single-line " name="IdNacionalidad[' + iRowCnt + ']" value="' + $('#selecNacionalidad option:selected').val() + '"  /> '
        //]).draw(false);
        var item = "item" + ($(".bodyNacionalidad tr").length)
        $(".bodyNacionalidad").append(
            '<tr class="' + item + '"><td><a class="btn btn-link remove" onclick="eliminarNacionalidadFormulario(' + $(".bodyNacionalidad tr").length + ','+ $('#selecNacionalidad option:selected').val()+')"> Eliminar</a>'
            +"</td><td>" + $('#selecNacionalidad option:selected').text() + "</td><td>"+
            ' <input type="hidden" class="text-box single-line " name="IdNacionalidad[' + iRowCnt + ']" value="' + $('#selecNacionalidad option:selected').val() + '"  /> </td></tr>')
        $('#selecNacionalidad option[value="' + $('#selecNacionalidad option:selected').val() + '"]').attr('class', 'hiddenElement')
        $('#selecNacionalidad').val(0);
        iRowCnt++;
        ValidarDisclaimer()
    }
}


function OcultarJustificacion(estado) {

    if (estado == 1) {
        $(".bConflictoInteres").val(true);
        $(".PorqueConflicto").show()
    } else {
        $(".bConflictoInteres").val(false);
        $(".PorqueConflicto").hide()
        $("#sJustificacion").val("")
    }
}
/*
 Script para ajustar el dropdonw de disclaimer
 */
function reiniciarDropdownNacionalidad() {
    $("[name*=NacionalidadEdit]").each(function (index) {
    
        $('#selecNacionalidad option[value="' + $(this).val() + '"]').attr('class', 'hiddenElement')
    });
}
/*
 Script para agregar las secciones a la tabla de disclaimer del formulario
 */
function AddRow(idTable) {
    var counter = 1;
    $(idTable).append(
        '<tr class="NewItem"><td> <input type="Text" class="form-control   " name="IdParticipacion[' + iRowCnt + ']" /></td>' +
        '<td><a class="btn btn-link remove" > Eliminar</a></td>' +
        '<td><select name="NombreCompleto[' + iRowCnt + ']" id="NombreCompleto' + iRowCnt + '" onchange="SetParentesco(this,' + iRowCnt + ')" class="form-select" aria-label="Default select " >' +
        '  <option selected value="0">Seleccione</option>' + NombresOptions +
        '</select>' +
        '<div class="row"><div class="col-8"><input onchange="ValidarDisclaimer() " type = "Text" class= "form-control  col single-line hiddenElement " id="NuevaParticipacionNombre' + iRowCnt + '" name="NuevaParticipacionNombre[' + iRowCnt + ']" /></div>' +
        '<div class="col-4"><a id="btnAgregarFamiliarParentesco' + iRowCnt + '" class="hiddenElement col" onclick="cancelarCreacionFamiliar(' + iRowCnt + ')">Cancelar</a></div></div></td>' +
        '<td><select onchange="ValidarDisclaimer()" name="Parentesco[' + iRowCnt + ']" id="ParentescoSelect' + iRowCnt + '" class="form-select valid" aria-label="Default select " >' +
        '  <option selected value="0">Seleccione</option>' + ParentescoOptions +
        '</select></td>' +
        ' <td>   <input type="Text" onchange="ValidarDisclaimer() " class="form-control  " maxlength="100" id="NombreEmpresa' + iRowCnt + '" name="NombreEmpresa[' + iRowCnt + ']"   /> </td>' +
        '<td><select onchange="ValidarDisclaimer() " name="Pais[' + iRowCnt + ']" id="PaisSelect' + iRowCnt + '" class="form-select" aria-label="Default select " >' +
        '  <option selected value="0"> Seleccione</option>' + PaisesOptions +
        '</select></td>' +
        ' <td><input type="number"  min="0"  max="100" onKeyUp="if(this.value>100){this.value=\'100\';}else if(this.value<0){this.value=\'0\';}"  class="form-control  "  onchange="ValidarDisclaimer() " id="PorcentajeParticipacion' + iRowCnt + '" name="PorcentajeParticipacion[' + iRowCnt + ']" /></td>' +
        '<td><input type="Text" class="form-control   " maxlength="50" onchange="ValidarDisclaimer() " id="CargoParticipacion' + iRowCnt + '"  name="CargoParticipacion[' + iRowCnt + ']" /></td>' +
        '<td><select onchange="ValidarDisclaimer(); LlenarCargo(EntidadesParticipacionSelect' + iRowCnt + ')" name="EntidadesParticipacion[' + iRowCnt + ']" id="EntidadesParticipacionSelect' + iRowCnt + '" class="form-select" aria-label="Default select " >' +
        '  <option selected value="0"> Seleccione</option>' + CatalogoCargosOptions +
        '</select>' +
        '<input hidden="hidden" type="Text" class="form-control   " maxlength="50" id="bOtro' + iRowCnt + '"  name="bOtro[' + iRowCnt + ']" />' +
        '<input hidden="hidden" type="Text" class="form-control   " maxlength="50" id="nTipoCargo' + iRowCnt + '"  name="nTipoCargo[' + iRowCnt + ']" />' +
        '<input onblur="LlenarCargoOtro(this)" hidden="hidden" type="Text" class="form-control   " maxlength="50" id="CargoParticipacionTexto' + iRowCnt + '"  name="CargoParticipacionTexto[' + iRowCnt + ']" />' +
        '</td >' +
        '<td class="mes__anio__container">' +
        '<select onchange="ValidarDisclaimer() " name="Mes[' + iRowCnt + ']" id="MesSelect' + iRowCnt + '" class="form-select" aria-label="Default select " >' +
        '  <option selected value="0"> Mes</option>' + MesesOptions +
        '</select>' +
        '<select onchange="ValidarDisclaimer() " name="Anio[' + iRowCnt + ']" id="AnioSelect' + iRowCnt + '" class="form-select" aria-label="Default select " >' +
        '  <option selected value="0"> Año</option>' + CatalogoAniosOptions +
        '</select>' +
        '</td >'
        + '</tr > '
    )
    document.getElementById("ParentescoSelect" + iRowCnt).selectedIndex = 0;
    document.getElementById("PaisSelect" + iRowCnt).selectedIndex = 0;
    document.getElementById("NombreCompleto" + iRowCnt).selectedIndex = 0;
    ValidarDisclaimer();
    iRowCnt++;
}
/*
 Script validar si el usuario marca disclaimers de participacion  
 */
function ValidarParticipacion(element, Seccion, IdTable) {
    if (element.checked == true) {
        var Dialog = confirm("Declarara no tener participaciones. Si tiene participaciones guardadas seran eliminadas. Desea continuar");
        if (Dialog == true) {
            $("#Agregar" + Seccion).attr('class', 'btn btn-primary m-2 disabled');
            if ($('.body' + Seccion + ' tr').length > 0) {
                var table = $('#' + IdTable + " tbody tr").remove();
                $('#' + IdTable + '_wrapper').addClass('hiddenElement');
            }
        } else {
            $("#Agregar" + Seccion).attr('class', 'btn btn-primary m-2');
            $("#Check" + Seccion).prop("checked", false);
            $('#' + IdTable + '_wrapper').removeClass('hiddenElement');
        }
    } else {
        $('#' + IdTable + '_wrapper').removeClass('hiddenElement');
        $("#Agregar" + Seccion).attr('class', 'btn btn-primary m-2');
    }
}
/*
 Script para obtener la ciudad de los paises
 */
function GetCiudadByPais() {
    var dfr = $.Deferred();
    $.getJSON('/Administracion/Ciudad/GetCiudadByPais?IdPais=' + $('#IdPais').val(), function (data) {
        $.each(data, function (i, city) {
      
            reiniciarSelect(selectorCiudad)
            if (city.length > 0) {
                for (var i = 0; i < city.length; i++) {
                    $(selectorCiudad).append($('<option>').val(city[i].idCiudad).text(city[i].nombreCiudad));
                }
            }
        });
        $("#IdCiudad").val($('input#IdCiudad').val())
        ValidarDisclaimer();
    });
    return dfr.promise();
}
/*
 Script para obtener  los paises dada la ciudad
 */
function GetPaises(idPaisSelector) {
    var dfr = $.Deferred();
    $.getJSON('/Administracion/Pais/GetPaises', function (data) {
        $.each(data, function (i, Paises) {
        
            try {
                reiniciarSelect(idPaisSelector)
                if (Paises.length > 0) {
                    for (var i = 0; i < Paises.length; i++) {
                        PaisesOptions += '<option value="' + Paises[i].idPais + '">' + Paises[i].nombrePais + '</option>';
                    }
                }
            }
            catch (e) {
                console.log(e);
            }
            $("#loader").removeClass("loading")
            ValidarDisclaimer();
        });
    });
    return dfr.promise();
}
/*
 Script para obtener el listado del catalogo Cargos
 */
function GetCatalogoCargos(idCatalogoCargosSelector) {
    var dfr = $.Deferred();
    $.getJSON('/Administracion/CatalogoCargos/GetCatalogoCargos', function (data) {
        $.each(data, function (i, Cargos) {

            try {
                reiniciarSelect(idCatalogoCargosSelector)
                if (Cargos.length > 0) {
                    for (var i = 0; i < Cargos.length; i++) {
                        CatalogoCargosOptions += '<option value="' + Cargos[i].id + '">' + Cargos[i].opcion + '</option>';
                    }
                }
            }
            catch (e) {
                console.log(e);
            }
            $("#loader").removeClass("loading")
            ValidarDisclaimer();
        });
    });
    return dfr.promise();
}
/*
 Script para obtener el listado del catalogo Anios
 */
function GetCatalogoAnios(idCatalogoAniosSelector) {
    var dfr = $.Deferred();
    $.getJSON('/Administracion/CatalogoAnios/GetCatalogoAnios', function (data) {
        $.each(data, function (i, Anios) {

            try {
                reiniciarSelect(idCatalogoAniosSelector)
                if (Anios.length > 0) {
                    for (var i = 0; i < Anios.length; i++) {
                        CatalogoAniosOptions += '<option value="' + Anios[i].id + '">' + Anios[i].anio + '</option>';
                    }
                }
            }
            catch (e) {
                console.log(e);
            }
            $("#loader").removeClass("loading")
            ValidarDisclaimer();
        });
    });
    return dfr.promise();
}
/*
 Script para obtener  los parentesco
 */
function GetParentesco(idParentescoSelector) {
    var dfr = $.Deferred();
    $.getJSON('/Administracion/Parentesco/GetParentesco', function (data) {
        $.each(data, function (i, Parentesco) {
     
            reiniciarSelect(idParentescoSelector)
            if (Parentesco.length > 0) {
                for (var i = 0; i < Parentesco.length; i++) {
                    if (Parentesco[i].nombreParentesco == "Persona funcionaria") {
                        IdParentescoFuncionario = Parentesco[i].idParentesco;
                    }
                    ParentescoOptions += '<option value="' + Parentesco[i].idParentesco + '">' + Parentesco[i].nombreParentesco + '</option>';
                }
            }

        });
    });
    return dfr.promise();
}

function desbloquearAgregar() {
    if ($('#AgregarParticipacion').length > 0) {
        // Exists.

        $('#AgregarParticipacion').removeClass("disabled");
        $('#AgregarResponsabilidad').removeClass("disabled");
    }
   
}

function bloquearAgregar() {
    if ($('#AgregarParticipacion').length > 0) {
        // Exists.

        $('#AgregarParticipacion').addClass("disabled");
        $('#AgregarResponsabilidad').addClass("disabled");
    }

}
/*
 Script para obtener  los familiares
 */
function GetFamiliaresByFuncionario(idFamiliarSelector) {
    var dfr = $.Deferred();
    $.getJSON('/Administracion/FAmiliar/GetFamiliaresByFuncionario?idFuncionario=' + $('#IdFuncionario').val(), function (data) {
        $.each(data, function (i, familiar) {
          
            reiniciarSelect(idFamiliarSelector)
            NombresOptions += '<option data-idParentesco="' + IdParentescoFuncionario + '" value="' + $('#Funcionario_Nombres').val() + " " + $('#Funcionario_Apellidos').val() + '">' + $('#Funcionario_Nombres').val() + " " + $('#Funcionario_Apellidos').val() + '</option>';
            if (familiar.length > 0) {
                for (var i = 0; i < familiar.length; i++) {
                    NombresOptions += '<option data-idParentesco="' + familiar[i].idParentesco + '" value="' + familiar[i].nombreFamiliar + " " + familiar[i].apellidoFamiliar + '">' + familiar[i].nombreFamiliar + " " + familiar[i].apellidoFamiliar + '</option>';
                }
            }
            NombresOptions += '<option data-idParentesco="-1" value="-1">' + "Agregar Nuevo" + '</option>';
           
        });
    });
    return dfr.promise();
}
/*
 Script para actualizar parentesco de un familiar existente
 */
function SetParentesco(element, idRow) {
    if ($("#NombreCompleto" + idRow + " option:selected").val() == "-1") {
        $('#NuevaParticipacionNombre' + idRow).removeClass('hiddenElement');
        $('#btnAgregarFamiliarParentesco' + idRow).removeClass('hiddenElement');
        $('#NombreCompleto' + idRow).addClass('hiddenElement');
    } else {
        if ($("#NombreCompleto" + idRow + " option:selected").val() !== "0") {
            document.getElementById("ParentescoSelect" + idRow).selectedIndex = $("#" + element.id + " option:selected").attr("data-idparentesco");
        }
        ValidarDisclaimer()
    }
}
function EliminarDeclaracion(IdParticipacion, IdDeclaracion, IdPais, Idparentesco) {
    $.getJSON('/Administracion/Declaraciones/DeletParticipacion?IdParticipacion=' + IdParticipacion + "&IdDeclaracion=" + IdDeclaracion + "&IdPais=" + IdPais + "&Idparentesco=" + Idparentesco
        , function (data) {
            $.each(data, function (i, Eliminado) {
         
                if (Eliminado == true) {
                    alert('eliminado.')
                } else {
                    alert('No es posible eliminar la relación en estos momentos.')
                }
            });
        });
}
/*
 Script para actualizar parentesco de un familiar existente
 */
function cancelarCreacionFamiliar(idRow) {
    $('#NuevaParticipacionNombre' + idRow).addClass('hiddenElement').val("").text("");
    $('#btnAgregarFamiliarParentesco' + idRow).addClass('hiddenElement');
    $('#NombreCompleto' + idRow).removeClass('hiddenElement').val(0);
}
/*
 Script para reiniciar las secciones conservando las que no seran bloqueads
 */
function reiniciarSelect(SelectorID) {
    $(SelectorID + " option").remove();
    $(SelectorID).append($('<option>').val(0).text("Seleccione"));
    $(SelectorID).val(0)
}

function LlenarCargoOtro(element) {
    var valorCargoText = $('#' + element.id).val();
    var EntidadParticipacionRowId = element.id.replace("CargoParticipacionTexto", "");
    var cargoParticipacion = $('#CargoParticipacion' + EntidadParticipacionRowId);
    cargoParticipacion.val(valorCargoText);
}
function LlenarCargo(element) {
    var varElementId = $('#' + element.id);
    var optionSelected = element.options[element.selectedIndex].text;
    var EntidadParticipacionRowId = element.id.replace("EntidadesParticipacionSelect", "");
    var cargoParticipacion = $('#CargoParticipacion' + EntidadParticipacionRowId);
    var ntipoCargo = $('#nTipoCargo' + EntidadParticipacionRowId);
    ntipoCargo.val(1); // el valor ntipoCargo 1 es para las tablas con el campo Cargo
    var cargoParticipacionTexto = $('#CargoParticipacionTexto' + EntidadParticipacionRowId);

    // obtiene td padre
    var parentTd = $(element).closest('td');
    // obtiene siguiente td
    var nextTd = parentTd.next();

    // en caso de haber elegido Otro (a) se mostrara el campo de texto
    if (optionSelected == "Otro (a)") {
        var bOtro = $('#bOtro' + EntidadParticipacionRowId)
        bOtro.val(true);
        // agrega clase para mejora visual cuando el campo otro esta visible
        nextTd.addClass('mes__anio__ShowingOtro');
        cargoParticipacionTexto.attr("hidden", false);
    } else {
        var bOtro = $('#bOtro' + EntidadParticipacionRowId)
        bOtro.val(false);
        // remueve clase para mejora visual cuando el campo otro no esta visible
        nextTd.removeClass('mes__anio__ShowingOtro');
        cargoParticipacionTexto.attr("hidden", true);
        cargoParticipacion.val(optionSelected);
    }
    
}
// Validar si las condciones estan correctas para diligenciar es llamado en los checkbox, en los addrow y los deletes.
function ValidarDisclaimer() {
    var validateTable = true;
    //Verificamos que sea formulario nacionalidades
    if ($('#Formulario_IdTipoDeclaracion').val() == "2") {
        //Verificamos que tenga por lo menos una nacionalidad
        if ($('.dataTables_length').length>=0) {
            if ($('#TableNacionalidad tbody tr').length- $('.TableNacionalidad tr.hiddenElement').length  > 0) {
                validateTable = true;
            } else {
                validateTable = false;
            }
        }
    }
    //validamos que sea inversiones
    // para inversiones: si los check de las tablas estan marcados se deben tomar en cuenta
    var acumValidarCondiciones = 0;
    if ($('#Formulario_IdTipoDeclaracion').val() == "1") {
        if ($('#CheckParticipacion').prop('checked') == false && $('#DataTables_Table_0 tbody tr').length > 0) {
            acumValidarCondiciones = 1;
        }
        if ($('#CheckEntidadesParticipacion').prop('checked') == false && $('#DataTables_Table_1 tbody tr').length > 0) {
            acumValidarCondiciones += 1;
        }
        if ($('#CheckResponsabilidad').prop('checked') == false && $('#DataTables_Table_2 tbody tr').length > 0) {
            acumValidarCondiciones += 1;
        }
       
            validateTable = ValideTableInversiones();
        
    }
    //validamos si la ciudad estaa vacia
    if ($("#IdCiudad").val() == "0" || $("#IdCiudad").val() == "" || $("#IdCiudad").val() == null || $("#IdPais").val() == "0" || $("#IdPais").val() == "" || $("#IdPais").val() == null ){
        validateTable = false;
    }

    //si es de seguridad validamos el textarea
    if (($("#Formulario_IdTipoDeclaracion").val() == "3" && $('Textarea#Cargo').val() == "")
        || ($("#Formulario_IdTipoDeclaracion").val() == "2" && $('Textarea#Cargo').val() == "")
        || ($("#Formulario_IdTipoDeclaracion").val() == "4" && $('Textarea#Cargo').val() == "")
        ) {
        validateTable = false;

    }

    // si todos los checkbox son marcados y se valido la tabla ( nacionalidad) 
    if ($('input:checkbox:checked').length == $('#CountDisclaimer').val() && validateTable == true) {
        $('#DilegenciarDeclaracion').removeAttr('disabled').removeClass('disabled').removeClass("Bloqueado")
        $('#DilegenciarDeclaracion').attr('title', 'Completar').attr('type', 'submit')
        $('#AceptarDeclaracion').removeAttr('disabled').removeClass('disabled').removeClass("Bloqueado").attr("onclick", 'mostrarModal()')
        $('#AceptarDeclaracion').attr('title', 'Completar')
    } else {
        if ($('input:checkbox:checked').length + acumValidarCondiciones == $('#CountDisclaimer').val() && validateTable == true) {
            $('#DilegenciarDeclaracion').removeAttr('disabled').removeClass('disabled').removeClass("Bloqueado")
            $('#DilegenciarDeclaracion').attr('title', 'Completar').attr('type', 'submit')    //=""
            $('#AceptarDeclaracion').removeAttr('disabled').removeClass('disabled').removeClass("Bloqueado").attr("onclick",'mostrarModal()')
            $('#AceptarDeclaracion').attr('title', 'Completar')
        } else {
          //  $('#DilegenciarDeclaracion').attr('disabled', 'disabled').addClass('disabled')
            $('#DilegenciarDeclaracion').removeAttr('type').attr('title', 'Debe completar toda la información para poder continuar').addClass("Bloqueado")

     
            if ($("#Formulario_IdTipoDeclaracion").val() == 2    ) {
                var contarItem = $(".bodyNacionalidad tr").length - $(".bodyNacionalidad tr.hiddenElement").length;

                if (contarItem<=0) {

                    $('#AceptarDeclaracion').attr("disabled", "disabled").attr('title', 'Debe completar toda la información para poder continuar').addClass("Bloqueado").attr("onclick", 'alert("Debe ingresar por lo menos una nacionalidad. Asegúrese de seleccionar un país y presionar el botón de +AGREGAR.")')
                } else {
                    $('#AceptarDeclaracion').attr("disabled", "disabled").attr('title', 'Debe completar toda la información para poder continuar').addClass("Bloqueado").attr("onclick", '')
                }
                
            } else {

                $('#AceptarDeclaracion').attr("disabled", "disabled").attr('title', 'Debe completar toda la información para poder continuar').addClass("Bloqueado").attr("onclick", '')
            }
           
        }
    }
}
//Create json Nacionalidades NacionalidadesDelete
var jsonObjDeleteNacionalidad = [];
function createJSONNacionalidad() {
    jsonObj = [];
    jsonObjDeleteNacionalidad = []
    var t = $(".TableNacionalidad").DataTable({
        language: {
            url: '/js/dataTables.es-es.json'
        }
    });
    for (var i = 0; i < t.data().length; i++) {
        var data = t.column(2).data().toArray(); /// columna con el campo input y el id de la nacionalidad
        var id = $(data[i]).val();
        var CheckClass = $(data[i]).attr("class");
        if (CheckClass !== "NacionalidadEdit" && CheckClass !== "NacionalidadEdit deleteElemen" && CheckClass !== "text-box single-line deleteElemen") {
            jsonObj.push($(data[i]).val());
        }
        if (CheckClass == "NacionalidadEdit deleteElemen") {
            jsonObjDeleteNacionalidad.push($(data[i]).val());
        }
    }
    return jsonObj;
}
function createJSONParticipacion() {
    jsonObj = [];
    var t = $("#DataTables_Table_0").DataTable({
        language: {
            url: '/js/dataTables.es-es.json'
        }
    }).draw(false);
    for (var i = 0; i < t.data().length; i++) {
        var NombreCompleto = t.column(2).data().toArray(); ///
        var Pais = t.column(5).data().toArray(); ///
        var PorcentajeParticipacion = t.column(6).data().toArray(); ///
        var NombreEmpresa = t.column(4).data().toArray(); ///
        var CargoParticipacion = t.column(7).data().toArray(); ///
        var Parentesco = t.column(3).data().toArray(); ///
        item = {}
        item["NombreCompleto"] = $(NombreCompleto[i]).find(" option:selected").val();
        item["IdPais"] = $(Pais[i]).val();
        item["PorcentajeParticipacion"] = $(PorcentajeParticipacion[i]).val();
        item["NombreEmpresa"] = $(NombreEmpresa[i]).val();
        item["CargoParticipacion"] = $(CargoParticipacion[i]).val();
        item["Parentesco"] = $(Parentesco[i]).val();
        console.log(item); jsonObj.push(item);
    }
    return jsonObj;
}
// create json Declaraciones
function createDeclaracion(values, jsonNacionalidades) {
    return declaracion = {
        declaracion: {
            IdFormulario: values["IdFormulario"],
            IdDeclaracion: values["IdDeclaracion"],
            IdEstadoDeclaracion: values["IdEstadoDeclaracion"],
            Cargo: values["Cargo"],
            IdCiudad: values["IdCiudad"],
            IdFuncionario: values["IdFuncionario"],
            UnidadOrganizacional: values["UnidadOrganizacional"] === undefined ? "" : values["UnidadOrganizacional"],
            RecibidaEnFisico: values["RecibidaEnFisico"] == "true" ? true : false,
            FechaDeclaracion: values["FechaDeclaracion"],
            ConfirmacionResponsabilidad: values["ConfirmacionResponsabilidad"] == "true" ? true : false
        },
        Nacionalidades: jsonNacionalidades,
        NacionalidadesDelete:jsonObjDeleteNacionalidad
    };
}
//Summit form 
function SummitFormDeclaraciones() {
    $(".ModalMensaje").modal("hide");
    $("#loader").removeClass("loading")
    $("#loader").addClass("loading")


    const form = document.querySelector('#FormDeclaracion');
    var jsonNacionalidades = createJSONNacionalidad();
    var formData = new FormData(form);
    var object = {};
    formData.forEach((value, key) => object[key] = value)

    const token = $("[name='__RequestVerificationToken']").val();
    var data = createDeclaracion(object, jsonNacionalidades, jsonObjDeleteNacionalidad)
    $.ajax({
        url: '/Administracion/Declaraciones/EditDeclaraciones',
        method: 'POST',
        headers: { 'RequestVerificationToken': `${token}`, 'Accept': 'application/json', "Content-Type": "application/json" },
        contentType: 'application/json',
        data: JSON.stringify(data),
        dataType: 'json',
        success: function (response) {
            if (response.result == true) {
              
                var url = window.location.href;
                if (url.toLowerCase().indexOf("administracion")) {
                    url = window.location.href.split('Edit');
                } else {
                    url = window.location.href.split('Declaraciones');
                }
                console.log("Actualizado");
                window.location.href = url[0];
            } else {
                console.log("Something went wrong");
            }
        },
        failure: function (response) {
            alert(response.responseText);
        },
        error: function (response) {
            alert(response.responseText);
        }
    });
}
function onSuccess(res) {
    console.log('success', res);
}
function onError(res) {
    console.log('success', res);
}
function SearchFuncionariosByProceso() {
    if ($('#selecProceso').val() > 0) {
        $('#Idfuncionario option').remove()
        $('#Idfuncionario').append('<option value="0"> Seleccione</option>')
        $('#Idfuncionario').append('<option value="-1">Todos</option>')
        $.getJSON('/Administracion/Declaraciones/GetFuncionariosbyProcess?IdProceso=' + $('#selecProceso').val()
            , function (data) {
          
                $.each(JSON.parse(data.result), function (i, Funcionario) {
                
                    $('#Idfuncionario').append('<option value="' + Funcionario.IdFuncionarioNavigation.IdFuncionario + '">' + Funcionario.IdFuncionarioNavigation.Nombres + " " + Funcionario.IdFuncionarioNavigation.Apellidos + " - " + Funcionario.IdFuncionarioNavigation.Email + '</option>')
                });
            });
    }
}


/*
 Script para agregar las secciones a la tabla de disclaimer del formulario
 */
function AddRowPreview(idTable) {
    var counter = 1;
    $(idTable).append(
        '<tr class="NewItem"><td> <input type="Text" class="form-control   " name="IdParticipacion[' + iRowCnt + ']" /></td>' +
        '<td></td>' +
        '<td>' +
        '<div class="row"><div class="col-12"><input onchange="ValidarDisclaimer() " type = "Text" class= "form-control  col single-line " id="NuevaParticipacionNombre' + iRowCnt + '" name="NuevaParticipacionNombre[' + iRowCnt + ']" /></div>' +
        '<div class="col-4"><a id="btnAgregarFamiliarParentesco' + iRowCnt + '" class="hiddenElement col" onclick="cancelarCreacionFamiliar(' + iRowCnt + ')">Cancelar</a></div></div></td>' +
        '<td><input type="Text" onchange="ValidarDisclaimer() " class="form-control  " id="NombreEmpresa' + iRowCnt + '" name="NombreEmpresa[' + iRowCnt + ']"   /></td>' +
        ' <td>   <input type="Text" onchange="ValidarDisclaimer() " class="form-control  " id="NombreEmpresa' + iRowCnt + '" name="NombreEmpresa[' + iRowCnt + ']"   /> </td>' +
        '<td><input type="Text" onchange="ValidarDisclaimer() " class="form-control  " id="NombreEmpresa' + iRowCnt + '" name="NombreEmpresa[' + iRowCnt + ']"   /></td>' +
        ' <td><input type="Text" class="form-control  "  onchange="ValidarDisclaimer() " id="PorcentajeParticipacion' + iRowCnt + '" name="PorcentajeParticipacion[' + iRowCnt + ']" /></td>' +
        '<td><input type="Text" class="form-control   "  id="CargoParticipacion' + iRowCnt + '"  name="CargoParticipacion[' + iRowCnt + ']" /></td></tr>'
    )
    document.getElementById("ParentescoSelect" + iRowCnt).selectedIndex = 0;
    document.getElementById("PaisSelect" + iRowCnt).selectedIndex = 0;
    document.getElementById("NombreCompleto" + iRowCnt).selectedIndex = 0;
    ValidarDisclaimer();
    iRowCnt++;
}
function ChangeCiudadSelect() {
    $('input#IdCiudad').val($('select#IdCiudad').val())
    ValidarDisclaimer()
}


/*
 Script para agregar las secciones a la tabla de disclaimer del formulario
 */
function AddRowNacionalidadPreview() {
    //Add your html fields using iRowCnt as the index
  
    var t = $(".TableNacionalidad").DataTable();
        t.row.add([
            '<a class="btn btn-link remove" onclick="eliminarNacionalidadFormulario(' + iRowCnt + ',' + $('#selecNacionalidad option:selected').val() + ')"> Eliminar</a>',
            ' <input type="Text" class="text-box single-line " name="IdNacionalidad[' + iRowCnt + ']"  /> ',
            ' <input type="hidden" class="text-box single-line " name="IdNacionalidad[' + iRowCnt + ']"  /> '
        ]).draw(false);
       
        iRowCnt++;
    
    
}


