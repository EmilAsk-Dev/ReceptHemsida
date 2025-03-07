// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.


const userModal = document.getElementById("userModal")
userModal.style.display = "none";
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

//Register
const togglePasswordVisibility = (inputElement,
    toggleElement,iconElement) => {
    if (inputElement.type === "password") {
        inputElement.type = "text";
        iconElement.remove("fa-eye-slash"); 
        iconElement.add("fa-eye");
    }
    else {
        inputElement.type = "password";
        iconElement.remove("fa-eye-slash");
        iconElement.add("fa-eye");
    }

    const passwordInput = document.getElementById("password");
    const togglePassword = document.getElementById("togglePassword");
    const passwordConfirm= document.getElementById("passwordConfirm");
    const togglePasswordConfirm = document.getElementById("togglePasswordConfirm");

    togglepassword.addEventListener("click", () => {
        togglePasswordVisibility(passwordInput, togglepassword);
    });

    togglePasswordConfirm.addEventListener("click", () => {
        togglePasswordVisibility(passwordConfirm, togglePasswordConfirm.querySelector("i"));
    });
}


