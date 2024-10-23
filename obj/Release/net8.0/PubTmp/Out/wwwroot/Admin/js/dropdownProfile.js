//document.addEventListener('DOMContentLoaded', function () {
//    var profilePhoto = document.querySelector('.profile-photo');
//    var dropdownMenu = document.querySelector('.dropdown-menu');

//    profilePhoto.addEventListener('click', function (event) {
//        event.stopPropagation();
//        console.log('Profile photo clicked');
//        dropdownMenu.style.display = dropdownMenu.style.display === 'block' ? 'none' : 'block';
//        console.log('Dropdown menu display:', dropdownMenu.style.display);
//    });

//    document.addEventListener('click', function () {
//        if (dropdownMenu.style.display === 'block') {
//            dropdownMenu.style.display = 'none';
//            console.log('Dropdown menu closed');
//        }
//    });

//    dropdownMenu.addEventListener('click', function (event) {
//        event.stopPropagation();
//    });
//});
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
