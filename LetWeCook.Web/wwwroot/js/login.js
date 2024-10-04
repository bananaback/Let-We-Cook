$(document).ready(function () {
    // Call the function on document ready
    handleSlideNavBtnClick();
});

// Function to handle slide-nav-btn click event
function handleSlideNavBtnClick() {
    $('.slide-nav-btn').on('click', function () {
        // When a button is clicked, change its background color
        $(this).siblings().removeClass('bg-gray-600').addClass('bg-slate-50'); // Remove active state from siblings

        $(this).removeClass('bg-slate-50').addClass('bg-gray-600');
    });
}
