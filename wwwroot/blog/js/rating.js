function setRating(value) {
    // Define o valor do rating no campo oculto
    document.getElementById('RatingValue').value = value;

    // Remove a classe "filled" de todas as estrelas
    var stars = document.querySelectorAll('.star');
    stars.forEach(star => {
        star.classList.remove('filled');
    });

    // Adiciona a classe "filled" às estrelas até o valor clicado
    for (let i = 1; i <= value; i++) {
        stars[i - 1].classList.add('filled');
    }
}