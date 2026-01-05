$(document).ready(function () {
  loadDataTable();
});

var dataTable;

function loadDataTable() {
  dataTable = $("#tblData").DataTable({
    ajax: {
      url: "/admin/products/getall",
      type: "GET",
      datatype: "json",
    },
    pageLength: 10,
    lengthChange: false,
    responsive: true,
    ordering: true,
    language: {
      emptyTable: "No products found",
    },
    columns: [
      { data: "title", width: "20%" },
      { data: "author", width: "15%" },
      { data: "isbn", width: "15%" },
      {
        data: "price",
        className: "text-end fw-semibold",
        render: (data) => `₹${data}`,
        width: "10%",
      },
      {
        data: "category.name",
        className: "text-center",
        width: "20%",
      },
      {
        data: "id",
        className: "text-center",
        render: function (data) {
          return `
                        <div class="d-flex justify-content-center gap-2">
                            <a href="/Admin/Products/Upsert?id=${data}"
                               class="btn btn-sm btn-soft-primary"
                               title="Edit">
                                <i class="bi bi-pencil-square"></i>
                            </a>

                            <a onclick="deleteProduct('/Admin/Products/DeleteAPI?id=${data}')"
                               class="btn btn-sm btn-soft-danger"
                               title="Delete">
                                <i class="bi bi-trash"></i>
                            </a>
                        </div>
                    `;
        },
        width: "10%",
      },
    ],
  });
}

function deleteProduct(url) {
  Swal.fire({
    title: "Are you sure?",
    text: "You won't be able to revert this!",
    icon: "warning",
    showCancelButton: true,
    confirmButtonColor: "#3085d6",
    cancelButtonColor: "#d33",
    confirmButtonText: "Yes, delete it!",
  }).then((result) => {
    if (result.isConfirmed) {
      $.ajax({
        type: "DELETE",
        url: url,
        success: function (data) {
          console.log("Delete response:", data);
          if (data.success) {
            toastr.success(data.message);
            setTimeout(function () {
              dataTable.ajax.reload(function () {
                console.log("DataTable reloaded successfully");
              });
            }, 500);
          } else {
            toastr.error(data.message);
          }
        },
        error: function (xhr, status, error) {
          console.error("Delete error:", xhr.responseText);
          toastr.error("Error deleting product");
        }
      });
    }
  });
}

