function getRecipeDetails() {
    let recipeId = $('#recipe-id').text();
    $.ajax({
        url: `/api/recipes/${recipeId}`, 
        type: 'GET',
        data: {}, // Pass any additional data if required
        success: function (response) {
            console.log(response);
            Swal.fire('Fetched recipe', JSON.stringify(response), 'success');
            populateRecipeDetails(response);
            populateIngredients(response);
        },
        error: function (xhr) {
            const errorMessage = xhr.responseJSON?.message || 'An error occurred while deleting the recipe.';
            Swal.fire('Error!', errorMessage, 'error');
        }
    });
}

function populateIngredients(recipeData) {
    const ingredientsSection = $('#ingredientsSection'); // Container for the ingredients

    recipeData.recipeIngredientDTOs.forEach(ingredientDTO => {
        // Create a new ingredient container
        const ingredientDiv = $('<div>', { class: 'mt-2 ingredient-dropdown flex items-center space-x-4' });

        // Create close button
        const closeButton = $('<button>', { class: 'ml-2 text-red-500 hover:text-red-700', text: 'X' });
        closeButton.click(function () {
            ingredientDiv.remove(); // Remove the ingredient dropdown when the close button is clicked
        });

        // Create ingredient dropdown
        const ingredientSelect = $('<select>', { class: 'w-full rounded border border-gray-300 px-3 py-2 text-gray-800 shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-400' });
        ingredientSelect.append($('<option>', { value: "", text: "Select Ingredient", disabled: true }));

        // Populate dropdown with fetched ingredients
        fetchIngredients(); // Ensure ingredients are fetched
        setTimeout(() => {
            ingredients.forEach(ingredient => {
                const option = $('<option>', {
                    value: ingredient.id,
                    text: ingredient.ingredientName,
                    selected: ingredient.id === ingredientDTO.ingredientId // Auto-select if matching
                });
                ingredientSelect.append(option);
            });
        }, 500); // Timeout to ensure fetch completion (adjust as necessary)

        // Create hidden input for ingredient ID
        const hiddenIdInput = $('<input>', {
            type: 'hidden',
            class: 'ingredient-id',
            name: 'ingredientId[]', // Array for multiple ingredients
            value: ingredientDTO.ingredientId // Set to the ingredient ID from the DTO
        });

        // Create quantity input
        const quantityInput = $('<input>', {
            type: 'number',
            class: 'w-20 rounded border border-gray-300 px-3 py-2 text-gray-800 shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-400',
            placeholder: 'Quantity',
            min: '0.01',
            step: '0.01',
            value: ingredientDTO.quantity // Set to the quantity from the DTO
        });

        // Create measure unit dropdown
        const unitSelect = $('<select>', { class: 'w-20 rounded border border-gray-300 px-3 py-2 text-gray-800 shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-400' });
        unitSelect.append($('<option>', { value: "", text: "Unit", disabled: true }));

        // Populate measure unit options
        const units = ['kg', 'g', 'oz', 'lb', 'cup', 'tbsp', 'tsp'];
        units.forEach(unit => {
            const option = $('<option>', {
                value: unit,
                text: unit,
                selected: unit === ingredientDTO.unit // Auto-select if matching
            });
            unitSelect.append(option);
        });

        // Append the select dropdowns, input, and hidden ID to the ingredient container
        ingredientDiv.append(ingredientSelect);
        ingredientDiv.append(quantityInput);
        ingredientDiv.append(unitSelect);
        ingredientDiv.append(hiddenIdInput);
        ingredientDiv.append(closeButton);

        // Append the ingredient container to the ingredients section
        ingredientsSection.append(ingredientDiv);

        // Handle ingredient selection change
        ingredientSelect.change(function () {
            // Update the hidden input value when an ingredient is selected
            const selectedIngredientId = $(this).val();
            hiddenIdInput.val(selectedIngredientId);
        });
    });
}

function populateRecipeDetails(recipe) {
    $("#title").val(recipe.title);
    $("#description").val(recipe.description);
    $("#coverImageInfo").text(recipe.recipeCoverImage.url);
    $("#coverImagePreview").attr('src', recipe.recipeCoverImage.url).removeClass("hidden");
    $("#cuisine").val(recipe.cuisine);
    $("#difficulty").val(recipe.difficulty);
    $("#cookTime").val(recipe.cookTimeInMinutes);
    $("#serving").val(recipe.serving);
    $("#coverMediaId").val(recipe.recipeCoverImage.id);


    // Sort the steps by their 'order' property
    const sortedSteps = recipe.stepDTOs.sort((a, b) => a.order - b.order);

    // Loop through sorted steps and dynamically populate the step template
    sortedSteps.forEach((step, index) => {

        // Populate the stepTemplate with the current step's data
        const stepTemplate = `
        <div class="space-y-4 rounded-md bg-gray-100 p-4 shadow-sm" data-step="${stepCounter}">
            <div class="flex items-center justify-between">
                <h4 class="text-md font-semibold text-gray-800">Step ${stepCounter}</h4>
                <button type="button" class="deleteStepButton text-xl text-red-500">&times;</button>
            </div>
            <textarea placeholder="Describe this step..." class="w-full rounded border border-gray-300 px-3 py-2 text-gray-800 shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-400" required>${step.text}</textarea>

            <!-- Step Image -->
            <div class="mt-2">
                <label class="block text-sm font-medium text-gray-700">Step Image</label>
                <button type="button" class="uploadImageButton mt-2 w-full rounded border p-2 bg-blue-500 text-white" data-type="step" data-step-id="${stepCounter}">Upload Image</button>
                <img id="stepImagePreview-${stepCounter}" class="uploadedImagePreview mt-4 ${step.imageUrl ? '' : 'hidden'} h-32 w-32 rounded-md border object-cover" src="${step.imageUrl}" alt="Step Image Preview">
                <input type="hidden" id="stepImageId-${stepCounter}" name="stepImageId-${stepCounter}" value="${step.imageId}" />
                <input type="hidden" id="stepImageUrl-${stepCounter}" name="stepImageUrl-${stepCounter}" value="${step.imageUrl}" />
            </div>

            <!-- Step Video -->
            <div class="mt-2">
                <label class="block text-sm font-medium text-gray-700">Step Video</label>
                <button type="button" class="uploadVideoButton mt-2 w-full rounded border p-2 bg-blue-500 text-white" data-type="step" data-step-id="${stepCounter}">Upload Video</button>
                <video id="stepVideoPreview-${stepCounter}" class="uploadedVideoPreview mt-4 ${step.videoUrl ? '' : 'hidden'} h-auto w-3/4 rounded-md" width="320" controls src="${step.videoUrl}"></video>
                <input type="hidden" id="stepVideoId-${stepCounter}" name="stepVideoId-${stepCounter}" value="${step.videoId}" />
                <input type="hidden" id="stepVideoUrl-${stepCounter}" name="stepVideoUrl-${stepCounter}" value="${step.videoUrl}" />
            </div>
        </div>
    `;

        // Append the populated template to the steps container
        $('#stepsContainer').append(stepTemplate);
        stepCounter++; // Generate step counter dynamically

    });


}

// Initialize an empty array to store ingredients data
let ingredients = [];

// Function to fetch ingredients from the API
function fetchIngredients() {
    // Check if ingredients array is empty (not fetched yet)
    if (ingredients.length === 0) {
        $.ajax({
            url: "https://localhost:7157/Editor/Ingredient/GetIngredientList", // API endpoint
            type: "GET",
            dataType: "json",
            success: function (response) {
                // Check if response is successful
                if (response.isSuccess) {
                    ingredients = response.data; // Save the fetched ingredients data
                } else {
                    console.error("Failed to fetch ingredients: ", response.data);
                }
            },
            error: function (xhr, status, error) {
                console.error("Error fetching ingredients: ", error);
            }
        });
    }
}

let stepCounter = 1;

$(document).ready(function () {
    

    // Add Step functionality
    $('#addStepButton').on('click', function () {
        const stepTemplate = `
            <div class="space-y-4 rounded-md bg-gray-100 p-4 shadow-sm" data-step="${stepCounter}">
                <div class="flex items-center justify-between">
                    <h4 class="text-md font-semibold text-gray-800">Step ${stepCounter}</h4>
                    <button type="button" class="deleteStepButton text-xl text-red-500">&times;</button>
                </div>
                <textarea placeholder="Describe this step..." class="w-full rounded border border-gray-300 px-3 py-2 text-gray-800 shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-400" required></textarea>

                <!-- Step Image -->
                <div class="mt-2">
                    <label class="block text-sm font-medium text-gray-700">Step Image</label>
                    <button type="button" class="uploadImageButton mt-2 w-full rounded border p-2 bg-blue-500 text-white" data-type="step" data-step-id="${stepCounter}">Upload Image</button>
                    <img id="stepImagePreview-${stepCounter}" class="uploadedImagePreview mt-4 hidden h-32 w-32 rounded-md border object-cover" alt="Step Image Preview">
                    <input type="hidden" id="stepImageId-${stepCounter}" name="stepImageId-${stepCounter}" />
                    <input type="hidden" id="stepImageUrl-${stepCounter}" name="stepImageUrl-${stepCounter}" />
                </div>

                <!-- Step Video -->
                <div class="mt-2">
                    <label class="block text-sm font-medium text-gray-700">Step Video</label>
                    <button type="button" class="uploadVideoButton mt-2 w-full rounded border p-2 bg-blue-500 text-white" data-type="step" data-step-id="${stepCounter}">Upload Video</button>
                    <video id="stepVideoPreview-${stepCounter}" class="uploadedVideoPreview mt-4 hidden h-auto w-3/4 rounded-md" width="320" controls></video>
                    <input type="hidden" id="stepVideoId-${stepCounter}" name="stepVideoId-${stepCounter}" />
                    <input type="hidden" id="stepVideoUrl-${stepCounter}" name="stepVideoUrl-${stepCounter}" />
                </div>
            </div>
        `;

        $('#stepsContainer').append(stepTemplate);
        stepCounter++;
    });

    $(document).on('click', '.deleteStepButton', function () {
        // Remove the closest step element
        $(this).closest('.space-y-4').remove();

        // Reassign the step number and update both the text and the data-step attribute for the remaining steps
        $('#stepsContainer .space-y-4').each(function (index) {
            $(this).find('h4').text(`Step ${index + 1}`);
            $(this).attr('data-step', index + 1); // Update the data-step attribute
        });

        // Update the stepCounter based on the remaining number of steps
        stepCounter = $('#stepsContainer .space-y-4').length + 1;
    });


    const mediaWidget = cloudinary.createUploadWidget(
        {
            cloudName: "dxclyqubm",
            uploadPreset: "letwecook_preset",
            sources: ["local", "url", "camera"],
            multiple: false,
            cropping: true,
            maxImageFileSize: 2000000, // 2MB for image
            clientAllowedFormats: ["jpg", "jpeg", "png", "mp4", "mov", "avi"], // Allow video formats too
        },
        async (error, result) => {
            if (!error && result && result.event === "success") {
                const mediaUrl = result.info.secure_url;
                const activeButton = mediaWidget.triggerButton;
                const mediaType = $(activeButton).data("type");

                try {
                    const response = await $.ajax({
                        url: "/Media/SaveMediaUrl",
                        type: "POST",
                        data: { url: mediaUrl },
                        dataType: "json",
                    });

                    if (response && response.data) {
                        const mediaId = response.data.id; // The ID of the saved media

                        if (mediaType === "cover") {
                            $("#coverImagePreview").attr("src", mediaUrl).removeClass("hidden");
                            $("#coverImageInfo").text(`${mediaUrl}`).removeClass("hidden");
                            $("#coverMediaId").val(mediaId);

                            populateRecipePreview();
                        } else if (mediaType === "step") {
                            const stepId = $(activeButton).data("step-id");

                            // Handle image upload
                            if (result.info.format === "jpg" || result.info.format === "jpeg" || result.info.format === "png") {
                                $(`#stepImagePreview-${stepId}`).attr("src", mediaUrl).removeClass("hidden");
                                $(`#stepImageId-${stepId}`).val(mediaId);
                                $(`#stepImageUrl-${stepId}`).val(mediaUrl);
                                populateRecipePreview();
                            }

                            // Handle video upload
                            if (result.info.format === "mp4" || result.info.format === "mov" || result.info.format === "avi") {
                                $(`#stepVideoPreview-${stepId}`).attr("src", mediaUrl).removeClass("hidden");
                                $(`#stepVideoId-${stepId}`).val(mediaId);
                                $(`#stepVideoUrl-${stepId}`).val(mediaUrl);
                                populateRecipePreview();
                            }
                        }
                    } else {
                        console.error("Failed to save media URL on server");
                    }
                } catch (error) {
                    console.error("Error sending media URL to server:", error);
                }
            } else if (error) {
                console.error("Cloudinary upload error:", error);
            }
        }
    );

    $(document).on("click", ".uploadImageButton", function () {
        mediaWidget.triggerButton = this;
        mediaWidget.open();
    });

    $(document).on("click", ".uploadVideoButton", function () {
        mediaWidget.triggerButton = this;
        mediaWidget.open();
    });



    

    // Handle "Add Ingredient" button click to add new ingredient dropdown
    $('#addIngredientButton').click(function () {
        // Create new ingredient container
        const ingredientDiv = $('<div>', { class: 'mt-2 ingredient-dropdown flex items-center space-x-4' });

        // Create close button
        const closeButton = $('<button>', { class: 'ml-2 text-red-500 hover:text-red-700', text: 'X' });
        closeButton.click(function () {
            ingredientDiv.remove(); // Remove the ingredient dropdown when the close button is clicked
        });

        // Create ingredient dropdown
        const ingredientSelect = $('<select>', { class: 'w-full rounded border border-gray-300 px-3 py-2 text-gray-800 shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-400' });
        ingredientSelect.append($('<option>', { value: "", text: "Select Ingredient", disabled: true, selected: true }));

        // Fetch ingredients if not already fetched
        fetchIngredients();

        // Wait for ingredients to be fetched before adding options
        setTimeout(function () {
            ingredients.forEach(ingredient => {
                ingredientSelect.append($('<option>', {
                    value: ingredient.id,
                    text: ingredient.ingredientName // Display ingredient name in the dropdown
                }));
            });
        }, 500); // Timeout to wait for fetch completion (adjust as needed)

        // Create hidden input for ingredient ID
        const hiddenIdInput = $('<input>', {
            type: 'hidden',
            class: 'ingredient-id',
            name: 'ingredientId[]', // Add square brackets to make it an array for multiple ingredients
            value: "" // Initial value will be empty, will be populated when user selects an ingredient
        });

        // Create quantity input
        const quantityInput = $('<input>', {
            type: 'number',
            class: 'w-20 rounded border border-gray-300 px-3 py-2 text-gray-800 shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-400',
            placeholder: 'Quantity',
            min: '0.01',
            step: '0.01'
        });

        // Create measure unit dropdown
        const unitSelect = $('<select>', { class: 'w-20 rounded border border-gray-300 px-3 py-2 text-gray-800 shadow-sm focus:outline-none focus:ring-2 focus:ring-blue-400' });
        unitSelect.append($('<option>', { value: "", text: "Unit", disabled: true, selected: true }));

        // Add measure unit options
        const units = ['kg', 'g', 'oz', 'lb', 'cup', 'tbsp', 'tsp'];
        units.forEach(unit => {
            unitSelect.append($('<option>', { value: unit, text: unit }));
        });

        // Append the select dropdowns, input, and hidden ID to the ingredient container
        ingredientDiv.append(ingredientSelect);
        ingredientDiv.append(quantityInput);
        ingredientDiv.append(unitSelect);
        ingredientDiv.append(hiddenIdInput);
        ingredientDiv.append(closeButton);

        // Append the ingredient container to the ingredients section
        $('#ingredientsSection').append(ingredientDiv);

        // Handle ingredient selection change
        ingredientSelect.change(function () {
            // Set the hidden ID input value when an ingredient is selected
            const selectedIngredientId = $(this).val();
            hiddenIdInput.val(selectedIngredientId);
        });
    });


    $('#preview-btn').on('click', function () {
        populateRecipePreview();
    });

    $('#save-recipe-button').on('click', function () {
        // Gather recipe data from the form or editor
        let recipe = gatherRecipeData();
        let recipeId = $('#recipe-id').text(); // Get the recipe ID from the page

        // Validate the gathered recipe data
        if (validateRecipeData(recipe)) {
            // Prepare data for the backend, including the recipe ID
            const backendRequestData = prepareRecipeDataForBackend(recipe);
            backendRequestData.id = recipeId; // Add the ID property

            console.log('Prepared request data:', backendRequestData);

            // Make an AJAX PUT request to the Update endpoint
            $.ajax({
                url: `/api/recipes/${recipeId}`, // Ensure the ID is included in the URL
                type: 'PUT', // Use PUT for updates
                contentType: 'application/json',
                data: JSON.stringify(backendRequestData), // Serialize the data for the backend
                success: function (response) {
                    // Redirect to the updated recipe details page
                    window.location.href = `/Editor/Recipe/Edit/${response.id}`;
                },
                error: function (xhr, status, error) {
                    // Handle errors
                    let errorMessage = 'Error updating recipe: ';
                    if (xhr.responseJSON && xhr.responseJSON.Errors) {
                        // Extract validation errors from the response
                        errorMessage += xhr.responseJSON.Errors.join(', ');
                    } else {
                        errorMessage += xhr.responseText || status || error;
                    }
                    showErrorMessage(errorMessage);
                }
            });
        }
    });


    showSuccessMessage("Welcome to our recipe editor! Happy editing :)");
    $('.buttons').fadeOut();

    $('#success-message-close-btn').on('click', function () {
        $('.notification-area').fadeOut();
    })
});

function showSuccessMessage(message = "We've created your recipe successfully!") {
    $('.notification-area').fadeIn();
    $('.buttons').fadeIn();
    $('.noti-message').text(message);
    $('.notification-area').addClass('bg-green-500').removeClass('bg-red-500');
}

function showErrorMessage(message = "Failed to create your recipe due to error: ") {
    $('.notification-area').fadeIn();
    $('.buttons').fadeOut();
    $('.noti-message').text(message);
    $('.notification-area').removeClass('bg-green-500').addClass('bg-red-500');
}

function prepareRecipeDataForBackend(recipeData) {
    return {
        title: recipeData.title,
        description: recipeData.description,
        coverImageId: recipeData.coverImageId,
        cuisine: recipeData.cuisine,
        difficulty: recipeData.difficulty,
        cookingTimeInMinutes: recipeData.cookTime,
        serving: recipeData.servings,
        recipeIngredientDTOs: recipeData.ingredients.map(ingredient => ({
            recipeId: '00000000-0000-0000-0000-000000000000', // Use empty Guid as fallback
            ingredientId: ingredient.id,
            ingredientName: ingredient.name,
            quantity: ingredient.quantity,
            unit: ingredient.unit,
        })),
        stepDTOs: recipeData.steps.map((step, index) => ({
            id: step.id || '00000000-0000-0000-0000-000000000000', // Use empty Guid as fallback
            text: step.text,
            order: index + 1,
            imageUrl: step.imageUrl || "",
            videoUrl: step.videoUrl || "",
            imageId: step.imageId || "",
            videoId: step.videoId || ""
        }))
    };
}



function populateRecipePreview() {
    const recipeData = gatherRecipeData();

    // Set cover image with a fallback for empty or missing image URLs
    $("#coverImagePreviewPreview").attr("src", recipeData.coverImageUrl || "default-cover-image-url.jpg");

    // Set title, description, cuisine, difficulty, cook time, and servings
    $("#recipeTitle").text(recipeData.title);
    $("#recipeDescription").text(recipeData.description);
    $("#recipeCuisine").text(recipeData.cuisine);
    $("#recipeDifficulty").text(recipeData.difficulty);
    $("#recipeCookTime").text(`${recipeData.cookTime} mins`);
    $("#recipeServings").text(`${recipeData.servings} servings`);

    // Populate Ingredients List with colorful badges
    const ingredientsContainer = $("#ingredientsList");
    ingredientsContainer.empty();
    recipeData.ingredients.forEach(ingredient => {
        const ingredientItem = `
            <div class="inline-block rounded-full bg-yellow-100 px-3 py-2 text-sm font-semibold text-yellow-800 shadow-md">
                ${ingredient.name}: ${ingredient.quantity} ${ingredient.unit}
            </div>`;
        ingredientsContainer.append(ingredientItem);
    });

    // Populate Steps List with rounded gray background
    const stepsContainer = $("#stepsList");
    stepsContainer.empty();
    recipeData.steps.forEach((step, index) => {
        const stepItem = `
            <div class="rounded-lg bg-gray-100 p-4 shadow-md">
                <p class="font-semibold text-gray-800 mb-2">Step ${index + 1}</p>
                <p class="text-gray-700">${step.text}</p>
                ${step.imageId ? `<img src="${step.imageUrl}" class="w-full h-auto object-cover rounded-md mt-4" />` : ''}
                ${step.videoId ? `<video src="${step.videoUrl}" class="w-full h-auto object-cover rounded-md mt-4" controls></video>` : ''}
            </div>`;
        stepsContainer.append(stepItem);
    });
}

function collectAllSteps() {
    const steps = [];

    // Iterate over all steps
    $('#stepsContainer .space-y-4').each(function () {
        const stepNumber = $(this).data('step');
        const stepText = $(this).find('textarea').val() || ""; // Get the text or default to ""
        const stepImageId = $(this).find(`input[id="stepImageId-${stepNumber}"]`).val() || ""; // Get the image ID or default to ""
        const stepVideoId = $(this).find(`input[id="stepVideoId-${stepNumber}"]`).val() || ""; // Get the video ID or default to ""
        const stepImageUrl = $(this).find(`input[id="stepImageUrl-${stepNumber}"]`).val() || "";
        const stepVideoUrl = $(this).find(`input[id="stepVideoUrl-${stepNumber}"]`).val() || "";

        // Push the step data into the array
        steps.push({
            id: null,
            order: stepNumber,
            text: stepText,
            imageId: stepImageId,
            videoId: stepVideoId,
            imageUrl: stepImageUrl,
            videoUrl: stepVideoUrl
        });
    });

    return steps;
}

// Function to collect all ingredients (name, id, quantity, and unit)
function collectIngredients() {
    const ingredientsList = [];

    // Iterate over each ingredient container in the ingredients section
    $('.ingredient-dropdown').each(function () {
        const ingredientSelect = $(this).find('select').first(); // Ingredient dropdown
        const quantityInput = $(this).find('input[type="number"]'); // Quantity input
        const unitSelect = $(this).find('select').last(); // Unit dropdown
        const hiddenIdInput = $(this).find('input.ingredient-id'); // Hidden ID field

        // Get selected ingredient data
        const ingredientId = hiddenIdInput.val(); // Ingredient ID
        const ingredientName = ingredientSelect.find('option:selected').text(); // Ingredient Name
        const quantity = parseFloat(quantityInput.val()); // Quantity (ensure it's a float)
        const unit = unitSelect.val(); // Measure unit

        // Check if the necessary fields have valid values
        if (ingredientId && ingredientName && !isNaN(quantity) && unit) {
            ingredientsList.push({
                id: ingredientId,
                name: ingredientName,
                quantity: quantity,
                unit: unit
            });
        }
    });

    return ingredientsList; // Return the collected ingredients array
}

function gatherRecipeData() {
    // Initialize an object to store all the recipe data
    let recipeData = {
        title: "",
        description: "",
        coverImageId: "",
        coverImageUrl: "",
        cuisine: "",
        difficulty: "",
        cookTime: "",
        servings: "",
        ingredients: [],
        steps: []
    };

    // Collect Recipe Title
    recipeData.title = $("#title").val().trim() || '';

    // Collect Recipe Description
    recipeData.description = $("#description").val().trim() || '';

    // Collect Cover Image (media ID)
    recipeData.coverImageId = $("#coverMediaId").val().trim() || '';  // Get media ID from the hidden element
    recipeData.coverImageUrl = $("#coverImageInfo").text().trim() || '';  // Get media URL from the hidden element


    // Collect Cuisine (handle null/empty)
    recipeData.cuisine = $("#cuisine").val() ? $("#cuisine").val().trim() : '';

    // Collect Difficulty (handle null/empty)
    recipeData.difficulty = $("#difficulty").val() ? $("#difficulty").val().trim() : '';

    // Collect Cook Time
    recipeData.cookTime = parseFloat($("#cookTime").val()) || 0;

    // Collect Servings
    recipeData.servings = parseInt($("#serving").val()) || 0;

    // Collect Ingredients using the pre-defined function
    recipeData.ingredients = collectIngredients();

    // Collect Steps using the pre-defined function
    recipeData.steps = collectAllSteps();

    // Log the collected recipe data (for debugging)
    //console.log(recipeData);

    // Return the collected recipe data
    return recipeData;
}

function validateRecipeData(recipeData) {
    // Validate Recipe Title
    if (!recipeData.title || recipeData.title.trim() === '') {
        alert("Title is required.");
        return false;
    }

    // Validate Recipe Description
    if (!recipeData.description || recipeData.description.trim() === '') {
        alert("Description is required.");
        return false;
    }

    // Validate Cover Image ID (must be a valid GUID)
    if (!recipeData.coverImageId || !isValidGuid(recipeData.coverImageId)) {
        alert("A valid Cover Image ID is required.");
        return false;
    }

    // Validate Cuisine
    if (!recipeData.cuisine || recipeData.cuisine === '') {
        alert("Cuisine is required.");
        return false;
    }

    // Validate Difficulty
    if (!recipeData.difficulty || recipeData.difficulty === '') {
        alert("Difficulty is required.");
        return false;
    }

    // Validate Cook Time
    if (recipeData.cookTime <= 0) {
        alert("Cook Time must be greater than 0.");
        return false;
    }

    // Validate Servings
    if (recipeData.servings <= 0) {
        alert("Servings must be greater than 0.");
        return false;
    }

    // Validate Ingredients
    if (!recipeData.ingredients || recipeData.ingredients.length === 0) {
        alert("At least one ingredient is required.");
        return false;
    }

    for (let ingredient of recipeData.ingredients) {
        if (!ingredient.name || ingredient.name.trim() === '') {
            alert("Ingredient name is required.");
            return false;
        }
        if (!ingredient.id || !isValidGuid(ingredient.id)) {
            alert("Ingredient ID must be a valid GUID.");
            return false;
        }
        if (ingredient.quantity <= 0) {
            alert("Ingredient quantity must be greater than 0.");
            return false;
        }
        if (!ingredient.unit || ingredient.unit.trim() === '') {
            alert("Ingredient unit is required.");
            return false;
        }
    }

    // Validate Steps
    if (!recipeData.steps || recipeData.steps.length === 0) {
        alert("At least one step is required.");
        return false;
    }

    for (let step of recipeData.steps) {
        if (!step.text || step.text.trim() === '') {
            alert("Step text description is required.");
            return false;
        }
        if (step.imageId && !isValidGuid(step.imageId)) {
            alert("Step image ID must be a valid GUID.");
            return false;
        }
        if (step.videoId && !isValidGuid(step.videoId)) {
            alert("Step video ID must be a valid GUID.");
            return false;
        }
    }

    // All validations passed
    return true;
}

// Helper function to validate GUID format
function isValidGuid(guid) {
    const guidRegex = /^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$/;
    return guidRegex.test(guid);
}
