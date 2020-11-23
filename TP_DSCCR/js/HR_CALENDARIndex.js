var HR_CALENDARIndex = {
    LoginUrl: null,
    Action: null,
    HR_CALENDAR: null,
    ConfirmAction: null,

    Page_Init: function () {
        this.EventBinding();
        this.OptionRetrieve();
        this.ActionSwitch('R');
        HR_CALENDARIndex.HR_CALENDARRetrieve();
    },

    EventBinding: function () {

        $('#query').click(function () {
            HR_CALENDARIndex.HR_CALENDARRetrieve();
        });

        $('#page_number, #page_size').change(function () {
            HR_CALENDARIndex.HR_CALENDARRetrieve();
        });

        $('#create').click(function () {
            HR_CALENDARIndex.ActionSwitch('C');
        });

        $('#create-year').click(function () {
            HR_CALENDARIndex.ActionSwitch('Y');
        });


        $('#save').click(function () {
            if (HR_CALENDARIndex.DataValidate()) {
                if (HR_CALENDARIndex.Action === 'C') {
                    HR_CALENDARIndex.HR_CALENDARCreate();
                } else if (HR_CALENDARIndex.Action === 'U') {
                    HR_CALENDARIndex.HR_CALENDARUpdate();
                } else if (HR_CALENDARIndex.Action === 'Y') {
                    HR_CALENDARIndex.HR_CALENDARCreateYear();
                }
            }
        });

        $('#undo').click(function () {
            HR_CALENDARIndex.ValueRecover(HR_CALENDARIndex.Action);
        });

        $('#delete').click(function () {
            if (HR_CALENDARIndex.Action === 'Y') {
                if (!HR_CALENDARIndex.DataValidate()) {
                    return false;
                }
                HR_CALENDARIndex.ConfirmAction = 'delete-year';
            }
            if (HR_CALENDARIndex.Action === 'U') {
                HR_CALENDARIndex.ConfirmAction = 'delete';
            }
            $('#modal_action .modal-title').text('提示訊息');
            $('#modal_action .modal-body').html('<p>確定要刪除該筆資料?</p>');
            //$('#confirm').attr('data-action', 'delete');
            
            $('#modal_action #confirm').show();
            $('#modal_action').modal('show');

        });

        $('#return').click(function () {
            HR_CALENDARIndex.ActionSwitch('R');
            HR_CALENDARIndex.ValueRecover();
            HR_CALENDARIndex.HR_CALENDARRetrieve();

        });

        $('#modal_action #confirm').click(function () {
            $('#confirm').hide();
            $('#modal_action').modal('hide');
            if (HR_CALENDARIndex.ConfirmAction === 'delete') {
                HR_CALENDARIndex.HR_CALENDARDelete();
            }
            if (HR_CALENDARIndex.ConfirmAction === 'delete-year') {
                HR_CALENDARIndex.HR_CALENDARDeleteYear();
            }
        });

        $('#login').click(function () {
            window.location.href = HR_CALENDARIndex.LoginUrl;
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
        section_retrieve.find('input[name=HR_DATE_START]').datetimepicker({ timepicker: false, format: 'Y-m-d', value: new Date(today.getFullYear(), 0, 1) });
        section_retrieve.find('input[name=HR_DATE_END]').datetimepicker({ timepicker: false, format: 'Y-m-d', value: new Date(today.getFullYear(), 11, 31) });
    },

    ActionSwitch: function (Action) {
        $('form').hide();
        $('.card-header button').hide();
        if (Action === 'R') {
            $('#query').show();
            $('#create').show();
            $('#create-year').show();
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
        } else if (Action === 'Y') {
            $('#save').show();
            $('#delete').show();
            $('#return').show();
            $('#undo').show();
            $('#section_year').show();
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
            TableItem: ['YearList'],
            PhraseGroup: ['DATE_TYPE']
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

                    //$.each(response.ItemList.page_size, function (idx, row) {
                    //    $('#page_size').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    //});
                    //$('#page_size option[value="100"]').attr("selected", true);

                    //var section_modify = $('#section_modify');
                    //section_modify.find("select[name='DATE_TYPE']").append('<option value=""></option>');
                    $("select[name='DATE_TYPE']").find('option').remove();
                    $("select[name='DATE_TYPE']").append('<option value=""></option>');
                    $.each(response.ItemList.DATE_TYPE, function (idx, row) {
                        $("select[name='DATE_TYPE']").append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });
                    

                    var section_retrieve = $('#section_retrieve');
                    //section_retrieve.find("select[name='YEAR']").append('<option value=""></option>');
                    section_retrieve.find("select[name='YEAR']").find('option').remove();
                    $.each(response.ItemList.YearList, function (idx, row) {
                        section_retrieve.find("select[name='YEAR']").append($('<option></option>').attr('value', row.Key).text(row.Value));
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

    HR_CALENDARRetrieve: function () {
        var url = 'HR_CALENDARRetrieve';
        var section_retrieve = $('#section_retrieve');
        var request = {
            HR_CALENDAR: {
                //DATE_TYPE: section_retrieve.find('select[name=DATE_TYPE]').val()
            },
            YEAR: section_retrieve.find('select[name=YEAR]').val(),
            //HR_DATE_START: section_retrieve.find('input[name=HR_DATE_START]').val(),
            //HR_DATE_END: section_retrieve.find('input[name=HR_DATE_END]').val(),
            PageNumber: $('#page_number').val() ? $('#page_number').val() : 1,
            PageSize: $('#page_size').val() ? $('#page_size').val() : 1
        };
        
        var table_calendar = $('#table-calendar');
        table_calendar.html('');

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                HR_CALENDARIndex.ModalSwitch(response.Result.State);
                if (response.Result.State === 0) {
               

                    var firstDay, lastDay;
                    $.each(response.HR_MONTHS, function (idx, rowMonth) {

                        firstDay = new Date(request.YEAR, rowMonth.Month - 1, 1);
                        lastDay = new Date(request.YEAR, rowMonth.Month - 1, 1);
                        lastDay.setMonth(lastDay.getMonth() + 1);
                        lastDay.setDate(lastDay.getDate() - 1);

                        //RWD最外層單元
                        var unit = $('<div/>', {
                            'class': 'col-xl-6',
                            appendTo: $(table_calendar)
                        });

                        //card元件
                        var card = $('<div/>', {
                            'class': 'card',
                            appendTo: $(unit)
                        });

                        //card表頭
                        var card_header = $('<div/>', {
                            'class': 'card-header font-weight-bold',
                            css: {
                                textAlign: 'center'
                            },
                            html: rowMonth.Month + '月',
                            appendTo: $(card)
                        });

                        //card表身
                        var card_body = $('<div/>', {
                            'class': 'card-body',
                            appendTo: $(card)
                        });

                        //card表身底下的月曆元件
                        var card_body_table = $('<table/>', {
                            'class': 'table table-sm table-bordered table-hover table-striped hr-month',
                            appendTo: $(card_body)
                        });

                        //table底下thead
                        var thead = $('<thead/>', {
                            html: '<tr><th class="text-danger">日</th><th>一</th><th>二</th><th>三</th><th>四</th><th>五</th><th class="text-danger">六</th></tr>',
                            appendTo: $(card_body_table)
                        });

                        //table底下tbody
                        tbody = $('<tbody/>', {
                            appendTo: $(card_body_table)
                        });

                        //tbody底下tr & td
                        var tr, td;
                        var cols = 0, rows = 0, field = 1, day;
                        $.each(rowMonth.HR_CALENDARS, function (idx, row) {

                            if (cols === 7) {
                                cols = 0;
                                rows++;
                            }

                            if (cols === 0) {
                                tr = $('<tr/>', {
                                    appendTo: $(tbody)
                                });

                                if (rows === 0) {
                                    if (firstDay.getDay() !== 0) {
                                        for (i = 0; i < firstDay.getDay(); i++) {
                                            td = $('<td/>', {
                                                appendTo: $(tr)
                                            });
                                            cols++;

                                            if (cols === 7) {
                                                cols = 0;
                                                rows++;
                                                if (cols === 0) {
                                                    tr = $('<tr/>', {
                                                        appendTo: $(tbody)
                                                    });
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            day = parseInt(row.HR_DATE.substr(8, 2));
                            while (field < day) {
                                
                                td = $('<td />', {
                                    html: '<div onclick="HR_CALENDARIndex.HR_CALENDARAppendByDate(\'' + request.YEAR + '-' + ('00' + rowMonth.Month).substr(('00' + rowMonth.Month).length - 2) + '-' + ('00' + field).substr(('00' + field).length - 2) + '\')" class="pr"><div class="tl">' + field + '</div><div class="br"></div></div>',
                                    appendTo: $(tr)
                                });

                                field++;
                                cols++;

                                if (cols === 7) {
                                    cols = 0;
                                    rows++;
                                    if (cols === 0) {
                                        tr = $('<tr/>', {
                                            appendTo: $(tbody)
                                        });
                                    }
                                }
                            }

                            td = $('<td/>', {
                                click: function () {
                                    HR_CALENDARIndex.HR_CALENDARQueryByDate(row.HR_DATE.substr(0, 10));
                                },
                                html: '<div class="pr' + ((row.DATE_TYPE === '休假日') ? ' text-danger' : '') + '"><div class="tl">' + field + '</div><div class="br">' + row.DATE_TYPE + '</div></div>',
                                appendTo: $(tr)
                            });
                            field++;
                            cols++;

                        });

                        //當月後半段沒資料
                        while (field <= lastDay.getDate()) {

                            if (cols === 7) {
                                cols = 0;
                                rows++;
                                if (cols === 0) {
                                    tr = $('<tr/>', {
                                        appendTo: $(tbody)
                                    });
                                }
                            }

                            td = $('<td />', {
                                html: '<div onclick="HR_CALENDARIndex.HR_CALENDARAppendByDate(\'' + request.YEAR + '-' + ('00' + rowMonth.Month).substr(('00' + rowMonth.Month).length - 2) + '-' + ('00' + field).substr(('00' + field).length - 2) + '\')" class="pr"><div class="tl">' + field + '</div><div class="br"></div></div>',
                                appendTo: $(tr)
                            });
                            field++;
                            cols++;

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
                $('#modal .modal-title').text(ajaxOptions);
                $('#modal .modal-body').html('<p>' + xhr.status + ' ' + thrownError + '</p>');
                $('#modal').modal('show');
            },
            complete: function (xhr, status) {
            }
        });
    },

    HR_CALENDARCreateYear: function () {
        var url = 'HR_CALENDARCreateYear';
        //var section_modify = $('#section_modify');
        var section_year = $('#section_year');
        var request = {
            HR_CALENDAR: {
                //HR_DATE: section_modify.find('input[name=HR_DATE]').val(),
                //DATE_TYPE: section_modify.find('select[name=DATE_TYPE]').val(),
                //MEMO: section_modify.find('textarea[name=MEMO]').val()
            },
            YEAR: section_year.find('input[name=YEAR]').val(),
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                HR_CALENDARIndex.ModalSwitch(response.Result.State);
                if (response.Result.State === 1) {
                    HR_CALENDARIndex.OptionRetrieve();
                    HR_CALENDARIndex.HR_CALENDARRetrieve();
                    HR_CALENDARIndex.ActionSwitch('R');
                    HR_CALENDARIndex.ValueRecover();
                } else if (response.Result.State === -10) {
                    response.Result.Msg = request.YEAR + '年行事曆內，至少有一個日期已存在，不可重複設定。';
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

    HR_CALENDARCreate: function () {
        var url = 'HR_CALENDARCreate';
        var section_modify = $('#section_modify');
        var request = {
            HR_CALENDAR: {
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
                HR_CALENDARIndex.ModalSwitch(response.Result.State);
                if (response.Result.State === 1) {
                    HR_CALENDARIndex.OptionRetrieve();
                    HR_CALENDARIndex.HR_CALENDARQuery(response.HR_CALENDAR.SN);
                    HR_CALENDARIndex.ActionSwitch('U');
                } else if (response.Result.State === -10) {
                    response.Result.Msg = '日期' + request.HR_CALENDAR.HR_DATE + '已存在，不可重複設定。';
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

    HR_CALENDARUpdate: function () {
        var url = 'HR_CALENDARUpdate';
        var section_modify = $('#section_modify');
        var request = {
            HR_CALENDAR: {
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
                HR_CALENDARIndex.ModalSwitch(response.Result.State);
                if (response.Result.State === 2) {
                    HR_CALENDARIndex.OptionRetrieve();
                    HR_CALENDARIndex.HR_CALENDARQuery(response.HR_CALENDAR.SN);
                } else if (response.Result.State === -10) {
                    response.Result.Msg = '日期' + request.HR_CALENDAR.HR_DATE + '已存在，不可重複設定。';
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

    HR_CALENDARDeleteYear: function () {
        var url = 'HR_CALENDARDeleteYear';
        var section_year = $('#section_year');
        var request = {
            HR_CALENDAR: {
            },
            YEAR: section_year.find('input[name=YEAR]').val(),
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                HR_CALENDARIndex.ModalSwitch(response.Result.State);
                if (response.Result.State === 3) {
                    HR_CALENDARIndex.OptionRetrieve();
                    HR_CALENDARIndex.ActionSwitch('R');
                    HR_CALENDARIndex.ValueRecover();
                    HR_CALENDARIndex.HR_CALENDARRetrieve();
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

    HR_CALENDARDelete: function () {
        var url = 'HR_CALENDARDelete';
        var section_modify = $('#section_modify');
        var request = {
            HR_CALENDAR: {
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
                HR_CALENDARIndex.ModalSwitch(response.Result.State);
                if (response.Result.State === 3) {
                    HR_CALENDARIndex.OptionRetrieve();
                    HR_CALENDARIndex.ActionSwitch('R');
                    HR_CALENDARIndex.ValueRecover();
                    HR_CALENDARIndex.HR_CALENDARRetrieve();
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

    HR_CALENDARAppendByDate: function (date) {
        var section_modify = $('#section_modify');
        section_modify.find('input[name=HR_DATE]').val(date);
        HR_CALENDARIndex.ActionSwitch('C');
    },

    HR_CALENDARQueryByDate: function (date) {
        var url = 'HR_CALENDARQuery';
        var request = {
            HR_CALENDAR: {
                HR_DATE: date
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                HR_CALENDARIndex.ModalSwitch(response.Result.State);
                var section_modify = $('#section_modify');
                if (response.Result.State === 0) {
                    section_modify.find('input[name=SN]').val(response.HR_CALENDAR.SN);
                    section_modify.find('input[name=HR_DATE]').val(response.HR_CALENDAR.HR_DATE.substr(0, 10));
                    section_modify.find('select[name=DATE_TYPE]').val(response.HR_CALENDAR.DATE_TYPE);
                    section_modify.find('textarea[name=MEMO]').val(response.HR_CALENDAR.MEMO);
                    section_modify.find('input[name=CDATE]').val(response.HR_CALENDAR.CDATE.replace('T', ' '));
                    section_modify.find('input[name=CUSER]').val(response.HR_CALENDAR.CUSER);
                    section_modify.find('input[name=MDATE]').val(response.HR_CALENDAR.MDATE.replace('T', ' '));
                    section_modify.find('input[name=MUSER]').val(response.HR_CALENDAR.MUSER);

                    HR_CALENDARIndex.HR_CALENDAR = response.HR_CALENDAR;
                    HR_CALENDARIndex.ActionSwitch('U');
                }
                else if (response.Result.State === -6) {
                    section_modify.find('input[name=HR_DATE]').val(date);
                    HR_CALENDARIndex.ActionSwitch('C');
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

    HR_CALENDARQuery: function (SN) {
        var url = 'HR_CALENDARQuery';
        var request = {
            HR_CALENDAR: {
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
                HR_CALENDARIndex.ModalSwitch(response.Result.State);
                if (response.Result.State === 0) {
                    var section_modify = $('#section_modify');
                    section_modify.find('input[name=SN]').val(response.HR_CALENDAR.SN);
                    section_modify.find('input[name=HR_DATE]').val(response.HR_CALENDAR.HR_DATE.substr(0, 10));
                    section_modify.find('select[name=DATE_TYPE]').val(response.HR_CALENDAR.DATE_TYPE);
                    section_modify.find('textarea[name=MEMO]').val(response.HR_CALENDAR.MEMO);
                    section_modify.find('input[name=CDATE]').val(response.HR_CALENDAR.CDATE.replace('T', ' '));
                    section_modify.find('input[name=CUSER]').val(response.HR_CALENDAR.CUSER);
                    section_modify.find('input[name=MDATE]').val(response.HR_CALENDAR.MDATE.replace('T', ' '));
                    section_modify.find('input[name=MUSER]').val(response.HR_CALENDAR.MUSER);

                    HR_CALENDARIndex.HR_CALENDAR = response.HR_CALENDAR;
                    HR_CALENDARIndex.ActionSwitch('U');
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
        var section_year = $('#section_year');

        var request = {
            HR_CALENDAR: {
                HR_DATE: section_modify.find('input[name=HR_DATE]').val(),
                DATE_TYPE: section_modify.find('select[name=DATE_TYPE]').val(),
                MEMO: section_modify.find('textarea[name=MEMO]').val()
            },
            YEAR: section_year.find('input[name=YEAR]').val(),
        };

        if (HR_CALENDARIndex.Action === 'C' || HR_CALENDARIndex.Action === 'U') {
            if (!request.HR_CALENDAR.HR_DATE) {
                error += '日期不可空白<br />';
            }
            if (!request.HR_CALENDAR.DATE_TYPE) {
                error += '類別不可空白<br />';
            }
        }

        if (HR_CALENDARIndex.Action === 'Y') {
            if (!request.YEAR) {
                error += '年份不可空白<br />';
            }
            else if (!HR_CALENDARIndex.YearValidate(request.YEAR) || request.YEAR < 1753) {
                error += '年份輸入錯誤<br />';
            }
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
            if (HR_CALENDARIndex.HR_CALENDAR) {
                $('.modify').each(function (index, value) {
                    if (value.id === 'CDATE' || value.id === 'MDATE') {
                        $(value).val(HR_CALENDARIndex.HR_CALENDAR[value.id].replace('T', ' '));
                    }
                    else if (value.id === 'HR_DATE') {
                        $(value).val(HR_CALENDARIndex.HR_CALENDAR[value.id].substr(0, 10));
                    }
                    else {
                        $(value).val(HR_CALENDARIndex.HR_CALENDAR[value.id]);
                    }
                });
            }
        }
        else {
            $('.modify').each(function (index, value) {
                $(value).val('');
            });
        }
    },

    YearValidate: function (year) {
        var valid = /^\d{4}$/.test(year);
        return valid;
    }
};
