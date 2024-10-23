document.addEventListener('DOMContentLoaded', () => {
    const icon = document.querySelector('.material-icons-sharp.color');
    const colors = ['#1B9C85', '#F7D060', '#FF0060']; // Três cores desejadas
    let currentIndex = 0;

    // Carregar a cor salva do localStorage
    const savedColor = localStorage.getItem('colorSuccess');
    if (savedColor) {
        document.documentElement.style.setProperty('--color-success', savedColor);
        currentIndex = colors.indexOf(savedColor);
        if (currentIndex === -1) currentIndex = 0; // Se a cor salva não estiver na lista, começa do início
    }

    // Definir evento de clique para alternar a cor da variável CSS
    icon.addEventListener('click', () => {
        // Incrementar o índice da cor
        currentIndex = (currentIndex + 1) % colors.length;

        // Obter a nova cor
        const newColor = colors[currentIndex];

        // Aplicar a nova cor à variável CSS no :root
        document.documentElement.style.setProperty('--color-success', newColor);

        // Salvar a nova cor no localStorage
        localStorage.setItem('colorSuccess', newColor);
    });
});
