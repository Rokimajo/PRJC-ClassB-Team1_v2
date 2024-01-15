function validateTitle(isEdit) {
    const titleInput = document.getElementById('eventTitle' + isEdit);
    const titleError = document.getElementById('titleError' + isEdit);
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

function validateDesc(event, isEdit) {
    const descInput = document.getElementById('eventDescription' + isEdit);
    const descError = document.getElementById('descError' + isEdit);
    // HTML <textarea> doesn't support patterns, so the length checking must be done manually.
    if (!descInput.validity.valid) {
        if (descInput.validity.valueMissing) {
            descError.textContent = 'Beschrijving is vereist.';
        }
        descError.style.display = 'inline';
        if (event !== undefined)
            event.preventDefault();
    } else if (descInput.value.length > 250 || descInput.value.length < 1) {
        descError.textContent = 'Beschrijving moet 1-250 karakters bevatten.';
        descError.style.display = 'inline';
        if (event !== undefined)
            event.preventDefault();
    } else {
        descError.textContent = ''; // Clear the error message
        descError.style.display = 'none'; // Hide the error message
    }
}

function validateLoc(isEdit) {
    const locInput = document.getElementById('eventLocation' + isEdit);
    const locError = document.getElementById('locError' + isEdit);
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

function validateDate(event, isEdit) {
    const dateInput = document.getElementById('eventDate' + isEdit);
    const dateError = document.getElementById('dateError' + isEdit);

    if (dateInput.value === '') {
        dateError.textContent = 'Datum is vereist.';
        dateError.style.display = 'inline'; // Show the error message
        if (event !== undefined)
            event.preventDefault();
    } else {
        dateError.textContent = ''; // Clear the error message
        dateError.style.display = 'none'; // Hide the error message
    }
}

function validateStartTime(event, isEdit) {
    const startTimeInput = document.getElementById('eventStartTime' + isEdit);
    const startTimeError = document.getElementById('startTimeError' + isEdit);
    const endTimeInput = document.getElementById('eventEndTime' + isEdit);
    if (startTimeInput.value === '') {
        startTimeError.textContent = 'Starttijd is vereist.';
        startTimeError.style.display = 'inline'; // Show the error message
        if (event !== undefined)
            event.preventDefault();
    } else if (endTimeInput.value !== '') {
        if (startTimeInput.value > endTimeInput.value) {
            startTimeError.textContent = 'Starttijd kan niet later zijn dan de eindtijd.';
            startTimeError.style.display = 'inline'; // Show the error message
            if (event !== undefined)
                event.preventDefault();
        } else if (startTimeInput.value === endTimeInput.value) {
            startTimeError.textContent = 'Starttijd en eindtijd kunnen niet op dezelfde tijd vallen.';
            startTimeError.style.display = 'inline'; // Show the error message
            if (event !== undefined)
                event.preventDefault();
        } else {
            startTimeError.textContent = ''; // Clear the error message
            startTimeError.style.display = 'none'; // Hide the error message
        }
    } else {
        startTimeError.textContent = ''; // Clear the error message
        startTimeError.style.display = 'none'; // Hide the error message
    }
}

function validateEndTime(event, isEdit) {
    const endTimeInput = document.getElementById('eventEndTime' + isEdit);
    const endTimeError = document.getElementById('endTimeError' + isEdit);
    const startTimeInput = document.getElementById('eventStartTime' + isEdit);

    if (endTimeInput.value === '') {
        endTimeError.textContent = 'Eindtijd is vereist.';
        endTimeError.style.display = 'inline'; // Show the error message
        if (event !== undefined)
            event.preventDefault();
    } else if (startTimeInput.value !== '') {
        if (endTimeInput.value < startTimeInput.value) {
            endTimeError.textContent = 'Eindtijd kan niet vroeger zijn dan de starttijd.';
            endTimeError.style.display = 'inline'; // Show the error message
            if (event !== undefined)
                event.preventDefault();
        } else if (endTimeInput.value === startTimeInput.value) {
            endTimeError.textContent = 'Starttijd en eindtijd kunnen niet op dezelfde tijd vallen.';
            endTimeError.style.display = 'inline'; // Show the error message
            if (event !== undefined)
                event.preventDefault();
        } else {
            endTimeError.textContent = ''; // Clear the error message
            endTimeError.style.display = 'none'; // Hide the error message
        }
    } else {
        endTimeError.textContent = ''; // Clear the error message
        endTimeError.style.display = 'none'; // Hide the error message
    }
}

function buttonClicked(event, isEdit) {
    validateTitle(isEdit); // Trigger validation when the button is clicked
    validateDesc(event, isEdit);
    validateLoc(isEdit);
    validateDate(event, isEdit);
    validateStartTime(event, isEdit);
    validateEndTime(event, isEdit);
}