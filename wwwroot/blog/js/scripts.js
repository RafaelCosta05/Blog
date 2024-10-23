let header = document.querySelector('header')

window.addEventListener('scroll', () => {
    header.classList.toggle("shadow", window.scrollY > 0)
});

const menuIcon = document.querySelector('.menu-icon');
const subMenu = document.querySelector('.sub-menu');

function openSidebar() {
    sideBar.classList.toggle('active');
}

function openSubMenu(element, iconSpan) {
    // Busca o container do submenu dentro da div pai do elemento clicado (h3)
    const subMenuContainer = element.parentElement.querySelector('.sub-menu-container');

    if (subMenuContainer) {
        subMenuContainer.classList.toggle('active');
    }

    if (iconSpan) {
        if (iconSpan.textContent === 'chevron_right') {
            iconSpan.textContent = 'keyboard_arrow_down';
        } else {
            iconSpan.textContent = 'chevron_right';
        }
    }
}
