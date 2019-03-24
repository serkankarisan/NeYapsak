$(document).ready(function () {
    $("button#sort").click(function () {
        var Sort = $(this).data("sort");
        console.log(Sort);
        console.log(window.location);
        var params = window.location.href.split('&');
        window.location.href = params[0] + Sort; 
    });
    });