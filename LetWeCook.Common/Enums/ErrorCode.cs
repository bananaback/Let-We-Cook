﻿namespace LetWeCook.Common.Enums
{
    public enum ErrorCode
    {
        UserNotFound,
        EmailConfirmationFailed,
        EmailConfirmationException,
        MediaUrlCreationFailed,
        MediaUrlDeletionFailed,
        MediaUrlSaveFailed,
        MediaUrlRetrievalFailed,
        MediaUrlNotFound,
        CloudinaryImageUploadFailed,
        IngredientCreationFailed,
        IngredientRetrievalFailed,
        IngredientNotFound,
        RecipeRetrievalFailed,
        RecipeCreationFailed,
        RecipeNotFound,
        UserProfileRetrievalFailed,
        UserProfileCreationFailed,
        UserProfileNotFound,
        DatabaseSaveFailed,
        InvalidBase64Image,
        InvalidMediaUrl,
        UnexpectedError,
        None
    }
}
