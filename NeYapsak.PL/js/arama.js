//Etkinlik Arama

$(document).ready(function () {
    $("button#tum-etk-ara").click(function () {
        var kelime = $('input#tum-etk-ara').val().trim();
        if (kelime.trim() != '') {
            $.ajax({
                url: '/Home/MainBySearch?search=' + kelime,
                data: { search: kelime },
                success: function (result) {
                    window.location.href = '/Home/MainBySearch?search=' + kelime;
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

//Kullanıcı Arama

$(document).ready(function () {
    $('input#tum-kul-ara').keydown(function (event) {
        if (event.which == 13) {
            $("button#button-addon2").trigger("click");
            event.preventDefault();
        };
    });
    $("button#button-addon2").click(function () {
        var user = $('input#tum-kul-ara').val().trim();
        if (user.trim() != '') {
            $.ajax({
                url: '/Home/Profiles?user=' + user,
                data: { user: user },
                success: function (result) {
                    window.location.href = '/Home/Profiles?user=' + user;
                }
            });
        }
    });
});