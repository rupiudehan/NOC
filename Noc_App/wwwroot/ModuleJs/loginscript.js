﻿
function toggleValue(module) {
    var resultProjectMessage = $('#result' + module + 'Message');
    resultProjectMessage.css('display', 'none');
    resultProjectMessage.empty();
};
function ValidateInputFields(modulename, formName) {
    const errorMessage = document.getElementById('result' + modulename + 'Message');
    const fields = document.querySelectorAll('#' + formName + 'Form input');

    let allValid = true;
    errorMessage.textContent = ''; // Clear previous error message

    fields.forEach(field => {
        if (!field.value.trim()) {
            allValid = false;
            errorMessage.textContent = 'Please fill in all red marked fields.';
            field.classList.add('error-border');
        } else {
            field.classList.remove('error-border');
        }
    });
    if (!allValid) {
        var resultProjectMessage = $('#result' + modulename + 'Message');
        resultProjectMessage.css('display', 'block');
        resultProjectMessage.html('<div class="alert alert-danger">' + errorMessage.textContent + '<span class="close-icon" style="float:right" onclick="toggleValue(\'' + modulename + '\')">&times;</span></div>');
    }
    return allValid;
}

function ToggleLoadder(isVisible) {
    if (isVisible) {
        $("#divLoader").show(); $('#outerDiv').hide();
    } else {
        $("#divLoader").hide();
        $('#outerDiv').show();
    }
}

$(function () {

    $('#loginForm').on('submit', function (event) {
        event.preventDefault();
        ToggleLoadder(true);
        var email = $('#Email').val();
        var password = $('#Password').val();
        var token = $('#loginToken').val();
        var module = 'login';
        var resultProjectMessage = $('#result' + module + 'Message');
        var body = $('#tbroles');
        var table = $("#tblroles");
        body.empty();
        $.ajax({
            url: "/Account/Login",
            type: "POST",
            data: { Email: email, Password: password, Token: token },
            complete: function (r) {
                ToggleLoadder(false);
            },

            success: function (response) {
                if (response.success == '1') {
                    //$('#Name').val(response.name);
                    //$('#Designation').val(response.designation);
                    //$('#EmpID').val(response.empID);
                    //$('#DistrictID').val(response.districtID);
                    //$('#DivisionID').val(response.divisionID);
                    //$('#REmail').val(response.email);
                    $.each(response.roles, function (key, value) {
                        var tr = '<tr>';
                        tr += '<td><form action="RedirecToLoginRole" id="loginByRoleForm' + value.id + '" method="post" role="form" class="php-email-form"><input type="hidden" name="Name" id="Name" value="' + response.name + '" /><input type="hidden" name="Designation" id="Designation" value="' + response.designation + '" /><input type="hidden" name="EmpID" id="EmpID" value="' + response.empID + '" />';
                        tr += '<input type="hidden" name="DistrictID" id="DistrictID" value="' + response.districtID + '" /><input type="hidden" name="DivisionID" id="DivisionID" value="' + response.divisionID + '" />';
                        tr += '<input type="hidden" name="Email" id="REmail" value="' + response.email + '" />';
                        tr += '<input type="hidden" name="RoleID" id="RoleID" value="' + value.id + '"/><input type="hidden" name="role" id="rolename" value="' + value.roleName + '"/><div class="text-center" > <button type="submit" style="width:100%;background-color:#4c9e37" class="btn btn-success btn-user btn-block">' + value.roleName +'</button></div></form></td> ';
                        
                        tr += '</tr>';
                        body.append(tr);
                        $('.logRe').on('submit', function (event) {
                            event.preventDefault();
                        });
                    });
                    resultProjectMessage.css('display', 'none');
                    //resultProjectMessage.html('<div class="alert alert-success">LoggedIn successfully!<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module + '\')">&times;</span></div>');
                    $('#myModal').modal('show');
                } else {
                    var errors = response.errors;
                    resultProjectMessage.css('display', 'block');
                    resultProjectMessage.html('<div class="alert alert-danger">' + errors + '<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module + '\')">&times;</span></div>');
                }
            },
            error: function () {
                resultProjectMessage.css('display', 'block');
                resultProjectMessage.html('<div class="alert alert-danger">An error occurred while saving the project.<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module + '\')">&times;</span></div>');
            }
        });
    });
    
});
