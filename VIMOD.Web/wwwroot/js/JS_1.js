let datatable;
$(function () {
    loadDataTable();
});

function loadDataTable() {
    datatable = $('#table_rooms').DataTable({
        language: {
            lengthMenu: "Mostrar _MENU_ registros por página",
            zeroRecords: "No hay registros disponibles.",
            info: "Pág. _PAGE_ de _PAGES_ - Mostrando del _START_ al _END_ de _TOTAL_ registros",
            infoEmpty: "No hay registros disponibles.",
            infoFiltered: "(filtrado de un total _MAX_ registros)",
            loadingRecords: "Cargando en curso...",
            emptyTable: "No hay registros disponibles.",
            search: "Buscar",
            paginate: {
                first: "Primero",
                last: "Último",
                "next": "Siguiente",
                "previous": "Anterior"
            }
        },
        ajax: {
            url: "/Rooms/GetAll"
        },
        columns: [
            {
                data: "roomId",
                render: function (data) {
                    return `
                        <a href="/Rooms/Edit/${data}" class="btn btn-sm btn-success text-white" style="cursor:pointer;">
                            <i class="bi bi-pencil-square"></i>
                        </a>
                        <a href="/Rooms/Details/${data}" class="btn btn-sm btn-info text-white" style="cursor:pointer;">
                            <i class="bi bi-list"></i>
                        </a>
                        <a onclick=Delete("/Rooms/DeleteRoom/${data}") class="btn btn-sm btn-danger text-white" style="cursor:pointer;">
                            <i class="bi bi-trash3-fill"></i>
                        </a>
                    `;
                }, width: "15%", orderable: false, searchable: false, className: "text-center"
            },
            { data: "roomId", width: "10%", className: "text-center" },
            { data: "name" },
            { data: "category.name" },
            { data: "unitPrice" },
            {
                data: "status",
                render: function (data) {
                    if (data == true) {
                        return `<span class="badge rounded-pill bg-info">ACTIVO</span>`;
                    } else {
                        return `<span class="badge rounded-pill bg-warning">INACTIVO</span>`;
                    }
                }, width: "10%", orderable: false, searchable: false, className: "text-center"
            }
        ]
    });
}
function Delete(url) {
    swal({
        title: "¿Estás seguro de eliminar la habitación?",
        text: "Este registro no se podrá recuperar.",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    })
        .then((borrar) => {
            if (borrar) {
                $.ajax({
                    type: "POST",
                    url: url, // Rooms/DeleteRoom/3
                    success: function (data) {
                        if (data.completado) {
                            toastr.success(data.mensaje); // Notificación satisfactoria
                            datatable.ajax.reload(); // Recargar el datatable
                        }
                        else {
                            toastr.error(data.mensaje); // Notificación error
                        }
                    }
                });
            }
        });
}