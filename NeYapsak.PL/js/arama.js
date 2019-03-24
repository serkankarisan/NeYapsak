$(document).ready(function () {
    $("button#tum-etk-ara").click(function () {
        var kelime = $('input#tum-etk-ara').val().trim();
        if (kelime.trim() != '') {
            $.ajax({
                url: '/Home/MainBySearch?kelime=' + kelime,
                data: { kelime: kelime },
                success: function (result) {
                    window.location.href = '/Home/MainBySearch?kelime=' + kelime;
                }
            });
        }
    });
    $(document).ready(function () {
        $('input#tum-etk-ara').keydown(function (event) {
            if (event.which == 13) {
                $("button#tum-etk-ara").trigger("click");
                event.preventDefault();
            }
        });
    });
});