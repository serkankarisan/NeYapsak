/*typewriter*/ 
//made by vipul mirajkar thevipulm.appspot.com
var TxtType = function(el, toRotate, period) {
	this.toRotate = toRotate;
	this.el = el;
	this.loopNum = 0;
	this.period = parseInt(period, 10) || 2000;
	this.txt = '';
	this.tick();
	this.isDeleting = false;
};

TxtType.prototype.tick = function() {
	var i = this.loopNum % this.toRotate.length;
	var fullTxt = this.toRotate[i];

	if (this.isDeleting) {
	this.txt = fullTxt.substring(0, this.txt.length - 1);
	} else {
	this.txt = fullTxt.substring(0, this.txt.length + 1);
	}

	this.el.innerHTML = '<span class="wrap">'+this.txt+'</span>';

	var that = this;
	var delta = 200 - Math.random() * 100;

	if (this.isDeleting) { delta /= 2; }

	if (!this.isDeleting && this.txt === fullTxt) {
	delta = this.period;
	this.isDeleting = true;
	} else if (this.isDeleting && this.txt === '') {
	this.isDeleting = false;
	this.loopNum++;
	delta = 500;
	}

	setTimeout(function() {
	that.tick();
	}, delta);
};

window.onload = function() {
	var elements = document.getElementsByClassName('typewrite');
	for (var i=0; i<elements.length; i++) {
		var toRotate = elements[i].getAttribute('data-type');
		var period = elements[i].getAttribute('data-period');
		if (toRotate) {
		  new TxtType(elements[i], JSON.parse(toRotate), period);
		}
	}
	// INJECT CSS
	var css = document.createElement("style");
	css.type = "text/css";
	css.innerHTML = ".typewrite > .wrap { border-right: 0.08em solid #fff}";
	document.body.appendChild(css);
};


$(document).ready(function(){
	
	// Scrollspy initiation
	$('body').scrollspy({ 
		target: '#scroll-spy',
		offset: 70
	});

	// On render, adjust body padding to ensure last Scroll target can reach top of screen
	var height = $('#Kayit').innerHeight();
	var windowHeight = $(window).height();
	var navHeight = $('nav.navbar').innerHeight();
	var siblingHeight = $('#Kayit').nextAll().innerHeight();


	if(height < windowHeight){
		$('body').css("padding-bottom", windowHeight-navHeight-height-siblingHeight + "px");
	}

	// On window resize, adjust body padding to ensure last Scroll target can reach top of screen
	$(window).resize(function(event){
		var height = $('#Kayit').innerHeight();
		var windowHeight = $(window).height();
		var navHeight = $('nav.navbar').innerHeight();
		var siblingHeight = $('#Kayit').nextAll().innerHeight();
		
		
		if(height < windowHeight){
			$('body').css("padding-bottom", windowHeight-navHeight-height-siblingHeight + "px");
		}
	});
	
	$('nav.navbar a, .scrollTop').click(function(event){
		// Make sure this.hash has a value before overriding default behavior
		if (this.hash !== "") {
			// Prevent default anchor click behavior
			event.preventDefault();

			// Store hash (#)
			var hash = this.hash;
			
			// Ensure no section has relevant class
			$('section').removeClass("focus");

			// Add class to target
			$(hash).addClass("focus");

			// Remove thy class after timeout
			setTimeout(function(){
				$(hash).removeClass("focus");
			}, 2000);			
			
    // Using jQuery's animate() method to add smooth page scroll
    // The optional number (800) specifies the number of milliseconds it takes to scroll to the specified area (the speed of the animation)
			$('html, body').animate({
				scrollTop: $(hash).offset().top - 69
			}, 600, function(){
				
				// Add hash (#) to URL when done scrolling (default click behavior)
				window.location.hash = hash;				
			});
					
			// Collapse Navbar for mobile view
			$(".navbar-collapse").collapse('hide');			
		}

	});
	$(window).scroll(function(){
		var scrollPos = $('body').scrollTop();
		if(scrollPos > 0){
			$('.navbar').addClass('show-color');
			$('.scrollTop').addClass('show-button');
		} else{
			$('.navbar').removeClass('show-color');
			$('.scrollTop').removeClass('show-button');
		}
		
	});
});
    $(document).ready(function () {
        $("#GirisYap").click(function () {
            var loginmodel = {};
            loginmodel.Email = $('#logemail').val();
            loginmodel.RememberMe = $('#logbenihatirla').val();
            loginmodel.Password = $('#logsifre').val();
            $.ajax({
                type: "POST",
                url: '/Account/Login',
                data: { model: loginmodel },
                success: function (result) {
                    if (result === "True") {
                        window.location.href = "/Home/Main";
                    }
                    else {
                        $('#modal-login-body').text("");
                        $.each(result, function (i, hata) {
                            $('#modal-login-body').append(hata + "<br/><br/>");
                        });
                        
                        $('#LoginModalCenter').modal('show');
                    }
                }
            });
        });
});
$(document).ready(function () {
    $("#KayitOl").click(function () {
        var registermodel = {};
        registermodel.Name = $('#regname').val();
        registermodel.Surname = $('#regsurname').val();
        registermodel.Email = $('#regemail').val();
        registermodel.DogumTarihi = $('#regdogumtarihi').val();
        registermodel.Password = $('#regsifre').val();
        registermodel.ConfirmPassword = $('#regsifretekrar').val();
        console.log("rmodel "+registermodel);
        $.ajax({
            type: "POST",
            url: '/Account/Register',
            data: { model: registermodel },
            success: function (result) {
                console.log(result);
                if (result === "True") {
                    window.location.href = "/Account/DisplayEmail";
                }
                else {
                    $('#modal-register-body').text("");
                    $.each(result, function (i, hata) {
                        $('#modal-register-body').append(hata + "<br/><br/>");
                    });

                    $('#RegisterModalCenter').modal('show');
                }
            }
        });
    });
});