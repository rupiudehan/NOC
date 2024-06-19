function populateDropdown(data, dropdownId) {
    var dropdown = $("#Selected" + dropdownId);
    dropdown.html('<option value="">Select</option>');
    $.each(data, function (key, value) {
        dropdown.append('<option value="' + value.value + '">' + value.text + '</option>');
    });
}
$(document).ready(function () {
    $("#PinCode").on("keypress", function (event) {
        // Allow only numeric characters (0-9) in the input field
        var charCode = event.which;
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
        }
    });
    $("#PinCode").on("blur", function () {
        validatePinCode($(this).val(), $(this));
    });

    function validatePinCode(pinCode, field) {
        var pinCodeRegex = /^\d{6}$/;
        var PinCode = $('#PinCode');
        if (parseInt(pinCode) < 0) {
            field.val('');
            PinCode.next(".text-danger").text("Please enter a valid 6-digit PIN code.");
        }
        else if (!pinCodeRegex.test(pinCode)) {
            // PinCode.addClass("is-invalid");
            PinCode.next(".text-danger").text("Please enter a valid 6-digit PIN code.");
        }

        else {
            // PinCode.removeClass("is-invalid");
            PinCode.next(".text-danger").text("");
        }
    }

    $("#SelectedDivisionId").change(function () {
        var divisionId = $(this).val();
        $.ajax({
            url: "/Village/GetSubDivisions",
            type: "POST",
            data: { divisionId: divisionId },
            success: function (data) {
                populateDropdown(data, "SubDivisionId");
                var dropdownlist = $("#SelectedTehsilBlockId");
                dropdownlist.empty();
                dropdownlist.html('<option value="">Select</option>');
            },
            failure: function (f) {
                alert(f);
            },
            error: function (e) {
                alert('Error ' + e);
            }
        });
    });

    $("#SelectedSubDivisionId").change(function () {
        var subDivisionId = $(this).val();
        $.ajax({
            url: "/Village/GetTehsilBlocks",
            type: "POST",
            data: { subDivisionId: subDivisionId },
            success: function (data) {
                populateDropdown(data, "TehsilBlockId");
                // $("#villageDropdown").empty();
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