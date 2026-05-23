$(document).ready(function () {
  loadDataTable();
});

var dataTable;

function loadDataTable() {
  dataTable = $("#tblData").DataTable({
    ajax: {
      url: "/admin/order/getall",
      type: "GET",
      datatype: "json",
    },

    pageLength: 10,
    lengthChange: false,
    responsive: true,
    ordering: true,

    language: {
      emptyTable: "No orders found",
    },

    columns: [
      {
        data: "id",
        width: "8%",
      },

      {
        data: "name",
        width: "18%",
      },

      {
        data: "applicationUser.email",
        width: "22%",
      },

      {
        data: "phoneNumber",
        width: "15%",
      },

      {
        data: "orderStatus",
        className: "text-center",
        render: function (data) {
          if (data === "Approved") {
            return `
              <span class="badge bg-success">
                ${data}
              </span>
            `;
          }

          if (data === "Pending") {
            return `
              <span class="badge bg-warning text-dark">
                ${data}
              </span>
            `;
          }

          if (data === "Processing") {
            return `
              <span class="badge bg-info text-dark">
                ${data}
              </span>
            `;
          }

          if (data === "Shipped") {
            return `
              <span class="badge bg-primary">
                ${data}
              </span>
            `;
          }

          if (data === "Cancelled") {
            return `
              <span class="badge bg-danger">
                ${data}
              </span>
            `;
          }

          if (data === "Refunded") {
            return `
              <span class="badge bg-secondary">
                ${data}
              </span>
            `;
          }

          return `
            <span class="badge bg-dark">
              ${data}
            </span>
          `;
        },
        width: "15%",
      },

      {
        data: "orderTotal",
        className: "text-end fw-semibold",
        render: function (data) {
          return `$${parseFloat(data).toFixed(2)}`;
        },
        width: "12%",
      },

      {
        data: "id",
        className: "text-center",
        render: function (data) {
          return `
            <div class="d-flex justify-content-center gap-2">

              <a href="/Admin/Order/Details?id=${data}"
                 class="btn btn-sm btn-soft-primary"
                 title="View Details">

                <i class="bi bi-eye"></i>

              </a>

            </div>
          `;
        },
        width: "10%",
      },
    ],
  });
}
