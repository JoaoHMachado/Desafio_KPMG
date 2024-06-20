// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function OcultarBtn() {
    var noPeopleMessage = document.getElementById("no-people-message");
    if (noPeopleMessage) {
        var btns = document.getElementsByName("Consultar");
        for (var i = 0; i < btns.length; i++) {
            btns[i].style.display = "none";
        }
   }
}


