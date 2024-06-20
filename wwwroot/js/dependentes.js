// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function addDependent() {
    // Create a new div for the new dependent inputs
    var div = document.createElement('div');
    div.className = 'form-group dependent';

    // Create the name input for the dependent
    var nameLabel = document.createElement('label');
    nameLabel.innerHTML = 'Nome do Dependente';
    var nameInput = document.createElement('input');
    nameInput.type = 'text';
    nameInput.name = 'dependenteNomes';
    nameInput.className = 'form-control';

    // Create the age input for the dependent
    var ageLabel = document.createElement('label');
    ageLabel.innerHTML = 'Idade do Dependente';
    var ageInput = document.createElement('input');
    ageInput.type = 'date';
    ageInput.name = 'dependenteIdades';
    ageInput.className = 'form-control';

    // Add the new inputs to the div
    div.appendChild(nameLabel);
    div.appendChild(nameInput);
    div.appendChild(ageLabel);
    div.appendChild(ageInput);

    // Add the new div to the dependents container
    document.getElementById('dependents-container').appendChild(div);
}

function validateDependentes() {
    var isValid = true;
    const today = new Date();
    const dates = document.querySelectorAll('[name$="dependenteIdades"]');
    for (let i = 0; i < dates.length; i++) {
        const dateInput = dates[i];
        const birthDate = new Date(dateInput.value);
        let age = today.getFullYear() - birthDate.getFullYear();
        const m = today.getMonth() - birthDate.getMonth();
        if (m < 0 || (m === 0 && today.getDate() < birthDate.getDate())) {
            age--;
        }
        if (isNaN(age) || age < 18) {
            alert('Todos os dependentes devem ter 18 anos ou mais.');
            isValid = false;
        }
    }
    if (isValid) {
        var campos = validateAllFields()
        if (campos == true) {
            return isValid
        }
        else {
            return campos
        }
    }
}
function OcultarBtn() {
    var noPeopleMessage = document.getElementById("no-people-message");
    if (noPeopleMessage) {
        var btns = document.getElementsByName("Consultar");
        for (var i = 0; i < btns.length; i++) {
            btns[i].style.display = "none";
        }
   }
}

function validateAllFields() {
    var isValid = true;
    var form = document.querySelector('form');

    form.querySelectorAll('input, select, textarea').forEach(function (input) {
        if (input.type !== 'hidden' && input.type !== 'button' && !input.value.trim()) {
            alert('Todos os campos devem ser preenchidos.');
            isValid = false;
            return false; // break out of the loop
        }
    });

    return isValid;
}