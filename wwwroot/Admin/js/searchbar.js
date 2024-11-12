document.getElementById("searchInput").addEventListener("input", function () {
    const searchTerm = this.value.toLowerCase();

    // Filtra a tabela de usuários
    const tableRows = document.querySelectorAll("tbody tr");
    tableRows.forEach(row => {
        const rowText = row.innerText.toLowerCase();
        row.style.display = rowText.includes(searchTerm) ? "" : "none";
    });

    // Filtra a lista de posts
    const postCards = document.querySelectorAll(".card");
    postCards.forEach(card => {
        const cardText = card.innerText.toLowerCase();
        card.style.display = cardText.includes(searchTerm) ? "" : "none";
    });
});
