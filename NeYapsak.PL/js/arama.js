$(document).ready(function () {
    $("button#tum-etk-ara").click(function () {
        var kelime = $('input#tum-etk-ara').val().trim();
        if (kelime.trim()!='') {
            $.ajax({
                url: '/Home/MainBySearch?kelime=' + kelime,
                data: { kelime: kelime },
                success: function (result) {
                    window.location.href = '/Home/MainBySearch?kelime=' + kelime;
                }
            });
        }
    })});