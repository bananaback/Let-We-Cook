$(document).ready(function () {
    // Elements and initial values
    const $searchForm = $("#searchForm");
    const $itemsPerPageSelector = $("#itemsPerPage");
    const $goToPageInput = $("#goToPage");
    const $goButton = $("#goButton");

    // Extract initial data from data attributes
    const baseUrl = "/Cooking/Ingredient"; // Set the base URL for your link
    const initialSearchTerm = $searchForm.data("searchTerm");
    const initialItemsPerPage = $itemsPerPageSelector.data("items-per-page"); // Use data-items-per-page
    const currentPage = $("[data-current-page]").data("current-page"); // Use data-current-page
    const totalPages = $("[data-total-pages]").data("total-pages"); // Use data-total-pages

    // Function to build URLs dynamically
    function buildUrl(page, itemsPerPage, searchTerm) {
        return `${baseUrl}?page=${page}&itemsPerPage=${itemsPerPage}&search=${encodeURIComponent(searchTerm || '')}`;
    }

    // Search Form Submission
    $searchForm.on("submit", function (event) {
        event.preventDefault();
        const searchQuery = $("input[name='search']").val();
        window.location.href = buildUrl(1, initialItemsPerPage, searchQuery); // Go to first page with new search
    });

    // Items Per Page Change
    $itemsPerPageSelector.on("change", function () {
        const itemsPerPage = $(this).val();
        window.location.href = buildUrl(1, itemsPerPage, initialSearchTerm); // Reset to page 1
    });

    // Go to Specific Page
    $goButton.on("click", function () {
        const goToPage = parseInt($goToPageInput.val(), 10); // Parse input value as an integer
        if (isNaN(goToPage) || goToPage < 1 || goToPage > totalPages) {
            alert(`Please enter a valid page number between 1 and ${totalPages}`);
        } else {
            window.location.href = buildUrl(goToPage, initialItemsPerPage, initialSearchTerm);
        }
    });

    $("[data-id]").on('click', function () {
        // Get the data-id attribute of the clicked card
        const ingredientId = $(this).data("id");
        // Redirect to the ingredient details page
        window.location.href = `/Cooking/Ingredient/Details/${ingredientId}`;
    });
});
