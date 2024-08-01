function populateDropdown(data, dropdownId) {
    var dropdown = $("#Selected" + dropdownId);
    dropdown.html('<option value="">Select</option>');
    $.each(data, function (key, value) {
        dropdown.append('<option value="' + value.value + '">' + value.text + '</option>');
    });
}
$(document).ready(function () {
    $("#LGD_ID").on("keypress", function (event) {
        // Allow only numeric characters (0-9) in the input field
        var charCode = event.which;
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
        }
    });
    $("#SelectedDistrictId").change(function () {
        var districtId = $(this).val();
        $.ajax({
            url: "/TehsilBlock/GetDivisions",
            type: "POST",
            data: { districtId: districtId },
            success: function (data) {
                populateDropdown(data, "DivisionId");
            },
            failure: function (f) {
                alert(f);
            },
            error: function (e) {
                alert('Error ' + e);
            }
        });
    });
});