const themeToggler = document.querySelector('.theme-toggler');

// Function to set theme based on localStorage value
function setTheme(theme) {
    if (theme === 'dark') {
        document.body.classList.add('dark-mode-variables');
        themeToggler.innerHTML = `<span class="material-icons-sharp dark-mode">dark_mode</span> 
                                   <h3>Tema</h3>`;
    } else {
        document.body.classList.remove('dark-mode-variables');
        themeToggler.innerHTML = `<span class="material-icons-sharp light-mode">light_mode</span> 
                                   <h3>Tema</h3>`;
    }
}


// TOGGLE THEME
if (themeToggler) {
    themeToggler.addEventListener('click', () => {
        const isDarkMode = document.body.classList.toggle('dark-mode-variables');
        const newTheme = isDarkMode ? 'dark' : 'light';
        localStorage.setItem('theme', newTheme);
        setTheme(newTheme);
    });
}

// SAVE THEME WHEN CHANGE PAGE
const savedTheme = localStorage.getItem('theme');
if (savedTheme) {
    setTheme(savedTheme);
}


const sideBar = document.querySelector('.sidebar');
const closeBtn = document.getElementById('close');


function sidebar() {
    sideBar.classList.toggle('active');
    
}
