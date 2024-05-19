function deleteRecord(table, id) {
    if (confirm('Confirma a exclusão do registro?'))
        location.href = table + '/Delete?id=' + id;
}

function displayImage() {
    var oFReader = new FileReader();
    oFReader.readAsDataURL(document.getElementById("Image").files[0]);
    oFReader.onload = function (oFREvent) {
        document.getElementById("imgPreview").src = oFREvent.target.result;
    };
}