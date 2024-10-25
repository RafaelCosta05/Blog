const menuIcon = document.querySelector('.menu-icon');
const subMenu = document.querySelector('.sub-menu');
const sideBar = document.querySelector('.sidebar')


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


function filterPosts() {
    const searchInput = document.getElementById('searchInput');
    const filter = searchInput.value.toLowerCase();
    const postBoxes = document.querySelectorAll('.post-box'); // Seleciona todos os posts

    postBoxes.forEach(post => {
        const title = post.querySelector('.post-title').textContent.toLowerCase();

        // Verifica se o título contém o termo de busca
        if (title.includes(filter)) {
            post.style.display = ''; // Mostra o post
        } else {
            post.style.display = 'none';
        }
    });
}