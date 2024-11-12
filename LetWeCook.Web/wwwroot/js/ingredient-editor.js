let defaultStyleSchema = {
    blueButton: {
        normalButton: {
            background: "bg-gray-800",
            text: "text-blue-400",
            hoverBackground: "hover:bg-blue-400",
            hoverText: "hover:text-white"
        },
        selectedButton: {
            background: "bg-blue-400",
            text: "text-white",
            hoverBackground: "hover:bg-blue-400",
            hoverText: "hover:text-white"
        }
    },
    redButton: {
        normalButton: {
            background: "bg-gray-800",
            text: "text-red-400",
            hoverBackground: "hover:bg-red-400",
            hoverText: "hover:text-white"
        },
        selectedButton: {
            background: "bg-gray-800",
            text: "text-white",
            hoverBackground: "hover:bg-red-400",
            hoverText: "hover:text-white"
        }
    },
    greenButton: {
        normalButton: {
            background: "bg-gray-800",
            text: "text-green-400",
            hoverBackground: "hover:bg-green-400",
            hoverText: "hover:text-white"
        },
        selectedButton: {
            background: "bg-gray-800",
            text: "text-white",
            hoverBackground: "hover:bg-green-400",
            hoverText: "hover:text-white"
        }
    },
    yellowButton: {
        normalButton: {
            background: "bg-gray-800",
            text: "text-yellow-400",
            hoverBackground: "hover:bg-yellow-400",
            hoverText: "hover:text-white"
        },
        selectedButton: {
            background: "bg-gray-800",
            text: "text-white",
            hoverBackground: "hover:bg-yellow-400",
            hoverText: "hover:text-white"
        }
    }
};

let frames = [];

let $editorContent = $('#editor-content');

function createFrameObject() {
    return {
        contentType: "text",
        textContent: "",
        imageContent: ""
    }
}

function addNewFrameObjectToArray(frames) {
    frames.push(createFrameObject());
}

// -- Underlying data ^


function addNewFrameDOM() {
    let $frameElement = $('<div></div>');

    $frameElement.addClass("frame flex flex-col rounded-lg h-96 w-full bg-gray-800 items-center justify-start");

    $frameElement.append(createControlBar());

    $frameElement.append(createTextInputContainer());

    $frameElement.append(createImageInputContainer());

    $editorContent.append($frameElement);
}

function createNewFrameDOM() {
    let $frameElement = $('<div></div>');

    $frameElement.addClass("frame flex flex-col rounded-lg h-96 w-full bg-gray-800 items-center justify-start");

    $frameElement.append(createControlBar());

    $frameElement.append(createTextInputContainer());

    $frameElement.append(createImageInputContainer());

    return $frameElement;
}

function createControlBar() {
    let $controlBar = $('<div></div>');
    $controlBar.addClass("control-bar flex flex-col items-center justify-start w-full");

    let $contentRelatedButtonBar = $('<div></div>');
    $contentRelatedButtonBar.addClass("flex flex-row items-center justify-start w-full");

    let $switchToTextBtn = createControlBarButton("blueButton", "switch-text-btn", "Text", true);
    let $switchToImageBtn = createControlBarButton("blueButton", "switch-image-btn", "Image", false);

    $switchToTextBtn.on('click', () => handleSwitchToText($controlBar));
    $switchToImageBtn.on('click', () => handleSwitchToImage($controlBar));

    $contentRelatedButtonBar.append($switchToTextBtn);
    $contentRelatedButtonBar.append($switchToImageBtn);

    let $layoutRelatedButtonBar = $('<div></div>');
    $layoutRelatedButtonBar.addClass("flex flex-row items-center justify-start w-full");

    let $removeFrameBtn = createControlBarButton("redButton", "remove-frame-btn", "Remove Frame", false);
    let $addFrameAboveBtn = createControlBarButton("greenButton", "add-frame-above-btn", "Add Frame Above", false);
    let $addFrameBelowBtn = createControlBarButton("greenButton", "add-frame-below-btn", "Add Frame Below", false);
    let $moveFrameUp = createControlBarButton("yellowButton", "move-frame-up-btn", "Move Frame Up", false);
    let $moveFrameDown = createControlBarButton("yellowButton", "move-frame-down-btn", "Move Frame Down", false);

    $removeFrameBtn.on('click', () => handleRemoveFrame($controlBar));
    $addFrameAboveBtn.on('click', () => handleAddFrameAbove($controlBar));
    $addFrameBelowBtn.on('click', () => handleAddFrameBelow($controlBar));
    $moveFrameUp.on('click', () => handleMoveFrameUp($controlBar));
    $moveFrameDown.on('click', () => handleMoveFrameDown($controlBar));


    // Append all buttons to the control bar
    $layoutRelatedButtonBar.append($removeFrameBtn);
    $layoutRelatedButtonBar.append($addFrameAboveBtn);
    $layoutRelatedButtonBar.append($addFrameBelowBtn);
    $layoutRelatedButtonBar.append($moveFrameUp);
    $layoutRelatedButtonBar.append($moveFrameDown);

    $controlBar.append($contentRelatedButtonBar);
    $controlBar.append($layoutRelatedButtonBar);


    return $controlBar;
}

// Handler for removing the frame
function handleRemoveFrame($controlBar) {
    // Find the corresponding frame
    let $frame = $controlBar.closest('.frame');

    let frameIndex = getFrameIndex($controlBar);
    frames.splice(frameIndex, 1);

    // Remove the frame from the DOM
    $frame.remove();
}

// Handler for adding a frame above the current one
function handleAddFrameAbove($controlBar) {
    // Find the corresponding frame
    let $frame = $controlBar.closest('.frame');

    // Find the current frame index
    let frameIndex = getFrameIndex($controlBar);

    // Create a new frame element
    let $newFrame = createNewFrameDOM();

    // Insert the new frame in the underlying array (insert before the current one)
    frames.splice(frameIndex, 0, createFrameObject()); // Adjust based on how you manage frame objects

    // Check if $frame exists and insert new frame above it
    if ($frame.length) {
        $newFrame.insertBefore($frame); // Properly insert the new frame before the current one
    } else {
        // If $frame is not found, append to the container as a fallback
        $editorContent.append($newFrame);
    }
}

// Handler for moving a frame up
function handleMoveFrameUp($controlBar) {
    // Find the corresponding frame
    let $frame = $controlBar.closest('.frame');

    // Find the current frame index
    let frameIndex = getFrameIndex($controlBar);

    // If it's the first frame, we can't move it up
    if (frameIndex === 0) {
        return; // No action if it's the first frame
    }

    // Find the previous frame in the DOM
    let $previousFrame = $frame.prev('.frame');

    // Swap the frames in the underlying array
    let temp = frames[frameIndex];
    frames[frameIndex] = frames[frameIndex - 1];
    frames[frameIndex - 1] = temp;

    // Move the current frame up in the DOM
    if ($previousFrame.length) {
        $frame.insertBefore($previousFrame);
    }
}

// Handler for moving a frame down
function handleMoveFrameDown($controlBar) {
    // Find the corresponding frame
    let $frame = $controlBar.closest('.frame');

    // Find the current frame index
    let frameIndex = getFrameIndex($controlBar);

    // If it's the last frame, we can't move it down
    if (frameIndex === frames.length - 1) {
        return; // No action if it's the last frame
    }

    // Find the next frame in the DOM
    let $nextFrame = $frame.next('.frame');

    // Swap the frames in the underlying array
    let temp = frames[frameIndex];
    frames[frameIndex] = frames[frameIndex + 1];
    frames[frameIndex + 1] = temp;

    // Move the current frame down in the DOM
    if ($nextFrame.length) {
        $frame.insertAfter($nextFrame);
    }
}


// Handler for adding a frame below the current one
function handleAddFrameBelow($controlBar) {
    // Find the corresponding frame
    let $frame = $controlBar.closest('.frame');

    // Find the current frame index
    let frameIndex = getFrameIndex($controlBar);

    // Create a new frame element
    let $newFrame = createNewFrameDOM();

    // Insert the new frame in the underlying array (insert after the current one)
    frames.splice(frameIndex + 1, 0, createFrameObject()); // Adjust based on how you manage frame objects

    // Check if $frame exists and insert new frame below it
    if ($frame.length) {
        $newFrame.insertAfter($frame); // Properly insert the new frame after the current one
    } else {
        // If $frame is not found, append to the container as a fallback
        $editorContent.append($newFrame);
    }
}


function handleSwitchToText($controlBar) {
    let $frame = $controlBar.closest('.frame');
    let $imageInputContainer = $frame.find('.image-input-container');
    let $textInputContainer = $frame.find('.text-input-container');

    let $switchToTextBtn = $controlBar.find('.switch-text-btn');
    let $switchToImageBtn = $controlBar.find('.switch-image-btn');

    styleButton("blueButton", $switchToTextBtn, true);
    styleButton("blueButton", $switchToImageBtn, false);

    $imageInputContainer.addClass("hidden");
    $textInputContainer.removeClass("hidden");

    // Update the underlying frame's contentType
    let frameIndex = getFrameIndex($controlBar);
    frames[frameIndex].contentType = "text"; // Set content type to "text"
}

function handleSwitchToImage($controlBar) {
    let $frame = $controlBar.closest('.frame');
    let $imageInputContainer = $frame.find('.image-input-container');
    let $textInputContainer = $frame.find('.text-input-container');

    let $switchToTextBtn = $controlBar.find('.switch-text-btn');
    let $switchToImageBtn = $controlBar.find('.switch-image-btn');

    styleButton("blueButton", $switchToTextBtn, false);
    styleButton("blueButton", $switchToImageBtn, true);

    $imageInputContainer.removeClass("hidden");
    $textInputContainer.addClass("hidden");

    // Update the underlying frame's contentType
    let frameIndex = getFrameIndex($controlBar);
    frames[frameIndex].contentType = "image"; // Set content type to "image"
}

function createControlBarButton(colorKey, buttonNameClass, buttonName, isSelected) {
    let $button = $(`<div class=>${buttonName}</div>`);
    $button.addClass(buttonNameClass)

    $button.addClass("bg-gray-800 py-2 font-semibold flex text-sm flex-row items-center justify-center px-4 h-12 cursor-pointer text-center");

    styleButton(colorKey, $button, isSelected);

    return $button;
}

function styleButton(colorKey, buttonElement, isSelected) {
    // Clear all previous classes
    buttonElement.removeClass(defaultStyleSchema[colorKey].normalButton.background);
    buttonElement.removeClass(defaultStyleSchema[colorKey].normalButton.hoverBackground);
    buttonElement.removeClass(defaultStyleSchema[colorKey].normalButton.text);
    buttonElement.removeClass(defaultStyleSchema[colorKey].normalButton.hoverText);
    buttonElement.removeClass(defaultStyleSchema[colorKey].selectedButton.background);
    buttonElement.removeClass(defaultStyleSchema[colorKey].selectedButton.hoverBackground);
    buttonElement.removeClass(defaultStyleSchema[colorKey].selectedButton.text);
    buttonElement.removeClass(defaultStyleSchema[colorKey].selectedButton.hoverText);

    // Add the correct classes based on selection state
    if (isSelected) {
        buttonElement.addClass(defaultStyleSchema[colorKey].selectedButton.background);
        buttonElement.addClass(defaultStyleSchema[colorKey].selectedButton.hoverBackground);
        buttonElement.addClass(defaultStyleSchema[colorKey].selectedButton.text);
        buttonElement.addClass(defaultStyleSchema[colorKey].selectedButton.hoverText);
    } else {
        buttonElement.addClass(defaultStyleSchema[colorKey].normalButton.background);
        buttonElement.addClass(defaultStyleSchema[colorKey].normalButton.hoverBackground);
        buttonElement.addClass(defaultStyleSchema[colorKey].normalButton.text);
        buttonElement.addClass(defaultStyleSchema[colorKey].normalButton.hoverText);
    }
}

function createTextInputContainer() {
    let $textInputContainer = $('<div></div>');
    $textInputContainer.addClass("text-input-container flex flex-row items-center justify-center w-full h-full");

    let $textInput = $('<textarea></textarea>');
    $textInput.addClass("w-full h-60 mx-6 rounded-lg p-4");

    // Add event listener for the textarea to update the underlying content
    $textInput.on('input', function () {
        // Get the index of the frame (assuming you can access the input's context)
        let frameIndex = getFrameIndex(this);
        updateFrameTextContent(frameIndex, $(this).val()); // Update the underlying content
    });

    $textInputContainer.append($textInput);

    return $textInputContainer;
}

function updateFrameTextContent(frameIndex, contentString) {
    frames[frameIndex].textContent = contentString; // Update the underlying data with the new content
}


function createImageInputContainer() {
    let $imageInputContainer = $('<div></div>');
    $imageInputContainer.addClass('image-input-container hidden flex flex-col w-full h-full items-center justify-start rounded-lg shadow-lg');

    let $imageLabel = $('<label>Click here to upload an image!</label>');
    $imageLabel.addClass('text-lg underline cursor-pointer font-medium text-white py-4');

    let $imageInput = $('<input type="file" accept="image/*">'); // Removed id
    $imageInput.addClass('hidden');

    let $previewImage = $('<img class="image-preview" alt="image-preview">');
    $previewImage.addClass("w-48 h-48 hidden");

    // Append label and input to the container
    $imageInputContainer.append($imageLabel);
    $imageInputContainer.append($imageInput);
    $imageInputContainer.append($previewImage);

    // Attach event listener for image input change
    $imageInput.on('change', function () {
        handleImageInputChange(this, $previewImage);
    });

    // Attach click event to the label to trigger the input click
    $imageLabel.on('click', function () {
        $imageInput.click();
    });

    return $imageInputContainer;
}

function handleImageInputChange(input, $previewImage) {
    let frameIndex = getFrameIndex(input);

    // Check for valid frame index and input files
    if (frameIndex === -1 || !input.files || !input.files[0]) {
        return;
    }

    let file = input.files[0];
    let reader = new FileReader();

    // This function is called when the file is read
    reader.onload = function (e) {
        let base64String = e.target.result; // Get Base64 string from reader
        updateFrameImageContent(frameIndex, base64String); // Update the frame content string
        updateImagePreview($previewImage, base64String); // Update the image preview
    };

    // Start reading the file as a Data URL (Base64)
    reader.readAsDataURL(file);
}


function getFrameIndex(input) {
    let $frame = input.closest('.frame'); // Find the closest .frame ancestor
    let frameIndex = -1;

    // Loop through all .frame elements to find the index
    $editorContent.find('.frame').each(function (index) {
        if ($(this).is($frame)) { // Compare the current frame with the one we found
            frameIndex = index; // Update the index if it matches
            return false; // Break the loop
        }
    });

    return frameIndex; // Return the found index or -1 if not found
}


function updateFrameImageContent(frameIndex, base64String) {
    frames[frameIndex].imageContent = base64String; // Update underlying data
}

function updateImagePreview($previewImage, base64String) {
    $previewImage.attr('src', base64String);  // Set the image src to the Base64 string
    $previewImage.removeClass('hidden');      // Make the image visible
}

// -- DOM related code ^

function attachAddNewFrameButtonClickHandler() {
    let addNewFrameButton = $('#add-new-frame-btn').on('click', function () {
        addNewFrameObjectToArray(frames);
        addNewFrameDOM();
    });
}

function populatePreview() {
    var ingredientName = $('#ingredient-name').val();
    var ingredientDescription = $('#ingredient-description').val();
    var $previewContainer = $('#preview-container');

    // Clear any previous preview content
    $previewContainer.empty();

    // Create elements for name and description
    var $nameElement = $('<h2></h2>').addClass('pt-4 text-center w-full text-2xl font-semibold mb-4').text(ingredientName);
    var $descriptionElement = $('<p></p>').addClass('text-center w-full text-base mb-4').text(ingredientDescription);

    // Append name and description to the preview container
    $previewContainer.append($nameElement);
    $previewContainer.append($descriptionElement);

    // Get the image preview source
    var $imagePreview = $('#image-preview');
    if ($imagePreview.attr('src')) {
        var $imageElement = $('<img>').addClass('h-64 w-64 rounded-md border border-gray-300 object-cover mb-4').attr('src', $imagePreview.attr('src'));
        $previewContainer.append($imageElement);
    }

    // Render frames content from the frames array
    frames.forEach(function (frame) {
        if (frame.contentType === 'text') {
            // Render text content
            var $textElement = $('<p></p>').addClass('text-center w-full text-base mb-4').text(frame.textContent);
            $previewContainer.append($textElement);
        } else if (frame.contentType === 'image' && frame.imageContent) {
            // Render image content
            var $frameImageElement = $('<img>').addClass('w-96 rounded-md border border-gray-300 object-cover mb-4').attr('src', frame.imageContent);
            $previewContainer.append($frameImageElement);
        }
    });
}

function saveIngredient() {
    if (!validateIngredientInput()) {
        return; // Stop the function if validation fails
    }

    showLoadingScreen();

    // Collecting data from input fields
    var ingredientName = $('#ingredient-name').val();
    var ingredientDescription = $('#ingredient-description').val();
    var coverImageBase64 = $('#image-preview').attr('src'); // Assuming it's base64 image

    // Collect frames data and add `order` based on index in the array
    var framesData = frames.map(function (frame, index) {
        return {
            contentType: frame.contentType,
            imageContent: frame.imageContent || "", // Handle empty image content
            textContent: frame.textContent || "",  // Handle empty text content
            order: index // Set `order` based on the frame's index
        };
    });

    // Create the request object matching CreateIngredientRequest
    var requestData = {
        ingredientName: ingredientName,
        ingredientDescription: ingredientDescription,
        coverImageBase64: coverImageBase64 || "",
        rawFrameDTOs: framesData
    };

    // Sending the Ajax POST request
    $.ajax({
        url: '/Cooking/Ingredient/CreateIngredient',  // The endpoint URL
        type: 'POST',
        contentType: 'application/json',
        data: JSON.stringify(requestData),
        success: function (response, status, xhr) {
            if (xhr.status === 200) {
                setTimeout(() => {
                    hideLoadingScreen();
                    setMessageTitle("Information");
                    setMessageContent("Ingredient saved successfully.");
                    showMessageBox();
                }, 1000);
            } else {
                showErrorMessage("Failed to save ingredient. Please try again.");
            }
        },
        error: function (xhr, status, error) {
            var errorMessage = xhr.responseJSON && xhr.responseJSON.message
                ? xhr.responseJSON.message
                : "An error occurred while saving the ingredient. Please try again.";

            showErrorMessage(errorMessage);
            console.error('Error saving ingredient:', error);
        }
    });
}

function validateIngredientInput() {
    var ingredientName = $('#ingredient-name').val();
    var ingredientDescription = $('#ingredient-description').val();
    var coverImageBase64 = $('#image-preview').attr('src');

    var errors = []; // Array to store error messages

    if (!ingredientName) {
        errors.push("Ingredient name is required.");
    }

    if (!ingredientDescription) {
        errors.push("Ingredient description is required.");
    }

    if (!coverImageBase64) {
        errors.push("Cover image is required.");
    }

    if (errors.length > 0) {
        // Join errors with a new line and show as a single message
        showErrorMessage(errors.join("\n"));
        return false;
    }

    return true; // All required fields are provided
}

function showErrorMessage(message) {
    hideLoadingScreen();
    setMessageTitle("Error");
    setMessageContent(message);
    showMessageBox();
}



function setMessageTitle(message) {
    $('#message-box-title').text(message);
}

function setMessageContent(message) {
    $('#message-box-content').text(message);
}

function showMessageBox() {
    $('#message-box').removeClass('opacity-0').addClass('opacity-100');
    $('#message-box').removeClass('h-0').addClass('h-72');
    $('#title-container').removeClass('hidden').addClass('flex');
    $('#content-container').removeClass('hidden').addClass('flex');
}

function hideMessageBox() {
    $('#message-box').addClass('opacity-0').removeClass('opacity-100');
    $('#message-box').addClass('h-0').removeClass('h-72');
    $('#title-container').addClass('hidden').removeClass('flex');
    $('#content-container').addClass('hidden').removeClass('flex');
}

function showLoadingScreen() {
    $('#loading-screen').removeClass('hidden').addClass('flex');
}

function hideLoadingScreen() {
    $('#loading-screen').addClass('hidden').removeClass('flex');
}

$(document).ready(function () {

    attachAddNewFrameButtonClickHandler();

    // Image input change event to update the preview image
    $('#ingredient-image').on('change', function () {
        var input = this;
        var file = input.files[0];

        if (file) {
            var reader = new FileReader();
            reader.onload = function (e) {
                $('#image-preview').attr('src', e.target.result);
            };
            reader.readAsDataURL(file);
        }
    });

    $('#preview-btn').on('click', function () {
        populatePreview();
    });

    $('#save-btn').on('click', function () {
        saveIngredient();
    });


    $('#message-box-close-btn').on('click', function () {
        hideMessageBox();
    });

});