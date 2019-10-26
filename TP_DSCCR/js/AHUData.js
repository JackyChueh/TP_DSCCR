var AHUData = {

    Page_Init: function () {
        AHUData.EventBinding();
        AHUData.OptionRetrieve();
        AHUData.ActionSwitch('R');
    },

    EventBinding: function () {
        $('#SDATE').datetimepicker({ formatTime: 'H', format: 'Y/m/d H:00', value: new Date(2019, 9, 15, 0, 0) });
        $('#EDATE').datetimepicker({ formatTime: 'H', format: 'Y/m/d H:00' });

        $('#query').click(function () {
            AHUData.AHURetrieve();
        });

        $('#page_number, #page_size').change(function () {
            AHUData.AHURetrieve();
        });

        $('#LOCATION').change(function () {
            AHUData.SubOptionRetrieve($('#DEVICE_ID'), $(this).val());
        });

        $('#GRAPH_TYPE').change(function () {
            AHUData.AHUGraph('AHU05', $(this).val());
        });


        //$('#modal-chart').on('show', function () {
        //    $(this).find('.modal-body').css({
        //        'max-height': '100%'
        //    });
        //});
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

    OptionRetrieve: function () {
        var url = '/Main/ItemListRetrieve';
        var request = {
            //TableItem: ['userName'],
            PhraseGroup: ['page_size', 'AHU_LOCATION', 'GROUP_BY_DT', 'GRAPH_TYPE']
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
                    $.each(response.ItemList.AHU_LOCATION, function (idx, row) {
                        $('#LOCATION').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });
                    $('#LOCATION option[value="0B1F"]').attr("selected", true);
                    $("#LOCATION").trigger("change");

                    //$('#GROUP_BY_DT').append('<option value=""></option>');
                    $.each(response.ItemList.GROUP_BY_DT, function (idx, row) {
                        $('#GROUP_BY_DT').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });
                    $('#GROUP_BY_DT option[value="HOUR"]').attr("selected", true);


                    //$('#GRAPH_TYPE').append('<option value=""></option>');
                    $.each(response.ItemList.GRAPH_TYPE, function (idx, row) {
                        $('#GRAPH_TYPE').append($('<option></option>').attr('value', row.Key).text(row.Value));
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
    },

    SubOptionRetrieve: function (obj, parentKey) {
        if (parentKey) {
            var url = '/Main/SubItemListRetrieve';
            var request = {
                PhraseGroup: 'AHU_DEVICE_ID',
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

    AHURetrieve: function () {
        var url = 'AHURetrieve';
        var request = {
            AHU: {
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
                        $.each(response.AHUData, function (idx, row) {
                            htmlRow = '<tr>';
                            htmlRow += '<td>' + row.CDATE + '</td>';
                            htmlRow += '<td>' + row.LOCATION + '</td>';
                            htmlRow += '<td>' + row.DEVICE_ID + '</td>';
                            var css = '';
                            if (row.AHU01 === "停止") {
                                css = ' class="text-danger"';
                            }
                            htmlRow += '<td' + css + '>' + row.AHU01 + '</td>';
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
                        htmlRow = '<tr><td colspan="14" style="text-align:center">data not found</td></tr>';
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
    }

};
