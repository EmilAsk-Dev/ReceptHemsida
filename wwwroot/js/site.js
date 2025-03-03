// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function openModal() {
    document.getElementById("userModal").style.display = "flex";
}

function closeModal() {
    document.getElementById("userModal").style.display = "none";
}

// Stänger modalen om användaren klickar utanför den
window.onclick = function (event) {
    let modal = document.getElementById("userModal");
    if (event.target == modal) {
        modal.style.display = "none";
    }
}
