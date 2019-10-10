var AHUGraph = {
    Action: null,
    CASE: null,
    INTERVIEW: [],
    //Option: null,
    QueryStartDate: null,
    QueryEndDate: null,

    Page_Init: function () {
        AHUGraph.EventBinding();
        //AHUGraph.OptionRetrieve();
        AHUGraph.ActionSwitch('R');

        //var obj;
        //obj = document.getElementById('chartLine').getContext('2d');
        //obj.height = 200;
        //var chartLines = new Chart(obj, {
        //    // The type of chart we want to create
        //    type: 'line',

        //    // The data for our dataset
        //    data: {
        //        labels: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],
        //        datasets: [{
        //            label: '訂單查詢',
        //            fill: false,
        //            backgroundColor: 'rgb(255, 99, 132)',
        //            borderColor: 'rgb(255, 99, 132)',
        //            data: [0, 1000, 500, 200, 2000, 3000, 4500, 1000, 500, 200, 2000, 2200]
        //        }, {
        //            label: '意見反應',
        //            fill: false,
        //            borderDash: [5, 5],
        //            backgroundColor: 'rgb(0, 255, 132)',
        //            borderColor: 'rgb(0, 255, 132)',
        //            data: [0, 2000, 1000, 1700, 500, 3400, 3500, 4000, 5000, 1700, 500, 3300]
        //        }, {
        //            label: '會員權益',
        //            backgroundColor: 'rgb(54, 162, 235,0.3)',
        //            borderColor: 'rgb(54, 162, 235,0,3)',
        //            fill: true,
        //            data: [0, 3000, 500, 1200, 1000, 3500, 5000, 1500, 1500, 1200, 200, 1828]
        //        }]
        //    },

        //    // Configuration options go here
        //    options: {
        //        responsive: true,
        //        maintainAspectRatio: false,
        //        title: {
        //            display: true,
        //            text: '進線類型'
        //        }
        //    }
        //});
        //var ctx = document.getElementById('myChart').getContext('2d');
        //obj = $('#chartLine');
        //var chartLine = new Chart(obj, {
        //    // The type of chart we want to create
        //    type: 'line',

        //    // The data for our dataset
        //    data: {
        //        labels: ['06/01', '06/02', '06/03', '06/04', '06/05', '06/06', '06/07'],
        //        datasets: [{
        //            label: '手機',
        //            backgroundColor: 'rgb(255, 99, 132)',
        //            borderColor: 'rgb(255, 99, 132)',
        //            data: [0, 10000, 5000, 2000, 20000, 30000, 45000]
        //        }, {
        //            label: '電話',
        //            backgroundColor: 'rgb(00, 255, 132)',
        //            borderColor: 'rgb(00, 255, 132)',
        //            data: [0, 40000, 50000, 17000, 5000, 34000, 40408]
        //        }, {
        //            label: '超商',
        //            backgroundColor: 'rgb(54, 162, 235)',
        //            borderColor: 'rgb(54, 162, 235)',
        //            data: [0, 50000, 5000, 12000, 10000, 35000, 55000]
        //        }]
        //    },

        //    // Configuration options go here
        //    options: {
        //        scales: {
        //            yAxes: [{
        //                stacked: true
        //            }]
        //        },
        //        title: {
        //            display: true,
        //            text: '來源管道'
        //        }
        //    }
        //});


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
        var url = 'AHUGraph';
        var request = {
            AHU: {
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
                    var obj;
                    obj = document.getElementById('chartLine').getContext('2d');
                    obj.height = 200;
                    var chartLines = new Chart(obj, response.ChartLine);
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
    }

};
