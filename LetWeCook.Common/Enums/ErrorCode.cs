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
        DatabaseSaveFailed,
        InvalidBase64Image,
        InvalidMediaUrl,
        UnexpectedError,
        None
    }
}
