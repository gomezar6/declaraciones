var iRowCnt = 0; // conteo de elementos agregados a la tabla. Para no repeter ID


$(document).ready(function () {
    TipoDeclaracion_SelectedIndexChanged("onready");
    reiniciarDropdownDiscraimer();

    $('.TableDisclaimer').on('click', '.remove', function () {
        var table = $('.TableDisclaimer').DataTable();
        table
            .row($(this).parents('tr'))
            .remove()
            .draw(false);
     
    });


    $('.backButton ').on('click', function () {
        $("#loader").removeClass("loading")
        $("#loader").addClass("loading")
    });


    $('#VistaPrevia').on('click', function () {
        //window.open("/Administracion/Declaraciones/DetailsPreview/" + $('#IdFormulario').val(), '_blank')



        $('.Mensaje').text("");
        $('.Mensaje iframe').remove();
        $('.Mensaje').append("<iframe class='iframe-Preview' src='" + window.location.origin+"/Administracion/Declaraciones/DetailsPreview/" + $('#IdFormulario').val()+"'></iframe>");
        $(".modal-Preview").modal("show");
    });

    const myTimeout = setTimeout(ClearAlert, 5000);

});



function ClearAlert() {
    $(".alert").hide(500);
   

    const nextURL = window.location.href.replace("&Save=true","")
    window.history.pushState(null, null, nextURL);
}

$('#SummitForm').submit(function () {
    // DO STUFF...
    const nextURL = window.location.href + "&Save=true"
    window.history.pushState(null, null, nextURL);
    
    console.log("antes de subir")
    return true; // return false to cancel form action
});

/*
 Script para validar las secciones
 */
function validarSecciones(NumSecciones) {
    var checkSeccionText = true;
    for (var i = NumSecciones; i >= 1; i--) {

        if ($('#Texto' + i).val() == "" && checkSeccionText==true) {
            checkSeccionText = false;
            i = 0;
            
        }
    
    }
    if (checkSeccionText == true) {
        console.log("Todo bien")
    } else {
        console.log("1 no cumple")
    }
}
/*
/*
 Script para reiniciar las secciones conservando las que no seran bloqueads
 */
function reiniciarSecciones(Conservar) {
    for (var i = 1; i < 5; i++) {
        $("#Texto" + i).attr("disabled", "disabled");
        if (i <= Conservar) {
            $('#Texto'+i).val("")
        }
     
    }
}
/*
 Script para desbloquear las secciones dado el tipo de declaracion
 */
function TipoDeclaracion_SelectedIndexChanged(status) {

    $.getJSON('/Administracion/Formularios/SeccionesByTipoDeclaracion/' + $("#selecTipoFormulario").val(), function (data) {
        $.each(data, function (i, num) {
            console.log(num)
            if (num > 0) {
                if (status !== "onready") {
                    reiniciarSecciones(0);
                }

                for (var i = 1; i <= num; i++) {
                    $("#Texto" + i).removeAttr("disabled");
                }

                if ($("#selecTipoFormulario").val() == 1) {
                    if (location.href.indexOf("Formularios/Details") > 0 || location.href.indexOf("Formularios/Delete") > 0) {

                        $("#Texto5").attr("disabled");
                        $('.onlyInversiones ').show()
                    } else {
                        $("#Texto5").removeAttr("disabled");
                        $('.onlyInversiones ').show()
                    }
                    
                } else {
                    $("#Texto5").attr("disabled");
                    $('.onlyInversiones ').hide()
                }
            } else {
                if (status !== "onready") {
                    reiniciarSecciones(0);
                } 
            
              }
        });
    });
}

/*
 Script verificar si existe el formulario

 */
function FormularioTipoDeclaracion_SelectedIndexChanged() {

    console.log($('#selecTipoFormulario option:selected').val())
    console.log($('#selecTipoFormulario option:selected').text())
    if ($('#selecTipoFormulario option:selected').val() != "0") {

        if ($('#selecTipoFormulario option:selected').text() == "Estándar") {

            TipoDeclaracion_SelectedIndexChanged("onfunction")
        } else {

            $.getJSON('/Administracion/Formularios/getTipoDeclaracionEnFormulario?IdProceso=' + $('input#IdProceso').val() + '&IdTipoDeclaracion=' + $('#selecTipoFormulario option:selected').val(), function (data) {
                $.each(data, function (i, existe) {
                    console.log(existe)

                    if (existe == true) {


                        $('.Mensaje').text(("Ya existe un formulario del tipo " + $('#selecTipoFormulario option:selected').text() + " en el proceso"))
                        $(".modal").modal("show");

                        $('#selecTipoFormulario').val(0);
                    } else {
                        TipoDeclaracion_SelectedIndexChanged("onfunction")
                    }


                });
            });
        }
        
    }


    }
  




/*
 Script para agregar las secciones a la tabla de disclaimer del formulario
 */
function AddRow() {
    var t = $(".TableDisclaimer").DataTable();

   
    //Add your html fields using iRowCnt as the index
    if ($('#selecDisclaimer option:selected').val() == 0) {
        /*  alert("Debe seleccionar un disclaimer");*/
    
        $('.Mensaje').text(("Debe seleccionar un disclaimer"))
        $(".ModalMensaje").modal("show");
    } else { 



    t.row.add([

        '<a class="btn btn-link remove" onclick="eliminarDisclaimerFormulario(' + iRowCnt + ',' + $('#selecDisclaimer option:selected').val() + ')"> Eliminar</a>',
        $('#selecDisclaimer option:selected').text(),
        ' <input type="hidden" class="text-box single-line " name="IdDisclaimer[' + iRowCnt + ']" value="' + $('#selecDisclaimer option:selected').val() + '"  /> '
    ]).draw(false);
    
        
        
        //$('.bodyDisclaimer').append(myRow);
    //oculta el disclaimer usado
    $('#selecDisclaimer option[value="' + $('#selecDisclaimer option:selected').val()+'"]').attr('class', 'hiddenElement')

    $('#selecDisclaimer').val(0);
        iRowCnt++;
    }
}

function cancelarModal() {
    $('.Mensaje').text("")
    $(".modal").modal("hide");
}

/*
 Script para eliminar las secciones a la tabla de disclaimer del formulario CREATE FORM
 */
function eliminarDisclaimerFormulario(id, valueselecDisclaimer) {


    $('.DisclaimerFormulario' + id).remove()
    $('#selecDisclaimer option[value="' + valueselecDisclaimer+'"]').removeClass('hiddenElement')

}
/*
 Script para eliminar las secciones a la tabla de disclaimer del formulario EDIT FORM
 */
function eliminarDisclaimerFormularioEdit(idproceso,idformulario,iddisclaimer) {




    $.getJSON('/Administracion/Formularios/DeleteDisclaimerEnFormulario?IdProceso=' + idproceso + "&IdFormulario=" + idformulario + "&IdDisclaimer=" + iddisclaimer , function (data) {
        $.each(data, function (i, Eliminado) {
            console.log(Eliminado)
            if (Eliminado == true) {
                $('.TableDisclaimer ').DataTable().destroy();
                $('.DisclaimerFormulario' + iddisclaimer).remove()
                $('#selecDisclaimer option[value="' + iddisclaimer + '"]').removeClass('hiddenElement')
                    $('.TableDisclaimer ').DataTable()
            } else {
                alert('No es posible eliminar la relación en estos momentos.')
            }
          
        });
    });


    

}


/*
 Script para ajustar el dropdonw de disclaimer
 */
function reiniciarDropdownDiscraimer() {
   
    $("[name*=IdDisclaimeredit]").each(function (index) {
        console.log(index + ": " + $(this).val());
        $('#selecDisclaimer option[value="' + $(this).val() + '"]').attr('class', 'hiddenElement')  
    });
}