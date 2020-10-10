tinymce.init({
    selector: "textarea",
    plugins: "image code paste",
    paste_data_images: true,
    toolbar: "undo redo | image code paste",

    images_upload_handler: function (blobInfo, success, failure) {
        var xhr = new XMLHttpRequest();
        xhr.withCredentials = false;
        xhr.open("POST", '@Url.Action("UploadImage", "Recipe")');

        xhr.onload = function () {
            if (xhr.status !== 200) {
                failure("HTTP Error: " + xhr.status);
                return;
            }

            var json = xhr.responseText;

            success(json);

        };

        var formData = new FormData();
        formData.append("file", blobInfo.blob(), blobInfo.filename());

        xhr.send(formData);
    }
});