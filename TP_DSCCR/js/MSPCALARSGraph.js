﻿var MSPCALARSGraph = {
    LoginUrl: null,
    ChartObj: null,

    Page_Init: function () {
        MSPCALARSGraph.EventBinding();
        MSPCALARSGraph.OptionRetrieve();
        MSPCALARSGraph.ActionSwitch('R');
        $('#FIELD option:nth-child(1)').attr("selected", true);
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
            MSPCALARSGraph.MSPCALARSGraph();
        });

        $('#print').click(function () {
            MSPCALARSGraph.MSPCALARSGraphPrint();
        });

        $('#LOCATION').change(function () {
            $('#WATER_TOWER').val('');
            MSPCALARSGraph.SubOptionRetrieve($('#DEVICE_ID'), $(this).val());
        });

        $('#WATER_TOWER').change(function () {
            $('#LOCATION').val('');
            MSPCALARSGraph.WaterTowerSubOptionRetrieve($('#DEVICE_ID'), $(this).val());
        });

        $('#login').click(function () {
            window.location.href = MSPCALARSGraph.LoginUrl;
        });
    },

    ActionSwitch: function (action) {
        $('form').hide();
        $('.card-header button').hide();
        if (action === 'R') {
            $('#query').show();
            $('#print').show();
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
            PhraseGroup: ['page_size', 'MSPCALARS_LOCATION', 'GROUP_BY_DT', 'GRAPH_TYPE', 'MSPCALARS_WATER_TOWER']
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
                    $.each(response.ItemList.MSPCALARS_LOCATION, function (idx, row) {
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

                    $('#WATER_TOWER').append('<option value=""></option>');
                    $.each(response.ItemList.MSPCALARS_WATER_TOWER, function (idx, row) {
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
                //alert('' + xhr.status + ';' + status );
            }
        });
    },

    SubOptionRetrieve: function (obj, parentKey) {
        if (parentKey) {
            var url = '/Main/SubItemListRetrieve';
            var request = {
                PhraseGroup: 'MSPCALARS_DEVICE_ID',
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

    WaterTowerSubOptionRetrieve: function (obj, waterTowerKey) {
        if (waterTowerKey) {
            var url = '/Main/WaterTowerSubItemListRetrieve';
            var request = {
                PhraseGroup: 'MSPCALARS_DEVICE_ID',
                WaterTowerKey: waterTowerKey
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

    MSPCALARSGraph: function () {
        var url = 'MSPCALARSGraph';
        var request = {
            MSPCALARS: {
                LOCATION: $('#LOCATION').val(),
                DEVICE_ID: $('#DEVICE_ID').val(),
                WATER_TOWER: $('#WATER_TOWER').val()
            },
            SDATE: $('#SDATE').val(),
            EDATE: $('#EDATE').val(),
            FIELD: $('#FIELD').val(),
            FIELD_NAME: $('#FIELD :selected').text(),
            GROUP_BY_DT: $('#GROUP_BY_DT').val(),
            GROUP_BY_DT_NAME: $('#GROUP_BY_DT :selected').text(),
            GRAPH_TYPE: $('#GRAPH_TYPE').val()
        };

        $.ajax({
            type: 'post',
            url: url,
            contentType: 'application/json',
            data: JSON.stringify(request),
            success: function (data) {
                var response = JSON.parse(data);
                MSPCALARSGraph.ModalSwitch(response.Result.State);
                if (response.Result.State === 0) {
                    if (response.Chart) {
                        if (MSPCALARSGraph.ChartObj) {
                            MSPCALARSGraph.ChartObj.destroy();
                        }
                        var obj;
                        obj = document.getElementById('chartCanvas').getContext('2d');
                        MSPCALARSGraph.ChartObj = new Chart(obj, response.Chart);
                        //MSPCALARSGraph.ChartObj.resize();
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

    MSPCALARSGraphPrint: function () {
        var win = window.open();
        //win.document.open();
        win.document.write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\"  \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">");
        win.document.write('<head><title></title>');
        win.document.write('</head><body style="padding:0;margin-top:0 !important;margin-bottom:0!important;"   onLoad="self.print();self.close();">');
        //var content_vlue = document.getElementById("chartCanvas").innerHTML;
        //win.document.write(content_vlue);
        var canvas = document.getElementById("chartCanvas");
        win.document.write("<img src='" + canvas.toDataURL("image/png", 1) + "'/>");
        win.document.write('</body></html>');
        //document.write(doct + divContent.innerHTML);
        win.document.close();
        win.focus();
    }


};
