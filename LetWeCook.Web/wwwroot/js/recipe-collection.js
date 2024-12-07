

$(document).ready(function () {
    
    const $collectionsContainer = $("#recipe-collections");
    const $widget = $("#manage-collection-widget");
    const $input = $("#collection-name-input");

    const renderRecipes = (recipes) => {
        if (!recipes || recipes.length === 0) {
            return `
            <div class="w-full text-center col-span-4 text-gray-500 italic">
                No recipes available in this collection.
            </div>`;
        }
        return recipes
            .map(
                (recipe) => `
                <a href="/Cooking/Recipe/Details/${recipe.id}" class="relative aspect-square overflow-hidden rounded-lg bg-gray-100 group hover:shadow-lg transition-all duration-300">
                    <img class="absolute left-0 top-0 h-full w-full object-cover group-hover:scale-110 transition-all duration-300" src="${recipe.recipeCoverImage.url}" alt="${recipe.title}">
                    <h3 class="absolute bottom-0 left-0 w-full truncate bg-black bg-opacity-50 p-2 text-center font-medium text-white">${recipe.title}</h3>
                </a>`
            )
            .join("");

    };

    const renderCollections = async () => {
        // Show a loading screen if necessary
        $collectionsContainer.empty(); // Clear the container

        // Call the real API endpoint
        $.ajax({
            url: '/api/collections', // Replace with the actual endpoint
            method: 'GET', // Use the appropriate HTTP method
            success: function (response) {
                console.log(response);
                // Assuming the API response structure matches your mocked data
                if (!response || response.length === 0) {
                    $collectionsContainer.append(`
                    <div class="text-center text-gray-500 italic">
                        No collections found.
                    </div>
                `);
                    hideLoadingScreen(); // Hide the loading screen
                    return;
                }

                response.forEach((collection) => {
                    const collectionHTML = `
                <div class="rounded-lg bg-white p-4 shadow">
                    <div class="flex items-center justify-between mb-4">
                    <input type="hidden" id="collectionId" name="collectionId" value="${collection.id}">
                        <h2 class="text-xl font-semibold text-gray-700">${collection.name}</h2>
                        <div class="flex flex-row items-center justify-between gap-x-4">
                            <button class="manage-collection-btn px-4 py-2 rounded bg-blue-500 text-white font-medium hover:bg-blue-600 transition-all duration-300">
                                Manage Collection
                            </button>
                            <button class="delete-collection-btn px-4 py-2 rounded bg-red-500 text-white font-medium hover:bg-red-600 transition-all duration-300">
                                Delete Collection
                            </button>
                        </div>
                    </div>
                    <div class="grid grid-cols-2 gap-4 md:grid-cols-3 lg:grid-cols-4">
                        ${renderRecipes(collection.recipes)}
                    </div>
                </div>`;
                    $collectionsContainer.append(collectionHTML);
                });
                hideLoadingScreen(); // Hide the loading screen
            },
            error: function (xhr, status, error) {
                // Handle errors
                console.error('Error fetching collections:', xhr.responseText);
                alert('Failed to fetch collections. Please try again.');
            }
        });
    };



    // Show/Hide Create Collection Widget
    $("#create-collection-btn").on("click", () => showCreateCollectionWidget());
    $("#cancel-btn").on("click", () => hideCreateCollectionWidget());

    // Replace the mock API save function with your real API call
    async function saveCollectionToAPI(data) {
        try {
            const response = await fetch('/api/collections', {  // Replace with your actual endpoint
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(data),
            });
            if (!response.ok) {
                throw new Error('Failed to save the collection.');
            }
            const result = await response.json();
            return result;
        } catch (error) {
            throw error; // Handle error
        }
    }

    // Save New Collection
    $("#save-btn").on("click", async () => {
        showLoadingScreen();
        const name = $("#collection-name-input").val().trim();
        const description = $("#collection-description-input").val().trim();  // Get description value
        if (!name) {
            hideLoadingScreen();
            return alert("Collection name cannot be empty.");
        } 

        const newCollection = { name, description };

        try {
            const result = await saveCollectionToAPI(newCollection); // Use the real API call

            const newCollectionHTML = `
            <div class="rounded-lg bg-white p-4 shadow">
                <div class="flex items-center justify-between mb-4">
                    <h2 class="text-xl font-semibold text-gray-700">${result.name}</h2>
                    <button class="px-4 py-2 rounded bg-blue-500 text-white font-medium hover:bg-blue-600 transition-all duration-300">
                        Manage Collection
                    </button>
                </div>
                <div class="grid grid-cols-2 gap-4 md:grid-cols-3 lg:grid-cols-4">
                    <!-- Empty Recipes -->
                    <div class="col-span-full flex flex-col items-center justify-center text-gray-500">
                        <svg class="w-16 h-16 mb-2" fill="none" stroke="currentColor" stroke-width="2" viewBox="0 0 24 24" xmlns="http://www.w3.org/2000/svg">
                            <path stroke-linecap="round" stroke-linejoin="round" d="M3 10h18M3 14h18M4 6h16m-7 8h7"></path>
                        </svg>
                        <p class="text-sm">This collection is currently empty. Start adding recipes now!</p>
                    </div>
                </div>
            </div>`;
            $("#recipe-collections").append(newCollectionHTML);


            // Reset the input fields and hide the widget
            $("#collection-name-input").val("");
            $("#collection-description-input").val(""); // Reset description input
            hideCreateCollectionWidget();

            // Scroll to the bottom of the page
            $("html, body").animate({ scrollTop: $(document).height() }, "slow");

        } catch (error) {
            // Show error message if the API call fails
            alert(error);
        }
        hideLoadingScreen();
    });

    renderCollections();

    // Handle Delete Collection Button Click
    $(document).on('click', '.delete-collection-btn', function () {
        const collectionId = $(this).closest('.rounded-lg').find('input#collectionId').val();
        const confirmed = confirm('Are you sure you want to delete this collection?');
        if (confirmed) {
            showLoadingScreen();
            // Call API to delete the collection
            $.ajax({
                url: `/api/collections/${collectionId}`, // Replace with actual DELETE API endpoint
                method: 'DELETE',
                success: function (response) {
                    // Remove the collection from the DOM if delete is successful
                    $(`#collection-${collectionId}`).remove();
                    hideLoadingScreen();
                    alert('Collection deleted successfully!');
                    renderCollections();
                },
                error: function (xhr, status, error) {
                    hideLoadingScreen();
                    console.error('Error deleting collection:', xhr.responseText);
                    alert('Failed to delete collection. Please try again.');
                }
            });
        }
    });

    // Handle Delete Collection Button Click
    $(document).on('click', '.manage-collection-btn', function () {
        const collectionId = $(this).closest('.rounded-lg').find('input#collectionId').val();
        window.location.href = "/Account/Profile/CollectionDetails?id=" + collectionId;
    });

});

function showCreateCollectionWidget() {
    $('#manage-collection-widget').addClass('flex').removeClass('hidden');
    $('#manage-collection-widget').removeClass('opacity-0').addClass('opacity-100');
}

function hideCreateCollectionWidget() {
    $('#manage-collection-widget')
        .removeClass('opacity-100')
        .addClass('opacity-0'); // Fade-out effect (1 second)

    setTimeout(() => {
        $('#manage-collection-widget').addClass('hidden').removeClass('flex'); // Add 'hidden' after 1 second
    }, 1000); // Match the duration of the transition
}

function hideLoadingScreen() {
    $('#loading-overlay').addClass('hidden').removeClass('flex');
}

function showLoadingScreen() {
    $('#loading-overlay').removeClass('hidden').addClass('flex');

}