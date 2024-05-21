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

function aplicaFiltroConsultaAvancada() {
    var vDescricao = document.getElementById('title').value;
    var vAutor = document.getElementById('authorName').value;
    var vCategoria = document.getElementById('categoryDescription').value;
    var vDataInicial = document.getElementById('dataInicial').value;
    var vDataFinal = document.getElementById('dataFinal').value;
    $.ajax({
        url: "/book/ObtemDadosConsultaAvancada",
        data: { descricao: vDescricao, autor: vAutor, categoria: vCategoria, dataInicial: vDataInicial, dataFinal: vDataFinal },
        success: function (dados) {
            if (dados.erro != undefined) {
                alert(dados.msg);
            }
            else {
                document.getElementById('resultadoConsulta').innerHTML = dados;
            }
        },
    });

}