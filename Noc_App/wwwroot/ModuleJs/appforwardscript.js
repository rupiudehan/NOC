function populateDropdown(data, dropdownId, page) {
    var dropdown = $("#Selected" + dropdownId);
    dropdown.removeAttr("disabled");
    $('#btnForward').removeAttr('disabled');
    dropdown.html('<option value="">Select</option>');
    $.each(data, function (key, value) {
        if (page == 'F' || page == 'T') {
            dropdown.append('<option value="' + value.id + '">' + value.name + '</option>');
        } else {
            dropdown.append('<option value="' + value.value + '">' + value.text + '</option>');
        }
    });
}
$(document).ready(function () {
    $('.validation-summary-errors ul li').each(function () {
        // 'this' refers to the current 'li' element
        var liText = $(this).text();
        if (liText.includes('The value')) {
            $(this).remove(); // Remove the current <li> element
            // Add any other logic you need here
        }
    });
        $("#SelectedOfficerId").change(function () {
            var role = $('#ForwardToRole').val();
            //if (role == "EXECUTIVE ENGINEER" || role == "JUNIOR ENGINEER" || role == "SUB DIVISIONAL OFFICER" || role == "CIRCLE OFFICER" || role == "DWS,CIRCLE OFFICER") {
            var userid = $(this).val();
                //var ForwardToRole = $('#ForwardToRole').val();
                var division = '0';
                if ($('#pageName').val() == 'F') {
                    //ForwardToRole = $('#ForwardToRole').val();
                    division = $('#ToLocationId').val();
            }
            var app = $('#ApplicationID').val();
           
            $("#divLoader").show(); $('.barcontainer').hide();
            $.ajax({
                url: "/ApprovalProcess/GetOfficerLocation",
                    type: "POST",
                    data: { divisionId: division, roleName: role, userid: userid, appId: app },
                    complete: function (r) {
                        $("#divLoader").hide();
                        $('.barcontainer').show();
                    },
                    success: function (data) {//roleName

                        //if (role == 'DWS,CIRCLE OFFICER') {
                        //    $.each(data, function (key, value) {
                        //        if (value.roleName == 'DWS') {
                        //            var dropdown = $("#SelectedDivisionId");
                        //            dropdown.empty();
                        //            $('#dv').css('display', 'none');
                        //        } else {
                        //            $('#dv').css('display', 'block');
                        //            populateDropdown(data, "DivisionId", $('#pageName').val());
                        //        }
                        //    });

                        //} else {
                            populateDropdown(data, "DivisionId", $('#pageName').val());
                        //}
                    },
                    failure: function (f) {
                        alert(f);
                    },
                    error: function (e) {
                        alert('Error ' + e);
                    }
                });
            //}
        });
    
    $("#IsDrainNotified").change(function () {
        if (this.checked) {
            $('#DrainWidth').text("Width as per Notification");
        } else {
            $('#DrainWidth').text("Width as per Calculations");
        }
    });

    $("body").on("input", ".numericField,.numericField1", function () {
        // Remove non-numeric characters using a regular expression
        $(this).val($(this).val().replace(/[^1-9.]/g, ''));

        // Remove multiple dots, leaving only one
        if ($(this).val().split('.').length > 2) {
            $(this).val($(this).val().replace(/\.+$/, ''));
        }
        if (parseFloat($(this).val()) <= 0) {
            $(this).val($(this).val().replace(/[^1-9.]/g, ''));
        }
    });
    if ($('.numericField').val() == '0') $('.numericField').val('');
    
});
