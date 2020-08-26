var ALERT_CONFIGIndex = {
    Action: null,
    ALERT_CONFIG: null,
    ConfirmAction: null,

    Page_Init: function () {
        this.EventBinding();
        this.OptionRetrieve();
        this.ActionSwitch('R');
    },

    EventBinding: function () {

        $('#query').click(function () {
            ALERT_CONFIGIndex.ALERT_CONFIGRetrieve('R');
        });

        $('#page_number, #page_size').change(function () {
            ALERT_CONFIGIndex.ALERT_CONFIGRetrieve('R');
        });

        $('#create').click(function () {
            ALERT_CONFIGIndex.ActionSwitch('C');
        });

        $('#save').click(function () {
            if (ALERT_CONFIGIndex.DataValidate()) {
                if (ALERT_CONFIGIndex.Action === 'C') {
                    ALERT_CONFIGIndex.ALERT_CONFIGCreate();
                } else if (ALERT_CONFIGIndex.Action === 'U') {
                    ALERT_CONFIGIndex.ALERT_CONFIGUpdate();
                }
            }
        });

        $('#undo').click(function () {
            ALERT_CONFIGIndex.ValueRecover(ALERT_CONFIGIndex.Action);
        });

        $('#delete').click(function () {
            $('#modal_action .modal-title').text('提示訊息');
            $('#modal_action .modal-body').html('<p>確定要刪除該筆資料?</p>');
            //$('#confirm').attr('data-action', 'delete');
            ALERT_CONFIGIndex.ConfirmAction = 'delete';
            $('#modal_action #confirm').show();
            $('#modal_action').modal('show');
        });

        $('#return').click(function () {
            ALERT_CONFIGIndex.ActionSwitch('R');
            ALERT_CONFIGIndex.ValueRecover();
            ALERT_CONFIGIndex.ALERT_CONFIGRetrieve();
        });

        $('#modal_action #confirm').click(function () {
            $('#confirm').hide();
            $('#modal_action').modal('hide');
            console.log(ALERT_CONFIGIndex.ConfirmAction);
            if (ALERT_CONFIGIndex.ConfirmAction === 'delete') {
                ALERT_CONFIGIndex.ALERT_CONFIGDelete();
            }
        });

        //$('#section_modify #ID').keyup(function () {
        //    $("#section_modify #ID").val($("#section_modify #ID").val().toUpperCase());
        //});

        //$('#section_modify #PASSWORD').keyup(function () {
        //    $("#section_modify #PASSWORD").val($("#section_modify #PASSWORD").val().toUpperCase());
        //});

        $('#HR_DATE').datetimepicker({ timepicker: false, format: 'Y-m-d' });

        var section_retrieve = $('#section_retrieve');
        var today = new Date();
        section_retrieve.find('input[name=HR_DATE_START]').datetimepicker({ timepicker: false, format: 'Y-m-d', value: new Date(today.getFullYear(),0,1) });
        section_retrieve.find('input[name=HR_DATE_END]').datetimepicker({ timepicker: false, format: 'Y-m-d', value: new Date(today.getFullYear(), 11, 31) });

        $('#SUN_STIME').datetimepicker({ datepicker: false, step: 15, format: 'H:i' });
        $('#SUN_ETIME').datetimepicker({ datepicker: false, step: 15, format: 'H:i' });
        $('#MON_STIME').datetimepicker({ datepicker: false, step: 15, format: 'H:i' });
        $('#MON_ETIME').datetimepicker({ datepicker: false, step: 15, format: 'H:i' });
        $('#TUE_STIME').datetimepicker({ datepicker: false, step: 15, format: 'H:i' });
        $('#TUE_ETIME').datetimepicker({ datepicker: false, step: 15, format: 'H:i' });
        $('#WED_STIME').datetimepicker({ datepicker: false, step: 15, format: 'H:i' });
        $('#WED_ETIME').datetimepicker({ datepicker: false, step: 15, format: 'H:i' });
        $('#THU_STIME').datetimepicker({ datepicker: false, step: 15, format: 'H:i' });
        $('#THU_ETIME').datetimepicker({ datepicker: false, step: 15, format: 'H:i' });
        $('#FRI_STIME').datetimepicker({ datepicker: false, step: 15, format: 'H:i' });
        $('#FRI_ETIME').datetimepicker({ datepicker: false, step: 15, format: 'H:i' });
        $('#STA_STIME').datetimepicker({ datepicker: false, step: 15, format: 'H:i' });
        $('#STA_ETIME').datetimepicker({ datepicker: false, step: 15, format: 'H:i' });
    },

    ActionSwitch: function (Action) {
        $('form').hide();
        $('.card-header button').hide();
        if (Action === 'R') {
            $('#query').show();
            $('#create').show();
            $('#section_retrieve').show();
        } else if (Action === 'U') {
            $('#save').show();
            $('#delete').show();
            $('#return').show();
            $('#undo').show();
            $('#section_modify').show();
        } else if (Action === 'C') {
            $('#save').show();
            $('#return').show();
            $('#undo').show();
            $('#section_modify').show();
        }
        this.Action = Action;
    },

    OptionRetrieve: function () {
        var url = '/Main/ItemListRetrieve';
        var request = {
            //TableItem: ['userName'],
            PhraseGroup: ['page_size', 'DATE_TYPE']
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
                    $('#page_size option[value="30"]').attr("selected", true);

                    //var section_retrieve = $('#section_retrieve');
                    //section_retrieve.find("select[name='DATE_TYPE']").append('<option value=""></option>');
                    $("select[name='DATE_TYPE']").append('<option value=""></option>');
                    $.each(response.ItemList.DATE_TYPE, function (idx, row) {
                        $("select[name='DATE_TYPE']").append($('<option></option>').attr('value', row.Key).text(row.Value));
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

    ALERT_CONFIGRetrieve: function () {
        var url = 'ALERT_CONFIGRetrieve';
        var section_retrieve = $('#section_retrieve');
        var request = {
            ALERT_CONFIG: {
                DATE_TYPE: section_retrieve.find('select[name=DATE_TYPE]').val()
            },
            HR_DATE_START: section_retrieve.find('input[name=HR_DATE_START]').val(),
            HR_DATE_END: section_retrieve.find('input[name=HR_DATE_END]').val(),
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
                        $.each(response.ALERT_CONFIG, function (idx, row) {
                            htmlRow = '<tr>';
                            htmlRow += '<td><a class="fa fa-edit fa-lg" onclick="ALERT_CONFIGIndex.ALERT_CONFIGQuery(' + row.SN + ');" data-toggle="tooltip" data-placement="right" title="修改"></a></td>';
                            htmlRow += '<td>' + row.HR_DATE.substr(0, 10) + '</td>';
                            htmlRow += '<td>' + row.DATE_TYPE + '</td>';
                            htmlRow += '<td>' + (row.MEMO ? row.MEMO : '') + '</td>';
                            htmlRow += '<td>' + row.MDATE.replace('T', ' ') + '</td>';
                            htmlRow += '<td>' + row.MUSER + '</td>';
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

    ALERT_CONFIGCreate: function () {
        var url = 'ALERT_CONFIGCreate';
        var section_modify = $('#section_modify');
        var request = {
            ALERT_CONFIG: {
                HR_DATE: section_modify.find('input[name=HR_DATE]').val(),
                DATE_TYPE: section_modify.find('select[name=DATE_TYPE]').val(),
                MEMO: section_modify.find('textarea[name=MEMO]').val()
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                if (response.Result.State === 1) {
                    ALERT_CONFIGIndex.ALERT_CONFIGQuery(response.ALERT_CONFIG.SN);
                    ALERT_CONFIGIndex.ActionSwitch('U');
                } else if (response.Result.State === -10) {
                    response.Result.Msg = '日期' + request.ALERT_CONFIG.HR_DATE + '已存在，不可重複設定。';
                }

                $('#modal .modal-title').text('交易訊息');
                $('#modal .modal-body').html('<p>交易說明:' + response.Result.Msg + '<br /> 交易代碼:' + response.Result.State + '</p>');
                $('#modal').modal('show');
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

    ALERT_CONFIGUpdate: function () {
        var url = 'ALERT_CONFIGUpdate';
        var section_modify = $('#section_modify');
        var request = {
            ALERT_CONFIG: {
                SN: section_modify.find('input[name=SN]').val(),
                HR_DATE: section_modify.find('input[name=HR_DATE]').val(),
                DATE_TYPE: section_modify.find('select[name=DATE_TYPE]').val(),
                MEMO: section_modify.find('textarea[name=MEMO]').val()
            }         
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                if (response.Result.State === 2) {
                    ALERT_CONFIGIndex.ALERT_CONFIGQuery(response.ALERT_CONFIG.SN);
                } else if (response.Result.State === -10) {
                    response.Result.Msg = '日期' + request.ALERT_CONFIG.HR_DATE + '已存在，不可重複設定。';
                }
                $('#modal .modal-title').text('交易訊息');
                $('#modal .modal-body').html('<p>交易說明:' + response.Result.Msg + '<br /> 交易代碼:' + response.Result.State + '</p>');
                $('#modal').modal('show');
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

    ALERT_CONFIGDelete: function () {
        var url = 'ALERT_CONFIGDelete';
        var section_modify = $('#section_modify');
        var request = {
            ALERT_CONFIG: {
                SN: section_modify.find('input[name=SN]').val()
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                if (response.Result.State === 3) {
                    ALERT_CONFIGIndex.ALERT_CONFIGRetrieve();
                    ALERT_CONFIGIndex.ActionSwitch('R');
                    //ALERT_CONFIGIndex.ValueRecover();
                }
                $('#modal .modal-title').text('交易訊息');
                $('#modal .modal-body').html('<p>交易說明:' + response.Result.Msg + '<br /> 交易代碼:' + response.Result.State + '</p>');
                $('#modal').modal('show');
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

    ALERT_CONFIGQuery: function (SN) {
        var url = 'ALERT_CONFIGQuery';
        var request = {
            ALERT_CONFIG: {
                SN: SN
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                if (response.Result.State === 0) {
                    var section_modify = $('#section_modify');
                    section_modify.find('input[name=SN]').val(response.ALERT_CONFIG.SN);
                    section_modify.find('input[name=HR_DATE]').val(response.ALERT_CONFIG.HR_DATE.substr(0, 10));
                    section_modify.find('select[name=DATE_TYPE]').val(response.ALERT_CONFIG.DATE_TYPE);
                    section_modify.find('textarea[name=MEMO]').val(response.ALERT_CONFIG.MEMO);
                    section_modify.find('input[name=CDATE]').val(response.ALERT_CONFIG.CDATE.replace('T', ' '));
                    section_modify.find('input[name=CUSER]').val(response.ALERT_CONFIG.CUSER);
                    section_modify.find('input[name=MDATE]').val(response.ALERT_CONFIG.MDATE.replace('T', ' '));
                    section_modify.find('input[name=MUSER]').val(response.ALERT_CONFIG.MUSER);

                    ALERT_CONFIGIndex.ALERT_CONFIG = response.ALERT_CONFIG;
                    ALERT_CONFIGIndex.ActionSwitch('U');
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

    DataValidate: function () {
        var error = '';
        var section_modify = $('#section_modify');

        var ALERT_CONFIG = {
            HR_DATE: section_modify.find('input[name=HR_DATE]').val(),
            DATE_TYPE: section_modify.find('select[name=DATE_TYPE]').val(),
            MEMO: section_modify.find('textarea[name=MEMO]').val()
        };

        if (!ALERT_CONFIG.HR_DATE) {
            error += '日期不可空白<br />';
        }
        if (!ALERT_CONFIG.DATE_TYPE) {
            error += '類別不可空白<br />';
        }
       
        if (error.length > 0) {
            $('#modal .modal-title').text('提示訊息');
            $('#modal .modal-body').html('<p>' + error + '</p>');
            $('#modal').modal('show');
        }
        return error.length === 0;
    },

    ValueRecover: function (action) {
        if (action === 'U') {
            if (ALERT_CONFIGIndex.ALERT_CONFIG) {
                $('.modify').each(function (index, value) {
                    if (value.id === 'CDATE' || value.id === 'MDATE') {
                        $(value).val(ALERT_CONFIGIndex.ALERT_CONFIG[value.id].replace('T', ' '));
                    }
                    else {
                        $(value).val(ALERT_CONFIGIndex.ALERT_CONFIG[value.id]);
                    }
                });
            }
        }
        else {
            $('.modify').each(function (index, value) {
                $(value).val('');
            });
        }
    }

};
