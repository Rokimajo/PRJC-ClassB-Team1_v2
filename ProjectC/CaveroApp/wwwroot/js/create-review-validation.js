function validateStars(isEdit) {
    var stars = document.getElementById('reviewStars').value;
    if (stars < 1 || stars > 5) {
        document.getElementById('starsError').innerHTML = 'Kies een aantal sterren tussen 1 en 5.';
    } else {
        document.getElementById('starsError').innerHTML = '';
    }
}
function validateFeedback() {
    var review = document.getElementById('reviewText').value;
    if (review.length < 1 || review.length > 500) {
        document.getElementById('reviewError').innerHTML = 'Review moet 1-500 karakters bevatten.';
    } else {
        document.getElementById('reviewError').innerHTML = '';
    }
}
