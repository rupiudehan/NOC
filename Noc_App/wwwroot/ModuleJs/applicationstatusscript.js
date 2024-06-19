$(function () {
    LoadReport($("#SelectedDivisionId").val());
    $("#SelectedDivisionId").on('change', function () {
        var divisionId = $(this).val();
        var body = $('#tbody');
        var table = $("#dataTable");
        body.empty();
        $("#divLoader").show(); $('.card-body').hide();
        LoadReport(divisionId);
    }
    );
}
);
function LoadReport(divisionId) {
    var body = $('#tbody');
    var table = $("#dataTable");
    $.ajax({
        url: "/Home/GetApplicationStatusReport",
        type: "POST",
        data: { divisiondetailId: divisionId },
        complete: function (r) {
            $("#divLoader").hide();
            $('.card-body').show();
        },
        success: function (r) {
            if (r != null) {
                if ($.fn.DataTable.isDataTable('#dataTable')) {
                    table.DataTable().destroy();
                    body.empty();
                }
                var count = 1; var len = r.length;
                $.each(r, function (key, value) {
                    var tr = '<tr>';
                    tr += '<td>' + count + '</td>';
                    // if (count == 1) { $('#dvBranch').text("Branch: " + value.BranchName); $('#dvLevel').text('Level: ' + LevelType); $('#dvRule').text('Under: ' + ActionName); }
                    tr += '<td>' + value.applicationID + '</td>';
                    tr += '<td>' + value.createdOn.split("T")[0] + '</td>';
                    tr += '<td>' + value.name + '</td>';
                    tr += '<td>' + value.locationDetails + '</td>';
                    tr += '<td>' + value.processedToRole + '</td>';
                    tr += '</tr>';
                    body.append(tr);
                    count++;
                });
                BindTable();
                // table.DataTable().clear().draw();
                // table.DataTable().destroy();
                // BindTable();
            }

        },
        failure: function (f) {
            alert(f);
        },
        error: function (e) {
            alert('Error ' + e);
        }
    });
}
    function BindTable() {
        $("#dataTable").DataTable(
            {
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
                        title: 'Pendency Report'
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
                        title: 'Pendency Report'
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