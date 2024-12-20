function fetchIngredientDetails(ingredientId) {
    const apiUrl = `/api/ingredients/${ingredientId}`;

    return $.ajax({
        url: apiUrl,
        type: 'GET',
        dataType: 'json', // Expected response data format
        success: function (ingredientDetails) {
            console.log('Ingredient Details:', ingredientDetails);
        },
        error: function (xhr, status, error) {
            console.error(`Failed to fetch ingredient details: ${status} - ${error}`);
        }
    });
}

function populateInputElements(ingredient) {
    $('#ingredient-name').val(ingredient.ingredientName);
    $('#ingredient-description').val(ingredient.ingredientDescription);

    $('.cover-image-preview.preview-area').html(
        `<div class="preview-image-container w-full h-60 rounded-lg border-2 border-gray-500 bg-white overflow-hidden">
                                <img src="${ingredient.coverImageUrl}" alt="Uploaded Image" class="w-full h-full object-cover" />
                            </div>
                            <button type="button" class="px-2 py remove-image-btn text-red-500 hover:text-red-700">Remove Image</button>`
    ).siblings(".open-cloudinary-btn").hide();

    function populateFrames(frames) {
        // Clear existing sub-description items if needed
        const container = $('#sub-desc-container'); // Adjust the container selector as needed
        container.empty();

        frames.forEach(frame => {
            const mode = frame.mediaUrl ? "image" : "text"; // Determine mode based on mediaUrl and textContent
            const subDescriptionItem = createSubDescriptionItem(mode);

            // Populate data
            if (mode === "text") {
                subDescriptionItem.find('textarea').val(frame.textContent);
            } else {
                subDescriptionItem.find('.preview-area').html(
                    `<div class="preview-image-container w-full h-32 rounded-lg border-2 border-gray-500 bg-gray-800 overflow-hidden">
                                <img src="${frame.mediaUrl}" alt="Uploaded Image" class="w-full h-full object-contain" />
                            </div>
                            <button type="button" class="px-2 py remove-image-btn text-red-500 hover:text-red-700">Remove Image</button>`
                ).siblings(".open-cloudinary-btn").hide();;
            }

            // Add the item to the container
            container.append(subDescriptionItem);
        });
    }

    populateFrames(ingredient.frames);

}

// Function to create a sub-description item
const createSubDescriptionItem = (mode = "text") => {
    const isTextMode = mode === "text";

    return $(`
            <div class="sub-description-item rounded-lg shadow bg-white overflow-hidden w-full" w-full data-mode="${mode}">
                <!-- Header -->
                <div class="sub-description-header flex items-center justify-between bg-gray-100 px-4 py-2 cursor-grab">
                    <span class="text-gray-700 font-semibold">Sub Description</span>
                    <button type="button" class="remove-item-btn text-red-500 hover:text-red-700">
                        <svg xmlns="http://www.w3.org/2000/svg" class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M6 18L18 6M6 6l12 12" />
                        </svg>
                    </button>
                </div>

                <!-- Content Area -->
                <div class="content-area h-full flex items-start justify-between p-4 gap-4">
                    <!-- Text Mode -->
                    <div class="w-full h-36 text-mode ${isTextMode ? '' : 'hidden'}">
                        <textarea class="w-full h-36 flex-1 rounded-md border-gray-300 bg-gray-50 shadow-sm focus:ring-emerald-500 focus:border-emerald-500 sm:text-lg px-4 py-3" placeholder="Enter sub-description text"></textarea>
                    </div>
                    <!-- Image Mode -->
                    <div class="image-mode h-36 ${isTextMode ? 'hidden' : ''}">
                        <button type="button" class="open-cloudinary-btn text-sm font-semibold text-emerald-600 bg-emerald-50 px-4 py-2 rounded hover:bg-emerald-100 focus:ring-2 focus:ring-emerald-500">
                            Choose File
                        </button>
                        <div class="preview-area flex-1 text-sm text-gray-500"></div>
                    </div>
                    <button type="button" class="toggle-mode-btn text-sm font-semibold text-emerald-600 bg-emerald-50 px-4 py-2 rounded hover:bg-emerald-100 focus:ring-2 focus:ring-emerald-500">
                        ${isTextMode ? "Switch to Image" : "Switch to Text"}
                    </button>
                </div>
            </div>
        `);
};

$(document).ready(function () {
    const ingredientId = $('#ingredient-id').text();

    // Fetch the ingredient details and populate the input elements when the data is ready
    fetchIngredientDetails(ingredientId)
        .done(function (data) {
            populateInputElements(data);
        })
        .fail(function (xhr, status, error) {
            console.error(`Error fetching ingredient details: ${status} - ${error}`);
        });
    // Cloudinary Upload Widget
    const mediaWidget = cloudinary.createUploadWidget(
        {
            cloudName: "dxclyqubm", // Replace with your Cloudinary cloud name
            uploadPreset: "letwecook_preset", // Replace with your upload preset
            sources: ["local", "url", "camera"],
            multiple: false,
            cropping: true,
            maxFileSize: 2000000, // 2MB limit
            clientAllowedFormats: ["jpg", "jpeg", "png", "mp4", "mov", "avi"],
            resourceType: "auto", // Automatically detect resource type (image or video)
        },
        (error, result) => {
            if (!error && result.event === "success") {
                const targetPreviewArea = mediaWidget.targetPreviewArea;
                const resourceType = result.info.resource_type;
                const publicUrl = result.info.secure_url;

                // Set the preview content based on file type
                if (targetPreviewArea) {
                    if (resourceType === "image") {
                        if (targetPreviewArea.hasClass('cover-image-preview')) {
                            targetPreviewArea.html(
                                `<div class="preview-image-container w-full h-60 rounded-lg border-2 border-gray-500 bg-white overflow-hidden">
                                <img src="${publicUrl}" alt="Uploaded Image" class="w-full h-full object-cover" />
                            </div>
                            <button type="button" class="px-2 py remove-image-btn text-red-500 hover:text-red-700">Remove Image</button>`
                            );
                        } else {
                            targetPreviewArea.html(
                                `<div class="preview-image-container w-full h-32 rounded-lg border-2 border-gray-500 bg-gray-800 overflow-hidden">
                                <img src="${publicUrl}" alt="Uploaded Image" class="w-full h-full object-contain" />
                            </div>
                            <button type="button" class="px-2 py remove-image-btn text-red-500 hover:text-red-700">Remove Image</button>`
                            );
                        }

                    } else if (resourceType === "video") {
                        targetPreviewArea.html(
                            `<video controls class="w-full rounded-lg shadow-sm">
                                <source src="${publicUrl}" type="video/${result.info.format}" />
                                Your browser does not support this video.
                            </video>`
                        );
                    }
                    // Hide the "Choose File" button after upload
                    mediaWidget.targetPreviewArea.siblings(".open-cloudinary-btn").hide();
                }
            }
        }
    );

    

    // Add Sub Description Item
    $("#add-sub-desc-btn").on("click", function () {
        const newSubDescription = createSubDescriptionItem();
        newSubDescription
            .css({ opacity: 0, transform: "translateY(-10px)" }) // Initial state for animation
            .appendTo("#sub-desc-container")
            .animate({ opacity: 1, transform: "translateY(0)" }, 300); // Smooth animation
    });

    // Open Cloudinary Widget and assign target preview area
    $(document).on("click", ".open-cloudinary-btn", function () {
        mediaWidget.targetPreviewArea = $(this).siblings(".preview-area"); // Set the target preview area
        mediaWidget.open(); // Open the widget
    });

    // Toggle between Text and Image mode
    $(document).on("click", ".toggle-mode-btn", function () {
        const subDescriptionItem = $(this).closest(".sub-description-item");
        const currentMode = subDescriptionItem.data("mode");
        const newMode = currentMode === "text" ? "image" : "text";

        console.log('newMode: ' + newMode);

        // Update the mode in the DOM
        subDescriptionItem.data("mode", newMode);
        console.log(subDescriptionItem.data('mode'));

        // Toggle visibility based on mode
        subDescriptionItem.find(".text-mode").toggleClass("hidden", newMode !== "text");
        subDescriptionItem.find(".image-mode").toggleClass("hidden", newMode !== "image");

        // Update the button text
        $(this).text(newMode === "text" ? "Switch to Image" : "Switch to Text");

        console.log(subDescriptionItem.data('mode'));

    });

    // Remove Sub Description Item
    $(document).on("click", ".remove-item-btn", function () {
        const subDescriptionItem = $(this).closest(".sub-description-item");
        const previewArea = subDescriptionItem.find(".preview-area");

        // Restore the "Choose File" button before removal if the preview exists
        if (previewArea.find("img").length > 0 || previewArea.find("video").length > 0) {
            subDescriptionItem.find(".open-cloudinary-btn").show();
        }

        // Remove the item with animation
        subDescriptionItem.animate(
            { opacity: 0, height: 0, marginBottom: 0 },
            300,
            function () {
                $(this).remove();
            }
        );
    });

    // Remove image
    $(document).on("click", ".remove-image-btn", function () {
        const previewArea = $(this).closest(".preview-area");


        previewArea.siblings(".open-cloudinary-btn").show();
        previewArea.empty();

    });


    // Make Sub Descriptions sortable
    $("#sub-desc-container").sortable({
        revert: true,  // Enable revert effect
        scroll: false,  // Enable default scrolling behavior
    });

    // Function to validate the input fields (Name, Description, Cover Image URL)
    function validateInput(ingredientName, ingredientDescription, coverImageUrl) {
        // Check if ingredient name, description, and cover image URL are valid
        if (!ingredientName || ingredientName.trim() === "") {
            Toastify({
                text: "Ingredient name is required.",
                duration: 3000,
                backgroundColor: "red",
                close: true
            }).showToast();
            return false;
        }
        if (!ingredientDescription || ingredientDescription.trim() === "") {
            Toastify({
                text: "Ingredient description is required.",
                duration: 3000,
                backgroundColor: "red",
                close: true
            }).showToast();
            return false;
        }
        if (!coverImageUrl) {
            Toastify({
                text: "Cover image is required.",
                duration: 3000,
                backgroundColor: "red",
                close: true
            }).showToast();
            return false;
        }
        return true;
    }

    $('#save-ingredient-btn').on('click', function () {
        var ingredientName = $('#ingredient-name').val();
        var ingredientDescription = $('#ingredient-description').val();

        // Get the URL of the cover image
        var coverImageUrl = $('.cover-image-preview').find('.preview-image-container').find('img').attr('src');

        // Validate the inputs
        if (!validateInput(ingredientName, ingredientDescription, coverImageUrl)) {
            return; // Stop the process if validation fails
        }

        // Initialize an empty array to store the sub-description objects
        var subDescriptionData = [];

        // Loop through each sub-description item
        $("#sub-desc-container .sub-description-item").each(function (index) {
            var mode = $(this).data("mode"); // Get the mode (text or image)
            var contentType = mode === "text" ? "text" : "image"; // Determine content type
            var subDescriptionObj = {
                contentType: contentType,
                imageUrl: "", // Default empty string for image content
                textContent: "",  // Default empty string for text content
                order: index + 1 // Set order based on index (1-based)
            };

            // Capture content based on the mode
            if (contentType === "image") {
                var imageUrl = $(this).find(".preview-area img").attr('src') || "";
                console.log('url ' + imageUrl);
                subDescriptionObj.imageUrl = imageUrl; // Store the image URL
            } else {
                var textContent = $(this).find(".text-mode textarea").val() || "";
                subDescriptionObj.textContent = textContent; // Store the text content
            }

            // Push the object to the array
            subDescriptionData.push(subDescriptionObj);
        });

        // Log the ingredient data and the list of sub-description objects
        console.log("Ingredient Name:", ingredientName);
        console.log("Ingredient Description:", ingredientDescription);
        console.log("Cover Image URL:", coverImageUrl);
        console.log("Sub Description Data:", subDescriptionData);

        // Create the request object matching CreateIngredientRequest
        var requestData = {
            id: ingredientId,
            ingredientName: ingredientName,
            ingredientDescription: ingredientDescription,
            coverImageUrl: coverImageUrl || "",
            rawFrameDTOs: subDescriptionData
        };

        // Sending the Ajax POST request
        $.ajax({
            url: '/api/ingredients',  // The endpoint URL
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(requestData),
            success: function (response, status, xhr) {
                if (xhr.status === 200) {
                    Swal.fire({
                        title: 'Updated Successfully!',
                        text: 'The ingredient has been updated successfully.',
                        icon: 'success',
                        confirmButtonText: 'OK',
                        customClass: {
                            confirmButton: 'bg-emerald-500 hover:bg-emerald-600 text-white font-semibold px-4 py-2 rounded'
                        }
                    }).then(() => {
                        // Refresh the page
                        //window.location.reload();
                    });
                } else {
                    // Show error notification with Toastify
                    Toastify({
                        text: "Failed to save ingredient: " + response.errorMessage + ". Please try again.",
                        duration: 3000,
                        backgroundColor: "red", // Error color
                        close: true
                    }).showToast();
                }
            },
            error: function (xhr, status, error) {
                var errorMessage = xhr.responseJSON && xhr.responseJSON.message
                    ? xhr.responseJSON.message
                    : "An error occurred while saving the ingredient. Please try again.";

                // Show error notification with Toastify
                Toastify({
                    text: errorMessage,
                    duration: 3000,
                    backgroundColor: "red", // Error color
                    close: true
                }).showToast();

                console.error('Error saving ingredient:', error);
            }
        });

    });



});
