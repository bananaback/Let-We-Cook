$(document).ready(function () {
    $('.delete-recipe-btn').on('click', function () {
        let recipeId = $(this).data('id');
        console.log(recipeId);

        // Use SweetAlert2
        Swal.fire({
            title: 'Confirm Delete',
            text: `Are you sure you want to delete recipe?`,
            icon: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Yes, delete it!',
            cancelButtonText: 'No, cancel'
        }).then((result) => {
            if (result.isConfirmed) {
                // User confirmed - make an AJAX call to delete the recipe
                $.ajax({
                    url: `/api/recipes/${recipeId}`, // Adjust URL as needed
                    type: 'POST',
                    data: {}, // Empty data object since this is a POST request
                    success: function () {
                        Swal.fire('Deleted!', `Recipe has been deleted.`, 'success');
                        // Optionally remove the recipe from the DOM
                        $(`[data-id="${recipeId}"]`).closest('.overflow-hidden').remove();
                    },
                    error: function (xhr) {
                        const errorMessage = xhr.responseJSON?.message || 'An error occurred while deleting the recipe.';
                        Swal.fire('Error!', errorMessage, 'error');
                    }
                });
            } else if (result.isDismissed) {
                // User canceled
                console.log('Deletion canceled.');
            }
        });
    });
    $('.edit-recipe-btn').on('click', function () {
        let recipeId = $(this).data('id');
        window.location.href = `/Editor/Recipe/Edit/${recipeId}`;
    });
});
