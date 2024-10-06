$(document).ready(function () {
    // Attach the click event handler on document ready
    attachSlideNavButtonClickHandler();

    //
    attachRegisterButtonClickHandler();

    // Start automatic slide transition after a specific duration
    startAutoSlideChange(5000); // Change slides every 5 seconds
});

function attachRegisterButtonClickHandler() {
    $('.register-btn button').on('click', function (event) {
        // Prevent the default form submission
        event.preventDefault();

        // Serialize form data
        var formData = $('form').serialize();

        // Make the AJAX POST request
       
    });
}

// Function to attach the click event handler to the slide-nav buttons
function attachSlideNavButtonClickHandler() {
    $('.slide-nav-btn').on('click', function () {
        toggleActiveButtonStyle($(this));
        changeImageBasedOnIndex($(this).data('index')); // Call image change function on click
    });
}

// Function to toggle the active button's style
function toggleActiveButtonStyle($button) {
    $button.siblings().removeClass('bg-gray-600 w-8').addClass('bg-slate-50 w-4'); // Reset styles for siblings
    $button.removeClass('bg-slate-50 w-4').addClass('bg-gray-600 w-8'); // Set active style for clicked button
}

// Extracted function to change the image based on the index
function changeImageBasedOnIndex(index) {
    $('.image-holder img').each(function () {
        if ($(this).data('imageindex') === index) {
            $(this).removeClass('opacity-0').addClass('opacity-100');
        } else {
            $(this).removeClass('opacity-100').addClass('opacity-0');
        }
    });
}

// Function to automatically change slides after a specific duration
function startAutoSlideChange(interval) {
    let currentIndex = 0;
    const totalButtons = $('.slide-nav-btn').length;

    setInterval(function () {
        currentIndex = (currentIndex + 1) % totalButtons; // Loop through buttons
        const $nextButton = $('.slide-nav-btn').eq(currentIndex); // Get the next button

        toggleActiveButtonStyle($nextButton); // Toggle active button style
        changeImageBasedOnIndex($nextButton.data('index')); // Change image based on button's index
    }, interval);
}
