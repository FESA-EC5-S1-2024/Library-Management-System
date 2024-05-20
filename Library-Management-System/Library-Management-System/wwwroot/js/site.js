function deleteRecord(table, id) {
    swal({
        title: "Confirma a exclusão do registro?",
        text: "Se você excluir, não será possível recuperar este registro!",
        type: "warning",
        showCancelButton: true,
        confirmButtonClass: "btn-danger",
        confirmButtonText: "Sim, exclua!",
        cancelButtonText: "Cancelar",
        closeOnConfirm: false,
        closeOnCancel: false
    },
    function(isConfirm){
        if (isConfirm) {
            location.href = table + '/Delete?id=' + id;
        } else {
            swal("Cancelado", "Seu registro está seguro :)", "error");
        }
    });
}

function displayImage() {
    var oFReader = new FileReader();
    oFReader.readAsDataURL(document.getElementById("Image").files[0]);
    oFReader.onload = function (oFREvent) {
        document.getElementById("imgPreview").src = oFREvent.target.result;
    };
}