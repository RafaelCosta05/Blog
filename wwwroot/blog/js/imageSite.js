document.addEventListener("DOMContentLoaded", function () {
    const homeSection = document.getElementById("home");
    const backgroundUrl = homeSection.getAttribute("data-background-url");

    if (backgroundUrl) {
        const img = new Image();
        img.src = backgroundUrl;

        img.onload = function () {
            homeSection.style.background = `linear-gradient(rgba(0, 0, 0, 0.5), rgba(0, 0, 0, 0.5)), url('${backgroundUrl}')`;
            homeSection.style.backgroundSize = "cover";
            homeSection.style.backgroundPosition = "center";
        };

        img.onerror = function () {
            console.warn("Não foi possível carregar a imagem de fundo:", backgroundUrl);
        };
    }
});
