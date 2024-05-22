function deleteRecord(table, id) {
    Swal.fire({
        title: 'Confirma a exclusão do registro?',
        text: "Se você excluir, não será possível recuperar este registro!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Sim, exclua!',
        cancelButtonText: 'Cancelar'
    }).then((result) => {
        if (result.isConfirmed) {
            location.href = table + '/Delete?id=' + id;
        } else {
            Swal.fire(
                'Cancelado',
                'Seu registro está seguro :)',
                'info'
            );
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

function aplicaFiltroConsultaAvancadaLivro() {
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

function aplicaFiltroConsultaAvancadaEmprestimo() {
    var vUsuario = document.getElementById('userName').value;
    var vDescricao = document.getElementById('title').value;
    var vDataInicial = document.getElementById('dataInicial').value;
    var vDataFinal = document.getElementById('dataFinal').value;
    $.ajax({
        url: "/loan/ObtemDadosConsultaAvancada",
        data: { usuario: vUsuario, descricao: vDescricao, dataInicial: vDataInicial, dataFinal: vDataFinal },
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

$(document).ready(function () {
    $('.open-modal-btn').on('click', function () {
        var bookTitle = $(this).closest('tr').find('.book-title').text().trim();
        $('#modalS .modal-title').text("Sinopse - " + bookTitle);
        var isbn = $(this).closest('tr').find('.isbn').text().trim();
        var linkAPI = 'https://www.googleapis.com/books/v1/volumes?q=isbn:' + isbn;
        $.ajax({
            url: linkAPI,
            success: function (data) {
                var description;
                if (data.error != undefined || data == null || data.totalItems == 0 || data.items[0]?.volumeInfo?.description == null) {
                    description = "Sinopse do livro não encontrada.";
                } else {
                    description = data.items[0].volumeInfo.description;
                }

                var maxLength = 2000;
                if (description.length > maxLength) {
                    description = description.substring(0, maxLength) + '...';
                }

                $('#modalS .modal-body').text(description);
            },
            error: function (xhr, status, error) {
                $('#modalS .modal-body').text("Erro ao carregar a sinopse.");
            }
        });
    });
});