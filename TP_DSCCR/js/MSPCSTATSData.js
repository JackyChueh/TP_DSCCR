var MSPCSTATSData = {
    LoginUrl: null,

    Page_Init: function () {
        MSPCSTATSData.EventBinding();
        MSPCSTATSData.OptionRetrieve();
        MSPCSTATSData.ActionSwitch('R');
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
            MSPCSTATSData.MSPCSTATSRetrieve();
        });

        $('#page_number, #page_size').change(function () {
            MSPCSTATSData.MSPCSTATSRetrieve();
        });

        $('#LOCATION').change(function () {
            MSPCSTATSData.SubOptionRetrieve($('#DEVICE_ID'), $(this).val());
        });

        $('#excel').click(function () {
            MSPCSTATSData.MSPCSTATSExcel();
        });

        $('#login').click(function () {
            window.location.href = MSPCSTATSData.LoginUrl;
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
            PhraseGroup: ['page_size', 'MSPCSTATS_LOCATION', 'GROUP_BY_DT', 'GRAPH_TYPE','WATER_TOWER']
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

                    $('#LOCATION').append('<option value=""></option>');
                    $.each(response.ItemList.MSPCSTATS_LOCATION, function (idx, row) {
                        $('#LOCATION').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });
                    //$('#LOCATION option:nth-child(2)').attr("selected", true);
                    //$("#LOCATION").trigger("change");

                    //$('#GROUP_BY_DT').append('<option value=""></option>');
                    $.each(response.ItemList.GROUP_BY_DT, function (idx, row) {
                        $('#GROUP_BY_DT').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });

                    //$('#GRAPH_TYPE').append('<option value=""></option>');
                    $.each(response.ItemList.GRAPH_TYPE, function (idx, row) {
                        $('#GRAPH_TYPE').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });

                    $('#WATER_TOWER').append('<option value=""></option>');
                    $.each(response.ItemList.WATER_TOWER, function (idx, row) {
                        $('#WATER_TOWER').append($('<option></option>').attr('value', row.Key).text(row.Value));
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
                PhraseGroup: 'MSPCSTATS_DEVICE_ID',
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
                        $(obj).append('<option value=""></option>');
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

    MSPCSTATSRetrieve: function () {
        var url = 'MSPCSTATSRetrieve';
        var request = {
            MSPCSTATS: {
                LOCATION: $('#LOCATION').val(),
                DEVICE_ID: $('#DEVICE_ID').val(),
                WATER_TOWER: $('#WATER_TOWER').val()
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
                MSPCSTATSData.ModalSwitch(response.Result.State);
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
                        $.each(response.MSPCSTATSData, function (idx, row) {
                            htmlRow = '<tr>';
                            htmlRow += '<td>' + row.CDATE.substr(0, 10) + '</td>';
                            htmlRow += '<td>' + row.CDATE.substr(11, 5) + '</td>';
                            htmlRow += '<td>' + row.LOCATION + '</td>';
                            htmlRow += '<td>' + row.DEVICE_ID + '</td>';
                            htmlRow += '<td>' + row.WATER_TOWER + '</td>';
                            var css = '';
                            if (row.SEF01 === "停止") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.SEF01 + '</td>';
                            css = '';
                            if (row.SEF02 === "停止") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.SEF02 + '</td>';
                            css = '';
                            if (row.SEF03 === "停止") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.SEF03 + '</td>';
                            css = '';
                            if (row.SEF04 === "停止") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.SEF04 + '</td>';
                            css = '';
                            if (row.SEF05 === "停止") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.SEF05 + '</td>';
                            css = '';
                            if (row.SEF06 === "停止") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.SEF06 + '</td>';
                            css = '';
                            if (row.SEF07 === "停止") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.SEF07 + '</td>';
                            css = '';
                            if (row.SEF08 === "停止") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.SEF08 + '</td>';
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

    MSPCSTATSExcel: function () {
        var url = 'MSPCSTATSExcel';
        var request = {
            MSPCSTATS: {
                LOCATION: $('#LOCATION').val(),
                DEVICE_ID: $('#DEVICE_ID').val(),
                WATER_TOWER: $('#WATER_TOWER').val()
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
                MSPCSTATSData.ModalSwitch(response.Result.State);
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
