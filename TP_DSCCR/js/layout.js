$(document).ready(function () {
    $(document).ajaxStart($.blockUI).ajaxStop($.unblockUI);

    $("#sidebarCollapse").click(function (e) {
        e.preventDefault();
        $("#sidebar").toggleClass("active");
    });
});


