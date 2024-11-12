$(document).ready(function () {
    // Show dropdown on hover over the profile image or dropdown itself
    $('#profile-dropdown-trigger, #profile-dropdown-menu').hover(
        function () {
            $('#profile-dropdown-menu').removeClass('hidden');
        },
        function () {
            $('#profile-dropdown-menu').addClass('hidden');
        }
    );
});