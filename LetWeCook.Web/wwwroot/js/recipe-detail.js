$(document).ready(function () {
    getUserCollections();
    getUserBio();

    // Delegate click event to dynamically added collection items
    $('#collectionList').on('click', '.collection-item', function () {
        // Remove 'selected' class from all items
        $('#collectionList .collection-item').removeClass('bg-blue-100').addClass('bg-gray-100');

        // Add 'selected' class to the clicked item
        $(this).addClass('bg-blue-100').removeClass('bg-gray-100');
    });

    // Show the collection selector
    $("#add-to-collection-btn").on("click", function () {
        $("#collectionSelector").removeClass("hidden").addClass('flex');
    });

    // Handle confirm button click
    $("#confirmSelection").on("click", function () {
        // Get the recipe ID from the data attribute
        const recipeId = $("#collectionSelector").data("id");

        // Get the selected collection's ID
        const selectedCollection = $("#collectionList .collection-item.bg-blue-100");
        const collectionId = selectedCollection.data("id");

        if (!collectionId) {
            alert("Please select a collection first.");
            return;
        }

        // Output the IDs (or send them to your server)
        console.log("Recipe ID:", recipeId);
        console.log("Selected Collection ID:", collectionId);

        // Make an AJAX POST request to the API
        $.ajax({
            url: `/api/collections/${collectionId}/add-recipe/${recipeId}`, // API URL
            type: "POST", // HTTP method
            success: function (response) {
                // Handle successful response
                alert(response.message || "Recipe added to collection successfully.");
                // Hide the collection selector
                $("#collectionSelector").addClass("hidden").removeClass("flex");
            },
            error: function (xhr) {
                // Handle errors
                if (xhr.status === 401) {
                    alert("You are not authorized. Please log in.");
                } else if (xhr.status === 404) {
                    alert(xhr.responseJSON || "Collection or recipe not found.");
                } else {
                    //alert("An error occurred: " + (xhr.responseText || "Unknown error"));
                    alert("This recipe is already added to the collection.");
                }
                $("#collectionSelector").addClass("hidden").removeClass("flex");

            }
        });
    });


    let selectedRating = 0;

    // Highlight stars on hover and reset on mouse leave
    $('#star-rating .star').on('mouseover', function () {
        const rating = $(this).data('rating');
        $('#star-rating .star').each(function () {
            $(this).toggleClass('text-yellow-500', $(this).data('rating') <= rating);
            $(this).toggleClass('text-gray-400', $(this).data('rating') > rating);
        });
    });

    $('#star-rating .star').on('mouseleave', function () {
        $('#star-rating .star').each(function () {
            $(this).toggleClass('text-yellow-500', $(this).data('rating') <= selectedRating);
            $(this).toggleClass('text-gray-400', $(this).data('rating') > selectedRating);
        });
    });

    // Set selected rating on click
    $('#star-rating .star').on('click', function () {
        selectedRating = $(this).data('rating');
        $('#star-rating .star').each(function () {
            $(this).toggleClass('text-yellow-500', $(this).data('rating') <= selectedRating);
            $(this).toggleClass('text-gray-400', $(this).data('rating') > selectedRating);
        });
        console.log(`Selected Rating: ${selectedRating}`);
    });

    // Submit Review Button
    $('#submit-review').on('click', function () {
        const reviewText = $('#review-text').val();
        if (!selectedRating || !reviewText.trim()) {
            alert('Please provide a rating and a comment.');
            return;
        }

        const recipeIdInDOM = $("#collectionSelector").data("id");

        // Review data
        const reviewData = {
            review: reviewText.trim(),
            rating: selectedRating,
            recipeId: recipeIdInDOM // Replace with actual recipe ID
        };

        // Construct the query string
        const queryString = new URLSearchParams(reviewData).toString();

        // API URL
        const apiUrl = `/api/reviews?${queryString}`;

        // Make the API call
        fetch(apiUrl, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            }
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(err => { throw new Error(err); });
                }
                return response.json();
            })
            .then(data => {
                console.log('Review submitted:', data);
                alert('Review submitted successfully!');
                // Optionally, refresh the reviews section here
                location.reload();

            })
            .catch(error => {
                console.error('Error submitting review:', error);
                alert(`Please login to review recipe.`);
            });
    });

    // Delete Review Button
    $('.delete-review-btn').on('click', function () {
        const reviewId = $(this).data('review-id');

        // Confirm deletion
        if (!confirm('Are you sure you want to delete this review?')) {
            return;
        }

        // API URL for delete
        const apiUrl = `/api/reviews/${reviewId}`;

        // Make the DELETE API call
        fetch(apiUrl, {
            method: 'DELETE'
        })
            .then(response => {
                if (!response.ok) {
                    return response.text().then(err => { throw new Error(err); });
                }
                return response.text();
            })
            .then(data => {
                console.log('Review deleted:', data);
                alert('Review deleted successfully!');
                // Remove the review from the DOM
                location.reload();
            })
            .catch(error => {
                console.error('Error deleting review:', error);
                alert(`Failed to delete review: ${error.message}`);
            });
    });


});

function getUserBio() {
    const userId = $('#author-id').text().trim();

    // Array of chef quotes
    const chefQuotes = [
        "Cooking is an art, and every dish tells a story.",
        "A recipe has no soul. You, as the chef, must bring soul to the recipe.",
        "Good food is the foundation of genuine happiness.",
        "The secret ingredient is always love.",
        "Cooking is like painting or writing a song—each recipe is your canvas.",
        "To eat is a necessity, but to cook is an art.",
        "Cooking is about creating something delicious for someone else.",
        "A great meal is not about the food, it's about the memories.",
        "Happiness is homemade.",
        "Cooking is love made visible.",
        "The best dishes are the ones we make for loved ones.",
        "A kitchen is a place of family and comfort.",
        "Every chef has a story, and every dish is a chapter.",
        "Cooking is an adventure; don't be afraid to try new things.",
        "Good food, good mood.",
        "Life is uncertain, but dessert is a sure thing.",
        "A good cook is like a sorcerer who dispenses happiness.",
        "In cooking, as in life, balance is everything.",
        "There’s nothing better than a homemade meal.",
        "Cooking is the ultimate act of love."
    ];

    $.ajax({
        url: `/api/profile/${userId}`, // Endpoint URL with user ID
        type: 'GET', // HTTP method
        contentType: 'application/json', // Content type
        success: function (response) {
            console.log('User Bio:', response);

            // Update the avatar
            const avatarUrl = response.avatarUrl || "https://via.placeholder.com/80";
            $('#author-avatar').attr('src', avatarUrl);

            // Construct name
            const firstName = response.firstName || "";
            const lastName = response.lastName || "";
            const userName = response.userName ? `(${response.userName})` : "";
            const fullName = `${firstName} ${lastName}`.trim() || "Unknown User";
            $('#author-name').text(`${fullName} ${userName}`);

            // Gender-based salutation
            const genderPrefix = response.gender === "MALE" ? "Mr." : response.gender === "FEMALE" ? "Ms." : "";

            // Details: Gender, Age, Joined Date
            const age = response.age ? `Age: ${response.age}` : "";
            const dateJoined = new Date(response.dateJoined).toLocaleDateString();
            $('#author-details').text(`${genderPrefix} ${fullName}, ${age} | Joined: ${dateJoined}`);

            // Contact info
            const phone = response.phoneNumber || "N/A";
            $('#author-contact').html(`Contact: <span class="font-medium text-gray-700">${phone}</span>`);

            // Randomly select a quote
            const randomQuote = chefQuotes[Math.floor(Math.random() * chefQuotes.length)];
            $('#chef-quote p').text(`"${randomQuote}"`);
        },
        error: function (xhr, status, error) {
            console.error('Error fetching user bio:', xhr.responseText || error);
            alert('Failed to fetch user bio. Please try again later.');
        }
    });
}
function getUserCollections() {
    $.ajax({
        url: '/api/collections', // API endpoint
        method: 'GET', // HTTP method
        success: function (response) {
            console.log(response);

            // Clear the existing list
            $('#collectionList').empty();

            if (response.length === 0) {
                // If no collections, show a message and link
                $('#collectionList').append(
                    `<li class="text-gray-500">
                        You don't have any collections yet. 
                        <a href="/Account/Profile/RecipeCollections" class="text-blue-500 underline hover:text-blue-700">Create a collection</a>.
                    </li>`
                );
            } else {
                // Populate the list with fetched collections
                response.forEach(function (collection) {
                    $('#collectionList').append(
                        `<li class="collection-item cursor-pointer rounded bg-gray-100 px-4 py-2 hover:bg-gray-200" data-id="${collection.id}">
                            ${collection.name}
                        </li>`
                    );
                });
            }
        },
        error: function (xhr, status, error) {
            // Handle errors
            console.error('Error fetching collections:', xhr.responseText);
            alert('Failed to fetch collections. Please try again.');
        }
    });
}
