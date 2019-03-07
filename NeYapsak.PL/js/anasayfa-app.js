var etkOls = document.getElementById("etk-ols");
var etkOlsBtn = document.getElementById("btn-ols");

etkOlsBtn.onclick = function myFunction() {
    if (etkOls.style.display === "none") {
      etkOls.style.display = "block";
    } else {
      etkOls.style.display = "none";
    }
  }