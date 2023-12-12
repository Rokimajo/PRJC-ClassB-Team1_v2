function validateCreateReviewForm() {
    let rating = document.getElementById("rating").value;
    let review = document.getElementById("review").value;

    if (rating == "" || review == "") {
        alert("Please fill in all fields");
        return false;
    }
    else if (rating < 1 || rating > 5) {
        alert("Rating must be between 1 and 5");
        return false;
    }
    else if (review.length < 10 && review.length > 500) {
        alert("Review must be at least 10 characters long and max 500 characters long");
        return false;
    }
    else {
        return true;
    }
}
