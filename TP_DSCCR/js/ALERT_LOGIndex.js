var ALERT_LOGIndex = {
    LoginUrl: null,
    Action: null,
    ALERT_LOG: null,
    ConfirmAction: null,

    Page_Init: function () {
        this.EventBinding();
        this.OptionRetrieve();
        this.ActionSwitch('R');
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
            ALERT_LOGIndex.ALERT_LOGRetrieve('R');
        });

        $('#page_number, #page_size').change(function () {
            ALERT_LOGIndex.ALERT_LOGRetrieve('R');
        });

        $('#excel').click(function () {
            ALERT_LOGIndex.ALERT_LOGExcel();
        });


        $('#login').click(function () {
            window.location.href = ALERT_LOGIndex.LoginUrl;
        });

        var section_retrieve = $('#section_retrieve');
        section_retrieve.find('select[name=DATA_TYPE]').change(function () {    //監控類別
            ALERT_LOGIndex.SubOptionRetrieve(section_retrieve.find('select[name=LOCATION]'), $(this).val() + '_LOCATION', $(this).val());
            ALERT_LOGIndex.SubOptionRetrieve(section_retrieve.find('select[name=DATA_FIELD]'), $(this).val() + '_DATA_FIELD', $(this).val());
            section_retrieve.find('select[name=DEVICE_ID]').find('option').remove();
        });
        section_retrieve.find('select[name=LOCATION]').change(function () { //位置
            ALERT_LOGIndex.SubOptionRetrieve(section_retrieve.find('select[name=DEVICE_ID]'), section_retrieve.find('select[name=DATA_TYPE]').val() + '_DEVICE_ID', $(this).val());
        });

    },

    ActionSwitch: function (Action) {
        $('form').hide();
        $('.card-header button').hide();
        if (Action === 'R') {
            $('#query').show();
            $('#excel').show();
            $('#section_retrieve').show();
        } else if (Action === 'U') {
            $('#save').show();
            $('#delete').show();
            $('#return').show();
            $('#undo').show();
            $('#section_modify').show();
        } else if (Action === 'C') {
            $('#default').show();
            $('#save').show();
            $('#return').show();
            $('#undo').show();
            $('#section_modify').show();
        }
        this.Action = Action;
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
            PhraseGroup: ['page_size', 'mode', 'DATA_TYPE']
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

                    $.each(response.ItemList.page_size, function (idx, row) {
                        $('#page_size').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });
                    $('#page_size option[value="100"]').attr("selected", true);

                    $("select[name='MODE']").append('<option value=""></option>');
                    $.each(response.ItemList.mode, function (idx, row) {
                        $("select[name='MODE']").append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });

                    $("select[name='DATA_TYPE']").append('<option value=""></option>');
                    $.each(response.ItemList.DATA_TYPE, function (idx, row) {
                        $("select[name='DATA_TYPE']").append($('<option></option>').attr('value', row.Key).text(row.Value));
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

    SubOptionRetrieve: function (obj, PhraseGroup, parentKey) {
        if (parentKey) {
            var url = '/Main/SubItemListRetrieve';
            var request = {
                PhraseGroup: PhraseGroup,
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

    ALERT_LOGRetrieve: function () {
        var url = 'ALERT_LOGRetrieve';
        var section_retrieve = $('#section_retrieve');
        var request = {
            ALERT_LOG: {
                DATA_TYPE: section_retrieve.find('select[name=DATA_TYPE]').val(),
                LOCATION: section_retrieve.find('select[name=LOCATION]').val(),
                DEVICE_ID: section_retrieve.find('select[name=DEVICE_ID]').val(),
                DATA_FIELD: section_retrieve.find('select[name=DATA_FIELD]').val(),
            },
            SDATE: $('#SDATE').val(),
            EDATE: $('#EDATE').val(),
            PageNumber: $('#page_number').val() ? $('#page_number').val() : 1,
            PageSize: $('#page_size').val() ? $('#page_size').val() : 1
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                ALERT_LOGIndex.ModalSwitch(response.Result.State);
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
                        $.each(response.ALERT_LOG, function (idx, row) {
                            htmlRow = '<tr>';
                            //htmlRow += '<td><a class="fa fa-edit fa-lg" onclick="ALERT_LOGIndex.ALERT_LOGQuery(' + row.SID + ');" data-toggle="tooltip" data-placement="right" title="修改"></a></td>';
                            htmlRow += '<td>' + row.SID + '</td>';
                            htmlRow += '<td>' + row.DATA_TYPE + '</td>';
                            htmlRow += '<td>' + row.LOCATION + '</td>';
                            htmlRow += '<td>' + row.DEVICE_ID + '</td>';
                            htmlRow += '<td>' + row.DATA_FIELD + '</td>';
                            htmlRow += '<td>' + row.MAX_VALUE + '</td>';
                            htmlRow += '<td>' + row.MIN_VALUE + '</td>';
                            htmlRow += '<td>' + row.ALERT_VALUE + '</td>';
                            htmlRow += '<td>' + row.CHECK_DATE.replace('T', ' ') + '</td>';
                            htmlRow += '</tr>';
                            $('#gridview >  tbody').append(htmlRow);
                        });
                    }
                    else {
                        htmlRow = '<tr><td colspan="12" style="text-align:center">data not found</td></tr>';
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

    ALERT_LOGExcel: function () {
        var url = 'ALERT_LOGExcel';
        var section_retrieve = $('#section_retrieve');
        var request = {
            ALERT_LOG: {
                DATA_TYPE: section_retrieve.find('select[name=DATA_TYPE]').val(),
                LOCATION: section_retrieve.find('select[name=LOCATION]').val(),
                DEVICE_ID: section_retrieve.find('select[name=DEVICE_ID]').val(),
                DATA_FIELD: section_retrieve.find('select[name=DATA_FIELD]').val(),
            },
            SDATE: $('#SDATE').val(),
            EDATE: $('#EDATE').val(),
        };

        $.ajax({
            cache: false,
            type: 'post',
            url: url,
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                ALERT_LOGIndex.ModalSwitch(response.Result.State);
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
