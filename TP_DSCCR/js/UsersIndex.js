var UsersIndex = {
    Action: null,
    USERS: null,

    Page_Init: function () {
        this.EventBinding();
        this.OptionRetrieve();
        this.ActionSwitch('R');
    },

    EventBinding: function () {
        $('#query').click(function () {
            UsersIndex.UsersRetrieve('R');
        });

        $('#page_number, #page_size').change(function () {
            UsersIndex.UsersRetrieve('R');
        });

        $('#create').click(function () {
            UsersIndex.ActionSwitch('C');
        });

        $('#save').click(function () {
            if (UsersIndex.DataValidate()) {
                if (UsersIndex.Action === 'C') {
                    UsersIndex.UsersCreate();
                } else if (UsersIndex.Action === 'U') {
                    UsersIndex.UsersUpdate();
                }
            }
        });

        $('#undo').click(function () {
            UsersIndex.ValueRecover(UsersIndex.Action);
        });

        $('#delete').click(function () {
            $('#modal_action .modal-title').text('提示訊息');
            $('#modal_action .modal-body').html('<p>確定要刪除該筆資料?</p>');
            //$('#confirm').attr('data-action', 'delete');
            UsersIndex.ConfirmAction = 'delete';
            $('#modal_action #confirm').show();
            $('#modal_action').modal('show');
        });

        $('#reset').click(function () {
            $('#modal_action .modal-title').text('提示訊息');
            $('#modal_action .modal-body').html('<p>確定要重置密碼?</p>');
            //$('#confirm').attr('data-action', 'reset');
            UsersIndex.ConfirmAction = 'reset';
            $('#modal_action #confirm').show();
            $('#modal_action').modal('show');
        });

        $('#return').click(function () {
            UsersIndex.ActionSwitch('R');
            UsersIndex.ValueRecover();
            UsersIndex.UsersRetrieve();
        });

        $('#modal_action #confirm').click(function () {
            $('#confirm').hide();
            $('#modal_action').modal('hide');
            console.log(UsersIndex.ConfirmAction);
            if (UsersIndex.ConfirmAction === 'delete') {
                UsersIndex.UsersDelete();
            } if (UsersIndex.ConfirmAction === 'reset') {
                UsersIndex.UsersReset();
            }
        });

        $('#section_modify #ID').keyup(function () {
            $("#section_modify #ID").val($("#section_modify #ID").val().toUpperCase());
        });

        $('#section_modify #PASSWORD').keyup(function () {
            $("#section_modify #PASSWORD").val($("#section_modify #PASSWORD").val().toUpperCase());
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
            $('#reset').show();
            //$('#section_modify #ID').prop('disabled', true);
            $('#section_modify').show();
        } else if (Action === 'C') {
            $('#save').show();
            $('#return').show();
            $('#undo').show();
            //$('#section_modify #ID').prop('disabled', false);
            $('#section_modify').show();
        }
        this.Action = Action;
    },

    OptionRetrieve: function () {
        var url = '/Main/ItemListRetrieve';
        var request = {
            //TableItem: ['userName'],
            PhraseGroup: ['page_size', 'mode']
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

                    var section_retrieve = $('#section_retrieve');
                    section_retrieve.find("select[name='MODE']").append('<option value=""></option>');
                    $.each(response.ItemList.mode, function (idx, row) {
                        $("select[name='MODE']").append($('<option></option>').attr('value', row.Key).text(row.Value));
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

    UsersRetrieve: function () {
        var url = 'UsersRetrieve';
        var section_retrieve = $('#section_retrieve');
        var request = {
            USERS: {
                SN: section_retrieve.find('input[name=SN]').val(),
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
                            htmlRow += '<td>' + row.SN + '</td>';
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

    UsersCreate: function () {
        var url = 'UsersCreate';
        var section_modify = $('#section_modify');
        var request = {
            USERS: {
                ID: section_modify.find('input[name=ID]').val(),
                NAME: section_modify.find('input[name=NAME]').val(),
                PASSWORD: section_modify.find('input[name=PASSWORD]').val(),
                EMAIL: section_modify.find('input[name=EMAIL]').val(),
                MODE: section_modify.find('select[name=MODE]').val(),
                MEMO: section_modify.find('textarea[name=MEMO]').val()
            },
            GRANTS: {
                ROLES_SN: section_modify.find('select[name=ROLES]').val()
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
                    UsersIndex.UsersQuery(response.USERS.SN);
                    UsersIndex.ActionSwitch('U');
                }
                $('#modal .modal-title').text('交易訊息');
                $('#modal .modal-body').html('<p>交易代碼:' + response.Result.State + '<br/>交易說明:' + response.Result.Msg + '</p>');
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

    UsersUpdate: function () {
        var url = 'UsersUpdate';
        var section_modify = $('#section_modify');
        var request = {
            USERS: {
                SN: section_modify.find('input[name=SN]').val(),
                ID: section_modify.find('input[name=ID]').val(),
                NAME: section_modify.find('input[name=NAME]').val(),
                PASSWORD: section_modify.find('input[name=PASSWORD]').val(),
                EMAIL: section_modify.find('input[name=EMAIL]').val(),
                MODE: section_modify.find('select[name=MODE]').val(),
                MEMO: section_modify.find('textarea[name=MEMO]').val()
            },
            GRANTS: {
                ROLES_SN: section_modify.find('select[name=ROLES]').val()
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
                    UsersIndex.UsersQuery(response.USERS.SN);
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

    UsersDelete: function () {
        var url = 'UsersDelete';
        var section_modify = $('#section_modify');
        var request = {
            USERS: {
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
                    UsersIndex.UsersRetrieve();
                    UsersIndex.ActionSwitch('R');
                    //UsersIndex.ValueRecover();
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

    UsersReset: function () {
        var url = 'UsersReset';
        var request = {
            USERS: {
                SN: $('#section_modify #SN').val(),
                ID: $('#section_modify #ID').val()
            }
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                if (response.Result.State === 5) {
                    UsersIndex.UsersQuery(response.USERS.SN);
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

    UsersQuery: function (SN) {
        var url = 'UsersQuery';
        var request = {
            USERS: {
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
                    section_modify.find('input[name=SN]').val(response.USERS.SN),
                    section_modify.find('input[name=ID]').val(response.USERS.ID);
                    section_modify.find('input[name=NAME]').val(response.USERS.NAME);
                    section_modify.find('input[name=PASSWORD]').val(response.USERS.PASSWORD);
                    section_modify.find('input[name=EMAIL]').val(response.USERS.EMAIL);
                    section_modify.find('select[name=MODE]').val(response.USERS.MODE);
                    section_modify.find('textarea[name=MEMO]').val(response.USERS.MEMO);
                    section_modify.find('input[name=CDATE]').val(response.USERS.CDATE.replace('T', ' '));
                    section_modify.find('input[name=CUSER]').val(response.USERS.CUSER);
                    section_modify.find('input[name=MDATE]').val(response.USERS.MDATE.replace('T', ' '));
                    section_modify.find('input[name=MUSER]').val(response.USERS.MUSER);
                    section_modify.find('select[name=ROLES]').val(response.GRANTS.ROLES_SN);

                    UsersIndex.USERS = response.USERS;
                    UsersIndex.ActionSwitch('U');
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
        var USERS = {
            SN: section_modify.find('input[name=SN]').val(),
            ID: section_modify.find('input[name=ID]').val(),
            NAME: section_modify.find('input[name=NAME]').val(),
            PASSWORD: section_modify.find('input[name=PASSWORD]').val(),
            EMAIL: section_modify.find('input[name=EMAIL]').val(),
            MODE: section_modify.find('select[name=MODE]').val(),
            MEMO: section_modify.find('textarea[name=MEMO]').val()
        };
        if (!USERS.ID) {
            error += '帳號不可空白<br />';
        }
        if (!USERS.NAME) {
            error += '姓名不可空白<br />';
        }
        if (UsersIndex.Action === 'C') {
            if (!USERS.PASSWORD) {
                error += '密碼不可空白<br />';
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
            if (UsersIndex.USERS) {
                $('.modify').each(function (index, value) {
                    if (value.id === 'CDATE' || value.id === 'MDATE') {
                        $(value).val(UsersIndex.USERS[value.id].replace('T', ' '));
                    }
                    else {
                        $(value).val(UsersIndex.USERS[value.id]);
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
