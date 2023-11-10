

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

function validateDesc(event) {
    const descInput = document.getElementById('eventDescription');
    const descError = document.getElementById('descError');
    // HTML <textarea> doesn't support patterns, so the length checking must be done manually.
    if (!descInput.validity.valid) {
        if (descInput.validity.valueMissing) {
            descError.textContent = 'Beschrijving is vereist.';
        } 
        descError.style.display = 'inline';
        if (event !== undefined)
            event.preventDefault();
    }
    else if (descInput.value.length > 250 || descInput.value.length < 1) {
        descError.textContent = 'Beschrijving moet 1-250 karakters bevatten.';
        descError.style.display = 'inline';
        if (event !== undefined)
            event.preventDefault();
    }
    else {
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

function validateDate(event) {
    const dateInput = document.getElementById('eventDate');
    const dateError = document.getElementById('dateError');
    
    if (dateInput.value === '') {
        dateError.textContent = 'Datum is vereist.';
        dateError.style.display = 'inline'; // Show the error message
        if (event !== undefined) 
            event.preventDefault();
    }
    else {
        dateError.textContent = ''; // Clear the error message
        dateError.style.display = 'none'; // Hide the error message
    }
}

function validateStartTime(event) {
    const startTimeInput = document.getElementById('eventStartTime');
    const startTimeError = document.getElementById('startTimeError');
    const endTimeInput = document.getElementById('eventEndTime');
    if (startTimeInput.value === '') {
        startTimeError.textContent = 'Starttijd is vereist.';
        startTimeError.style.display = 'inline'; // Show the error message
        if (event !== undefined)
            event.preventDefault();
    }
    else if (endTimeInput.value !== '')
    {
        if (startTimeInput.value > endTimeInput.value)
        {
            startTimeError.textContent = 'Starttijd kan niet later zijn dan de eindtijd.';
            startTimeError.style.display = 'inline'; // Show the error message
            if (event !== undefined)
                event.preventDefault();
        }
        else if (startTimeInput.value === endTimeInput.value)
        {
            startTimeError.textContent = 'Starttijd en eindtijd kunnen niet op dezelfde tijd vallen.';
            startTimeError.style.display = 'inline'; // Show the error message
            if (event !== undefined)
                event.preventDefault();
        }
        else {
            startTimeError.textContent = ''; // Clear the error message
            startTimeError.style.display = 'none'; // Hide the error message
        }
    }
    else {
        startTimeError.textContent = ''; // Clear the error message
        startTimeError.style.display = 'none'; // Hide the error message
    }
}

function validateEndTime(event) {
    const endTimeInput = document.getElementById('eventEndTime');
    const endTimeError = document.getElementById('endTimeError');
    const startTimeInput = document.getElementById('eventStartTime');
    
    if (endTimeInput.value === '') {
        endTimeError.textContent = 'Eindtijd is vereist.';
        endTimeError.style.display = 'inline'; // Show the error message
        if (event !== undefined)
            event.preventDefault();
    }
    else if (startTimeInput.value !== '')
    {
        if (endTimeInput.value < startTimeInput.value)
        {
            endTimeError.textContent = 'Eindtijd kan niet vroeger zijn dan de starttijd.';
            endTimeError.style.display = 'inline'; // Show the error message
            if (event !== undefined)
                event.preventDefault();
        }
        else if ( endTimeInput.value === startTimeInput.value)
        {
            endTimeError.textContent = 'Starttijd en eindtijd kunnen niet op dezelfde tijd vallen.';
            endTimeError.style.display = 'inline'; // Show the error message
            if (event !== undefined)
                event.preventDefault();
        }
        else {
            endTimeError.textContent = ''; // Clear the error message
            endTimeError.style.display = 'none'; // Hide the error message
        }
    }
    else {
        endTimeError.textContent = ''; // Clear the error message
        endTimeError.style.display = 'none'; // Hide the error message
    }
}

function buttonClicked(event) {
    validateTitle(); // Trigger validation when the button is clicked
    validateDesc(event);
    validateLoc();
    validateDate(event);
    validateStartTime(event);
    validateEndTime(event);
}