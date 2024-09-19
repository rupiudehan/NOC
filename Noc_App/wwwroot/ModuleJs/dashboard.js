google.charts.load('current', { packages: ['corechart', 'bar'], fontName: 'Arial' });
google.charts.setOnLoadCallback(drawMultSeries);
function drawMultSeries() {
    var DivisionId = $('#hdnDivisionId').val();
    var subdivisionId = '0';
    var roleName = $('#RoleName').val();
    if (roleName == 'EXECUTIVE ENGINEER' || roleName == 'JUNIOR ENGINEER' || roleName == 'SUB DIVISIONAL OFFICER') {
        subdivisionId = $('#hdnSubDivisionId').val();
    }
    var options = {
        title: 'Pendency Role Wise ',
        width: 3000,
        height: 700,
        responsive: true,
        hAxis: {
            title: 'Division',
            bar: { groupWidth: '30%' },
        },
        vAxis: {
            title: 'Pending Applications To Forward (in numbers)', format: ''
        },
    };
    $("#divLoader").show(); $('.barcontainer,.drpd').hide();
    $.ajax({
        url: "/Home/GetRoleLevel",
        type: "POST",
        data: { divisiondetailId: DivisionId, subdivisiondetailId: subdivisionId, role: roleName },
        complete: function (r) {
            $("#divLoader").hide();
            $('.barcontainer,.drpd').show();
            $(".barcontainer text:contains('0')").each(function () {
                var text = $(this).text();
                if (text == '0') $(this).hide();
            });

        },
        success: function (r) {
            var isValidNumNonZero = 0
            var values = '[';
            var outervalues = '';
            $.each(r, function (key, value) {
                $.each(value, function (key1, value1) {

                    if (!isNaN(value1)) { if (value1 != '0') isValidNumNonZero = 1 }
                });
                if (isValidNumNonZero == 1) {
                    $("#chart_div").show();
                    var data = google.visualization.arrayToDataTable(r);

                    var totalColumns = data['bf'].length - 1
                    var view = new google.visualization.DataView(data);
                    var columns = [];
                    for (var i = 0; i <= totalColumns; i++) {
                        if (i > 0) {
                            columns.push(i);
                            columns.push({
                                calc: "stringify",
                                sourceColumn: i,
                                type: "string",
                                role: "annotation"
                            });

                        } else {
                            columns.push(i);
                        }
                    }

                    view.setColumns(columns);
                    var chart = new google.visualization.ColumnChart($("#chart_div")[0]);
                    chart.draw(view, options);
                    google.visualization.events.addListener(chart, 'select', setOption);
                    function setOption(e) {
                        var selection = chart.getSelection();
                        item = selection[0]
                        var column = 0;
                        if (selection.length > 0) {
                            if ((item.column) == 1)
                                column = item.column;
                            else if ((item.column - 1) == 2)
                                column = item.column - 1;
                            else if ((item.column - 2) == 3)
                                column = item.column - 2;
                            else if ((item.column - 3) == 4)
                                column = item.column - 3;
                            else if ((item.column - 4) == 5)
                                column = item.column - 4;
                            else if ((item.column - 5) == 6)
                                column = item.column - 5;
                            else if ((item.column - 6) == 7)
                                column = item.column - 6;
                            else if ((item.column - 7) == 8)
                                column = item.column - 7;
                            var category = data.getValue(item.row, 0);
                            // console.log(DivisionId + '-' + subdivisionId + '-' + category + '-' + data['bf'][column]['label']);
                            LoadReport(DivisionId, subdivisionId, data['bf'][column]['label']);
                        } else $("#myModal").modal();
                    }
                } else $("#chart_div").hide();
            });
        },
        failure: function (r) {
            alert(JSON.stringify(r))
        },
        error: function (r) {
            alert(r.d);
        }
    });
}

// function LoadBarPendancyChart() {
//     // google.charts.load('current', { packages: ['corechart', 'bar'] });
//     // google.charts.setOnLoadCallback(drawMultSeries);

//     drawMultSeries();
// }
function populateDropdown(data, dropdownId) {
    var dropdown = $("#Selected" + dropdownId);
    $.each(data, function (key, value) {
        dropdown.append('<option value="' + value.value + '">' + value.text + '</option>');
    });
}
$(function () {
    $("#SelectedDivisionId").change(function () {
        var divisionId = $(this).val();
        var role = $('#RoleName').val();
        $('#hdnDivisionId').val(divisionId);
        LoadBarPendancyChart();
    });
    LoadBarPendancyChart();

});
// Notice that e is not used or needed.

function BindTable(tbl) {

    if (tbl.find('tr').length > 0) {
        
        tbl.DataTable({
            destroy: true,
            responsive: false, // Disable responsive 
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
}
function LoadBarPendancyChart() {
    drawMultSeries();
}

function LoadReport(DivisionId, subdivisionId, roleName) {
    var body = $('#tbPendancy');
    var table = $("#tblPendancy");
    //$("#myModal").modal(); $("#myModal").addClass('show');
    $("#myModal").hide();
    body.empty();
    //table.DataTable().clear().destroy();
    $("#divLoader").show(); $('.barcontainer,.drpd').hide();

    $.ajax({
        url: "/Home/GetRoleLevelPendencyReport",
        type: "POST",
        data: { divisiondetailId: DivisionId, subdivisiondetailId: subdivisionId, role: roleName },
        complete: function (r) {
            $("#myModal").modal(); $("#myModal").addClass('show');
            $("#divLoader").hide();
            $('.barcontainer,.drpd').show();
            $(".barcontainer text:contains('0')").each(function () {
                var text = $(this).text();
                if (text == '0') $(this).hide();
            });

        },
        success: function (r) {
            if (r != null) {
                if ($.fn.DataTable.isDataTable('#tblPendancy')) {
                    table.DataTable().destroy();
                    body.empty();
                }
                var count = 1; var len = r.length;
                $.each(r, function (key, value) {
                    var pendency = value.pendency < 0 ? '0' : value.pendency;
                    var tr = '<tr>';
                    tr += '<td>' + count + '</td>';
                    // if (count == 1) { $('#dvBranch').text("Branch: " + value.BranchName); $('#dvLevel').text('Level: ' + LevelType); $('#dvRule').text('Under: ' + ActionName); }
                    tr += '<td>' + value.applicationID + '</td>';
                    tr += '<td>' + value.applyDate + '</td>';
                    tr += '<td>' + value.name + '</td>';
                    tr += '<td>' + value.division + '</td>';
                    tr += '<td>' + value.processedToRole + '</td>';
                    tr += '<td>' + pendency + '</td>';
                    tr += '</tr>';
                    body.append(tr);
                    count++;
                });
                BindTable(table);
            }
        },
        failure: function (r) {
            alert(r.d);
        },
        error: function (r) {
            alert(r.d);
        }
    });
}

function LoadReportApplications(flag) {
    if (flag == 'T') {
        LoadReportApplicationsT();
    }
    else if (flag == 'A') {
        LoadReportApplicationsA();
    }
    else if (flag == 'R') {
        LoadReportApplicationsR();
    }
    else
        LoadReportApplicationsP();
}
function LoadReportApplicationsP() {
    var DivisionId = 0;
    var body = $('#tbP');
    var table = $("#tblP");
    var thead = $('#tbhP');
    var modal = $("#myPendingModal");
    modal.hide();
    thead.empty();
    body.empty();
    var method = 'GetPendingApplicationReport';
    var title = $('#dvPTitle');
    title.text('In Process Applications');
    //table.DataTable().clear().destroy();
    $("#divLoaderApp").show(); $('.rptApp').hide();

    $.ajax({
        url: "/Home/" + method,
        type: "POST",
        data: { divisiondetailId: DivisionId },
        complete: function (r) {
            modal.modal(); modal.addClass('show');
            $("#divLoaderApp").hide();
            $('.rptApp').show();
        },
        success: function (r) {
            if (r != null) {

                var count = 1; var len = r.length;
                var thr = `<tr>
                            <th>#</th>
                            <th>Application ID</th>
                            <th>Date Of Application</th>
                            <th>Project Name</th>
                            <th>Division</th>
                            <th>Status</th>
                            <th>Pending With</th>
                            <th>Pending for (No. of Days)</th>
                        </tr>`;
                thead.html(thr);
                $.each(r, function (key, value) {
                    var status =  value.isshortfall == true ?'Reverted to Applicant Due To Discrepency':value.isapproved == true ? 'Certificate Issued' : value.isrejected == true ? 'Rejected' : value.processedLevel == '0' ? 'Unprocessed' : 'In Process';
                    var name = value.processedLevel == '0' ? 'EXECUTIVE ENGINEER' : value.isshortfall == true ? value.currentProcessedToName : value.currentProcessedToName == 'Applicant' ? (value.currentProcessedByName == null ? '' : value.currentProcessedByName) + ' - ' + (value.currentProcessedByRole == null ? '' : value.currentProcessedByRole) : (value.currentProcessedToName == null ? '' : value.currentProcessedToName) + ' - ' + (value.currentProcessedToRole == null ? '' : value.currentProcessedToRole);
                    var processedOn = value.currentProcessedOn == '1970-01-01T05:30:00+05:30' ? '' : formatDate(value.currentProcessedOn);
                    var applydate = formatOnlyDate(value.createdOn);
                    var pendency = value.pendency < 0 ? '0' : value.pendency;
                    var tr = '<tr>';
                    tr += '<td>' + count + '</td>';
                    tr += '<td>' + value.applicationID + '</td>';
                    tr += '<td>' + applydate + '</td>';
                    tr += '<td>' + value.name + '</td>';
                    tr += '<td>' + value.divisionName + '</td>';
                    tr += '<td>' + status + '</td>';
                    tr += '<td>' + name + '</td>';
                    tr += '<td>' + pendency + '</td>';
                    tr += '</tr>';
                    body.append(tr);
                    count++;
                });
                if ($.fn.DataTable.isDataTable('#tblP')) {
                    table.DataTable().destroy();

                    BindTable(table);
                } else {

                    BindTable(table);
                }

            }
        },
        failure: function (r) {
            alert(r.d);
        },
        error: function (r) {
            alert(r.d);
        }
    });

}
function LoadReportApplicationsA() {
    var DivisionId = 0;
    var body = $('#tbA');
    var table = $("#tblA");
    var thead = $('#tbhA');
    var modal = $("#myApprovedModal");
    modal.hide();
    thead.empty();
    body.empty();
    var method = 'GetApprovedApplicationReport';
    var title = $('#dvATitle');
    title.text('Approved Applications');
    $("#divLoaderApp").show(); $('.rptApp').hide();

    $.ajax({
        url: "/Home/" + method,
        type: "POST",
        data: { divisiondetailId: DivisionId },
        complete: function (r) {
            modal.modal(); modal.addClass('show');
            $("#divLoaderApp").hide();
            $('.rptApp').show();
        },
        success: function (r) {
            if (r != null) {

                var count = 1; var len = r.length;
                var thr = `<tr>
                            <th>#</th>
                            <th>Application ID</th>
                            <th>Date Of Application</th>
                            <th>Project Name</th>
                            <th>Division</th>
                            <th>Status</th>
                            <th>Processed On</th>
                        </tr>`;

                thead.html(thr);
                $.each(r, function (key, value) {

                    var status = value.isapproved == true ? 'Certificate Issued' : value.isrejected == true ? 'Rejected' : 'In Process';
                    var applydate = formatOnlyDate(value.createdOn);
                    var processedOn = value.currentProcessedOn == '1970-01-01T05:30:00+05:30' ? '' : formatDate(value.currentProcessedOn);
                    var tr = '<tr>';
                    tr += '<td>' + count + '</td>';
                    tr += '<td>' + value.applicationID + '</td>';
                    tr += '<td>' + applydate + '</td>';
                    tr += '<td>' + value.name + '</td>';
                    tr += '<td>' + value.divisionName + '</td>';
                    tr += '<td>' + status + '</td>';
                    tr += '<td>' + processedOn + '</td>';
                    tr += '</tr>';
                    body.append(tr);
                    count++;
                });
                if ($.fn.DataTable.isDataTable('#tblA')) {
                    table.DataTable().destroy();

                    BindTable(table);
                } else {

                    BindTable(table);
                }

            }
        },
        failure: function (r) {
            alert(r.d);
        },
        error: function (r) {
            alert(r.d);
        }
    });

}
function LoadReportApplicationsR() {
    var DivisionId = 0;
    var body = $('#tbR');
    var table = $("#tblR");
    var thead = $('#tbhR');
    var modal = $("#myRejectedModal");
    modal.hide();
    thead.empty();
    body.empty();
    var method = 'GetRejectedApplicationReport';
    var title = $('#dvRTitle');
    title.text('Rejected Applications');
    $("#divLoaderApp").show(); $('.rptApp').hide();

    $.ajax({
        url: "/Home/" + method,
        type: "POST",
        data: { divisiondetailId: DivisionId },
        complete: function (r) {
            modal.modal(); modal.addClass('show');
            $("#divLoaderApp").hide();
            $('.rptApp').show();
        },
        success: function (r) {
            if (r != null) {

                var count = 1; var len = r.length;
                var thr = `<tr>
                            <th>#</th>
                            <th>Application ID</th>
                            <th>Date Of Application</th>
                            <th>Project Name</th>
                            <th>Division</th>
                            <th>Status</th>
                            <th>Processed On</th>
                        </tr>`;

                thead.html(thr);
                $.each(r, function (key, value) {

                    var status = value.isapproved == true ? 'Certificate Issued' : value.isrejected == true ? 'Rejected' : 'In Process';
                    var applydate = formatOnlyDate(value.createdOn);
                    var processedOn = value.currentProcessedOn == '1970-01-01T05:30:00+05:30' ? '' : formatDate(value.currentProcessedOn);
                    var tr = '<tr>';
                    tr += '<td>' + count + '</td>';
                    tr += '<td>' + value.applicationID + '</td>';
                    tr += '<td>' + applydate + '</td>';
                    tr += '<td>' + value.name + '</td>';
                    tr += '<td>' + value.divisionName + '</td>';
                    tr += '<td>' + status + '</td>';
                    tr += '<td>' + processedOn + '</td>';
                    tr += '</tr>';
                    body.append(tr);
                    count++;
                });
                if ($.fn.DataTable.isDataTable('#tblR')) {
                    table.DataTable().destroy();

                    BindTable(table);
                } else {

                    BindTable(table);
                }

            }
        },
        failure: function (r) {
            alert(r.d);
        },
        error: function (r) {
            alert(r.d);
        }
    });

}
function LoadReportApplicationsT() {
    var DivisionId = 0;
    var body = $('#tb');
    var table = $("#tbl");
    var thead = $('#tbh');
    var modal = $("#myTotalModal");
    modal.hide();
    thead.empty();
    body.empty();
    var method = 'GetTotalApplicationReport';
    var title = $('#dvTitle');
    title.text('Total Applications');
    //table.DataTable().clear().destroy();
    $("#divLoaderApp").show(); $('.rptApp').hide();

    $.ajax({
        url: "/Home/" + method,
        type: "POST",
        data: { divisiondetailId: DivisionId },
        complete: function (r) {
            modal.modal(); modal.addClass('show');
            $("#divLoaderApp").hide();
            $('.rptApp').show();
        },
        success: function (r) {
            if (r != null) {
                console.log(JSON.stringify(r))
                var count = 1; var len = r.length;
                var thr = `<tr>
                            <th>#</th>
                            <th>Application ID</th>
                            <th>Date Of Application</th>
                            <th>Project Name</th>
                            <th>Division</th>
                            <th>Status</th>
                            <th>Pending With</th>
                            <th>Pending for (No. of Days)</th>
                            <th>Processed On</th>
                        </tr>`;
                thead.html(thr);
                $.each(r, function (key, value) {
                    var status = value.isshortfall == true ? 'Reverted to Applicant Due To Discrepency' : value.isapproved == true ? 'Certificate Issued' : value.isrejected == true ? 'Rejected' : value.processedLevel == '0' ? 'Unprocessed' : 'In Process';
                    var name = value.processedLevel == '0' ? 'EXECUTIVE ENGINEER' : value.isshortfall == true ? value.currentProcessedToName : value.currentProcessedToName == 'Applicant' ? (value.currentProcessedByName == null ? '' : value.currentProcessedByName) + ' - ' + (value.currentProcessedByRole == null ? '' : value.currentProcessedByRole) : (value.currentProcessedToName == null ? '' : value.currentProcessedToName) + ' - ' + (value.currentProcessedToRole == null ? '' : value.currentProcessedToRole);
                    var processedOn = value.currentProcessedOn == '1970-01-01T05:30:00+05:30' ? '' : formatDate(value.currentProcessedOn);
                    var applydate = formatOnlyDate(value.createdOn);
                    var pendency = value.pendency < 0 ? '0' : value.pendency;
                    var tr = '<tr>';
                    tr += '<td>' + count + '</td>';
                    tr += '<td>' + value.applicationID + '</td>';
                    tr += '<td>' + applydate + '</td>';
                    tr += '<td>' + value.name + '</td>';
                    tr += '<td>' + value.divisionName + '</td>';
                    tr += '<td>' + status + '</td>';
                    tr += '<td>' + name + '</td>';
                    tr += '<td>' + pendency + '</td>';
                    tr += '<td>' + processedOn + '</td>';
                    tr += '</tr>';
                    body.append(tr);
                    count++;
                });
                if ($.fn.DataTable.isDataTable('#tbl')) {
                    table.DataTable().destroy();

                    BindTable(table);
                } else {

                    BindTable(table);
                }

            }
        },
        failure: function (r) {
            alert(r.d);
        },
        error: function (r) {
            alert(r.d);
        }
    });

}

function formatDate(dateString) {
    // Create a new Date object from the input string
    var date = new Date(dateString);

    // Extract day, month, year, hours, minutes
    var day = ("0" + date.getDate()).slice(-2); // Add leading zero
    var month = ("0" + (date.getMonth() + 1)).slice(-2); // Months are zero-based
    var year = date.getFullYear();

    var hours = date.getHours();
    var minutes = ("0" + date.getMinutes()).slice(-2); // Add leading zero

    // Determine AM/PM
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // The hour '0' should be '12'

    // Format the time with leading zero if necessary
    var formattedTime = ("0" + hours).slice(-2) + ":" + minutes + " " + ampm;

    // Combine the formatted date and time
    return day + "/" + month + "/" + year + " " + formattedTime;
}
function formatOnlyDate(dateString) {
    // Create a new Date object from the input string
    var date = new Date(dateString);

    // Extract day, month, year, hours, minutes
    var day = ("0" + date.getDate()).slice(-2); // Add leading zero
    var month = ("0" + (date.getMonth() + 1)).slice(-2); // Months are zero-based
    var year = date.getFullYear();

    var hours = date.getHours();
    var minutes = ("0" + date.getMinutes()).slice(-2); // Add leading zero

    // Determine AM/PM
    var ampm = hours >= 12 ? 'PM' : 'AM';
    hours = hours % 12;
    hours = hours ? hours : 12; // The hour '0' should be '12'

    // Format the time with leading zero if necessary
    var formattedTime = ("0" + hours).slice(-2) + ":" + minutes + " " + ampm;

    // Combine the formatted date and time
    return day + "/" + month + "/" + year;
}