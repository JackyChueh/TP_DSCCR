var AHUIndex = {
    Action: null,
    CASE: null,
    INTERVIEW: [],
    //Option: null,
    QueryStartDate: null,
    QueryEndDate: null,

    Page_Init: function () {
        AHUIndex.EventBinding();
        //AHUIndex.OptionRetrieve();
        AHUIndex.ActionSwitch('R');
    },

    EventBinding: function () {
        $('#query').click(function () {
            AHUIndex.AHURetrieve();
        });

        $('#page_number, #page_size').change(function () {
            if ($('#section_retrieve').valid()) {
                AHUIndex.AHURetrieve();
            }
        });

        //var section_retrieve = $('#section_retrieve');
        //var obj = null;
        //obj = section_retrieve.find('[name=DATE_RANGE]');
        //AHUIndex.DateRangePickerBind2(obj);

        //obj.change(function () {
        //    if (!obj.val()) {
        //        AHUIndex.QueryStartDate = null;
        //        AHUIndex.QueryEndDate = null;
        //    }
        //});

    },

    ActionSwitch: function (action) {
        $('form').hide();
        $('.card-header button').hide();
        if (action === 'R') {
            $('#query').show();
            $('#create').show();
            $('#section_retrieve').show();
        } else if (action === 'U') {
            $('#save').show();
            $('#delete').show();
            $('#return').show();
            $('#undo').show();
            $('#add').show();
            $('#section_modify').show();
        } else if (action === 'C') {
            $('#save').show();
            $('#return').show();
            $('#undo').show();
            $('#add').show();
            $('#section_modify').show();
        }
        AHUIndex.Action = action;

    },

    OptionRetrieve: function () {
        var url = '/Main/ItemListRetrieve';
        var request = {
            TableItem: ['userName'],
            PhraseGroup: ['page_size', 'CONTACT_TIME', 'TREATMENT', 'CASE_SOURCE', 'GENDER', 'AGE', 'EDUCATION', 'CAREER', 'CITY', 'SPECIAL_IDENTITY', 'MARRIAGE', 'PHYSIOLOGY', 'PSYCHOLOGY', 'VISITED', 'INTERVIEW_CLASSIFY', 'SOLVE_DEGREE', 'FEELING', 'INTERVENTION', 'CASE_STATUS']
        };

        $.ajax({
            async: false,
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                //console.log(data);
                var response = JSON.parse(data);
                //console.log(response);
                if (response.ReturnStatus.Code === 0) {

                    //$('#page_size').append('<option value=""></option>');
                    $.each(response.ItemList.page_size, function (idx, row) {
                        $('#page_size').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });

                    var section_retrieve = $('#section_retrieve');
                    var obj = null;

                    obj = section_retrieve.find('[name=CASE_STATUS]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.CASE_STATUS, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = section_retrieve.find('[name=CONTACT_TIME]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.CONTACT_TIME, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = section_retrieve.find('[name=VOLUNTEER_SN]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.userName, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    var $detail_template = $('#detail_template');
                    var obj = null;

                    obj = $detail_template.find('[name=VOLUNTEER_SN]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.userName, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=CONTACT_TIME]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.CONTACT_TIME, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=TREATMENT]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.TREATMENT, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=CASE_SOURCE]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.CASE_SOURCE, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=GENDER]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.GENDER, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=AGE]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.AGE, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=EDUCATION]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.EDUCATION, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=CAREER]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.CAREER, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=CITY]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.CITY, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=MARRIAGE]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.MARRIAGE, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=PHYSIOLOGY]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.PHYSIOLOGY, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=PSYCHOLOGY]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.PSYCHOLOGY, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    obj = $detail_template.find('[name=VISITED]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.VISITED, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    //特殊身份別
                    $.each(response.ItemList.SPECIAL_IDENTITY, function (idx, row) {
                        var template = `
                            <div class="form-group col-12 col-sm-6 col-md-6 col-lg-4 col-xl-4 " >
                                <label><input type="checkbox" class="fields" name="SPECIAL_IDENTITY" value="`+ row.Key + `">` + row.Key + ` ` + row.Value + `</label>
                                <input type="text" name="SPECIAL_IDENTITY`+ row.Key + `" class="form-control form-control-sm fields">
                            </div>
                        `;
                        var temp = $detail_template.find('[name=special_identity]').append(template);
                    });

                    //來電者主述問題分類之圈選
                    $.each(response.ItemList.INTERVIEW_CLASSIFY, function (idx, row) {
                        var lbl = $('<label>').attr('class', 'form-check form-check-inline');
                        if (row.Desc) lbl.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                        lbl.append('<input class="form-check-input fields" type="checkbox" name="INTERVIEW_CLASSIFY" value="' + row.Key + '">' + row.Key + ' ' + row.Value);
                        var temp = $detail_template.find('[name=interview_classify]').append(lbl);
                    });

                    //案主解決問題的程度
                    obj = $detail_template.find('[name=SOLVE_DEGREE]');
                    obj.append('<option value=""></option>');
                    $.each(response.ItemList.SOLVE_DEGREE, function (idx, row) {
                        var temp = obj.append($('<option></option>').attr('value', row.Key).text(row.Value));
                        if (row.Desc) temp.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                    });

                    //案主在此困擾的情緒
                    $.each(response.ItemList.FEELING, function (idx, row) {
                        var lbl = $('<label>').attr('class', 'form-check form-check-inline');
                        if (row.Desc) lbl.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                        lbl.append('<input class="form-check-input fields" style="border:1px solid red;" type="checkbox" name="FEELING" value="' + row.Key + '">' + row.Key + ' ' + row.Value);
                        var temp = $detail_template.find('[name=feeling]').append(lbl);
                    });

                    //接案過程介入方式
                    $.each(response.ItemList.INTERVENTION, function (idx, row) {
                        var lbl = $('<label>').attr('class', 'form-check form-check-inline');
                        if (row.Desc) lbl.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                        lbl.append('<input class="form-check-input fields" type="checkbox" name="INTERVENTION" value="' + row.Key + '">' + row.Key + ' ' + row.Value);
                        var temp = $detail_template.find('[name=intervention]').append(lbl);
                    });

                    //完成進度
                    $.each(response.ItemList.CASE_STATUS, function (idx, row) {
                        var lbl = $('<label>').attr('class', 'form-check form-check-inline');
                        if (row.Desc) lbl.attr('title', row.Desc).attr('data-toogle', 'tooltip');
                        lbl.append('<input class="form-check-input fields" type="radio" name="CASE_STATUS" value="' + row.Key + '">' + row.Value);
                        var temp = $detail_template.find('[name=case_status]').append(lbl);
                    });
                    obj = $detail_template.find('[name=case_status]').find('input');
                    $(obj[0]).prop('checked', true);



                }
                else {
                    $('#modal .modal-title').text('交易訊息');
                    $('#modal .modal-body').html('<p>交易代碼:' + response.ReturnStatus.Code + '<br/>交易說明:' + response.ReturnStatus.Message + '</p>');
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
    },

    AHURetrieve: function () {
        var url = 'AHURetrieve';
        var request = {
            AHU: {
                SID: $('#SID').val()
            },
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
                    var temp = '';
                    if (response.Pagination.RowCount > 0) {
                        $.each(response.AHU, function (idx, row) {
                            htmlRow = '<tr>';
                            //htmlRow += '<td><a class="fa fa-edit fa-lg" onclick="AHUIndex.RowSelected(' + row.SN + ');" data-toggle="tooltip" data-placement="right" title="修改" style="cursor: pointer;"></a></td>';
                            //htmlRow += '<td>' + row.SID + '</td>';
                            htmlRow += '<td>' + row.CDATE + '</td>';
                            //htmlRow += '<td>' + row.AUTOID + '</td>';
                            //htmlRow += '<td>' + row.DATETIME + '</td>';
                            htmlRow += '<td>' + row.LOCATION + '</td>';
                            htmlRow += '<td>' + row.DEVICE_ID + '</td>';
                            htmlRow += '<td>' + row.AHU01 + '</td>';
                            htmlRow += '<td>' + row.AHU02 + '</td>';
                            htmlRow += '<td>' + row.AHU03 + '</td>';
                            htmlRow += '<td>' + row.AHU04 + '</td>';
                            htmlRow += '<td>' + row.AHU05 + '</td>';
                            htmlRow += '<td>' + row.AHU06 + '</td>';
                            htmlRow += '<td>' + row.AHU07 + '</td>';
                            htmlRow += '<td>' + row.AHU08 + '</td>';
                            htmlRow += '<td>' + row.AHU09 + '</td>';
                            htmlRow += '<td>' + row.AHU10 + '</td>';
                            htmlRow += '<td>' + row.AHU11 + '</td>';
                            htmlRow += '</tr>';
                            $('#gridview >  tbody').append(htmlRow);
                        });
                    }
                    else {
                        htmlRow = '<tr><td colspan="13" style="text-align:center">data not found</td></tr>';
                        $('#gridview >  tbody').append(htmlRow);
                    }
                }
                else {
                    $('#modal .modal-title').text('交易訊息');
                    $('#modal .modal-body').html('<p>交易代碼:' + response.ReturnStatus.Code + '<br/>交易說明:' + response.ReturnStatus.Message + '</p>');
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
};
