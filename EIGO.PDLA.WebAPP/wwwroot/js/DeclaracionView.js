var iRowCnt = 0; // conteo de elementos agregados a la tabla. Para no repeter ID
var selectorCiudad = "#IdCiudad";//Lista desplegable de ciudad
var IdParentescoFuncionario = 0;
var ParentescoOptions = "";
var PaisesOptions = "";
var NombresOptions = "";
$(document).ready(function () {

  

    GetCiudadByPais()

    $(".btnAddItem").remove()
    $("input").attr('disabled', 'disabled')
    $("select").attr('disabled', 'disabled')
    $("textarea").attr('disabled', 'disabled')
    $(".addNacionalidad").remove()

    $("input").prop("checked", true)

    if ($('.TablaParticipacion ').length > 0) {
        // Exists.
        if ($('.TablaParticipacion tr').length > 1) {
            $("#CheckParticipacion").prop("checked", false)
          

        } 


    }


    if ($('.TablaResponsabilidad  ').length > 0) {
        // Exists.
        if ($('.TablaResponsabilidad  tr').length > 1) {

            $("#CheckResponsabilidad").prop("checked", false)
        }


    }


    if ($('.TablaEntidadesParticipacion   ').length > 0) {
        // Exists.
        if ($('.TablaEntidadesParticipacion   tr').length > 1) {

            $("#CheckEntidadesParticipacion").prop("checked", false)
        }


    }


    if ($("#bConflictoInteres").val() == "true") {
        document.getElementById('respuestaSi').checked = true
       

        $(".PorqueConflicto").show()
    } else {
        document.getElementById('respuestaNo').checked = true
        $(".PorqueConflicto").hide()
       
        $("#sJustificacion").val("")
    }

    

});

$(window).on("load", function () {

    $('.tablaBlue ').DataTable().destroy();
    $('.tablaBlue ').DataTable().destroy();

    $('.TableNacionalidad ').DataTable().destroy();

  
 

    $('.TablaParticipacion th:nth-child(1)').hide()
    $('.TablaParticipacion td:nth-child(1)').hide()
    $('.TablaParticipacion th:nth-child(2)').hide()
    $('.TablaParticipacion td:nth-child(2)').hide()
    $('.TablaParticipacion th:nth-child(8)').hide()
    $('.TablaParticipacion td:nth-child(8)').hide()

    $('.TablaResponsabilidad th:nth-child(1)').hide()
    $('.TablaResponsabilidad td:nth-child(1)').hide()
    $('.TablaResponsabilidad th:nth-child(2)').hide()
    $('.TablaResponsabilidad td:nth-child(2)').hide()
    $('.TablaResponsabilidad th:nth-child(7)').hide()
    $('.TablaResponsabilidad td:nth-child(7)').hide()

    $('.TableNacionalidad th:nth-child(1)').hide()
    $('.TableNacionalidad td:nth-child(1)').hide()


    $('.TablaEntidadesParticipacionEdit th:nth-child(8)').show().attr("display","block!important")
    $('.TablaEntidadesParticipacionEdit td:nth-child(8)').show().attr("display", "block!important")
    $('.TablaEntidadesParticipacionEdit th:nth-child(9)').hide()
    $('.TablaEntidadesParticipacionEdit td:nth-child(9)').hide()
    $('.TablaEntidadesParticipacionEdit th:nth-child(7)').hide()
    $('.TablaEntidadesParticipacionEdit td:nth-child(7)').hide()
    $('.TablaEntidadesParticipacionEdit th:nth-child(1)').hide()
    $('.TablaEntidadesParticipacionEdit td:nth-child(1)').hide()
    $('.TablaEntidadesParticipacionEdit th:nth-child(2)').hide()
    $('.TablaEntidadesParticipacionEdit td:nth-child(2)').hide()
  

})



function imprimir() {
   /* */window.print()
    //var htmlWindows = $("html").html()
    //var win = window.open();
    //self.focus();
    //win.document.open();
    //win.document.write('<' + 'html' + '><' + 'body' + '>');
    //win.document.write(htmlWindows)
    //win.document.write('<' + '/body' + '><' + '/html' + '>');
    //win.document.close();
    //win.print();
    //win.close();

}


// validar si en inversiones las tablas tienen todos los campos correctos.
function ValideTableInversiones() {

    //if (("#CheckParticipacion").prop('checked')) {


    //} else {

    var _validateTable = false;

    if ($('.NewItem ').length > 0) {
        for (var i = 0; i <= iRowCnt; i++) {
            if ($('.NewItem #NombreCompleto' + i + ' option:selected').val() == '0' ||
                ($('.NewItem #NombreCompleto' + i + ' option:selected').val() == '-1' || $('.NewItem #NuevaParticipacionNombre' + i).val()) ||
                $('.NewItem #ParentescoSelect' + i + ' option:selected').val() == '0' ||
                $('.NewItem #PaisSelect' + i + ' option:selected').val() == '0' ||
                $('.NewItem #NombreEmpresa' + i).val() == '' ||
                ($('.NewItem #PorcentajeParticipacion' + i).val() == '' && $('.NewItem #PorcentajeParticipacion' + i).parent().parent().parent().parent().attr("id") == 'DataTables_Table_0') ||
                ($('.NewItem #CargoParticipacion' + i).val() == '' && $('.NewItem #CargoParticipacion' + i).parent().parent().parent().parent().attr("id") == 'DataTables_Table_1')) {
                _validateTable = true;
            }
        }
    }
    return _validateTable;



}

/*
 Script para agregar las secciones a la tabla de disclaimer del formulario
 */
function AddRowNacionalidad() {
    //Add your html fields using iRowCnt as the index
    if ($('#selecNacionalidad option:selected').val() == 0) {
        alert("Debe seleccionar una Nacionalidad");
    } else {

        var t = $(".TableNacionalidad").DataTable();

        t.row.add([

            '<a class="btn btn-link remove" onclick="eliminarNacionalidadFormulario(' + iRowCnt + ',' + $('#selecNacionalidad option:selected').val() + ')"> Eliminar</a>',
            $('#selecNacionalidad option:selected').text(),
            ' <input type="hidden" class="text-box single-line " name="IdNacionalidad[' + iRowCnt + ']" value="' + $('#selecNacionalidad option:selected').val() + '"  /> '
        ]).draw(false);


        $('#selecNacionalidad option[value="' + $('#selecNacionalidad option:selected').val() + '"]').attr('class', 'hiddenElement')

        $('#selecNacionalidad').val(0);
        iRowCnt++;
        ValidarDisclaimer()
    }

}

/*
 Script para ajustar el dropdonw de disclaimer
 */
function reiniciarDropdownNacionalidad() {

    $("[name*=NacionalidadEdit]").each(function (index) {
        console.log(index + ": " + $(this).val());
        $('#selecNacionalidad option[value="' + $(this).val() + '"]').attr('class', 'hiddenElement')
    });
}


/*
 Script para agregar las secciones a la tabla de disclaimer del formulario
 */
function AddRow(idTable) {
    //Add your html fields using iRowCnt as the index
    //var tableParticipaciones = $(idTable).DataTable();
    var counter = 1;




    $(idTable).append(
        '<tr class="NewItem"><td> <input type="Text" class="form-control   " name="IdParticipacion[' + iRowCnt + ']" /></td>' +

        '<td><a class="btn btn-link " > Eliminar</a></td>' +

        '<td><select name="NombreCompleto[' + iRowCnt + ']" id="NombreCompleto' + iRowCnt + '" onchange="SetParentesco(this,' + iRowCnt + ')" class="form-select" aria-label="Default select " >' +
        '  <option selected value="0">Seleccione</option>' + NombresOptions +
        '</select>' +
        '<div class="row"><div class="col-8"><input onchange="ValidarDisclaimer() " type = "Text" class= "form-control  col single-line hiddenElement " id="NuevaParticipacionNombre' + iRowCnt + '" name="NuevaParticipacionNombre[' + iRowCnt + ']" /></div>' +
        '<div class="col-4"><a id="btnAgregarFamiliarParentesco' + iRowCnt + '" class="hiddenElement col" onclick="cancelarCreacionFamiliar(' + iRowCnt + ')">Cancelar</a></div></div></td>' +

        '<td><select name="Parentesco[' + iRowCnt + ']" id="ParentescoSelect' + iRowCnt + '" class="form-select valid" aria-label="Default select " >' +
        '  <option selected value="0">Seleccione</option>' + ParentescoOptions +

        '</select></td>' +

        ' <td>   <input type="Text" onchange="ValidarDisclaimer() " class="form-control  " id="NombreEmpresa' + iRowCnt + '" name="NombreEmpresa[' + iRowCnt + ']"   /> </td>' +

        '<td><select onchange="ValidarDisclaimer() " name="Pais[' + iRowCnt + ']" id="PaisSelect' + iRowCnt + '" class="form-select" aria-label="Default select " >' +
        '  <option selected value="0"> Seleccione</option>' + PaisesOptions +
        '</select></td>' +

        ' <td><input type="Text" class="form-control  "  onchange="ValidarDisclaimer() " id="PorcentajeParticipacion' + iRowCnt + '" name="PorcentajeParticipacion[' + iRowCnt + ']" /></td>' +
        '<td><input type="Text" class="form-control   "  id="CargoParticipacion' + iRowCnt + '"  name="CargoParticipacion[' + iRowCnt + ']" /></td></tr>'
    )


    //$("#ParentescoSelect" + iRowCnt).val(0)
    //$("#PaisSelect" + iRowCnt).val(0)
    //$("#NombreCompleto" + iRowCnt).val(0)


    document.getElementById("ParentescoSelect" + iRowCnt).selectedIndex = 0;
    document.getElementById("PaisSelect" + iRowCnt).selectedIndex = 0;
    document.getElementById("NombreCompleto" + iRowCnt).selectedIndex = 0;
    //GetParentesco("#ParentescoSelect" + iRowCnt)
    //    .done(GetPaises("#PaisSelect" + iRowCnt))
    //    .done(GetFamiliaresByFuncionario("#NombreCompleto" + iRowCnt))


    //tableParticipaciones.columns.adjust().draw();
    //GetParentesco("#ParentescoSelect" + iRowCnt);

    //GetPaises("#PaisSelect" + iRowCnt)

    //GetFamiliaresByFuncionario("#NombreCompleto" + iRowCnt);

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
                var table = $('#' + IdTable + " tbody tr").remove();//DataTables_Table_0
                //table.rows().remove().draw();
                //table.columns.adjust().draw();
                /*iRowCnt = 0;*/
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
            console.log(city)
            reiniciarSelect(selectorCiudad)

            if (city.length > 0) {
                for (var i = 0; i < city.length; i++) {
                    $(selectorCiudad).append($('<option>').val(city[i].idCiudad).text(city[i].nombreCiudad));
                }
            }
        });
        $("#IdCiudad").val($('input#IdCiudad').val())
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
            console.log(Paises)
            reiniciarSelect(idPaisSelector)

            if (Paises.length > 0) {
                for (var i = 0; i < Paises.length; i++) {
                    //$(idPaisSelector).append($('<option>').val(Paises[i].idPais).text(Paises[i].nombrePais));
                    PaisesOptions += '<option value="' + Paises[i].idPais + '">' + Paises[i].nombrePais + '</option>';
                }
            }
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
                        CatalogoCargosOptions += '<option value="' + Cargos[i].id + '">' + Cargos[i].Opcion + '</option>';
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
            console.log(Parentesco)
            reiniciarSelect(idParentescoSelector)

            if (Parentesco.length > 0) {
                for (var i = 0; i < Parentesco.length; i++) {
                    //$(idParentescoSelector).append($('<option>').val(Parentesco[i].idParentesco).text(Parentesco[i].nombreParentesco));
                    if (Parentesco[i].nombreParentesco == "Funcionario") {
                        IdParentescoFuncionario = Parentesco[i].idParentesco;
                    }
                    ParentescoOptions += '<option value="' + Parentesco[i].idParentesco + '">' + Parentesco[i].nombreParentesco + '</option>';
                }
            }
        });
    });
    return dfr.promise();
}

/*
 Script para obtener  los familiares
 */

function GetFamiliaresByFuncionario(idFamiliarSelector) {
    var dfr = $.Deferred();
    $.getJSON('/Administracion/FAmiliar/GetFamiliaresByFuncionario?idFuncionario=' + $('#IdFuncionario').val(), function (data) {
        $.each(data, function (i, familiar) {
            console.log(familiar)
            reiniciarSelect(idFamiliarSelector)
            //$(idFamiliarSelector).append($('<option>').val($('#Funcionario_Nombres').val() + " " + $('#Funcionario_Apellidos').val()).text($('#Funcionario_Nombres').val() + " " + $('#Funcionario_Apellidos').val()).attr("data-idParentesco", IdParentescoFuncionario));
            NombresOptions += '<option data-idParentesco="' + IdParentescoFuncionario + '" value="' + $('#Funcionario_Nombres').val() + " " + $('#Funcionario_Apellidos').val() + '">' + $('#Funcionario_Nombres').val() + " " + $('#Funcionario_Apellidos').val() + '</option>';
            if (familiar.length > 0) {
                for (var i = 0; i < familiar.length; i++) {
                    //$(idFamiliarSelector).append($('<option>').val(familiar[i].nombreFamiliar + " " + familiar[i].apellidoFamiliar).text(familiar[i].nombreFamiliar + " " + familiar[i].apellidoFamiliar).attr("data-idParentesco", familiar[i].idParentesco));
                    NombresOptions += '<option data-idParentesco="' + familiar[i].idParentesco + '" value="' + familiar[i].nombreFamiliar + " " + familiar[i].apellidoFamiliar + '">' + familiar[i].nombreFamiliar + " " + familiar[i].apellidoFamiliar + '</option>';
                }
            }
            //$(idFamiliarSelector).append($('<option>').val("-1").text("Agregar Nuevo").attr("data-idParentesco", "-1"));
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
            //$("#ParentescoSelect" + idRow).val().change();;

            document.getElementById("ParentescoSelect" + idRow).selectedIndex = $("#" + element.id + " option:selected").attr("data-idparentesco");
            //$("#ParentescoSelect" + idRow).attr("disabled", "disabled")

        }
    }





}



function EliminarDeclaracion(IdParticipacion, IdDeclaracion, IdPais, Idparentesco) {

    //$.getJSON('/Administracion/Declaraciones/DeletParticipacion?IdParticipacion=' + IdParticipacion + "&IdDeclaracion=" + IdDeclaracion + "&IdPais=" + IdPais + "&Idparentesco=" + Idparentesco
    //    , function (data) {
    //        $.each(data, function (i, Eliminado) {
    //            console.log(Eliminado)
    //            if (Eliminado == true) {
    //                alert('eliminado.')
    //            } else {
    //                alert('No es posible eliminar la relación en estos momentos.')
    //            }

    //        });
    //    });
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




// Validar si las condciones estan correctas para diligenciar es llamado en los checkbox, en los addrow y los deletes.
function ValidarDisclaimer() {
    var validateTable = true;

    //Verificamos que sea formulario nacionalidades
    if ($('#Formulario_IdTipoDeclaracion').val() == "2") {
        //Verificamos que tenga por lo menos una nacionalidad
        if ($('.dataTables_length').length) {

       
        if ($('#TableNacionalidad').DataTable().rows().count() > 0) {
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
        if ($('#CheckResponsabilidad').prop('checked') == false && $('#DataTables_Table_1 tbody tr').length > 0) {
            acumValidarCondiciones += 1;

        }

        if (ValideTableInversiones() == true) {
            validateTable = false;
        }
    }

    // si todos los checkbox son marcados y se valido la tabla ( nacionalidad) 
    if ($('input:checkbox:checked').length == $('#CountDisclaimer').val() && validateTable == true) {

        $('#DilegenciarDeclaracion').removeAttr('disabled').removeClass('disabled')

    } else {

        if ($('input:checkbox:checked').length + acumValidarCondiciones == $('#CountDisclaimer').val() && validateTable == true) {
            $('#DilegenciarDeclaracion').removeAttr('disabled').removeClass('disabled')
        } else {
            $('#DilegenciarDeclaracion').attr('disabled', 'disabled').addClass('disabled')
        }


    }
}


//Create json Nacionalidades
function createJSONNacionalidad() {
    jsonObj = [];
    var t = $(".TableNacionalidad").DataTable();
    for (var i = 0; i < t.data().length; i++) {
        var data = t.column(2).data().toArray(); /// columna con el campo input y el id de la nacionalidad
        var id = $(data[i]).val();
        var CheckClass = $(data[i]).attr("class");
        //item = {}
        // item["IdNacionalidad"] = $(data[i]).val();
        if (CheckClass !== "NacionalidadEdit") {
            jsonObj.push($(data[i]).val());
        }

    }
    return jsonObj;

}



function createJSONParticipacion() {
    jsonObj = [];
    var t = $("#DataTables_Table_0").DataTable().draw(false);
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
        //if (CheckClass !== "NacionalidadEdit") {  
        console.log(item); jsonObj.push(item);


    }
    return jsonObj;

}

// create json Declaraciones
function createDeclaracion(values, jsonNacionalidades) {



    return declaracion = {


        declaracion: {
            idFormulario: values["idFormulario"],
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
        Nacionalidades: jsonNacionalidades
    };


}


//Summit form 

function SummitFormDeclaraciones() {

    const form = document.querySelector('#FormDeclaracion');

    var jsonNacionalidades = createJSONNacionalidad();
    //console.log(jsonNacionalidades);
    var formData = new FormData(form);
    //console.log(formData);
    //console.log(formData.values());
    //const values = [...formData.entries()];
    //console.log(values);

    //jsonObj = [];
    var object = {};
    formData.forEach((value, key) => object[key] = value)
    //jsonObj.push(object);

    console.log(object)


    /*const idProceso = $('#IdProceso').val();*/
    //const rows = $('#TableNacionalidad').DataTable().rows();
    ////rows.select();
    //const funcionarios = Array.from(rows.nodes().map(n => $($(n).children()[0]).children(':checked')[0]).filter(c => !!c).map((f => f.value)));
    //console.log('json', JSON.stringify(funcionarios));
    const token = $("[name='__RequestVerificationToken']").val();

    //const formData = new FormData($('#funcionariosForm')[0]);

    var data = createDeclaracion(object, jsonNacionalidades)

    /* console.log(data)*/
    /*console.log(JSON.stringify(data))*/
    $.ajax({
        url: '/Administracion/Declaraciones/EditDeclaraciones',
        method: 'POST',
        headers: { 'RequestVerificationToken': `${token}`, 'Accept': 'application/json', "Content-Type": "application/json" },
        contentType: 'application/json',
        data: JSON.stringify(data),
        dataType: 'json',
        success: function (response) {
            if (response.result == true) {
                console.log(response);
                alert("Actualizado");
                window.location.replace("/Administracion/Declaraciones/");
            } else {
                alert("Something went wrong");
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










































///*
// Script para eliminar las secciones a la tabla de disclaimer del formulario CREATE FORM
// */
//function eliminarDisclaimerFormulario(id, valueselecDisclaimer) {

//    $('.DisclaimerFormulario' + id).remove()
//    $('#selecDisclaimer option[value="' + valueselecDisclaimer+'"]').removeClass('hiddenElement')

//}
///*
// Script para eliminar las secciones a la tabla de disclaimer del formulario EDIT FORM
// */
//function eliminarDisclaimerFormularioEdit(idproceso,idformulario,iddisclaimer) {



//    $.getJSON('/Administracion/Formularios/DeleteDisclaimerEnFormulario?IdProceso=' + idproceso + "&IdFormulario=" + idformulario + "&IdDisclaimer=" + iddisclaimer , function (data) {
//        $.each(data, function (i, Eliminado) {
//            console.log(Eliminado)
//            if (Eliminado == true) {
//                $('.DisclaimerFormulario' + iddisclaimer).remove()
//                $('#selecDisclaimer option[value="' + iddisclaimer + '"]').removeClass('hiddenElement')
//            } else {
//                alert('No es posible eliminar la relación en estos momentos.')
//            }

//        });
//    });




//}


///*
// Script para ajustar el dropdonw de disclaimer
// */
//function reiniciarDropdownDiscraimer() {

//    $("[name*=IdDisclaimeredit]").each(function (index) {
//        console.log(index + ": " + $(this).val());
//        $('#selecDisclaimer option[value="' + $(this).val() + '"]').attr('class', 'hiddenElement')  
//    });
//}