function CheckDuplicate(control) {
    // alert($(control).val())
    $('#khasraTable').find('tbody>tr').each(function () {
        if ($(control).prop('name') != $(this).find('.khasra').prop('name')) {
            if ($(this).find('.khasra').val() == $(control).val()) {
                $('#ValidKhasra').val(0);
                $('#ValidKhasraName').val($(control).prop('name'));
                $(control).closest('div').find('span').html('<span>Khasra No. already exists</span>');
            } else {
                $('#ValidKhasra').val(1);
                $('#ValidKhasraName').val('');
                $(control).closest('div').find('span').html('');
            }
        }
    })
    $(control).val();
}
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
var today = new Date().toISOString().split('T')[0];
document.getElementById('PreviousDate').setAttribute('max', today);

$(function () {
    $('#grantForm').on('submit', function (e) {
        if ($('#ValidKhasra').val() == '0') {
            var name = $('#ValidKhasraName').val();
            $('input[name="' + name + '"]').closest('div').find('span').html('<span>Khasra No. already exists</span>');
            e.preventDefault();
            return false;
        }
    });
    document.getElementById("grantForm").addEventListener("submit", function (event) {
        var fileInputIDProofPhoto = document.getElementById("IDProofPhoto");
        var fileSizeIDProofPhoto = fileInputIDProofPhoto.files[0] != undefined ? fileInputIDProofPhoto.files[0].size : '';
        var fileInputAuthorizationLetterPhoto = document.getElementById("AuthorizationLetterPhoto");
        var fileSizeAuthorizationLetterPhoto = fileInputAuthorizationLetterPhoto.files[0] != undefined ? fileInputAuthorizationLetterPhoto.files[0].size : '';
        var fileInputAddressProofPhoto = document.getElementById("AddressProofPhoto");
        var fileSizeAddressProofPhoto = fileInputAddressProofPhoto.files[0] != undefined ? fileInputAddressProofPhoto.files[0].size : '';
        var fileInputKMLFile = document.getElementById("KMLFile");
        var fileSizeKMLFile = fileInputKMLFile.files[0] != undefined ? fileInputKMLFile.files[0].size : '';
        var maxSize = 4 * 1024 * 1024; // 10MB in bytes

        var summary = $('.validateAll');
        summary.find('ul li').each(function () {
            $(this).remove();
        });
        if (fileSizeIDProofPhoto > maxSize) {
            event.preventDefault(); // Prevent form submission
            document.getElementById("IDProofPhoto-error").textContent = "ID Proof File size cannot be greater than 4MB";
            summary.find('ul').append('<li id="IDProofPhotoSum">ID Proof File size cannot be greater than 4MB</li>');
            document.getElementById("IDProofPhoto-error").style.display = "block";
        } else {
            document.getElementById("IDProofPhoto-error").textContent = "";
            document.getElementById("IDProofPhoto-error").style.display = "none";
            summary.find('ul #IDProofPhotoSum').remove();
        }
        if (fileSizeAuthorizationLetterPhoto > maxSize) {
            event.preventDefault(); // Prevent form submission
            document.getElementById("AuthorizationLetterPhoto-error").textContent = "Authorization Letter File size cannot be greater than 4MB";
            summary.find('ul').append('<li id="AuthorizationLetterPhotoSum">Authorization Letter File size cannot be greater than 4MB</li>');
            document.getElementById("AuthorizationLetterPhoto-error").style.display = "block";
        } else {
            document.getElementById("AuthorizationLetterPhoto-error").textContent = "";
            document.getElementById("AuthorizationLetterPhoto-error").style.display = "none";
            summary.find('ul #AuthorizationLetterPhotoSum').remove();
        }
        if (fileSizeAddressProofPhoto > maxSize) {
            event.preventDefault(); // Prevent form submission
            document.getElementById("AddressProofPhoto-error").textContent = "Address Proof File size cannot be greater than 4MB";
            summary.find('ul').append('<li id="AddressProofPhotoSum">Address Proof File size cannot be greater than 4MB</li>');
            document.getElementById("AddressProofPhoto-error").style.display = "block";
        } else {
            document.getElementById("AddressProofPhoto-error").textContent = "";
            document.getElementById("AddressProofPhoto-error").style.display = "none";
            summary.find('ul #AddressProofPhotoSum').remove();
        }
        if (fileSizeKMLFile > maxSize) {
            event.preventDefault(); // Prevent form submission
            document.getElementById("KMLFile-error").textContent = "KML File size cannot be greater than 4MB";
            summary.find('ul').append('<li id="KMLFileSum">KML File size cannot be greater than 4MB</li>');
            document.getElementById("KMLFile-error").style.display = "block";
        } else {
            document.getElementById("KMLFile-error").textContent = "";
            document.getElementById("KMLFile-error").style.display = "none";
            summary.find('ul #KMLFileSum').remove();
        }
    });
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
    var total = 0.0;
    var totalSqFeet = 0.0;
    var totalSqMetre = 0.0;
    // if ($('#Latitude').val() == '0') $('#Latitude').val('');
    // if ($('#Longitute').val() == '0') $('#Longitute').val('');
    // if($('#SiteAreaOrSizeInFeet').val()=='0') $('#SiteAreaOrSizeInFeet').val('');
    // if ($('#SiteAreaOrSizeInInches').val() == '0') $('#SiteAreaOrSizeInInches').val('');

    $(".mobno,.pincode").on("keypress", function (event) {
        // Allow only numeric characters (0-9) in the input field
        var charCode = event.which;
        if (charCode < 48 || charCode > 57) {
            event.preventDefault();
        }
    });
    $('.mobno,.pincode').on('cut copy paste', function (e) {
        e.preventDefault(); return false;
    });
    $('body').on('change', '#SelectedSiteAreaUnitId', function () {
        var list = ($(this).find("option:selected").text()).split('/');
        var Bigha = 'K';
        var Biswa = 'M';
        var Biswansi = 'S';
        var unitId = $(this).val();
        $.ajax({
            url: "/Grant/GetUnitsDetail",
            type: "POST",
            data: { unitId: unitId },
            async: false,
            success: function (data) {
                $.each(data, function (key, value) {
                    $('#' + value.unitCode + 'UnitValue').val(value.unitValue);
                    $('#' + value.unitCode + 'Timesof').val(value.timesof);
                    $('#' + value.unitCode + 'DivideBy').val(value.divideBy);
                });

            },
            failure: function (f) {
                alert(f);
            },
            error: function (e) {
                alert('Error ' + e);
            }
        });

        $('#khasraTable .M1').each(function () {
            $(this).removeAttr('class');
            $(this).attr('class', 'form-control numericField M1 ' + Biswa);
        });
        $('#khasraTable .K2').each(function () {
            $(this).removeAttr('class');
            $(this).attr('class', 'form-control numericField K2 ' + Bigha);
        });
        $('#khasraTable .B3').each(function () {
            $(this).removeAttr('class');
            $(this).attr('class', 'form-control numericField B3 ' + Biswansi);
        });
        var khasraIndex = 0;
        var newEntry = $(".khasra-entry:first").clone();
        newEntry.find("input").each(function () {
            var name = $(this).attr("name").replace("[0]", "[" + khasraIndex + "]");
            var id = $(this).attr("id").replace("0", khasraIndex);
            $(this).attr("name", name);
            $(this).attr("id", id);
            $(this).val("");
        });
        khasraIndex++;
        $("#khasraTable tbody").empty();
        $("#khasraTable tbody").append(newEntry);
        $('#TotalArea').text(0.0);
        $('#TotalSiteAreaSqFeet').text(0.0);
        $('#TotalSiteAreaSqMetre').text(0.0);
    });
    $('#khasraTable tbody').on('change', '.numericField', function () {
        var grandTotal = 0.0;
        var _this = $(this);
        if ($('#SelectedSiteAreaUnitId').find("option:selected").text() != 'Select') {

            $('#khasraTable tbody .khasra-entry').each(function () {
                var _thisInner = $(this);
                var Marla = 0;
                var Biswansi = 0;
                var Kanal = 0;
                var Biswa = 0;
                var Sarsai = 0;
                var Bigha = 0;

                var k = 'K'; var m = 'M'; var s = 'S';
                if (_thisInner.find('.' + m).length > 0) {
                    Marla = _thisInner.find('.' + m).val() != '' ? (parseFloat(_thisInner.find('.' + m).val()) * parseFloat($('#' + m + 'UnitValue').val()) * parseFloat($('#' + m + 'Timesof').val())) / parseFloat($('#' + m + 'DivideBy').val()) : 0.0;
                }
                if (_thisInner.find('.' + s).length > 0) {

                    Biswansi = _thisInner.find('.' + s).val() != '' ? (parseFloat(_thisInner.find('.' + s).val()) * parseFloat($('#' + s + 'UnitValue').val()) * parseFloat($('#' + s + 'Timesof').val())) / parseFloat($('#' + s + 'DivideBy').val()) : 0.0;
                }
                if (_thisInner.find('.' + k).length > 0) {
                    Kanal = _thisInner.find('.' + k).val() != '' ? (parseFloat(_thisInner.find('.' + k).val()) * parseFloat($('#' + k + 'UnitValue').val()) * parseFloat($('#' + k + 'Timesof').val())) / parseFloat($('#' + k + 'DivideBy').val()) : 0.0;
                }
                grandTotal = parseFloat(grandTotal) + Marla + Biswansi + Kanal + Biswa + Sarsai + Bigha;
                total = parseFloat(grandTotal).toFixed(4);
                totalSqFeet = parseFloat(total * 43560).toFixed(4);
                totalSqMetre = parseFloat(total * 4046.86).toFixed(4);
            });
        }
        else {
            _this.val(0);
            alert('Please select Unit of Site Area!');
        }
        $('#TotalArea').text(total.toString());
        $('#TotalSiteArea').val(total.toString());
        $('#TotalSiteAreaSqFeet').text(totalSqFeet.toString());
        $('#TotalSiteAreaSqMetre').text(totalSqMetre.toString());
    });
    $('#TotalArea').text(total.toString());
    $('#TotalSiteAreaSqFeet').text(totalSqFeet.toString());
    $('#TotalSiteAreaSqMetre').text(totalSqMetre.toString());
    // $(".mobno").on("input", function () {
    //     // Remove non-numeric characters using a regular expression
    //     $(this).val($(this).val().replace(/[^0-9]/g, ''));
    //     validateMob($(this).val(), $(this));
    // });
    $(".mobno").on("blur", function () {
        validateMob($(this).val(), $(this));
    });
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

        if (parseInt(mobno) < 0) {
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
});

var ownerIndex = 1;

$("#addMoreBtn").on("click", function () {
    var newEntry = $(".owner-entry:first").clone();

    // newEntry.find("span").each(function () {
    //     var name = $(this).attr("name").replace("[0]", "[" + ownerIndex + "]");
    //     $(this).attr("name", name);
    //     $(this).text("");
    // });
    newEntry.find("select").each(function () {
        var name = $(this).attr("name").replace("[0]", "[" + ownerIndex + "]");
        $(this).attr("name", name);
        $(this).val("");
    });
    newEntry.find("hidden").each(function () {
        var name = $(this).attr("name").replace("[0]", "[" + ownerIndex + "]");
        $(this).attr("name", name);
        $(this).val("");
    });
    newEntry.find("input").each(function () {
        var name = $(this).attr("name").replace("[0]", "[" + ownerIndex + "]");
        $(this).attr("name", name);
        $(this).val("");
    });
    newEntry.find('.delOwner').append('<button type="button" id="owner' + ownerIndex + '" onclick="deleteRow(this,\'owner\',\'Owner\')" class="btn btn-danger btn-user btn-block">Delete</button>');
    ownerIndex++;
    $("#ownerTable tbody").append(newEntry);
    SetSerialNo('owner', 'Owner');
});

var khasraIndex = 1;

$("#addMoreKhasraBtn").on("click", function () {
    var newEntry = $(".khasra-entry:first").clone();
    newEntry.find("input").each(function () {
        var name = $(this).attr("name").replace("[0]", "[" + khasraIndex + "]");
        var id = $(this).attr("id").replace("0", khasraIndex);
        $(this).attr("name", name);
        $(this).attr("id", id);
        $(this).val("");
    });
    newEntry.find('.delKhasra').append('<button type="button" id="khasra' + khasraIndex + '" onclick="deleteRow(this,\'khasra\',\'Khasra\')" class="btn btn-danger btn-user btn-block">Delete</button>');
    khasraIndex++;
    $("#khasraTable tbody").append(newEntry);
    SetSerialNo('khasra', 'Khasra');
});
function SetSerialNo(classname, className2) {
    var sr = 1;
    $("." + classname + "-entry").parent().find('tr').each(function () {
        $(this).find('.sr' + className2 + ':first').text(sr++);
    });
}

function deleteRow(button, classname, className2) {
    var row = button.parentNode.parentNode;
    row.parentNode.removeChild(row);
    SetSerialNo(classname, className2);
}

function populateDropdown(data, dropdownId) {
    var dropdown = $("#Selected" + dropdownId);
    dropdown.html('<option value="">Select</option>');
    $.each(data, function (key, value) {
        dropdown.append('<option value="' + value.value + '">' + value.text + '</option>');
    });
}
$(document).ready(function () {
    var selectedText = document.getElementById("SelectedProjectTypeId").options[document.getElementById("SelectedProjectTypeId").selectedIndex].text;
    if (selectedText != 'Any Other') {
        $('.other').css('display', 'none');
        $('#IsOtherType').val(0);
    } else {
        $('.other').css('display', '');
        $('#IsOtherType').val(1);
    }
    var selectedText = document.getElementById("SelectedNocTypeId").options[document.getElementById("SelectedNocTypeId").selectedIndex].text;
    if (selectedText == 'Extension of Existing Project') {
        $('#IsExtension').val(1);
        $('.existing').css('display', '');
    } else {
        $('.existing').css('display', 'none');
        $('#IsExtension').val(0);
    }
    $("#SelectedVillageID").change(function () {
        var villageId = $(this).val();
        $.ajax({
            url: "/Grant/GetVillageDetail",
            type: "POST",
            data: { villageId: villageId },
            async: false,
            success: function (data) {
                $.each(data, function (key, value) {
                    if (key == 'pinCode') {
                        $('#Pincode').text(value);
                    }
                });
            },
            failure: function (f) {
                alert(f);
            },
            error: function (e) {
                alert('Error ' + e);
            }
        });
    });
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
        //var subDivisionId = $(this).val();
            $.ajax({
                url: "/Grant/GetTehsilBlocks",
                type: "POST",
                data: { divisionId: divisionId },
                async: false,
                success: function (data) {
                    populateDropdown(data, "TehsilBlockId");
                    var dropdownlistVillage = $("#SelectedVillageId");
                    dropdownlistVillage.empty();
                    dropdownlistVillage.html('<option value="">Select</option>');
                },
                failure: function (f) {
                    alert(f);
                },
                error: function (e) {
                    alert('Error ' + e);
                }
            });
    });

    //$("#SelectedSubDivisionId").change(function () {
    //    var subDivisionId = $(this).val();
    //    $.ajax({
    //        url: "/Grant/GetTehsilBlocks",
    //        type: "POST",
    //        data: { subDivisionId: subDivisionId },
    //        async: false,
    //        success: function (data) {
    //            populateDropdown(data, "TehsilBlockId");
    //            var dropdownlistVillage = $("#SelectedVillageId");
    //            dropdownlistVillage.empty();
    //            dropdownlistVillage.html('<option value="">Select</option>');
    //        },
    //        failure: function (f) {
    //            alert(f);
    //        },
    //        error: function (e) {
    //            alert('Error ' + e);
    //        }
    //    });
    //});


    $("#SelectedTehsilBlockId").change(function () {
        var tehsilBlockId = $(this).val();
        $.ajax({
            url: "/Grant/GetVillagess",
            type: "POST",
            data: { tehsilBlockId: tehsilBlockId },
            async: false,
            success: function (data) {
                populateDropdown(data, "VillageID");
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