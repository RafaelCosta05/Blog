const sideBar = document.querySelector('.sidebar');
const minIcon = document.querySelector('.min-sidebar-icon');
const mainContent = document.querySelector('.main-content');

function minSidebar() {
    sideBar.classList.toggle('min-sidebar');

    if (sideBar.classList.contains('min-sidebar')) {
        minIcon.innerHTML = `<span class="material-icons-sharp" onclick="minSidebar()">chevron_right</span>`;
        mainContent.classList.add('min-content');
    } else {
        minIcon.innerHTML = `<span class="material-icons-sharp" onclick="minSidebar()">chevron_left</span>`;
        mainContent.classList.remove('min-content');
    }
}

const closeBtn = document.getElementById('close');

function sidebar() {
    sideBar.classList.toggle('active');

}