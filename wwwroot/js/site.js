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
/*Addrecipe*/

document.getElementById('UploadedImage').addEventListener('change', function (e) {
    const file = e.target.files[0];
    if (file) {
        const reader = new FileReader();
        reader.onload = function (e) {
            document.getElementById('imagePreview').src = e.target.result;
        };
        reader.readAsDataURL(file);
    }
});

const tags = [];
const tagsContainer = document.getElementById('tagsContainer');
const tagsDataInput = document.getElementById('tagsData');

function updateTagsInput() {
    tagsDataInput.value = JSON.stringify(tags);
}

function renderTags() {
    tagsContainer.innerHTML = '';

    tags.forEach((tag, index) => {
        // Create tag element
        const tagElement = document.createElement('div');
        tagElement.className = 'tag-badge badge bg-secondary px-3 py-2';

        // Add tag text
        const tagText = document.createTextNode(tag + ' ');
        tagElement.appendChild(tagText);

        // Create remove button
        const removeBtn = document.createElement('button');
        removeBtn.type = 'button';
        removeBtn.className = 'tag-remove-btn btn-close btn-close-white ms-2';
        removeBtn.setAttribute('aria-label', 'Remove tag');
        removeBtn.setAttribute('data-index', index);

        // Add event listener to button
        removeBtn.addEventListener('click', function () {
            const index = parseInt(this.getAttribute('data-index'));
            tags.splice(index, 1);
            renderTags();
            updateTagsInput();
        });

        tagElement.appendChild(removeBtn);
        tagsContainer.appendChild(tagElement);
    });
}

document.getElementById('addTag').addEventListener('click', function () {
    const tagInput = document.getElementById('tagInput');
    const tag = tagInput.value.trim();

    if (tag && !tags.includes(tag)) {
        tags.push(tag);
        tagInput.value = '';
        renderTags();
        updateTagsInput();
    }
});
document.getElementById('tagInput').addEventListener('keypress', function (e) {
    if (e.key === 'Enter') {
        e.preventDefault();
        document.getElementById('addTag').click();
    }
});

// Dynamic ingredient fields
let ingredientCount = 1;
document.getElementById('addIngredient').addEventListener('click', function () {
    const container = document.getElementById('ingredientList');

    // Create main container div
    const ingredientRow = document.createElement('div');
    ingredientRow.className = 'ingredient-row mb-2';

    // Create row div
    const rowDiv = document.createElement('div');
    rowDiv.className = 'row ingredient-form-row';

    // Amount column
    const amountCol = document.createElement('div');
    amountCol.className = 'col-md-3 amount-column';

    const amountWrapper = document.createElement('div');
    amountWrapper.className = 'form-floating amount-wrapper';

    const amountInput = document.createElement('input');
    amountInput.type = 'text';
    amountInput.name = `Ingredients[${ingredientCount}].Quantity`;
    amountInput.className = 'form-control amount-input';
    amountInput.placeholder = 'Amount';

    const amountLabel = document.createElement('label');
    amountLabel.textContent = 'Amount';
    amountLabel.className = 'amount-label';

    amountWrapper.appendChild(amountInput);
    amountWrapper.appendChild(amountLabel);
    amountCol.appendChild(amountWrapper);

    // Unit column
    const unitCol = document.createElement('div');
    unitCol.className = 'col-md-3 unit-column';

    const unitWrapper = document.createElement('div');
    unitWrapper.className = 'form-floating unit-wrapper';

    const unitInput = document.createElement('input');
    unitInput.type = 'text';
    unitInput.name = `Ingredients[${ingredientCount}].Unit`;
    unitInput.className = 'form-control unit-input';
    unitInput.placeholder = 'Unit';

    const unitLabel = document.createElement('label');
    unitLabel.textContent = 'Unit';
    unitLabel.className = 'unit-label';

    unitWrapper.appendChild(unitInput);
    unitWrapper.appendChild(unitLabel);
    unitCol.appendChild(unitWrapper);

    // Ingredient name column
    const nameCol = document.createElement('div');
    nameCol.className = 'col-md-5 name-column';

    const nameWrapper = document.createElement('div');
    nameWrapper.className = 'form-floating name-wrapper';

    const nameInput = document.createElement('input');
    nameInput.type = 'text';
    nameInput.name = `Ingredients[${ingredientCount}].Name`;
    nameInput.className = 'form-control name-input';
    nameInput.placeholder = 'Ingredient';

    const nameLabel = document.createElement('label');
    nameLabel.textContent = 'Ingredient';
    nameLabel.className = 'name-label';

    nameWrapper.appendChild(nameInput);
    nameWrapper.appendChild(nameLabel);
    nameCol.appendChild(nameWrapper);

    const btnCol = document.createElement('div');
    btnCol.className = 'col-md-1 remove-column';

    const removeBtn = document.createElement('button');
    removeBtn.type = 'button';
    removeBtn.className = 'btn btn-outline-danger btn-sm remove-ingredient-btn';

    const trashIcon = document.createElement('i');
    trashIcon.className = 'bi bi-trash';

    removeBtn.appendChild(trashIcon);
    btnCol.appendChild(removeBtn);

    // Add remove event listener
    removeBtn.addEventListener('click', function () {
        ingredientRow.remove();
    });

    // Assemble all elements
    rowDiv.appendChild(amountCol);
    rowDiv.appendChild(unitCol);
    rowDiv.appendChild(nameCol);
    rowDiv.appendChild(btnCol);
    ingredientRow.appendChild(rowDiv);
    container.appendChild(ingredientRow);

    ingredientCount++;
});

// Dynamic instruction fields
let instructionCount = 1;
document.getElementById('addInstruction').addEventListener('click', function () {
    const container = document.getElementById('instructionList');
    instructionCount++;

    // Create main container
    const instructionRow = document.createElement('div');
    instructionRow.className = 'instruction-row d-flex mb-2';

    // Create floating form group
    const instructionWrapper = document.createElement('div');
    instructionWrapper.className = 'form-floating instruction-wrapper flex-grow-1';

    // Create textarea
    const instructionInput = document.createElement('textarea');
    instructionInput.name = `Instructions[${instructionCount - 1}].Step`;
    instructionInput.className = 'form-control instruction-input';
    instructionInput.style.height = '80px';
    instructionInput.placeholder = 'Step description';

    // Create label
    const stepLabel = document.createElement('label');
    stepLabel.textContent = `Step #${instructionCount}`;
    stepLabel.className = 'step-label';

    // Create remove button
    const removeBtn = document.createElement('button');
    removeBtn.type = 'button';
    removeBtn.className = 'btn btn-outline-danger btn-sm ms-2 remove-instruction-btn';

    const trashIcon = document.createElement('i');
    trashIcon.className = 'bi bi-trash';

    removeBtn.appendChild(trashIcon);

    // Add remove event listener
    removeBtn.addEventListener('click', function () {
        instructionRow.remove();
    });

    // Assemble all elements
    instructionWrapper.appendChild(instructionInput);
    instructionWrapper.appendChild(stepLabel);
    instructionRow.appendChild(instructionWrapper);
    instructionRow.appendChild(removeBtn);
    container.appendChild(instructionRow);
});


