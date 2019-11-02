$(document).ready(function () {
    //$(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);
    $(document).ajaxStart(function () {
        $.blockUI({
            message: '<img src="/img/Spinner168px.svg" /> ',
            css: { borderWidth: '0px', backgroundColor: 'transparent' }
        });
    });

    $(document).ajaxComplete(function () {
        $.unblockUI();
    });

    $("#sidebarCollapse").click(function (e) {
        //e.preventDefault();
        $("#sidebar").toggleClass("active");
    });

    $('#logout').click(function () {
        window.location.href = "/Main/Login";
    });
});


