var AHUGraph = {
    Action: null,
    ChartObj:null,

    Page_Init: function () {
        AHUGraph.EventBinding();
        AHUGraph.OptionRetrieve();

        $('#FIELD option[value="AHU05"]').attr("selected", true);   //test

        AHUGraph.ActionSwitch('R');
        $('#SDATE').datetimepicker({ formatTime: 'H', format: 'Y/m/d H:00' });
        $('#EDATE').datetimepicker({ formatTime: 'H', format: 'Y/m/d H:00' });
    },

    EventBinding: function () {
        $('#query').click(function () {
            AHUGraph.AHURetrieve();
        });

        $('#page_number, #page_size').change(function () {
            if ($('#section_retrieve').valid()) {
                AHUGraph.AHURetrieve();
            }
        });

        //var section_retrieve = $('#section_retrieve');
        //var obj = null;
        //obj = section_retrieve.find('[name=DATE_RANGE]');
        //AHUGraph.DateRangePickerBind2(obj);

        //obj.change(function () {
        //    if (!obj.val()) {
        //        AHUGraph.QueryStartDate = null;
        //        AHUGraph.QueryEndDate = null;
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
        AHUGraph.Action = action;

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
                    $('#LOCATION option[value="0B1F"]').attr("selected", true);

                    //$('#GROUP_BY_DT').append('<option value=""></option>');
                    $.each(response.ItemList.GROUP_BY_DT, function (idx, row) {
                        $('#GROUP_BY_DT').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });
                    $('#GROUP_BY_DT option[value="DAY"]').attr("selected", true);

                    //$('#GRAPH_TYPE').append('<option value=""></option>');
                    $.each(response.ItemList.GRAPH_TYPE, function (idx, row) {
                        $('#GRAPH_TYPE').append($('<option></option>').attr('value', row.Key).text(row.Value));
                    });

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
        var url = 'AHUGraph';
        var request = {
            AHU: {
                LOCATION: $('#LOCATION').val(),
                DEVICE_ID: $('#DEVICE_ID').val()
            },
            SDATE: $('#SDATE').val(),
            EDATE: $('#EDATE').val(),
            FIELD: $('#FIELD').val(),
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
                    if (AHUGraph.ChartObj) {
                        AHUGraph.ChartObj.clear();
                        AHUGraph.ChartObj.reset();
                        AHUGraph.ChartObj.destroy();
                    }
                    var obj;
                    obj = document.getElementById('chartLine').getContext('2d');
                    //obj.height = 200;
                    ChartObj = new Chart(obj, response.ChartLine);
                    //ChartObj.resize();
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
