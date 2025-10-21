/* Create an array with the values of all the checkboxes in a column */
$.fn.dataTable.ext.order['dom-checkbox'] = function (settings, col) {
    return this.api().column(col, { order: 'index' }).nodes().map(function (td, i) {
      
        return $('input', td).prop('checked') ? '1' : '0';
    });
};

function reordenar() {

    table = $('#funcionariosTable').DataTable();


    table.order([0, 'desc'])
        .draw();
}

$(document).ready(function () {
  
    
    var table = $('#funcionariosTable').DataTable({
        rowReorder: true,
        columnDefs: [
            {
                targets: 0,
                orderDataType: 'dom-checkbox'
            }
        ]
        , "order": [0, 'desc'],
        language: {
            url: '/js/dataTables.es-es.json'
        }

    });



    $('[type="search"]').attr("onchange", "reordenar()").attr("onblur", "reordenar()").attr("onclick", "reordenar()").attr("onkeypress", "reordenar()")
    $(':checkbox').on('change', function (e) {
        var row = $(this).closest('tr');
        var hmc = row.find(':checkbox:checked').length;
        
        table.row(row).invalidate('dom');
    });



    // Listen to change event from checkbox to trigger re-sorting
    $('#funcionariosTable input[type="checkbox"]').on('change', function () {
        // Update data-sort on closest <td>
        $(this).closest('td').attr('data-order', this.checked ? 1 : 0);

        // Store row reference so we can reset its data
        var $tr = $(this).closest('tr');

        // Force resorting
        table
            .row($tr)
            .invalidate()
            .order([0, 'desc'])
            .draw();
    });




});

