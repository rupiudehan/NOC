function BindTable() {
    $("#dataTable").DataTable({
        dom: 'Blfrtip',
        buttons: [

            // {
            //     extend: 'copy',
            //     text: '<i class="fas fa-copy"></i>',
            //     titleAttr: 'Copy',
            //     className: 'btn btn-md mr-2 btn-copy'
            // },
            {
                extend: 'excel',
                text: '<i class="fas fa-file-excel"></i>',
                titleAttr: 'Excel',
                className: 'btn btn-primary btn-sm',
                title: 'Report'
            },
            // {
            //     extend: 'pdf',
            //     text: '<i class="fas fa-file-pdf"></i>',
            //     titleAttr: 'PDF',
            //     className: 'btn btn-md mr-2 btn-pdf'
            // },
            {
                extend: 'print',
                text: '<i class="fas fa-print"></i>',
                titleAttr: 'Print',
                className: 'btn btn-md mr-2 btn-print',
                title: 'Report'
            },

        ],
        'ordering': true,
        'searching': true,
        'info': false,
        "serverSide": false,
        "lengthMenu": [[10, 30, 50 - 1], [10, 30, 50, "All"]],
        "pageLength": 10,

    });
}
$(document).ready(function () {
    if ($('#dataTable tbody tr').length > 0) {
        BindTable();
    }
});