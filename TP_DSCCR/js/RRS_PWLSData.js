﻿var RRS_PWLSData = {
    LoginUrl: null,

    Page_Init: function () {
        RRS_PWLSData.EventBinding();
        RRS_PWLSData.OptionRetrieve();
        RRS_PWLSData.ActionSwitch('R');
    },

    EventBinding: function () {
        var now = new Date();
        $('#SDATE').datetimepicker({ formatTime: 'H', format: "Y-m-d H:i", value: new Date(now.getFullYear(), now.getMonth(), now.getDate(), 0, 0, 0) });
        $('#EDATE').datetimepicker({
            formatTime: 'H',
            format: "Y-m-d H:i",
            value: new Date(now.getFullYear(), now.getMonth(), now.getDate(), 23, 59),
            onSelectTime: function (value) {
                var newDate = new Date(value.getFullYear(), value.getMonth(), value.getDate(), value.getHours(), 59);
                $('#EDATE').datetimepicker({
                    value: newDate
                });
            }
        });


        $('#query').click(function () {
            RRS_PWLSData.RRS_PWLSRetrieve();
        });

        $('#page_number, #page_size').change(function () {
            RRS_PWLSData.RRS_PWLSRetrieve();
        });

        $('#LOCATION').change(function () {
            RRS_PWLSData.SubOptionRetrieve($('#DEVICE_ID'), $(this).val());
        });

        $('#GRAPH_TYPE').change(function () {
            RRS_PWLSData.RRS_PWLSGraph('RRS_PWLS05', $(this).val());
        });

        $('#excel').click(function () {
            RRS_PWLSData.RRS_PWLSExcel();
        });

        $('#login').click(function () {
            window.location.href = RRS_PWLSData.LoginUrl;
        });

    },

    ActionSwitch: function (action) {
        $('form').hide();
        $('.card-header button').hide();
        if (action === 'R') {
            $('#query').show();
            $('#excel').show();
            $('#section_retrieve').show();
        }
    },

    ModalSwitch: function (state) {
        $('#close').show();
        $('#confirm').hide();
        $('#login').hide();
        if (state === -8) {
            $('#login').show();
        }
    },

    OptionRetrieve: function () {
        var url = '/Main/ItemListRetrieve';
        var request = {
            //TableItem: ['userName'],
            PhraseGroup: ['page_size', 'RRS_PWLS_LOCATION', 'GROUP_BY_DT', 'GRAPH_TYPE']
        };

        $.ajax({
            async: false,
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                if (response.Result.State === 0) {

                    //$('#page_size').append('<option value=""></option>');
                    $.each(response.ItemList.page_size, function (idx, row) {
                        $('#page_size').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });
                    $('#page_size option[value="100"]').attr("selected", true);

                    //$('#LOCATION').append('<option value=""></option>');
                    $.each(response.ItemList.RRS_PWLS_LOCATION, function (idx, row) {
                        $('#LOCATION').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });
                    //$('#LOCATION option:nth-child(2)').attr("selected", true);
                    $("#LOCATION").trigger("change");

                    //$('#GROUP_BY_DT').append('<option value=""></option>');
                    $.each(response.ItemList.GROUP_BY_DT, function (idx, row) {
                        if (row.Key === 'DETAIL') {
                            $('#GROUP_BY_DT').append($('<option></option>').attr('value', row.Key).text(row.Value));
                        }
                    });

                }
                else {
                    $('#modal .modal-title').text('交易訊息');
                    $('#modal .modal-body').html('<p>交易代碼:' + response.Result.State + '<br/>交易說明:' + response.Result.Msg + '</p>');
                    $('#modal').modal('show');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                alert('' + xhr.status + ';' + ajaxOptions + ';' + thrownError);
            },
            complete: function (xhr, status) {
            }
        });
    },

    SubOptionRetrieve: function (obj, parentKey) {
        if (parentKey) {
            var url = '/Main/SubItemListRetrieve';
            var request = {
                PhraseGroup: 'RRS_PWLS_DEVICE_ID',
                ParentKey: parentKey
            };

            $.ajax({
                async: false,
                type: 'post',
                url: url,
                contentType: 'application/json',
                data: JSON.stringify(request),
                success: function (data) {
                    var response = JSON.parse(data);
                    if (response.Result.State === 0) {
                        $(obj).find('option').remove();
                        //$(obj).append('<option value=""></option>');
                        $.each(response.SubItemList, function (idx, row) {
                            $(obj).append($('<option></option>').attr('value', row.Key).text(row.Value));
                        });
                    }
                    else {
                        $('#modal .modal-title').text('交易訊息');
                        $('#modal .modal-body').html('<p>交易代碼:' + response.Result.State + '<br/>交易說明:' + response.Result.Msg + '</p>');
                        $('#modal').modal('show');
                    }
                },
                error: function (xhr, ajaxOptions, thrownError) {
                    alert('' + xhr.status + ';' + ajaxOptions + ';' + thrownError);
                },
                complete: function (xhr, status) {
                    //alert('' + xhr.status + ';' + status );
                }
            });
        }
        else {
            $(obj).find('option').remove();
        }
    },

    RRS_PWLSRetrieve: function () {
        var url = 'RRS_PWLSRetrieve';
        var request = {
            RRS_PWLS: {
                LOCATION: $('#LOCATION').val(),
                DEVICE_ID: $('#DEVICE_ID').val()
            },
            SDATE: $('#SDATE').val(),
            EDATE: $('#EDATE').val(),
            GROUP_BY_DT: $('#GROUP_BY_DT').val(),
            PageNumber: $('#page_number').val() ? $('#page_number').val() : 1,
            PageSize: $('#page_size').val() ? $('#page_size').val() : 10
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                RRS_PWLSData.ModalSwitch(response.Result.State);
                if (response.Result.State === 0) {
                    $('#gridview >  tbody').html('');
                    $('#rows_count').text(response.Pagination.RowCount);
                    $('#interval').text(response.Pagination.MinNumber + '-' + response.Pagination.MaxNumber);
                    $('#page_number option').remove();
                    for (var i = 1; i <= response.Pagination.PageCount; i++) {
                        $('#page_number').append($('<option></option>').attr('value', i).text(i));
                    }
                    $('#page_number').val(response.Pagination.PageNumber);
                    $('#page_count').text(response.Pagination.PageCount);
                    $('#time_consuming').text((Date.parse(response.Pagination.EndTime) - Date.parse(response.Pagination.StartTime)) / 1000);

                    var htmlRow = '';
                    if (response.Pagination.RowCount > 0) {
                        $.each(response.RRS_PWLSData, function (idx, row) {
                            htmlRow = '<tr>';
                            htmlRow += '<td>' + row.CDATE.substr(0, 10) + '</td>';
                            htmlRow += '<td>' + row.CDATE.substr(11, 5) + '</td>';
                            //htmlRow += '<td>' + row.LOCATION + '</td>';
                            //htmlRow += '<td>' + row.DEVICE_ID + '</td>';
                            var css = '';
                            if (row.RRS01_PWLS01 === "發生") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.RRS01_PWLS01 + '</td>';
                            css = '';
                            if (row.RRS02_PWLS01 === "發生") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.RRS02_PWLS01 + '</td>';
                            css = '';
                            if (row.RRS03_PWLS01 === "發生") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.RRS03_PWLS01 + '</td>';
                            css = '';
                            if (row.RRS04_PWLS01 === "發生") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.RRS04_PWLS01 + '</td>';
                            css = '';
                            if (row.RRS05_PWLS01 === "發生") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.RRS05_PWLS01 + '</td>';
                            css = '';
                            if (row.RRS06_PWLS01 === "發生") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.RRS06_PWLS01 + '</td>';
                            css = '';
                            if (row.RRS07_PWLS01 === "發生") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.RRS07_PWLS01 + '</td>';
                            css = '';
                            if (row.RRS08_PWLS01 === "發生") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.RRS08_PWLS01 + '</td>';
                            css = '';
                            if (row.RRS09_PWLS01 === "發生") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.RRS09_PWLS01 + '</td>';
                            css = '';
                            if (row.RRS10_PWLS01 === "發生") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.RRS10_PWLS01 + '</td>';
                            css = '';
                            if (row.RRS11_PWLS01 === "發生") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.RRS11_PWLS01 + '</td>';
                            css = '';
                            if (row.RRS12_PWLS01 === "發生") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.RRS12_PWLS01 + '</td>';
                            css = '';
                            if (row.RRS13_PWLS01 === "發生") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.RRS13_PWLS01 + '</td>';
                            htmlRow += '</tr>';
                            $('#gridview >  tbody').append(htmlRow);
                        });
                    }
                    else {
                        htmlRow = '<tr><td colspan="' + $('#gridview > thead').children('tr').children('th').length +'" style="text-align:center">data not found</td></tr>';
                        $('#gridview >  tbody').append(htmlRow);
                    }
                }
                else {
                    $('#modal .modal-title').text('交易訊息');
                    $('#modal .modal-body').html('<p>交易代碼:' + response.Result.State + '<br/>交易說明:' + response.Result.Msg + '</p>');
                    $('#modal').modal('show');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#modal .modal-title').text(ajaxOptions);
                $('#modal .modal-body').html('<p>' + xhr.status + ' ' + thrownError + '</p>');
                $('#modal').modal('show');
            },
            complete: function (xhr, status) {
            }
        });
    },

    RRS_PWLSExcel: function () {
        var url = 'RRS_PWLSExcel';
        var request = {
            RRS_PWLS: {
                LOCATION: $('#LOCATION').val(),
                DEVICE_ID: $('#DEVICE_ID').val()
            },
            SDATE: $('#SDATE').val(),
            EDATE: $('#EDATE').val(),
            GROUP_BY_DT: $('#GROUP_BY_DT').val()
        };

        $.ajax({
            cache: false,
            type: 'post',
            url: url,
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                RRS_PWLSData.ModalSwitch(response.Result.State);
                if (response.Result.State === 0) {
                    window.location.href = '/Main/ExcelDownload?DataId=' + response.DataId
                        + '&FileName=' + response.FileName;
                }
                else {
                    $('#modal .modal-title').text('交易訊息');
                    $('#modal .modal-body').html('<p>交易代碼:' + response.Result.State + '<br/>交易說明:' + response.Result.Msg + '</p>');
                    $('#modal').modal('show');
                }
            },
            error: function (xhr, ajaxOptions, thrownError) {
                $('#modal .modal-title').text(ajaxOptions);
                $('#modal .modal-body').html('<p>' + xhr.status + ' ' + thrownError + '</p>');
                $('#modal').modal('show');
            },
            complete: function (xhr, status) {
            }
        });
    }


};
