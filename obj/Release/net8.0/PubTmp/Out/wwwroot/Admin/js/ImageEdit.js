document.getElementById('picture').addEventListener('change', function (event) {
    var reader = new FileReader();
    reader.onload = function () {
        var output = document.getElementById('thumbnailPreview');
        output.src = reader.result;
    };
    reader.readAsDataURL(event.target.files[0]);
});