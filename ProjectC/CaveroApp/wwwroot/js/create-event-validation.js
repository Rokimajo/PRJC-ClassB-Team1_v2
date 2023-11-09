

function validateTitle() {
    const titleInput = document.getElementById('eventTitle');
    const titleError = document.getElementById('titleError');
        if (!titleInput.validity.valid) {
            if (titleInput.validity.valueMissing) {
                titleError.textContent = 'Titel is vereist.';
            } else if (titleInput.validity.patternMismatch) {
                titleError.textContent = 'Titel moet 1-20 karakters bevatten.';
            }

            titleError.style.display = 'inline'; // Show the error message
        } else {
            titleError.textContent = ''; // Clear the error message
            titleError.style.display = 'none'; // Hide the error message
        }
}

function validateDesc() {
    const descInput = document.getElementById('eventDescription');
    const descError = document.getElementById('descError');
    if (!descInput.validity.valid) {
        if (descInput.validity.valueMissing) {
            descError.textContent = 'Beschrijving is vereist.';
        } else if (descInput.validity.patternMismatch) {
            descError.textContent = 'Beschrijving moet 1-150 karakters bevatten.';
        }

        descError.style.display = 'inline'; // Show the error message
    } else {
        descError.textContent = ''; // Clear the error message
        descError.style.display = 'none'; // Hide the error message
    }
}

function validateLoc() {
    const locInput = document.getElementById('eventLocation');
    const locError = document.getElementById('locError');
    if (!locInput.validity.valid) {
        if (locInput.validity.valueMissing) {
            locError.textContent = 'Locatie is vereist.';
        } else if (locInput.validity.patternMismatch) {
            locError.textContent = 'Locatie moet 1-20 karakters bevatten.';
        }

        locError.style.display = 'inline'; // Show the error message
    } else {
        locError.textContent = ''; // Clear the error message
        locError.style.display = 'none'; // Hide the error message
    }
}

function buttonClicked() {
    console.log("Button Clicked.")
    validateTitle(); // Trigger validation when the button is clicked
    validateDesc();
    validateLoc();
}
