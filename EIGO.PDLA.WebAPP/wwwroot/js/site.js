// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

$(document).ready(function () {
    $.fn.dataTable.moment('DD.MM.YYYY');
    $.fn.dataTable.moment('DD/MM/YYYY');
 

    if ($('#funcionariosTable').length)         // use this if you are using id to check
    {
        // it exists
       
    
    } else {
        $("table").DataTable(
            {
                language: {
                    url: '/js/dataTables.es-es.json'
                }
            }
        );
    }
});
