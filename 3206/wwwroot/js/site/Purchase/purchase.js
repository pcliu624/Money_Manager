let editlist = [];
let dellist = [];
$(function () {
    $("#datepicker").datepicker({ dateFormat: 'yy-mm-dd' });
    $('#datefilter').daterangepicker({
        opens: 'right',
        "startDate": moment().startOf('month'),
        "endDate": moment().endOf('month'),
    }, function (start, end, label) {
    });
    $('#paybyfilter').val('');
    $('#storefilter').val('');
    $('#typefilter').val('');
    $('#addproduct_s').click(function () {
        additem_s($(this));
    });
    $('#Payfor option').prop('selected', true);
    $('[data-toggle="tooltip"]').tooltip();
    $(document).on("click", ".add", function () {
        var empty = false;
        var input = $(this).parents("tr").find('input[type="text"]');
        input.each(function () {
            if (!$(this).val()) {
                $(this).addClass("error");
                empty = true;
            } else {
                $(this).removeClass("error");
            }
        });
        $(this).parents("tr").find(".error").first().focus();
        if (!empty) {
            input.each(function () {
                $(this).parent("td").html($(this).val());
            });
            $(this).parents("tr").find(".add, .edit").toggle();
            $(".add-new").removeAttr("disabled");
        }
        var cells = $(this).parents("tr").find("td:not(:last-child)");
        var rowData = {};
        var prop = ['Date', 'Payby', "Cost", 'Type', 'Store', 'Payfor'];
        // Loop through each cell in the row and extract the text content
        for (var j = 0; j < cells.length - 1; j++) {

            if (prop[j] == '') continue;
            else {
                var rowid = $(this).parents("tr").find("td:last-child").prevAll('td:hidden').first().text();
                rowData['Id'] = rowid;
            }
            var header = prop[j];
            var value = cells[j].textContent;
            rowData[header] = value;
        }
        if (editlist.length == 0)
            editlist.push(rowData);
        let found = false;
        for (let i = 0; i < editlist.length; i++) {
            if (rowData.Id === editlist[i].Id) {
                editlist[i] = rowData; // Replace the object
                found = true;
                break; // Break out of the loop since we found a match
            }
        }
        if (!found) editlist.push(rowData);
        console.log(editlist);

    });
    $(document).on("click", ".edit", function () {
        $(this).parents("tr").find("td:not(:last-child)").each(function () {
            $(this).html('<input type="text" class="form-control" value="' + $(this).text() + '">');
        });
        $(this).parents("tr").find(".add, .edit").toggle();
        $(".add-new").attr("disabled", "disabled");
    });
    $(document).on("click", ".delete", function () {
        $(this).parents("tr").remove();
        $(".add-new").removeAttr("disabled");
        $('.tooltip').not(this).hide();
        var rowid = $(this).parents("tr").find("td:last-child").prevAll('td:hidden').first().text();
        dellist.push(rowid);
        for (let i = 0; i < editlist.length; i++) {
            if (rowid === editlist[i].Id) {
                editlist = editlist.filter(x => x.Id !== rowid);
                break;
            }
        }
        console.log(dellist);
    });
    $("#btnsave").on('click', function () {
        DataPost();
    });
    $('#searchbtn').on('click', function () {
        BindTable();
    })
    $('#clearbtn').on('click', function () {
        $('#datefilter').val('');
        $('#paybyfilter').val('');
        $('#storefilter').val('');
        $('#typefilter').val('');
    })
    BindTable();
});

function BindTable() {
    $("#resultS").bootstrapTable("destroy");
    var indexOffset = 0;
    $("#resultS").bootstrapTable({
        contentType: "application/x-www-form-urlencoded; charset=UTF-8",
        queryParams: function (params) {
            jsondata = $.param({
                Date: $('#datefilter').val(),
                Payby: $('#paybyfilter').val(),
                store: $('#storefilter').val(),
                Type: $('#typefilter').val(),
                //Payfor: $('#payforfilter').val(),
                limit: params.limit,
                offset: params.offset
            }, true);
            return jsondata;
        },
        onLoadSuccess: function (data) {
            $('.no-records-found').remove();

        },
        onPageChange: function (number, size) {
            indexOffset = size * (number - 1);
        },
        onSort: function (name, order) {

        }
    });
}
function btnformat(value, row, index) {
    return '<a class="add" title="Add" data-toggle="tooltip"><i class="material-icons">&#xE03B;</i></a>' +
        '<a class="edit" title="Edit" data-toggle="tooltip"><i class="material-icons">&#xE254;</i></a>' +
        '<a class="delete" title="Delete" data-toggle="tooltip"><i class="material-icons">&#xE872;</i></a>';
}
function DataPost() {
    let s = $('#resultS tbody tr').length;
    var prop = ['Date', 'Payby', "Cost", 'Type', 'Store', 'Payfor', 'Id'];
    // Retrieve all the rows in the table
    let formdata = {
        EditList: editlist,
        DeleteList: dellist
    };
    $.ajax({
        url: "../Purchase/SavePurchaseRecord",
        type: "POST",
        cache: false,
        contentType: 'application/json; charset=utf-8',
        dataType: 'json',
        data: JSON.stringify(formdata),
        success: function (result) {
            window.location.reload();
            console.log(result);
        },
        error: function (result) {
            console.log(result.responseText);
        }
    })

}