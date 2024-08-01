if (document.getElementById("SelectedProjectTypeId") != null) {
    document.getElementById("SelectedProjectTypeId").addEventListener("change", function () {
        var selectedText = this.options[this.selectedIndex].text;
        if (selectedText != 'Any Other') {
            $('.other').css('display', 'none');
            $('#IsOtherType').val(0);
        } else {
            $('.other').css('display', '');
            $('#IsOtherType').val(1);
        }

    });
}
if (document.getElementById("SelectedNocTypeId") != null) {
    document.getElementById("SelectedNocTypeId").addEventListener("change", function () {
        var selectedText = this.options[this.selectedIndex].text;
        $('#PreviousDate').val('');
        if (selectedText == 'Extension of Existing Project') {
            $('#IsExtension').val(1);
            $('.existing').css('display', '');
        } else {
            $('.existing').css('display', 'none');
            $('#IsExtension').val(0);
        }

    });
}
function LoadFinalBlock(grantId, applicationid) {
   
    $('.finalSubmit').empty();
    var divs = `
                <hr />
                                        <form class="user" asp-action="Modify" asp-controller="Grant" method="post" id="finalForm">
                                        <div class="row float-start">
                                                <input type="checkbox" style="margin-left:8px;" id="IsConfirmed" data-val="true" data-val-required="The Confirm field is required." name="IsConfirmed" value="true">
                                                <span class="text-danger field-validation-valid" data-valmsg-for="IsConfirmed" data-valmsg-replace="true"></span>
                                                
                                        </div>
                                        <div class="row mt-4 mb-4">
                                            <input type="hidden" asp-for="FGrantId" id="FGrantId" value="`+ grantId +`" name="FGrantId" />
                                            <input type="hidden" asp-for="ApplicationID" id="FApplicationId" name="FApplicationId" value="`+ applicationid +`"/>
                                            <label asp-for="IsConfirmed">
                                                I / We confirm that the information furnished
                                                herewith is correct to the best of my/our
                                                knowledge and nothing has been concealed
                                                therein. The Government would be at liberty
                                                to take suitable action against the
                                                unit/project and the person(s) responsible
                                                who failed to comply with the provisions of
                                                the Law. Rules, Regulations & Directions in
                                                case the information /Documents are found
                                                to be incorrect /false/misleading the
                                                government may withdraw the NOC issued
                                                for the project
                                            </label>
                                        </div>
                                        <div class="row isFinal">
                                            <div class="col-sm-6 offset-sm-6">
                                                <button type="submit" id="btnFinalSubmit" class="btn btn-primary btn-user btn-block">Final Submit</button>
                                            </div>
                                        </div>
                                        <div class="row">
                                            <div id="resultfinalMessage" class="mt-3 error-message" style="display:none;">
                                                <div class="alert alert-success"><span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>
                                            </div>
                                        </div>
                                        </form>`;
    $('.finalSubmit').html(divs);
}
var today = new Date().toISOString().split('T')[0];
if (document.getElementById('PreviousDate') != null && document.getElementById('PreviousDate') != undefined)
    document.getElementById('PreviousDate').setAttribute('max', today);
function toggleValue(module) {
    var resultProjectMessage = $('#result' + module + 'Message');
    resultProjectMessage.css('display', 'none');
    resultProjectMessage.empty();
};

function populateDropdown(data, dropdownId) {
    var dropdown = $("#Selected" + dropdownId);
    dropdown.html('<option value="">Select</option>');
    $.each(data, function (key, value) {
        dropdown.append('<option value="' + value.value + '">' + value.text + '</option>');
    });
}


function validateMob(mobno, field) {
    var fieldRegex = /^\d{10}$/;

    if (parseInt(mobno) < 0) {
        field.val('');
        field.next(".text-danger").text("Please enter a valid mobile number.");
    }
    else if (!fieldRegex.test(mobno)) {
        // PinCode.addClass("is-invalid");
        field.next(".text-danger").text("Please enter a valid mobile number.");
    } else {
        // PinCode.removeClass("is-invalid");
        field.next(".text-danger").text("");
    }
}
$(".pincode").on("blur", function () {
    validatePin($(this).val(), $(this));
});
function validatePin(mobno, field) {
    var fieldRegex = /^\d{6}$/;

    if (mobno == '') {
        field.val('');
        field.next(".text-danger").text("Pincode field is required.");
    }
    else if (parseInt(mobno) < 0) {
        field.val('');
        field.next(".text-danger").text("Please enter a valid pincode.");
    }
    else if (!fieldRegex.test(mobno)) {
        // PinCode.addClass("is-invalid");
        field.next(".text-danger").text("Please enter a valid pincode.");
    } else {
        // PinCode.removeClass("is-invalid");
        field.next(".text-danger").text("");
    }
}
$(function () {
    $('#IDProofPhoto').on('change', function () {
        if ((this.files[0].size) > (4 * 1024 * 1024)) {
            $('#IDProofPhoto-error').text('File size cannot be greater than 4MB');
        }
    });
    $('#AuthorizationLetterPhoto').on('change', function () {
        if ((this.files[0].size) > (4 * 1024 * 1024)) {
            $('#AuthorizationLetterPhoto-error').text('File size cannot be greater than 4MB');
        }
    });
    $('#AddressProofPhoto').on('change', function () {
        if ((this.files[0].size) > (4 * 1024 * 1024)) {
            $('#AddressProofPhoto-error').text('File size cannot be greater than 4MB');
        }
    });
    $('#KMLFile').on('change', function () {
        if ((this.files[0].size) > (4 * 1024 * 1024)) {
            $('#KMLFile-error').text('File size cannot be greater than 4MB');
        }
    });
    $(".mobno,.pincode").on("keypress", function (event) {
        // Allow only numeric characters (0-9) in the input field
        var charCode = event.which;
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
        }
    });

    $(".village").on("blur", function () {
        validateVillage($(this).val(), $(this));
    });
    function validateVillage(mobno, field) {
        if (mobno.length > 70) {
            // PinCode.addClass("is-invalid");
            field.next(".text-danger").text("Village name cannot be exceeded 70 characters.");
        } else {
            // PinCode.removeClass("is-invalid");
            field.next(".text-danger").text("");
        }
    }

    $('.mobno,.pincode,.village').on('cut copy paste', function (e) {
        e.preventDefault(); return false;
    });
    $(".mobno").on("blur", function () {
        validateMob($(this).val(), $(this));
    });
    $("body").on("input", ".numericField", function () {
        // Remove non-numeric characters using a regular expression
        $(this).val($(this).val().replace(/[^0-9.]/g, ''));

        // Remove multiple dots, leaving only one
        if ($(this).val().split('.').length > 2) {
            $(this).val($(this).val().replace(/\.+$/, ''));
        }
    });
    $('.custom-file-input3').on('change', function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label3').html(fileName);
    });
    $('.custom-file-input1').on('change', function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label1').html(fileName);
    });
    $('.custom-file-input2').on('change', function () {
        var fileName = $(this).val().split("\\").pop();
        $(this).next('.custom-file-label2').html(fileName);
    });

    if (document.getElementById("SelectedProjectTypeId") != null) {
        var selectedprojectText = document.getElementById("SelectedProjectTypeId").options[document.getElementById("SelectedProjectTypeId").selectedIndex].text;
        if (selectedprojectText != 'Any Other') {
            $('.other').css('display', 'none');
            $('#IsOtherType').val(0);
        } else {
            $('.other').css('display', '');
            $('#IsOtherType').val(1);
        }
    }
    if (document.getElementById("SelectedNocTypeId") != null) {
        var selectedText = document.getElementById("SelectedNocTypeId").options[document.getElementById("SelectedNocTypeId").selectedIndex].text;
        if (selectedText == 'Extension of Existing Project') {
            $('#IsExtension').val(1);
            $('.existing').css('display', '');
        } else {
            $('.existing').css('display', 'none');
            $('#IsExtension').val(0);
            $('#NocNumber').val('');
            $('#PreviousDate').val('');
        }
    }

    //if (document.getElementById("SelectedVillageID") != null) {
    //    $("#SelectedVillageID").change(function () {
    //        var villageId = $(this).val();
    //        $.ajax({
    //            url: "/Grant/GetVillageDetail",
    //            type: "POST",
    //            data: { villageId: villageId },
    //            async: false,
    //            success: function (data) {
    //                $.each(data, function (key, value) {
    //                    if (key == 'pinCode') {
    //                        $('#Pincode').text(value);
    //                    }
    //                });
    //            },
    //            failure: function (f) {
    //                alert(f);
    //            },
    //            error: function (e) {
    //                alert('Error ' + e);
    //            }
    //        });
    //    });
    //}

    if (document.getElementById("SelectedDivisionId") != null) {
        $("#SelectedDivisionId").change(function () {
            var divisionId = $(this).val();
            $.ajax({
                url: "/Grant/GetSubDivisions",
                type: "POST",
                data: { divisionId: divisionId },
                async: false,
                success: function (data) {
                    populateDropdown(data, "SubDivisionId");
                    var dropdownlistTehsil = $("#SelectedTehsilBlockId");
                    dropdownlistTehsil.empty();
                    var dropdownlistVillage = $("#SelectedVillageId");
                    dropdownlistVillage.empty();
                    dropdownlistTehsil.html('<option value="">Select</option>');
                    dropdownlistVillage.html('<option value="">Select</option>');
                },
                failure: function (f) {
                    alert(f);
                },
                error: function (e) {
                    alert('Error ' + e);
                }
            });
            //$.ajax({
            //    url: "/Grant/GetTehsilBlocks",
            //    type: "POST",
            //    data: { divisionId: divisionId },
            //    async: false,
            //    success: function (data) {
            //        populateDropdown(data, "TehsilBlockId");
            //        var dropdownlistVillage = $("#SelectedVillageId");
            //        dropdownlistVillage.empty();
            //        dropdownlistVillage.html('<option value="">Select</option>');
            //    },
            //    failure: function (f) {
            //        alert(f);
            //    },
            //    error: function (e) {
            //        alert('Error ' + e);
            //    }
            //});
        });
    }
    //if (document.getElementById("SelectedSubDivisionId") != null) {
    //    $("#SelectedSubDivisionId").change(function () {
    //        var subDivisionId = $(this).val();
    //        $.ajax({
    //            url: "/Grant/GetTehsilBlocks",
    //            type: "POST",
    //            data: { subDivisionId: subDivisionId },
    //            async: false,
    //            success: function (data) {
    //                populateDropdown(data, "TehsilBlockId");
    //                var dropdownlistVillage = $("#SelectedVillageId");
    //                dropdownlistVillage.empty();
    //                dropdownlistVillage.html('<option value="">Select</option>');
    //            },
    //            failure: function (f) {
    //                alert(f);
    //            },
    //            error: function (e) {
    //                alert('Error ' + e);
    //            }
    //        });
    //    });

    //}
    //if (document.getElementById("SelectedTehsilBlockId") != null) {
    //    $("#SelectedTehsilBlockId").on('change', function () {
    //        var tehsilBlockId = $(this).val();
    //        $.ajax({
    //            url: "/Grant/GetVillagess",
    //            type: "POST",
    //            data: { tehsilBlockId: tehsilBlockId },
    //            async: false,
    //            success: function (data) {
    //                populateDropdown(data, "VillageID");
    //            },
    //            failure: function (f) {
    //                alert(f);
    //            },
    //            error: function (e) {
    //                alert('Error ' + e);
    //            }
    //        });
    //    });
    //}
    //if (document.getElementById("finalForm") != null) {
    //    $('#finalForm').on('submit', function (event) {
    //        var FGrantId = $('#FGrantId').val();
    //        var FApplicationId = $('#FApplicationId').val();
    //        var IsConfirmed = $('#IsConfirmed').prop('checked');
    //        event.preventDefault(); // Prevent the form from submitting normally
    //        var module = 'final';

    //        var resultProjectMessage = $('#result' + module + 'Message');
    //        if (
    //            IsConfirmed == false) {
    //            resultProjectMessage.css('display', 'block');
    //            resultProjectMessage.html('<div class="alert alert-danger">Please check checkbox.<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
    //            return false;
    //        } 
    //        //allValid = ValidateInputFields(module, module);
    //        if (IsConfirmed==true) {
                
    //            ToggleLoadder(true);
    //            $.ajax({
    //                url: "/Grant/ModifyFinalSubmit",
    //                type: "POST",
    //                data: { IsConfirmed: IsConfirmed, FApplicationId: FApplicationId, FGrantId: FGrantId },
    //                complete: function (r) {

    //                    ToggleLoadder(false);
    //                },

    //                success: function (response) {

    //                    if (response.success) {
    //                        resultProjectMessage.css('display', 'block');
    //                        resultProjectMessage.html('<div class="alert alert-success">Form submitted successfully!<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                            
    //                    } else {
    //                        var errors = response.errors.join('<br/>');
    //                        resultProjectMessage.css('display', 'block');
    //                        resultProjectMessage.html('<div class="alert alert-danger">' + errors + '<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
    //                    }
    //                },
    //                error: function () {
    //                    resultProjectMessage.css('display', 'block');
    //                    resultProjectMessage.html('<div class="alert alert-danger">An error occurred while saving the project.<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
    //                }
    //            });
    //        }
    //    });
    //}
    if (document.getElementById("projectForm") != null) {
        $('#projectForm').on('submit', function (event) {
            /*$('#btnprojectSubmit').click(function () {*/
            var pid = $('#PId').val();
            var projectid = $('#projectid').val();
            var projectApplicationId = $('#ProjectApplicationId').val();
            var name = $('#Name').val();
            var ProjectTypeId = parseInt($('#SelectedProjectTypeId').val());
            var otherProjectTypeDetail = $('#OtherProjectTypeDetail').val();
            var isOtherType = $('#IsOtherType').val();
            event.preventDefault(); // Prevent the form from submitting normally
            allValid = ValidateFields('Project', 'project');
            if (allValid) {
                var inputValidate = true;
                if (isOtherType == '1') {
                    inputValidate = ValidateInputFields('Project', 'project');
                }
                var module = 'Project';
                if (allValid && inputValidate) {
                    var resultProjectMessage = $('#resultProjectMessage');
                    var navtab = $('#nav-project-tab');
                    ToggleLoadder(true);
                    $.ajax({
                        url: "/Grant/ModifyProject",
                        type: "POST",
                        data: { ProjectApplicationId: projectApplicationId, PId: pid, Name: name, SelectedProjectTypeId: ProjectTypeId, OtherProjectTypeDetail: otherProjectTypeDetail, IsOtherTypeSelected: isOtherType, projectid: projectid },
                        complete: function (r) {

                            ToggleLoadder(false);
                        },

                        success: function (response) {

                            if (response.success) {
                                navtab.empty();
                                navtab.html('<i class="checkmark">✓</i>Project Details')
                                resultProjectMessage.css('display', 'block');
                                resultProjectMessage.html('<div class="alert alert-success">Detail saved successfully!<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                                $('#projectForm')[0].reset();
                                $('#AreAllSectionCompleted').val(response.completed);
                                if ($('#AreAllSectionCompleted').val() == '0') {
                                    LoadFinalBlock(pid, projectApplicationId);                               
                                }
                            } else {
                                var errors = response.errors.join('<br/>');
                                resultProjectMessage.css('display', 'block');
                                resultProjectMessage.html('<div class="alert alert-danger">' + errors + '<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                            }
                        },
                        error: function () {
                            resultProjectMessage.css('display', 'block');
                            resultProjectMessage.html('<div class="alert alert-danger">An error occurred while saving the project.<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                        }
                    });
                }
            }
        });
    }
    if (document.getElementById("addressForm") != null) {
        $('#addressForm').on('submit', function (event) {
            event.preventDefault();
            var module = 'address';
            var formData = new FormData();
            var AdId = $('#AdId').val();
            var addressid = $('#addressid').val();
            var AddressApplicationId = $('#AddressApplicationId').val();
            var Hadbast = $('#Hadbast').val();
            var PlotNo = parseInt($('#PlotNo').val());
            var SelectedTehsilBlockId = $('#SelectedTehsilBlockId').val();
            var VillageName = $('#SelectedVillageID').val();
            var Pincode = $('#Pincode').val();
            var SelectedSubDivisionId = $('#SelectedSubDivisionId').val();
            var fileInput = $('#AddressProofPhoto')[0];
            var SelectedPlanSanctionAuthorityId = $('#SelectedPlanSanctionAuthorityId').val();
            var allFileValid = true;
            allFileValid = ValidFileField(fileInput, module);
            if (allFileValid) {
                allFileValid = ValidFileField($('#LayoutPlanFilePhoto')[0], module);
                if (allFileValid) {
                    allFileValid = ValidFileField($('#FaradFilePoto')[0], module);
                    if (allFileValid) {
                        var allValid = true;
                        allValid = ValidateFields(module, module);
                        if (allValid) {
                            var hval = '1', pval = '1';
                            var resultProjectMessage = $('#result' + module + 'Message');
                            if (!Hadbast) {
                                hval = '0';
                            }
                            if (!PlotNo) {
                                pval = '0';
                            }
                            allHValid = true;
                            if (hval == '0' && pval == '0') {
                                allHValid = false;
                                resultProjectMessage.css('display', 'block');
                                resultProjectMessage.html('<div class="alert alert-danger">Atleast one field is required to fill out of Hadbast/Plot No.<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module + '\')">&times;</span></div>');
                            }
                            if (allValid && allHValid && allFileValid) {
                                /*var file = fileInput.files[0];*/
                                formData.append('file', $('#AddressProofPhoto')[0].files[0]);
                                //formData.append('ownerName', 'test');
                                //formData.append('address', 'testing');
                                formData.append('addressApplicationId', AddressApplicationId);
                                formData.append('adId', AdId);
                                formData.append('hadbast', Hadbast);
                                formData.append('plotNo', PlotNo);
                                formData.append('selectedTehsilBlockId', SelectedTehsilBlockId);
                                formData.append('villageName', VillageName);
                                formData.append('pincode', Pincode);
                                formData.append('selectedSubDivisionId', SelectedSubDivisionId);
                                formData.append('addressid', addressid);
                                formData.append('selectedPlanSanctionAuthorityId', SelectedPlanSanctionAuthorityId);
                                formData.append('layoutPlanFilePhoto', $('#LayoutPlanFilePhoto')[0].files[0]);
                                formData.append('faradFilePoto', $('#FaradFilePoto')[0].files[0]);
                                ToggleLoadder(true);
                                var navtab = $('#nav-address-tab');
                                $.ajax({
                                    url: '/uploadaddressproof',
                                    type: 'POST',
                                    data: formData,
                                    contentType: false,
                                    processData: false,
                                    complete: function (r) {

                                        ToggleLoadder(false);
                                    },
                                    success: function (response) {
                                        //console.log(JSON.stringify(response))
                                        if (response.success) {
                                            navtab.empty();
                                            navtab.html('<i class="checkmark">✓</i>Site Address Details');
                                            $('#AddressProofPhotoPath').attr('href', response.filepath);
                                            $('#LayoutPlanFilePhoto').attr('href', response.filepath2);
                                            $('#FaradFilePoto').attr('href', response.filepath3);
                                            
                                            $('#PlotNo').val(response.result.plotNo);
                                            $('#SelectedVillageID').val(response.result.villageName);
                                            $('#Pincode').val(response.result.pinCode);
                                            $('#Hadbast').val(response.result.hadbast);
                                            resultProjectMessage.css('display', 'block');
                                            resultProjectMessage.html('<div class="alert alert-success">Detail saved successfully!<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module + '\')">&times;</span></div>');
                                            $('#' + module + 'Form')[0].reset();
                                            $('#AreAllSectionCompleted').val(response.completed);
                                            if ($('#AreAllSectionCompleted').val() == '0') {
                                                LoadFinalBlock(AdId, AddressApplicationId);
                                            }
                                        } else {
                                            var errors = response.errors.join('<br/>');
                                            resultProjectMessage.css('display', 'block');
                                            resultProjectMessage.html('<div class="alert alert-danger">' + errors + '<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module + '\')">&times;</span></div>');
                                        }
                                        //alert('File uploaded successfully to ' + response.path);
                                    },
                                    error: function () {
                                        resultProjectMessage.css('display', 'block');
                                        resultProjectMessage.html('<div class="alert alert-danger">An error occurred while saving the address.<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module + '\')">&times;</span></div>');
                                    }
                                });
                            }
                        }
                    }
                }
            }
        });
    }
 
    if (document.getElementById("kmlForm") != null) {
        $('#kmlForm').on('submit', function (event) {
            event.preventDefault();
            var module = 'kml';
            var formData = new FormData();
            var KmlGrantId = $('#KmlGrantId').val();
            var kmlApplicationId = $('#kmlApplicationId').val();
            var KmlLinkName = $('#KmlLinkName').val();
            var kmlid = $('#kmlid').val();
            var fileInput = $('#KMLFile')[0];

            var allFileValid = true;
            allFileValid = ValidFileField(fileInput, module);
            if (allFileValid) {
                var allValid = true;
                allValid = ValidateFields(module, module);
                if (allValid) {
                    var resultProjectMessage = $('#result' + module + 'Message');

                    if (allValid && allFileValid) {
                        var file = fileInput.files[0];
                        formData.append('kmlFile', file);
                        formData.append('kmlApplicationId', kmlApplicationId);
                        formData.append('kmlGrantId', KmlGrantId);
                        formData.append('kmlLinkName', KmlLinkName);
                        formData.append('kmlid', kmlid);

                        ToggleLoadder(true);
                        var navtab = $('#nav-kml-tab');
                        $.ajax({
                            url: '/uploadkml',
                            type: 'POST',
                            data: formData,
                            contentType: false,
                            processData: false,
                            complete: function (r) {

                                ToggleLoadder(false);
                            },
                            success: function (response) {
                                if (response.success) {
                                    navtab.empty();
                                    navtab.html('<i class="checkmark">✓</i>Site KML File Details');
                                    $('#KMLFilePath').attr('href', response.filepath);
                                    resultProjectMessage.css('display', 'block');
                                    resultProjectMessage.html('<div class="alert alert-success">Detail saved successfully!<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                                    //$('#' + module + 'Form')[0].reset();
                                    $('#AreAllSectionCompleted').val(response.completed);
                                    
                                    if ($('#AreAllSectionCompleted').val() == '0') {
                                        LoadFinalBlock(KmlGrantId, kmlApplicationId);
                                    }
                                } else {
                                    var errors = response.errors.join('<br/>');
                                    resultProjectMessage.css('display', 'block');
                                    resultProjectMessage.html('<div class="alert alert-danger">' + errors + '<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                                }
                                //alert('File uploaded successfully to ' + response.path);
                            },
                            error: function () {
                                resultProjectMessage.css('display', 'block');
                                resultProjectMessage.html('<div class="alert alert-danger">An error occurred while saving the kml detail.<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                            }
                        });
                    }
                }
            }
        });
    }

    if (document.getElementById("permissionForm") != null) {
        $('#permissionForm').on('submit', function (event) {
            var module = 'permission';
            var PermisionGrantId = $('#PermisionGrantId').val();
            var permissionApplicationId = $('#permissionApplicationId').val();
            var PreviousDate = $('#PreviousDate').val();
            var SelectedNocPermissionTypeID = parseInt($('#SelectedNocPermissionTypeID').val());
            var SelectedNocTypeId = $('#SelectedNocTypeId').val();
            var NocNumber = $('#NocNumber').val();
            var IsExtension = $('#IsExtension').val();
            var permissionid = $('#permissionid').val();
            event.preventDefault(); // Prevent the form from submitting normally
            allValid = ValidateFields(module, module);
            if (allValid) {
                if (IsExtension == '1') {
                    allValid = ValidateInputFields('permission', 'permission');
                }

                if (allValid) {
                    var resultProjectMessage = $('#resultpermissionMessage');
                    ToggleLoadder(true);
                    var navtab = $('#nav-permission-tab');
                    $.ajax({
                        url: "/Grant/ModifyPermissionDetail",
                        type: "POST",
                        data: { permissionApplicationId: permissionApplicationId, permissionGrantId: PermisionGrantId, SelectedNocPermissionTypeID: SelectedNocPermissionTypeID, SelectedNocTypeId: SelectedNocTypeId, NocNumber: NocNumber, IsExtension: IsExtension, PreviousDate: PreviousDate, permissionid: permissionid },
                        complete: function (r) {

                            ToggleLoadder(false);
                        },
                        success: function (response) {

                            if (response.success) {
                                navtab.empty();
                                navtab.html('<i class="checkmark">✓</i>Types of Permissions');
                                resultProjectMessage.css('display', 'block');
                                resultProjectMessage.html('<div class="alert alert-success">Detail saved successfully!<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                                //$('#projectForm')[0].reset();
                                $('#AreAllSectionCompleted').val(response.completed);
                                if ($('#AreAllSectionCompleted').val() == '0') {
                                    LoadFinalBlock(PermisionGrantId, permissionApplicationId);
                                }
                            } else {
                                var errors = response.errors.join('<br/>');
                                resultProjectMessage.css('display', 'block');
                                resultProjectMessage.html('<div class="alert alert-danger">' + errors + '<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                            }
                        },
                        error: function () {
                            resultProjectMessage.css('display', 'block');
                            resultProjectMessage.html('<div class="alert alert-danger">An error occurred while saving the noc permission detail.<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                        }
                    });
                }
            }
        });
    }

    if (document.getElementById("applicantForm") != null) {
        $('#applicantForm').on('submit', function (event) {
            event.preventDefault();
            var module = 'applicant';
            var formData = new FormData();
            var applicantid = $('#applicantid').val();
            var ApplicantGrantId = $('#ApplicantGrantId').val();
            var applicantApplicationId = $('#applicantApplicationId').val();
            var ApplicantName = $('#ApplicantName').val();
            var ApplicantEmailID = $('#ApplicantEmailID').val();
            var fileInputId = $('#IDProofPhoto')[0];
            var fileInputAuth = $('#AuthorizationLetterPhoto')[0];
            var ownersecid = $('#ownersecid').val();
            
            var allFileValid = true; 
            allFileValid = ValidFileField(fileInputId, module);
            if (allFileValid) {
                
                allFileValid = ValidFileField(fileInputAuth, module);

                if (allFileValid) {
                    var allValid = true;
                    allValid = ValidateFields(module, module);
                    if (allValid) {
                        var resultProjectMessage = $('#result' + module + 'Message');

                        if (allValid && allFileValid) {
                            formData.append('idProofPhotoFile', fileInputId.files[0]);
                            formData.append('authorizationLetterPhotofile', fileInputAuth.files[0]);
                            formData.append('applicantApplicationId', applicantApplicationId);
                            formData.append('applicantGrantId', ApplicantGrantId);
                            formData.append('applicantName', ApplicantName);
                            formData.append('applicantEmailID', ApplicantEmailID);
                            formData.append('ownersecid', ownersecid);
                            formData.append('applicantid', applicantid);

                            ToggleLoadder(true);
                            var navtab = $('#nav-applicant-tab');
                            $.ajax({
                                url: '/uploadapplicant',
                                type: 'POST',
                                data: formData,
                                contentType: false,
                                processData: false,
                                complete: function (r) {

                                    ToggleLoadder(false);
                                },
                                success: function (response) {
                                    if (response.success) {
                                        navtab.empty();
                                        navtab.html('<i class="checkmark">✓</i>Applicant Details');
                                        $('#IDProofPhotoPath').attr('href', response.idfilePath);
                                        $('#AuthorizationLetterPhotoPath').attr('href', response.authfilePath);
                                        resultProjectMessage.css('display', 'block');
                                        resultProjectMessage.html('<div class="alert alert-success">Detail saved successfully!<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                                        $('#' + module + 'Form')[0].reset();
                                        $('#AreAllSectionCompleted').val(response.completed);
                                        if ($('#AreAllSectionCompleted').val() == '0') {
                                            LoadFinalBlock(ApplicantGrantId, applicantApplicationId);
                                        }
                                    } else {
                                        var errors = response.errors.join('<br/>');
                                        resultProjectMessage.css('display', 'block');
                                        resultProjectMessage.html('<div class="alert alert-danger">' + errors + '<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                                    }
                                    //alert('File uploaded successfully to ' + response.path);
                                },
                                error: function () {
                                    resultProjectMessage.css('display', 'block');
                                    resultProjectMessage.html('<div class="alert alert-danger">An error occurred while saving the applicant detail.<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                                }
                            });
                        }
                    }
                }
            }
        });
    }
    if (document.getElementById("ownerForm") != null)
    {
        $('#ownerForm').on('submit', function (e) {
            e.preventDefault(); // Prevent the form from submitting normally
            var OwnerGrantId = $('#OwnerGrantId').val();
            var OwnerApplicationId = $('#OwnerApplicationId').val();
            var OwnerId = $('#OwnerId').val();
            var SelectedOwnerTypeID = $('#SelectedOwnerTypeID').val();
            var OwnerName = $('#OwnerName').val();
            var OwnerAddress = $('#OwnerAddress').val();
            var OwnerMobileNo = $('#OwnerMobileNo').val();
            var OwnerEmail = $('#OwnerEmail').val();
            var ownersecid = $('#ownersecid').val();
            var module = 'owner';
            allValid = ValidateFields(module, module);
            inputValidate = ValidateInputFields(module, module);
            //if (allValid) {

            if (allValid && inputValidate) {
                var resultProjectMessage = $('#result' + module + 'Message');

                ToggleLoadder(true);
                var navtab = $('#nav-owner-tab');
                $.ajax({
                    url: "/Grant/ModifyOwnerDetail",
                    type: "POST",
                    data: { OwnerApplicationId: OwnerApplicationId, OwnerGrantId: OwnerGrantId, OwnerId: OwnerId, SelectedOwnerTypeID: SelectedOwnerTypeID, OwnerName: OwnerName, OwnerAddress: OwnerAddress, OwnerMobileNo: OwnerMobileNo, OwnerEmail: OwnerEmail, ownersecid: ownersecid },
                    complete: function (r) {

                        ToggleLoadder(false);
                    },
                    success: function (response) {

                        if (response.success) {
                            navtab.empty();
                            navtab.html('<i class="checkmark">✓</i>Details of Owners/Partners/Chief Executive/Full Time Directors');
                            var body = $('#ownerBody');
                            var table = $("#ownerTable");
                            if (response.grantownerdetail != null) {
                                var count = 1;
                                var len = response.grantownerdetail.length;
                                if (len > 0) {
                                    body.empty();
                                    $.each(response.grantownerdetail, function (key, value) {
                                        var tr = '<tr>';
                                        tr += '<td><span class="srOwner">' + count + '</span></td>';
                                        tr += '<td><input type="hidden" id = "Owner' + value.ownerId + '" /><label id="OwnerTypeName' + value.ownerId + '" class="form-control form-control-user">' + value.ownerTypeName + '</label><input hidden id="OwnerTypeID' + value.selectedOwnerTypeID + '" /> </td > ';
                                        tr += '<td><label id="Name' + value.ownerId + '" class="form-control form-control-user">' + value.ownerName + '</label></td>';
                                        tr += '<td><label id="Address' + value.ownerId + '" class="form-control form-control-user">' + value.ownerAddress + '</label></td>';
                                        tr += '<td><label id="MobileNo' + value.ownerId + '" class="form-control form-control-user">' + value.ownerMobileNo + '</label></td>';
                                        tr += '<td><label id="Email' + value.ownerId + '" class="form-control form-control-user">' + value.ownerEmail + '</label></td>';
                                        tr += '<td><a href="#" onclick="SetEdit(' + value.ownerId + ',\'' + value.ownerName + '\',\'' + value.ownerAddress + '\',\'' + value.ownerMobileNo + '\',\'' + value.ownerEmail + '\',' + value.selectedOwnerTypeID + ')" class="btn btn-warning">Edit</a></td>';
                                        tr += '</tr>';
                                        body.append(tr);
                                        count++;
                                    });
                                }
                            }
                            ResetControls();
                            resultProjectMessage.css('display', 'block');
                            resultProjectMessage.html('<div class="alert alert-success">Detail saved successfully!<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                            // $('#projectForm')[0].reset();
                            $('#AreAllSectionCompleted').val(response.completed);
                            if ($('#AreAllSectionCompleted').val() == '0') {
                                LoadFinalBlock(OwnerGrantId, OwnerApplicationId);
                            }
                        } else {
                            var errors = response.errors.join('<br/>');
                            resultProjectMessage.css('display', 'block');
                            resultProjectMessage.html('<div class="alert alert-danger">' + errors + '<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                        }
                    },
                    error: function () {
                        resultProjectMessage.css('display', 'block');
                        resultProjectMessage.html('<div class="alert alert-danger">An error occurred while saving the owner details.<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                    }
                });
            }
            //}
        });
    }

    if (document.getElementById("areaForm") != null) {
        $('#areaForm').on('submit', function (e) {
            e.preventDefault(); // Prevent the form from submitting normally
            var kid = $('#KId').val();
            var areaApplicationId = $('#AreaApplicationId').val();
            var KhasraId = $('#KhasraId').val();
            var kUnitValue = $('#KUnitValue').val();
            var kTimesof = $('#KTimesof').val();
            var kDivideBy = $('#KDivideBy').val();
            var mUnitValue = $('#MUnitValue').val();
            var mTimesof = $('#MTimesof').val();
            var mDivideBy = $('#MDivideBy').val();
            var sUnitValue = $('#SUnitValue').val();
            var sTimesof = $('#STimesof').val();
            var sDivideBy = $('#SDivideBy').val();
            var SelectedSiteAreaUnitId = $('#SelectedSiteAreaUnitId').val();
            var KhasraNo = $('#KhasraNo').val();
            var KanalOrBigha = $('#KanalOrBigha').val();
            var MarlaOrBiswa = $('#MarlaOrBiswa').val();
            var SarsaiOrBiswansi = $('#SarsaiOrBiswansi').val();
            var module = 'area';
            allValid = ValidateFields(module, module);
            if (allValid) {
                inputValidate = ValidateInputFields(module, module);

                if (allValid && inputValidate) {
                    var resultProjectMessage = $('#result' + module + 'Message');

                    ToggleLoadder(false);
                    $.ajax({
                        url: "/Grant/ModifyAreaDetail",
                        type: "POST",
                        data: { AreaApplicationId: areaApplicationId, KId: kid, KhasraId: KhasraId, KhasraNo: KhasraNo, SelectedSiteAreaUnitId: SelectedSiteAreaUnitId, MarlaOrBiswa: MarlaOrBiswa, KanalOrBigha: KanalOrBigha, SarsaiOrBiswansi: SarsaiOrBiswansi },
                        complete: function (r) {

                            ToggleLoadder(false);
                        },
                        success: function (response) {

                            if (response.success) {
                                resultProjectMessage.css('display', 'block');
                                resultProjectMessage.html('<div class="alert alert-success">Detail saved successfully!<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                                $('#projectForm')[0].reset();
                            } else {
                                var errors = response.errors.join('<br/>');
                                resultProjectMessage.css('display', 'block');
                                resultProjectMessage.html('<div class="alert alert-danger">' + errors + '<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                            }
                        },
                        error: function () {
                            resultProjectMessage.css('display', 'block');
                            resultProjectMessage.html('<div class="alert alert-danger">An error occurred while saving the project.<span class="close-icon" style="float:right" onclick="toggleValue(\'' + module +'\')">&times;</span></div>');
                        }
                    });
                }
            }
        });
    }

    $('#KanalOrBigha').on('change', function (e) {
        var kanal = $(this).val();
        var marla = $('#MarlaOrBiswa').val();
        var sarsai = $('#SarsaiOrBiswansi').val();
        var KUnitValue = $('#KUnitValue').val();
        var KTimesof = $('#KTimesof').val();
        var KDivideBy = $('#KDivideBy').val();
        var MUnitValue = $('#MUnitValue').val();
        var MTimesof = $('#MTimesof').val();
        var MDivideBy = $('#MDivideBy').val();
        var SUnitValue = $('#SUnitValue').val();
        var STimesof = $('#STimesof').val();
        var SDivideBy = $('#SDivideBy').val();
        KUnitValue = KUnitValue != '' ? parseFloat(KUnitValue) : 0;
        KTimesof = KTimesof != '' ? parseFloat(KTimesof) : 0;
        KDivideBy = KDivideBy != '' ? parseFloat(KDivideBy) : 0;
        MUnitValue = MUnitValue != '' ? parseFloat(MUnitValue) : 0;
        MTimesof = MTimesof != '' ? parseFloat(MTimesof) : 0;
        MDivideBy = MDivideBy != '' ? parseFloat(MDivideBy) : 0;
        SUnitValue = SUnitValue != '' ? parseFloat(SUnitValue) : 0;
        STimesof = STimesof != '' ? parseFloat(STimesof) : 0;
        SDivideBy = SDivideBy != '' ? parseFloat(SDivideBy) : 0;
        kanal = kanal != '' ? parseFloat(kanal) : 0;
        marla = marla != '' ? parseFloat(marla) : 0;
        sarsai = sarsai != '' ? parseFloat(sarsai) : 0;
        var total = ((kanal * KUnitValue * KTimesof) / KDivideBy) + ((marla * MUnitValue * MTimesof) / MDivideBy) + ((sarsai * SUnitValue * STimesof) / SDivideBy);
        var TotalArea = parseFloat($('#TotalArea').text()) + total;
        $('#TotalArea').text(TotalArea.toFixed(5));
        var totalSqFeet = (TotalArea * 43560);
        var totalSqMeter = (TotalArea * 4046.86);
        $('#TotalAreaSqFeet').text(totalSqFeet.toFixed(5));
        $('#TotalAreaSqMetre').text(totalSqMeter.toFixed(5));
    });
    $('#MarlaOrBiswa').on('change', function (e) {
        var marla = $(this).val();
        var kanal = $('#KanalOrBigha').val();
        var sarsai = $('#SarsaiOrBiswansi').val();
        var KUnitValue = $('#KUnitValue').val();
        var KTimesof = $('#KTimesof').val();
        var KDivideBy = $('#KDivideBy').val();
        var MUnitValue = $('#MUnitValue').val();
        var MTimesof = $('#MTimesof').val();
        var MDivideBy = $('#MDivideBy').val();
        var SUnitValue = $('#SUnitValue').val();
        var STimesof = $('#STimesof').val();
        var SDivideBy = $('#SDivideBy').val();
        KUnitValue = KUnitValue != '' ? parseFloat(KUnitValue) : 0;
        KTimesof = KTimesof != '' ? parseFloat(KTimesof) : 0;
        KDivideBy = KDivideBy != '' ? parseFloat(KDivideBy) : 0;
        MUnitValue = MUnitValue != '' ? parseFloat(MUnitValue) : 0;
        MTimesof = MTimesof != '' ? parseFloat(MTimesof) : 0;
        MDivideBy = MDivideBy != '' ? parseFloat(MDivideBy) : 0;
        SUnitValue = SUnitValue != '' ? parseFloat(SUnitValue) : 0;
        STimesof = STimesof != '' ? parseFloat(STimesof) : 0;
        SDivideBy = SDivideBy != '' ? parseFloat(SDivideBy) : 0;
        kanal = kanal != '' ? parseFloat(kanal) : 0;
        marla = marla != '' ? parseFloat(marla) : 0;
        sarsai = sarsai != '' ? parseFloat(sarsai) : 0;
        var total = ((kanal * KUnitValue * KTimesof) / KDivideBy) + ((marla * MUnitValue * MTimesof) / MDivideBy) + ((sarsai * SUnitValue * STimesof) / SDivideBy);
        var TotalArea = parseFloat($('#TotalArea').text()) + total;
        $('#TotalArea').text(TotalArea.toFixed(5));
        var totalSqFeet = (TotalArea * 43560);
        var totalSqMeter = (TotalArea * 4046.86);
        $('#TotalAreaSqFeet').text(totalSqFeet.toFixed(5));
        $('#TotalAreaSqMetre').text(totalSqMeter.toFixed(5));
    });
    $('#SarsaiOrBiswansi').on('change', function (e) {
        var sarsai = $(this).val();
        var marla = $('#MarlaOrBiswa').val();
        var kanal = $('#KanalOrBigha').val();
        var KUnitValue = $('#KUnitValue').val();
        var KTimesof = $('#KTimesof').val();
        var KDivideBy = $('#KDivideBy').val();
        var MUnitValue = $('#MUnitValue').val();
        var MTimesof = $('#MTimesof').val();
        var MDivideBy = $('#MDivideBy').val();
        var SUnitValue = $('#SUnitValue').val();
        var STimesof = $('#STimesof').val();
        var SDivideBy = $('#SDivideBy').val();
        KUnitValue = KUnitValue != '' ? parseFloat(KUnitValue) : 0;
        KTimesof = KTimesof != '' ? parseFloat(KTimesof) : 0;
        KDivideBy = KDivideBy != '' ? parseFloat(KDivideBy) : 0;
        MUnitValue = MUnitValue != '' ? parseFloat(MUnitValue) : 0;
        MTimesof = MTimesof != '' ? parseFloat(MTimesof) : 0;
        MDivideBy = MDivideBy != '' ? parseFloat(MDivideBy) : 0;
        SUnitValue = SUnitValue != '' ? parseFloat(SUnitValue) : 0;
        STimesof = STimesof != '' ? parseFloat(STimesof) : 0;
        SDivideBy = SDivideBy != '' ? parseFloat(SDivideBy) : 0;
        kanal = kanal != '' ? parseFloat(kanal) : 0;
        marla = marla != '' ? parseFloat(marla) : 0;
        sarsai = sarsai != '' ? parseFloat(sarsai) : 0;
        var total = ((kanal * KUnitValue * KTimesof) / KDivideBy) + ((marla * MUnitValue * MTimesof) / MDivideBy) + ((sarsai * SUnitValue * STimesof) / SDivideBy);
        var TotalArea = parseFloat($('#TotalArea').text()) + total;
        $('#TotalArea').text(TotalArea.toFixed(5));
        var totalSqFeet = (TotalArea * 43560);
        var totalSqMeter = (TotalArea * 4046.86);
        $('#TotalAreaSqFeet').text(totalSqFeet.toFixed(5));
        $('#TotalAreaSqMetre').text(totalSqMeter.toFixed(5));
    });

    const style = document.createElement('style');
    style.innerHTML = `
                .error-border {
                    border: 2px solid red;
                }`;
    document.head.appendChild(style);
});

function ValidFileField(fileInput, modulename) {
    
    var resultProjectMessage = $('#result' + modulename + 'Message');
    //if (fileInput.files.length == 0) {

    //    resultProjectMessage.css('display', 'block');
    //    resultProjectMessage.html('<div class="alert alert-danger">Please upload file<span class="close-icon" style="float:right" onclick="toggleValue(\'' + modulename +'\')">&times;</span></div>');

    //    return false;
    //}
    if (fileInput.files.length > 0) {
        var file = fileInput.files[0];
        if (!file) {

            resultProjectMessage.css('display', 'block');
            resultProjectMessage.html('<div class="alert alert-danger">Please upload address proof<span class="close-icon" style="float:right" onclick="toggleValue(\'' + modulename + '\')">&times;</span></div>');

            return false;
        }

        var validTypes = ['image/jpeg', 'image/png', 'application/pdf'];
        if (fileInput == "kml") validTypes = ['application/pdf'];
        var maxSize = 4 * 1024 * 1024; // 2 MB

        if ($.inArray(file.type, validTypes) === -1) {
            resultProjectMessage.css('display', 'block');
            if (fileInput == "kml")
                resultProjectMessage.html('<div class="alert alert-danger">Invalid file type. Only PDF files are allowed.<span class="close-icon" style="float:right" onclick="toggleValue(\'' + modulename + '\')">&times;</span></div>');
            else
                resultProjectMessage.html('<div class="alert alert-danger">Invalid file type. Only JPG, PNG, and PDF files are allowed.<span class="close-icon" style="float:right" onclick="toggleValue(\'' + modulename + '\')">&times;</span></div>');

            return false;
        }

        if (file.size > maxSize) {
            resultProjectMessage.css('display', 'block');
            resultProjectMessage.html('<div class="alert alert-danger">File size exceeds 4 MB.<span class="close-icon" style="float:right" onclick="toggleValue(\'' + modulename + '\')">&times;</span></div>');

            return false;
        }
    }
    return true;
}
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
        resultProjectMessage.html('<div class="alert alert-danger">' + errorMessage.textContent + '<span class="close-icon" style="float:right" onclick="toggleValue(\'' + modulename +'\')">&times;</span></div>');
    }
    return allValid;
}

function ValidateFields(modulename, formName) {
    const errorMessage = document.getElementById('result' + modulename + 'Message');
    //const fields = document.querySelectorAll('#' + formName +'Form input');

    let allValid = true;
    errorMessage.textContent = ''; // Clear previous error message
    const selectFields = document.querySelectorAll('#' + formName + 'Form select');
    selectFields.forEach(field => {
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
        resultProjectMessage.html('<div class="alert alert-danger">' + errorMessage.textContent + '<span class="close-icon" style="float:right" onclick="toggleValue(\'' + modulename +'\')">&times;</span></div>');
    }
    return allValid;
}

function SetEdit(id, name, address, mob, email, typeid) {
    $('#OwnerId').val(id);
    $('#SelectedOwnerTypeID').val(typeid);
    $('#OwnerName').val(name);
    $('#OwnerAddress').val(address);
    $('#OwnerMobileNo').val(mob);
    $('#OwnerEmail').val(email);
    $('#btnownerSubmit').text('Update');
    $('#btnownerCancel').css('visibility', 'visible');
}
function ResetControls() {
    $('#OwnerId').val(0);
    $('#OwnerName').val('');
    $('#OwnerAddress').val('');
    $('#OwnerMobileNo').val('');
    $('#OwnerEmail').val('');
    $('#SelectedOwnerTypeID').prop('selectedIndex', 0);
    $('#btnownerSubmit').text('Add More');
    $('#btnownerCancel').css('visibility', 'hidden');
}

function ToggleLoadder(isVisible) {
    if (isVisible) {
        $("#divLoader").show(); $('.default-tab').hide();
    } else {
        $("#divLoader").hide();
        $('.default-tab').show();
    }
}


    