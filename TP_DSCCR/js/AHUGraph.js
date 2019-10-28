var AHUGraph = {
    ChartObj:null,

    Page_Init: function () {
        AHUGraph.EventBinding();
        AHUGraph.OptionRetrieve();
        AHUGraph.ActionSwitch('R');
        $('#FIELD option:nth-child(3)').attr("selected", true);
    },

    EventBinding: function () {
        var now = new Date();
        $('#SDATE').datetimepicker({ formatTime: 'H', format: 'Y/m/d H:00', value: new Date(now.getFullYear(), now.getMonth(), now.getDate(), 0, 0) });
        $('#EDATE').datetimepicker({ formatTime: 'H', format: 'Y/m/d H:00' });

        $('#query').click(function () {
            AHUGraph.AHURetrieve();
        });

        $('#page_number, #page_size').change(function () {
            if ($('#section_retrieve').valid()) {
                AHUGraph.AHURetrieve();
            }
        });

        $('#LOCATION').change(function () {
            AHUGraph.SubOptionRetrieve($('#DEVICE_ID'), $(this).val());
        });
    },

    ActionSwitch: function (action) {
        $('form').hide();
        $('.card-header button').hide();
        if (action === 'R') {
            $('#query').show();
            $('#create').show();
            $('#section_retrieve').show();
        }
    },

    OptionRetrieve: function () {
        var url = '/Main/ItemListRetrieve';
        var request = {
            //TableItem: ['userName'],
            PhraseGroup: ['page_size', 'AHU_LOCATION', 'GROUP_BY_DT','GRAPH_TYPE']
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
                    $('#LOCATION option:nth-child(2)').attr("selected", true);
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
            //$(obj).find('option').not(':first').remove();
            //$(obj).append('<option value=""></option>');
        }
    },

    AHURetrieve: function () {
        var url = 'AHUGraph';
        var request = {
            AHU: {
                LOCATION: $('#LOCATION').val(),
                DEVICE_ID: $('#DEVICE_ID').val()
            },
            SDATE: $('#SDATE').val(),
            EDATE: $('#EDATE').val(),
            FIELD: $('#FIELD').val(),
            FIELD_NAME: $('#FIELD :selected').text(),
            GROUP_BY_DT: $('#GROUP_BY_DT').val(),
            GRAPH_TYPE: $('#GRAPH_TYPE').val()
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                if (response.Result.State === 0) {
                    if (response.Chart) {
                        if (AHUGraph.ChartObj) {
                            AHUGraph.ChartObj.destroy();
                        }
                        var obj;
                        obj = document.getElementById('chartLine').getContext('2d');
                        AHUGraph.ChartObj = new Chart(obj, response.Chart);
                        //AHUGraph.ChartObj.resize();
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
