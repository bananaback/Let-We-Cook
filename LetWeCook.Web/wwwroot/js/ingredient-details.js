$(document).ready(function () {
    // Handle delete button click
    $('.delete-btn').on('click', function () {
        var id = $(this).data('id');
        Swal.fire({
            title: 'Delete Ingredient',
            text: 'Are you sure you want to delete this ingredient?',
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'Cancel',
        }).then((result) => {
            if (result.isConfirmed) {
                // Make an AJAX call to the delete endpoint
                $.ajax({
                    url: `/api/ingredients/${id}`, // Ensure this matches your endpoint URL
                    type: 'DELETE',
                    success: function () {
                        Swal.fire({
                            title: 'Deleted!',
                            text: 'The ingredient has been successfully deleted.',
                            icon: 'success',
                            confirmButtonText: 'OK',
                        }).then(() => {
                            // Optionally, refresh the page or redirect
                            window.location.href = "/Cooking/Ingredient";
                        });
                    },
                    error: function (xhr) {
                        Swal.fire({
                            title: 'Error!',
                            text: xhr.responseJSON?.message || 'An error occurred while deleting the ingredient.',
                            icon: 'error',
                            confirmButtonText: 'OK',
                        });
                    },
                });
            } else {
                console.log('Delete cancelled.');
            }
        });
    });

    // Handle edit button click
    $('.edit-btn').on('click', function () {
        var id = $(this).data('id');
        Swal.fire({
            title: 'Edit Ingredient',
            text: 'Do you want to edit this ingredient?',
            icon: 'question',
            showCancelButton: true,
            confirmButtonText: 'Yes, edit it!',
            cancelButtonText: 'Cancel',
        }).then((result) => {
            if (result.isConfirmed) {
                // Redirect to the edit page
                window.location.href = `/Editor/Ingredient/Edit/${id}`;
            } else {
                console.log('Edit cancelled.');
            }
        });
    });
});
