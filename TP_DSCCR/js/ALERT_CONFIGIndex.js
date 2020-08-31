﻿var ALERT_CONFIGIndex = {
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

        //$('#HR_DATE').datetimepicker({ timepicker: false, format: 'Y-m-d' });

        //var section_retrieve = $('#section_retrieve');
        //var today = new Date();
        //section_retrieve.find('input[name=HR_DATE_START]').datetimepicker({ timepicker: false, format: 'Y-m-d', value: new Date(today.getFullYear(),0,1) });
        //section_retrieve.find('input[name=HR_DATE_END]').datetimepicker({ timepicker: false, format: 'Y-m-d', value: new Date(today.getFullYear(), 11, 31) });

        //時間起迄-選擇器初始化
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

        var section_retrieve = $('#section_retrieve');
        section_retrieve.find('select[name=DATA_TYPE]').change(function () {    //監控類別
            ALERT_CONFIGIndex.SubOptionRetrieve(section_retrieve.find('select[name=LOCATION]'), $(this).val() + '_LOCATION', $(this).val());
            ALERT_CONFIGIndex.SubOptionRetrieve(section_retrieve.find('select[name=DATA_FIELD]'), $(this).val() + '_DATA_FIELD', $(this).val());
            section_retrieve.find('select[name=DEVICE_ID]').find('option').remove();
        });
        section_retrieve.find('select[name=LOCATION]').change(function () { //位置
            ALERT_CONFIGIndex.SubOptionRetrieve(section_retrieve.find('select[name=DEVICE_ID]'), section_retrieve.find('select[name=DATA_TYPE]').val() + '_DEVICE_ID', $(this).val());
        });

        $('#DATA_TYPE').change(function () {    //監控類別
            ALERT_CONFIGIndex.SubOptionRetrieve($('#LOCATION'), $('#DATA_TYPE').val() + '_LOCATION', $(this).val());
            ALERT_CONFIGIndex.SubOptionRetrieve($('#DATA_FIELD'), $('#DATA_TYPE').val() + '_DATA_FIELD', $(this).val());
            $('#DEVICE_ID').find('option').remove();
        });
        $('#LOCATION').change(function () { //位置
            ALERT_CONFIGIndex.SubOptionRetrieve($('#DEVICE_ID'), $('#DATA_TYPE').val() + '_DEVICE_ID', $(this).val());
        });

        $('#mail_add').click(function () {  //加入-郵件通知清單
            $('#modal_mail').modal('show');
        });

        $('#modal_mail #confirm').click(function () {
            $('#modal_mail').modal('hide');
            if (ALERT_CONFIGIndex.MailValidate()) {
                var option = $('<option></option>').attr('value', $('#EMAIL').val()).text($('#EMAIL').val());
                $("#MAIL_TO").prepend(option);  //加入時放在第一個
                $("#MAIL_TO option").each(function (idx, val) { //去重複
                    $(this).siblings("[value='" + $(this).val() + "']").remove();
                });
                $('#EMAIL').val('');
            }
        });

        $('#mail_delete').click(function () {   //移除-郵件通知清單
            var options = $('#MAIL_TO option:selected');
            options.remove();
        });
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
                    $('#page_size option[value="30"]').attr("selected", true);

                    $("select[name='MODE']").append('<option value=""></option>');
                    $.each(response.ItemList.mode, function (idx, row) {
                        $("select[name='MODE']").append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });

                    $("select[name='DATA_TYPE']").append('<option value=""></option>');
                    $.each(response.ItemList.DATA_TYPE, function (idx, row) {
                        $("select[name='DATA_TYPE']").append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });

                    ////var section_retrieve = $('#section_retrieve');
                    ////section_retrieve.find("select[name='DATE_TYPE']").append('<option value=""></option>');
                    //$("select[name='DATE_TYPE']").append('<option value=""></option>');
                    //$.each(response.ItemList.DATE_TYPE, function (idx, row) {
                    //    $("select[name='DATE_TYPE']").append($('<option></option>').attr('value', row.Key).text(row.Value));
                    //});

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

    ALERT_CONFIGRetrieve: function () {
        var url = 'ALERT_CONFIGRetrieve';
        var section_retrieve = $('#section_retrieve');
        var request = {
            ALERT_CONFIG: {
                MODE: section_retrieve.find('select[name=MODE]').val(),
                DATA_TYPE: section_retrieve.find('select[name=DATA_TYPE]').val(),
                LOCATION: section_retrieve.find('select[name=LOCATION]').val(),
                DEVICE_ID: section_retrieve.find('select[name=DEVICE_ID]').val(),
                DATA_FIELD: section_retrieve.find('select[name=DATA_FIELD]').val(),
            },
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
                            htmlRow += '<td>' + row.DATA_TYPE + '</td>';
                            htmlRow += '<td>' + row.LOCATION + '</td>';
                            htmlRow += '<td>' + row.DEVICE_ID + '</td>';
                            htmlRow += '<td>' + row.DATA_FIELD + '</td>';
                            htmlRow += '<td>' + row.MAX_VALUE + '</td>';
                            htmlRow += '<td>' + row.MIN_VALUE + '</td>';
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
                SID: section_modify.find('input[name=SID]').val(),
                MODE: section_modify.find('input[name=MODE]').val(),
                DATA_TYPE: section_modify.find('input[name=DATA_TYPE]').val(),
                LOCATION: section_modify.find('input[name=LOCATION]').val(),
                DEVICE_ID: section_modify.find('input[name=DEVICE_ID]').val(),
                DATA_FIELD: section_modify.find('input[name=DATA_FIELD]').val(),
                MAX_VALUE: section_modify.find('input[name=MAX_VALUE]').val(),
                MIN_VALUE: section_modify.find('input[name=MIN_VALUE]').val(),
                CHECK_INTERVAL: section_modify.find('input[name=CHECK_INTERVAL]').val(),
                ALERT_INTERVAL: section_modify.find('input[name=ALERT_INTERVAL]').val(),
                SUN: section_modify.find('input[name=SUN]').val(),
                SUN_STIME: section_modify.find('input[name=SUN_STIME]').val(),
                SUN_ETIME: section_modify.find('input[name=SUN_ETIME]').val(),
                MON: section_modify.find('input[name=MON]').val(),
                MON_STIME: section_modify.find('input[name=MON_STIME]').val(),
                MON_ETIME: section_modify.find('input[name=MON_ETIME]').val(),
                TUE: section_modify.find('input[name=TUE]').val(),
                TUE_STIME: section_modify.find('input[name=TUE_STIME]').val(),
                TUE_ETIME: section_modify.find('input[name=TUE_ETIME]').val(),
                WED: section_modify.find('input[name=WED]').val(),
                WED_STIME: section_modify.find('input[name=WED_STIME]').val(),
                WED_ETIME: section_modify.find('input[name=WED_ETIME]').val(),
                THU: section_modify.find('input[name=THU]').val(),
                THU_STIME: section_modify.find('input[name=THU_STIME]').val(),
                THU_ETIME: section_modify.find('input[name=THU_ETIME]').val(),
                FRI: section_modify.find('input[name=FRI]').val(),
                FRI_STIME: section_modify.find('input[name=FRI_STIME]').val(),
                FRI_ETIME: section_modify.find('input[name=FRI_ETIME]').val(),
                STA: section_modify.find('input[name=STA]').val(),
                STA_STIME: section_modify.find('input[name=STA_STIME]').val(),
                STA_ETIME: section_modify.find('input[name=STA_ETIME]').val(),
                CHECK_DATE: section_modify.find('input[name=CHECK_DATE]').val(),
                ALERT_DATE: section_modify.find('input[name=ALERT_DATE]').val(),
                MAIL_TO: section_modify.find('input[name=MAIL_TO]').val(),
                CHECK_HR_CALENDAR: section_modify.find('input[name=CHECK_HR_CALENDAR]').val()
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

    UsersRetrieve: function () {
        var url = '/User/UsersRetrieve';
        var section_retrieve = $('#section_retrieve');
        var request = {
            USERS: {
                //SN: section_retrieve.find('input[name=SN]').val(),
                ID: section_retrieve.find('input[name=ID]').val(),
                NAME: section_retrieve.find('input[name=NAME]').val(),
                MODE: section_retrieve.find('select[name=MODE]').val()
            },
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
                        $.each(response.USERS, function (idx, row) {
                            htmlRow = '<tr>';
                            htmlRow += '<td><a class="fa fa-edit fa-lg" onclick="UsersIndex.UsersQuery(' + row.SN + ');" data-toggle="tooltip" data-placement="right" title="修改"></a></td>';
                            //htmlRow += '<td>' + row.SN + '</td>';
                            htmlRow += '<td>' + row.ID + '</td>';
                            htmlRow += '<td>' + row.NAME + '</td>';
                            htmlRow += '<td>' + (row.EMAIL ? row.EMAIL : '') + '</td>';
                            htmlRow += '<td>' + row.MODE + '</td>';
                            htmlRow += '<td>' + (row.MEMO ? row.MEMO : '') + '</td>';
                            //htmlRow += '<td>' + row.CDATE.replace('T', ' ') + '</td>';
                            //htmlRow += '<td>' + row.CUSER + '</td>';
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

    DataValidate: function () {
        var error = '';
        var section_modify = $('#section_modify');

        var request = {
            ALERT_CONFIG: {
                SID: section_modify.find('input[name=SID]').val(),
                MODE: section_modify.find('select[name=MODE]').val(),
                DATA_TYPE: section_modify.find('select[name=DATA_TYPE]').val(),
                LOCATION: section_modify.find('select[name=LOCATION]').val(),
                DEVICE_ID: section_modify.find('select[name=DEVICE_ID]').val(),
                DATA_FIELD: section_modify.find('select[name=DATA_FIELD]').val(),
                MAX_VALUE: section_modify.find('input[name=MAX_VALUE]').val(),
                MIN_VALUE: section_modify.find('input[name=MIN_VALUE]').val(),
                CHECK_INTERVAL: section_modify.find('input[name=CHECK_INTERVAL]').val(),
                ALERT_INTERVAL: section_modify.find('input[name=ALERT_INTERVAL]').val(),
                SUN: section_modify.find('input[name=SUN]').is(':checked'),
                SUN_STIME: section_modify.find('input[name=SUN_STIME]').val(),
                SUN_ETIME: section_modify.find('input[name=SUN_ETIME]').val(),
                MON: section_modify.find('input[name=MON]').is(':checked'),
                MON_STIME: section_modify.find('input[name=MON_STIME]').val(),
                MON_ETIME: section_modify.find('input[name=MON_ETIME]').val(),
                TUE: section_modify.find('input[name=TUE]').is(':checked'),
                TUE_STIME: section_modify.find('input[name=TUE_STIME]').val(),
                TUE_ETIME: section_modify.find('input[name=TUE_ETIME]').val(),
                WED: section_modify.find('input[name=WED]').is(':checked'),
                WED_STIME: section_modify.find('input[name=WED_STIME]').val(),
                WED_ETIME: section_modify.find('input[name=WED_ETIME]').val(),
                THU: section_modify.find('input[name=THU]').is(':checked'),
                THU_STIME: section_modify.find('input[name=THU_STIME]').val(),
                THU_ETIME: section_modify.find('input[name=THU_ETIME]').val(),
                FRI: section_modify.find('input[name=FRI]').is(':checked'),
                FRI_STIME: section_modify.find('input[name=FRI_STIME]').val(),
                FRI_ETIME: section_modify.find('input[name=FRI_ETIME]').val(),
                STA: section_modify.find('input[name=STA]').is(':checked'),
                STA_STIME: section_modify.find('input[name=STA_STIME]').val(),
                STA_ETIME: section_modify.find('input[name=STA_ETIME]').val(),
                CHECK_DATE: section_modify.find('input[name=CHECK_DATE]').val(),
                ALERT_DATE: section_modify.find('input[name=ALERT_DATE]').val(),
                //MAIL_TO: section_modify.find('input[name=MAIL_TO]').val(),
                CHECK_HR_CALENDAR: section_modify.find('input[name=CHECK_HR_CALENDAR]').val()
            }
        };

        if (!request.ALERT_CONFIG.MODE) {
            error += '警報狀態不可空白<br />';
        }
        if (!request.ALERT_CONFIG.DATA_TYPE) {
            error += '監控類別不可空白<br />';
        }
        if (!request.ALERT_CONFIG.LOCATION) {
            error += '位置不可空白<br />';
        }
        if (!request.ALERT_CONFIG.DEVICE_ID) {
            error += '設備名稱不可空白<br />';
        }
        if (!request.ALERT_CONFIG.DATA_FIELD) {
            error += '數據欄位不可空白<br />';
        }
        if (request.ALERT_CONFIG.MAX_VALUE) {
            if (!ALERT_CONFIGIndex.RealValidate(request.ALERT_CONFIG.MAX_VALUE)) {
                error += '最大值輸入錯誤<br />';
            }
        }
        else {
            error += '最大值不可空白<br />';
        }
        if (request.ALERT_CONFIG.MIN_VALUE) {
            if (!ALERT_CONFIGIndex.RealValidate(request.ALERT_CONFIG.MIN_VALUE)) {
                error += '最小值輸入錯誤<br />';
            }
        }
        else {
            error += '最小值不可空白<br />';
        }

        if (request.ALERT_CONFIG.CHECK_INTERVAL) {
            if (!ALERT_CONFIGIndex.MinuteValidate(request.ALERT_CONFIG.CHECK_INTERVAL)) {
                error += '檢查間隔時間(分)輸入錯誤<br />';
            }
        }
        else {
            error += '檢查間隔時間(分)不可空白<br />';
        }

        if (request.ALERT_CONFIG.ALERT_INTERVAL) {
            if (!ALERT_CONFIGIndex.MinuteValidate(request.ALERT_CONFIG.ALERT_INTERVAL)) {
                error += '通知間隔時間(分)輸入錯誤<br />';
            }
        }
        else {
            error += '通知間隔時間(分)不可空白<br />';
        }

        {

            //星期日
            if (request.ALERT_CONFIG.SUN_STIME) {
                if (!ALERT_CONFIGIndex.TimeValidate(request.ALERT_CONFIG.SUN_STIME)) {
                    error += '星期日-時間(起)之時間格式錯誤<br />';
                }
            }
            else {
                if (request.ALERT_CONFIG.SUN) {
                    if (!request.ALERT_CONFIG.SUN_STIME) {
                        error += '星期日-時間(起)不可空白<br />';
                    }
                }
            }
            if (request.ALERT_CONFIG.SUN_ETIME) {
                if (!ALERT_CONFIGIndex.TimeValidate(request.ALERT_CONFIG.SUN_ETIME)) {
                    error += '星期日-時間(迄)之時間格式錯誤<br />';
                }
            }
            else {
                if (request.ALERT_CONFIG.SUN) {
                    if (!request.ALERT_CONFIG.SUN_ETIME) {
                        error += '星期日-時間(迄)不可空白<br />';
                    }
                }
            }
            //星期一
            if (request.ALERT_CONFIG.MON_STIME) {
                if (!ALERT_CONFIGIndex.TimeValidate(request.ALERT_CONFIG.MON_STIME)) {
                    error += '星期一-時間(起)之時間格式錯誤<br />';
                }
            }
            else {
                if (request.ALERT_CONFIG.MON) {
                    if (!request.ALERT_CONFIG.MON_STIME) {
                        error += '星期一-時間(起)不可空白<br />';
                    }
                }
            }
            if (request.ALERT_CONFIG.MON_ETIME) {
                if (!ALERT_CONFIGIndex.TimeValidate(request.ALERT_CONFIG.MON_ETIME)) {
                    error += '星期一-時間(迄)之時間格式錯誤<br />';
                }
            }
            else {
                if (request.ALERT_CONFIG.MON) {
                    if (!request.ALERT_CONFIG.MON_ETIME) {
                        error += '星期一-時間(迄)不可空白<br />';
                    }
                }
            }
            //星期二
            if (request.ALERT_CONFIG.TUE_STIME) {
                if (!ALERT_CONFIGIndex.TimeValidate(request.ALERT_CONFIG.TUE_STIME)) {
                    error += '星期二-時間(起)之時間格式錯誤<br />';
                }
            }
            else {
                if (request.ALERT_CONFIG.TUE) {
                    if (!request.ALERT_CONFIG.TUE_STIME) {
                        error += '星期二-時間(起)不可空白<br />';
                    }
                }
            }
            if (request.ALERT_CONFIG.TUE_ETIME) {
                if (!ALERT_CONFIGIndex.TimeValidate(request.ALERT_CONFIG.TUE_ETIME)) {
                    error += '星期二-時間(迄)之時間格式錯誤<br />';
                }
            }
            else {
                if (request.ALERT_CONFIG.TUE) {
                    if (!request.ALERT_CONFIG.TUE_ETIME) {
                        error += '星期二-時間(迄)不可空白<br />';
                    }
                }
            }
            //星期三
            if (request.ALERT_CONFIG.WED_STIME) {
                if (!ALERT_CONFIGIndex.TimeValidate(request.ALERT_CONFIG.WED_STIME)) {
                    error += '星期三-時間(起)之時間格式錯誤<br />';
                }
            }
            else {
                if (request.ALERT_CONFIG.WED) {
                    if (!request.ALERT_CONFIG.WED_STIME) {
                        error += '星期三-時間(起)不可空白<br />';
                    }
                }
            }
            if (request.ALERT_CONFIG.WED_ETIME) {
                if (!ALERT_CONFIGIndex.TimeValidate(request.ALERT_CONFIG.WED_ETIME)) {
                    error += '星期三-時間(迄)之時間格式錯誤<br />';
                }
            }
            else {
                if (request.ALERT_CONFIG.WED) {
                    if (!request.ALERT_CONFIG.WED_ETIME) {
                        error += '星期三-時間(迄)不可空白<br />';
                    }
                }
            }
            //星期四
            if (request.ALERT_CONFIG.THU_STIME) {
                if (!ALERT_CONFIGIndex.TimeValidate(request.ALERT_CONFIG.THU_STIME)) {
                    error += '星期四-時間(起)之時間格式錯誤<br />';
                }
            }
            else {
                if (request.ALERT_CONFIG.THU) {
                    if (!request.ALERT_CONFIG.THU_STIME) {
                        error += '星期四-時間(起)不可空白<br />';
                    }
                }
            }
            if (request.ALERT_CONFIG.THU_ETIME) {
                if (!ALERT_CONFIGIndex.TimeValidate(request.ALERT_CONFIG.THU_ETIME)) {
                    error += '星期四-時間(迄)之時間格式錯誤<br />';
                }
            }
            else {
                if (request.ALERT_CONFIG.THU) {
                    if (!request.ALERT_CONFIG.THU_ETIME) {
                        error += '星期四-時間(迄)不可空白<br />';
                    }
                }
            }
            //星期五
            if (request.ALERT_CONFIG.FRI_STIME) {
                if (!ALERT_CONFIGIndex.TimeValidate(request.ALERT_CONFIG.FRI_STIME)) {
                    error += '星期五-時間(起)之時間格式錯誤<br />';
                }
            }
            else {
                if (request.ALERT_CONFIG.FRI) {
                    if (!request.ALERT_CONFIG.FRI_STIME) {
                        error += '星期五-時間(起)不可空白<br />';
                    }
                }
            }
            if (request.ALERT_CONFIG.FRI_ETIME) {
                if (!ALERT_CONFIGIndex.TimeValidate(request.ALERT_CONFIG.FRI_ETIME)) {
                    error += '星期五-時間(迄)之時間格式錯誤<br />';
                }
            }
            else {
                if (request.ALERT_CONFIG.FRI) {
                    if (!request.ALERT_CONFIG.FRI_ETIME) {
                        error += '星期五-時間(迄)不可空白<br />';
                    }
                }
            }
            //星期六
            if (request.ALERT_CONFIG.STA_STIME) {
                if (!ALERT_CONFIGIndex.TimeValidate(request.ALERT_CONFIG.STA_STIME)) {
                    error += '星期六-時間(起)之時間格式錯誤<br />';
                }
            }
            else {
                if (request.ALERT_CONFIG.STA) {
                    if (!request.ALERT_CONFIG.STA_STIME) {
                        error += '星期六-時間(起)不可空白<br />';
                    }
                }
            }
            if (request.ALERT_CONFIG.STA_ETIME) {
                if (!ALERT_CONFIGIndex.TimeValidate(request.ALERT_CONFIG.STA_ETIME)) {
                    error += '星期六-時間(迄)之時間格式錯誤<br />';
                }
            }
            else {
                if (request.ALERT_CONFIG.STA) {
                    if (!request.ALERT_CONFIG.STA_ETIME) {
                        error += '星期六-時間(迄)不可空白<br />';
                    }
                }
            }
        }

        if ($('#MAIL_TO option').length === 0) {
            error += '郵件通知清單不可空白<br />';
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
            $('#SUN').prop('checked', false);
            $('#MON').prop('checked', false);
            $('#TUE').prop('checked', false);
            $('#WED').prop('checked', false);
            $('#THU').prop('checked', false);
            $('#FRI').prop('checked', false);
            $('#STA').prop('checked', false);
            $('#LOCATION').find('option').remove();
            $('#DEVICE_ID').find('option').remove();
            $('#DATA_FIELD').find('option').remove();
            $('#MAIL_TO').empty();
        }
    },

    MailValidate: function () {
        var error = '';

        var MAIL = {
            EMAIL: $('input[name=EMAIL]').val()
        };

        if (!MAIL.EMAIL) {
            error += '收件不可空白<br />';
        }
        else {
            const re = /^(([^<>()\[\]\\.,;:\s@"]+(\.[^<>()\[\]\\.,;:\s@"]+)*)|(".+"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
            if (!re.test(MAIL.EMAIL)) {
                error += '電子郵件格式輸入錯誤<br />';
            }
        }

        if (error.length > 0) {
            $('#modal .modal-title').text('提示訊息');
            $('#modal .modal-body').html('<p>' + error + '</p>');
            $('#modal').modal('show');
        }
        return error.length === 0;
    },

    TimeValidate: function (time) {
        var valid = /^([0-1]?[0-9]|2[0-4]):([0-5][0-9])(:[0-5][0-9])?$/.test(time);
        return valid;
    },

    RealValidate: function (numbers) {
        var valid = /^[-+]?\d*\.?\d*$/.test(numbers);
        return valid;
    },

    MinuteValidate: function (minute) {
        var valid = /^\d+$/.test(minute);
        return valid;
    }



};
