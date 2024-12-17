function getUserBio() {
    const userId = $('#author-id').text().trim();
    console.log(userId);

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

$(document).ready(function () {
    getUserBio();
});