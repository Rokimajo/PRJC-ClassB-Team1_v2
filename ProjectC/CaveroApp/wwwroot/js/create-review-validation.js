function validateCreateReviewForm() {
    var rating = document.getElementById("rating").value;
    var feedback = document.getElementById("feedback").value;
    var error = document.getElementById("error");

    if (rating == "" || feedback == "") {
        error.innerHTML = "Please fill in all fields";
        return false;
    } else if (rating < 1 || rating > 5) {
        error.innerHTML = "Rating must be between 1 and 5";
        return false;
    } else if (feedback.length > 500) {
        error.innerHTML = "Feedback must be less than 500 characters";
        return false;
    } else {
        error.innerHTML = "";
        return true;
    }

}