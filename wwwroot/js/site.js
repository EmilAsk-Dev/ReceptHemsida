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

/*Add Recipe*/
let ingredientIndex = 1;

document.getElementById("addIngredient").addEventListener("click", function () {
    const container = document.getElementById("ingredientsContainer");

    const ingredientDiv = document.createElement("div");
    ingredientDiv.classList.add("ingredient");
    ingredientDiv.setAttribute("data-index", ingredientIndex); // Unikt ID för radering

    // Namn
    const nameLabel = document.createElement("label");
    nameLabel.textContent = "Namn:";
    const nameInput = document.createElement("input");
    nameInput.setAttribute("type", "text");
    nameInput.setAttribute("name", `Ingredients[${ingredientIndex}].Ingredient.Name`);
    nameInput.required = true;

    // Mängd
    const amountLabel = document.createElement("label");
    amountLabel.textContent = "Mängd:";
    const amountInput = document.createElement("input");
    amountInput.setAttribute("type", "number");
    amountInput.setAttribute("name", `Ingredients[${ingredientIndex}].Amount`);
    amountInput.required = true;

    // Enhet
    const unitLabel = document.createElement("label");
    unitLabel.textContent = "Enhet:";
    const unitInput = document.createElement("input");
    unitInput.setAttribute("type", "text");
    unitInput.setAttribute("name", `Ingredients[${ingredientIndex}].Unit`);
    unitInput.required = true;

    // Ta bort-knapp
    const removeButton = document.createElement("button");
    removeButton.textContent = "❌";
    removeButton.setAttribute("type", "button");
    removeButton.classList.add("remove-btn");

    removeButton.addEventListener("click", function () {
        ingredientDiv.remove();
    });

   

    // Lägg till div i container
    container.appendChild(ingredientDiv);

    ingredientIndex++;

})