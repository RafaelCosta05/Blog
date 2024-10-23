function updateClock() {
    const now = new Date();

    // Obter o nome do dia da semana e o nome do mês
    const dayName = now.toLocaleString('pt-BR', { weekday: 'long' });
    const day = now.getDate();
    const monthName = now.toLocaleString('pt-BR', { month: 'long' });
    const year = now.getFullYear();

    // Obter a hora, minuto e segundo
    const hours = now.getHours().toString().padStart(2, '0');
    const minutes = now.getMinutes().toString().padStart(2, '0');
    const seconds = now.getSeconds().toString().padStart(2, '0');

    // Formatar a data por extenso
    const formattedTime = `${dayName}, ${day} de ${monthName}, ${hours}:${minutes}:${seconds}`;

    document.getElementById('clock').textContent = formattedTime;
}

setInterval(updateClock, 1000);
updateClock();