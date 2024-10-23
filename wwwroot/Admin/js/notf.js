//document.addEventListener('DOMContentLoaded', (event) => {
//    const container = document.createElement('div');
//    container.className = 'notyf-container';
//    document.body.appendChild(container);

//    window.showNotyf = function (type, message) {
//        const notyf = document.createElement('div');
//        notyf.className = `notyf ${type}`;
//        notyf.textContent = message;

//        container.appendChild(notyf);

//        setTimeout(() => {
//            notyf.classList.add('show');
//        }, 10);

//        setTimeout(() => {
//            notyf.classList.remove('show');
//            setTimeout(() => {
//                container.removeChild(notyf);
//            }, 300);
//        }, 3000);
//    };
//});
