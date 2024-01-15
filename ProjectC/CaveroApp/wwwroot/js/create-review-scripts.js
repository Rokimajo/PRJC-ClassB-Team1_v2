var infoButtons = document.querySelectorAll(".button.info");

infoButtons.forEach(function (button) {
        button.addEventListener("click", function () {
                // Find the corresponding modal from the review card based on data attribute or class.
                var cardId = button.getAttribute("data-card-id");
                var modal = document.querySelector(".modal-card-" + cardId);
                var span = document.getElementsByClassName("close-" + cardId)[0];
                // When the user clicks on <span> (x), close the modal
                span.onclick = function () {
                    modal.style.display = "none";
                }

                // When the user clicks anywhere outside of the modal, close it
                window.onclick = function (event) {
                    if (event.target == modal) {
                        modal.style.display = "none";
                    }
                }

                // Open the modal.
                modal.style.display = "flex";
            }
        );
    }
);

var deleteButton = document.querySelectorAll(".button.delete");

deleteButton.forEach(function (button) {
        button.addEventListener("click", function () {
                var cardId = button.getAttribute("data-card-id");
                var modal = document.querySelector(".modal-card-delete-" + cardId);
                var span = document.querySelector(".close-delete-" + cardId);

                // When the user clicks on <span> (x), close the modal
                span.onclick = function () {
                    modal.style.display = "none";
                }

                // When the user clicks anywhere outside of the modal, close it
                window.onclick = function (event) {
                    if (event.target == modal) {
                        modal.style.display = "none";
                    }
                }

                // Open the modal.
                modal.style.display = "flex";
            }
        );
    }
);

