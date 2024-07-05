function populateDropdown(data, dropdownId) {
    var dropdown = $("#Selected" + dropdownId);
    dropdown.removeAttr("disabled");
    dropdown.html('<option value="">Select</option>');
    $.each(data, function (key, value) {
        dropdown.append('<option value="' + value.value + '">' + value.text + '</option>');
    });
}
$(document).ready(function () {
    $("#SelectedSubDivisionId").change(function () {
        var subdivisionId = $(this).val();
        var role = $('#ForwardToRole').val();
        console.log(role);
        $.ajax({
            url: "/ApprovalProcess/GetOfficers",
            type: "POST",
            data: { subdivisionId: subdivisionId, roleName: role },
            async: false,
            success: function (data) {
                populateDropdown(data, "OfficerId");
            },
            failure: function (f) {
                alert(f);
            },
            error: function (e) {
                alert('Error ' + e);
            }
        });
    });
    $("#IsDrainNotified").change(function () {
        if (this.checked) {
            $('#DrainWidth').text("Width as per Notification");
        } else {
            $('#DrainWidth').text("Width as per Calculations");
        }
    });
});