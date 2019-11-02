var Login = {

    Page_Init: function () {
        Login.EventBinding();
    },

    EventBinding: function () {
        $('#Login').click(function () {
            Login.LoginCheck();
        });
    },

    LoginCheck: function () {
        var url = 'LoginCheck';
        var request = {
            USERS: {
                ID: $('#ID').val(),
                PASSWORD: $('#PASSWORD').val()
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
                    //
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
