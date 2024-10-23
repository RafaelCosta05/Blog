const dropdowns = document.querySelectorAll('.dropdown');
dropdowns.forEach(dropdown => {
    const selectProfile = dropdown.querySelector('.select-profile');
    const caret = dropdown.querySelector('.caret');
    const menu = dropdown.querySelector('.menu');
    const buttons = dropdown.querySelectorAll('.dropdown-item');

    selectProfile.addEventListener('click', () => {
        selectProfile.classList.toggle('select-clicked');
        caret.classList.toggle('caret-rotate');
        menu.classList.toggle('menu-open');
    });

    buttons.forEach(button => {
        button.addEventListener('click', () => {
            selectProfile.classList.remove('select-clicked');
            caret.classList.remove('caret-rotate');
            menu.classList.remove('menu-open');
        });
    });
});
