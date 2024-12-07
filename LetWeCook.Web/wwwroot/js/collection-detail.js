$(document).on('click', '#remove-recipe-btn', function () {
    // Extract the recipe ID and collection ID from the DOM
    const recipeId = $(this).data('recipe-id');
    const collectionId = $(this).data('collection-id');

    if (!recipeId || !collectionId) {
        console.error('Missing recipe ID or collection ID.');
        return;
    }

    // Confirm the deletion (optional)
    if (!confirm('Are you sure you want to remove this recipe from the collection?')) {
        return;
    }

    // Make the DELETE API call
    $.ajax({
        url: `/api/collections/${collectionId}/remove-recipe/${recipeId}`,
        type: 'DELETE',
        success: function (response) {
            alert(response.message || 'Recipe removed successfully.');

            // Remove the recipe card from the DOM
            $(`#remove-recipe-btn[data-recipe-id="${recipeId}"]`).closest('.recipe-card').remove();
        },
        error: function (xhr) {
            const errorMessage = xhr.responseJSON?.message || 'Failed to remove the recipe. Please try again.';
            alert(errorMessage);
            console.error('Error:', xhr);
        }
    });
});
