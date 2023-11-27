var infoButtons = document.querySelectorAll(".button.info");

infoButtons.forEach(function (button) {
    button.addEventListener("click", function () {
        // Find the corresponding modal based on data attribute or class.
        var cardId = button.getAttribute("data-card-id");
        var modal = document.querySelector(".modal-card-" + cardId);
        var span = document.getElementsByClassName("close-" + cardId)[0];
        // When the user clicks on <span> (x), close the modal
        span.onclick = function() {
            modal.style.display = "none";
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function(event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }

        // Open the modal.
        modal.style.display = "flex";
    });
});

var deleteButton = document.querySelectorAll(".button.delete");

deleteButton.forEach(function (button) {
    button.addEventListener("click", function () {
        var cardId = button.getAttribute("data-card-id");
        var modal = document.querySelector(".modal-card-delete-" + cardId);
        var span = document.querySelector(".close-delete-" + cardId);

        // When the user clicks on <span> (x), close the modal
        span.onclick = function() {
            modal.style.display = "none";
        }

        // When the user clicks anywhere outside of the modal, close it
        window.onclick = function(event) {
            if (event.target == modal) {
                modal.style.display = "none";
            }
        }

        // Open the modal.
        modal.style.display = "flex";
    });
});

var createButton = document.querySelector("#createEventButton");
createButton.addEventListener("click", function () {
    var modal = document.querySelector("#createEventModal");
    var span = document.querySelector("#closeEvent");

    span.onclick = function() {
        modal.style.display = "none";
    }

    // When the user clicks anywhere outside of the modal, close it
    // window.onclick = function(event) {
    //   if (event.target == modal) {
    //     modal.style.display = "none";
    //   }
    // }

    // Open the modal.
    modal.style.display = "flex";
})

var editButton = document.querySelectorAll(".button.edit");
editButton.forEach(function (button) {
    button.addEventListener("click", function() {
        var cardId = button.getAttribute("data-card-id");
        var modal = document.querySelector(".modal-card-edit-" + cardId);
        var span = document.querySelector(".close-edit-" + cardId);
        
        span.onclick = function() {
            modal.style.display = "none";
        }

        // When the user clicks anywhere outside of the modal, close it
        // window.onclick = function(event) {
        //   if (event.target == modal) {
        //     modal.style.display = "none";
        //   }
        // }

        // Open the modal.
        modal.style.display = "flex";
    })
})

const editEventButton = document.querySelectorAll('.button.edit-event');

editEventButton.forEach(function(button) {
    button.addEventListener('click', function (event) {
        const Id = button.getAttribute("event-id-data");
        const editForm = document.getElementById('editEventForm-' + Id);
        buttonClicked(event, 'Edit-' + Id);
        if (!editForm.checkValidity()) {
            // Prevent form submission if there are validation errors
            event.preventDefault();
        }
    });
});

document.addEventListener('DOMContentLoaded', function () {
    const form = document.getElementById('createEventForm');
    const createEventButton = document.getElementById('createEventButtonModal');

    createEventButton.addEventListener('click', function (event) {
        buttonClicked(event, '');
        if (!form.checkValidity()) {
            // Prevent form submission if there are validation errors
            event.preventDefault();
        }
    });
    
    // FLATPICKR INITIALIZATIONS 
    
    flatpickr("#eventDate", {
        enableTime: false,
        dateFormat: "d-m-Y",
        minDate: "today",
        disable: [
            function (date) {
                // Disable weekends (Saturday and Sunday)
                return (date.getDay() === 0 || date.getDay() === 6);
            }
        ],
    });

    flatpickr("#eventStartTime", {
        enableTime: true,
        noCalendar: true,
        dateFormat: "H:i",
        time_24hr: true
    });

    flatpickr("#eventEndTime", {
        enableTime: true,
        noCalendar: true,
        dateFormat: "H:i",
        time_24hr: true
    });

    // Admin edit needs a flatpickr for each unique event. This loops over all edit event modals currently on screen
    // And gives them each their unique date pickers based on the event ID.
    const editEventButton = document.querySelectorAll('.button.edit-event');
    editEventButton.forEach(function(button) {
        const Id = button.getAttribute("event-id-data");
        // date pickers for the admin edit modal
        flatpickr("#eventDateEdit-" + Id, {
            enableTime: false,
            dateFormat: "d-m-Y",
            minDate: "today",
            disable: [
                function (date) {
                    // Disable weekends (Saturday and Sunday)
                    return (date.getDay() === 0 || date.getDay() === 6);
                }
            ],
        });

        flatpickr("#eventStartTimeEdit-" + Id, {
            enableTime: true,
            noCalendar: true,
            dateFormat: "H:i",
            time_24hr: true
        });

        flatpickr("#eventEndTimeEdit-" + Id, {
            enableTime: true,
            noCalendar: true,
            dateFormat: "H:i",
            time_24hr: true
        });
    });
});