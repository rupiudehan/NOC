function SetValue(id) {
    $('#id').val(id);
    var body = $('#tbRecommendation');
    var table = $("#tblRecommendation");
    $('.barcontainer').hide();
    $("#myModal").hide();
    body.empty();
    $("#divLoader").show(); $('.barcontainer').hide();
    $.ajax({
        url: "/ApprovalProcess/GetRecommendationDetail",
        type: "POST",
        data: { id: id },
        complete: function (r) {
            $("#myModal").modal(); $("#myModal").addClass('show');
            $("#divLoader").hide();
            $('.barcontainer').show();

        },
        success: function (r) {
            if (r != null) {
                var count = 1; var len = r.length;

                $.each(r, function (key, value) {
                    var remarks = value.remarks == null ? "" : value.remarks;
                    var tr = '<tr>';
                    tr += '<td>' + count + '</td>';
                    tr += '<td>' + value.applicationId + '</td>';
                    tr += '<td>' + value.recommended + '</td>';
                    tr += '<td>' + remarks + '</td>';
                    tr += '<td>' + value.recommendedByName + '</td>';
                    tr += '<td>' + value.recommendedBy + '</td>';
                    tr += '<td>' + value.recommendedToName + '</td>';
                    tr += '<td>' + value.recommendedTo + '</td>';
                    tr += '<td>' + value.createdOn + '</td>';
                    tr += '</tr>';
                    body.append(tr);
                    count++;
                });
                table.DataTable();
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

function SetProcessedValue(id) {
    $('#id').val(id);
    var body = $('#tbRecommendation');
    var table = $("#tblRecommendation");
    $('.barcontainer').hide();
    $("#myModal").hide();
    body.empty();
    $("#divLoader").show(); $('.barcontainer').hide();
    $.ajax({
        url: "/ApprovalProcess/GetRecommendationDetail",
        type: "POST",
        data: { id: id },
        complete: function (r) {
            $("#myModal").modal(); $("#myModal").addClass('show');
            $("#divLoader").hide();
            $('.barcontainer').show();

        },
        success: function (r) {
            if (r != null) {
                var count = 1; var len = r.length;

                $.each(r, function (key, value) {
                    var remarks = value.remarks == null ? "" : value.remarks;
                    var tr = '<tr>';
                    tr += '<td>' + count + '</td>';
                    tr += '<td>' + value.applicationId + '</td>';
                    tr += '<td>' + value.recommended + '</td>';
                    tr += '<td>' + remarks + '</td>';
                    tr += '<td>' + value.recommendedByName + '</td>';
                    tr += '<td>' + value.recommendedBy + '</td>';
                    tr += '<td>' + value.recommendedToName + '</td>';
                    tr += '<td>' + value.recommendedTo + '</td>';
                    tr += '</tr>';
                    body.append(tr);
                    count++;
                });
                table.DataTable();
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

function SetValueNA(id) {
    $('#id').val(id);
    var body = $('#tbRecommendation');
    var table = $("#tblRecommendation");
    $('.barcontainer').hide();
    $("#myModal").hide();
    body.empty();
    $("#divLoader").show(); $('.barcontainer').hide();
    $.ajax({
        url: "/ApprovalProcess/GetRecommendationDetailForOtherThanNA",
        type: "POST",
        data: { id: id },
        complete: function (r) {
            $("#myModal").modal(); $("#myModal").addClass('show');
            $("#divLoader").hide();
            $('.barcontainer').show();

        },
        success: function (r) {
            if (r != null) {
                var count = 1; var len = r.length;

                $.each(r, function (key, value) {
                    var tr = '<tr>';
                    tr += '<td>' + count + '</td>';
                    tr += '<td>' + value.applicationId + '</td>';
                    tr += '<td>' + value.recommended + '</td>';
                    tr += '<td>' + value.remarks + '</td>';
                    tr += '<td>' + value.recommendedByName + '</td>';
                    tr += '<td>' + value.recommendedBy + '</td>';
                    tr += '<td>' + value.recommendedToName + '</td>';
                    tr += '<td>' + value.recommendedTo + '</td>';
                    tr += '</tr>';
                    body.append(tr);
                    count++;
                });
                table.DataTable();
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
$(document).ready(function () {
    if ($('.table tbody tr').length > 0) {
        $('.table').DataTable();
    }
});