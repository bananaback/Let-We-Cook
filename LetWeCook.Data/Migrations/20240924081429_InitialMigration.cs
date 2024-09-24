using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LetWeCook.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "application_user",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_removed = table.Column<bool>(type: "bit", nullable: false),
                    balance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_application_user", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "badge",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_badge", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "dietary_preference",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    value = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dietary_preference", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "IdentityUserRole<Guid>",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IdentityUserRole<Guid>", x => new { x.UserId, x.RoleId });
                });

            migrationBuilder.CreateTable(
                name: "ingredient",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredient", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "media_url",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    alt = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_media_url", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "activity",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    activity_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reference_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_activity", x => x.id);
                    table.ForeignKey(
                        name: "FK_activity_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "application_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "course",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    instructor_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    is_published = table.Column<bool>(type: "bit", nullable: false),
                    certificate = table.Column<bool>(type: "bit", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_course", x => x.id);
                    table.ForeignKey(
                        name: "FK_course_application_user_instructor_id",
                        column: x => x.instructor_id,
                        principalTable: "application_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "dish_collection",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_dish_collection", x => x.id);
                    table.ForeignKey(
                        name: "FK_dish_collection_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "application_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "follow",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    follower_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    followed_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date_followed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_follow", x => x.id);
                    table.ForeignKey(
                        name: "FK_follow_application_user_followed_id",
                        column: x => x.followed_id,
                        principalTable: "application_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_follow_application_user_follower_id",
                        column: x => x.follower_id,
                        principalTable: "application_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "meal_plan",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    meal_type = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meal_plan", x => x.id);
                    table.ForeignKey(
                        name: "FK_meal_plan_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "application_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "notification",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    notification_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    reference_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    message = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_read = table.Column<bool>(type: "bit", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_notification", x => x.id);
                    table.ForeignKey(
                        name: "FK_notification_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "application_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipe",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cuisine = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    difficulty = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cook_time = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    serving = table.Column<int>(type: "int", nullable: false),
                    created_by = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipe", x => x.id);
                    table.ForeignKey(
                        name: "FK_recipe_application_user_created_by",
                        column: x => x.created_by,
                        principalTable: "application_user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "shopping_list",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_completed = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopping_list", x => x.id);
                    table.ForeignKey(
                        name: "FK_shopping_list_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "application_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_profile",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    phone_number = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    first_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    last_name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    age = table.Column<int>(type: "int", nullable: false),
                    gender = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_profile", x => x.id);
                    table.ForeignKey(
                        name: "FK_user_profile_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "application_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_badge",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    badge_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date_earned = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_badge", x => new { x.user_id, x.badge_id });
                    table.ForeignKey(
                        name: "FK_user_badge_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "application_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_user_badge_badge_badge_id",
                        column: x => x.badge_id,
                        principalTable: "badge",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "feed",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    activity_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_feed", x => x.id);
                    table.ForeignKey(
                        name: "FK_feed_activity_activity_id",
                        column: x => x.activity_id,
                        principalTable: "activity",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_feed_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "application_user",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "enrollment",
                columns: table => new
                {
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    course_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date_enrolled = table.Column<DateTime>(type: "datetime2", nullable: false),
                    progress = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_enrollment", x => new { x.user_id, x.course_id });
                    table.ForeignKey(
                        name: "FK_enrollment_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "application_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_enrollment_course_course_id",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "lesson",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    course_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_trial = table.Column<bool>(type: "bit", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lesson", x => x.id);
                    table.ForeignKey(
                        name: "FK_lesson_course_course_id",
                        column: x => x.course_id,
                        principalTable: "course",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "collection_recipe",
                columns: table => new
                {
                    collection_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    recipe_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    date_added = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_collection_recipe", x => new { x.collection_id, x.recipe_id });
                    table.ForeignKey(
                        name: "FK_collection_recipe_dish_collection_collection_id",
                        column: x => x.collection_id,
                        principalTable: "dish_collection",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_collection_recipe_recipe_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "meal_plan_recipe",
                columns: table => new
                {
                    meal_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    recipe_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    servings = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_meal_plan_recipe", x => new { x.meal_id, x.recipe_id });
                    table.ForeignKey(
                        name: "FK_meal_plan_recipe_meal_plan_meal_id",
                        column: x => x.meal_id,
                        principalTable: "meal_plan",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_meal_plan_recipe_recipe_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipe_ingredient",
                columns: table => new
                {
                    ingredient_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    recipe_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipe_ingredient", x => new { x.ingredient_id, x.recipe_id });
                    table.ForeignKey(
                        name: "FK_recipe_ingredient_ingredient_ingredient_id",
                        column: x => x.ingredient_id,
                        principalTable: "ingredient",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recipe_ingredient_recipe_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipe_review",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    recipe_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    rating = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    created_date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    is_possitive = table.Column<bool>(type: "bit", nullable: false),
                    review = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipe_review", x => x.id);
                    table.ForeignKey(
                        name: "FK_recipe_review_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "application_user",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recipe_review_recipe_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipe_step",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    recipe_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    step_number = table.Column<int>(type: "int", nullable: false),
                    instruction = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipe_step", x => x.id);
                    table.ForeignKey(
                        name: "FK_recipe_step_recipe_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipe",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "shopping_list_item",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    list_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ingredient_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quantity = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    unit = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_purchased = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shopping_list_item", x => x.id);
                    table.ForeignKey(
                        name: "FK_shopping_list_item_ingredient_ingredient_id",
                        column: x => x.ingredient_id,
                        principalTable: "ingredient",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shopping_list_item_shopping_list_list_id",
                        column: x => x.list_id,
                        principalTable: "shopping_list",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "user_dietary_preference",
                columns: table => new
                {
                    preference_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_profile_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user_dietary_preference", x => new { x.preference_id, x.user_profile_id });
                    table.ForeignKey(
                        name: "FK_user_dietary_preference_dietary_preference_preference_id",
                        column: x => x.preference_id,
                        principalTable: "dietary_preference",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_user_dietary_preference_user_profile_user_profile_id",
                        column: x => x.user_profile_id,
                        principalTable: "user_profile",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "lesson_content",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LessonId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    content_type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    content_data = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    order_number = table.Column<int>(type: "int", nullable: false),
                    lesson_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_lesson_content", x => x.id);
                    table.ForeignKey(
                        name: "FK_lesson_content_lesson_LessonId",
                        column: x => x.LessonId,
                        principalTable: "lesson",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "quiz",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    lesson_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date_created = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quiz", x => x.id);
                    table.ForeignKey(
                        name: "FK_quiz_lesson_lesson_id",
                        column: x => x.lesson_id,
                        principalTable: "lesson",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "recipe_step_media",
                columns: table => new
                {
                    recipe_step_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    media_url_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipe_step_media", x => new { x.recipe_step_id, x.media_url_id });
                    table.ForeignKey(
                        name: "FK_recipe_step_media_media_url_media_url_id",
                        column: x => x.media_url_id,
                        principalTable: "media_url",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recipe_step_media_recipe_step_recipe_step_id",
                        column: x => x.recipe_step_id,
                        principalTable: "recipe_step",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "quiz_question",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quiz_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    question_text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    correct_answer = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quiz_question", x => x.id);
                    table.ForeignKey(
                        name: "FK_quiz_question_quiz_quiz_id",
                        column: x => x.quiz_id,
                        principalTable: "quiz",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "quiz_result",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    user_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    quiz_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    score = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    date_completed = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_quiz_result", x => x.id);
                    table.ForeignKey(
                        name: "FK_quiz_result_application_user_user_id",
                        column: x => x.user_id,
                        principalTable: "application_user",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_quiz_result_quiz_quiz_id",
                        column: x => x.quiz_id,
                        principalTable: "quiz",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "question_option",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    question_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    option_text = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    option_letter = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    is_correct = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_question_option", x => x.id);
                    table.ForeignKey(
                        name: "FK_question_option_quiz_question_question_id",
                        column: x => x.question_id,
                        principalTable: "quiz_question",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_activity_user_id",
                table: "activity",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_application_user_Email",
                table: "application_user",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_collection_recipe_recipe_id",
                table: "collection_recipe",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "IX_course_instructor_id",
                table: "course",
                column: "instructor_id");

            migrationBuilder.CreateIndex(
                name: "IX_dish_collection_user_id",
                table: "dish_collection",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_enrollment_course_id",
                table: "enrollment",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_feed_activity_id",
                table: "feed",
                column: "activity_id");

            migrationBuilder.CreateIndex(
                name: "IX_feed_user_id",
                table: "feed",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_follow_followed_id",
                table: "follow",
                column: "followed_id");

            migrationBuilder.CreateIndex(
                name: "IX_follow_follower_id",
                table: "follow",
                column: "follower_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_course_id",
                table: "lesson",
                column: "course_id");

            migrationBuilder.CreateIndex(
                name: "IX_lesson_content_LessonId",
                table: "lesson_content",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_meal_plan_user_id",
                table: "meal_plan",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_meal_plan_recipe_recipe_id",
                table: "meal_plan_recipe",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "IX_notification_user_id",
                table: "notification",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_question_option_question_id",
                table: "question_option",
                column: "question_id");

            migrationBuilder.CreateIndex(
                name: "IX_quiz_lesson_id",
                table: "quiz",
                column: "lesson_id");

            migrationBuilder.CreateIndex(
                name: "IX_quiz_question_quiz_id",
                table: "quiz_question",
                column: "quiz_id");

            migrationBuilder.CreateIndex(
                name: "IX_quiz_result_quiz_id",
                table: "quiz_result",
                column: "quiz_id");

            migrationBuilder.CreateIndex(
                name: "IX_quiz_result_user_id",
                table: "quiz_result",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_recipe_created_by",
                table: "recipe",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "IX_recipe_ingredient_recipe_id",
                table: "recipe_ingredient",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "IX_recipe_review_recipe_id",
                table: "recipe_review",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "IX_recipe_review_user_id",
                table: "recipe_review",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_recipe_step_recipe_id",
                table: "recipe_step",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "IX_recipe_step_media_media_url_id",
                table: "recipe_step_media",
                column: "media_url_id");

            migrationBuilder.CreateIndex(
                name: "IX_shopping_list_user_id",
                table: "shopping_list",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_shopping_list_item_ingredient_id",
                table: "shopping_list_item",
                column: "ingredient_id");

            migrationBuilder.CreateIndex(
                name: "IX_shopping_list_item_list_id",
                table: "shopping_list_item",
                column: "list_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_badge_badge_id",
                table: "user_badge",
                column: "badge_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_dietary_preference_user_profile_id",
                table: "user_dietary_preference",
                column: "user_profile_id");

            migrationBuilder.CreateIndex(
                name: "IX_user_profile_user_id",
                table: "user_profile",
                column: "user_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "collection_recipe");

            migrationBuilder.DropTable(
                name: "enrollment");

            migrationBuilder.DropTable(
                name: "feed");

            migrationBuilder.DropTable(
                name: "follow");

            migrationBuilder.DropTable(
                name: "IdentityUserRole<Guid>");

            migrationBuilder.DropTable(
                name: "lesson_content");

            migrationBuilder.DropTable(
                name: "meal_plan_recipe");

            migrationBuilder.DropTable(
                name: "notification");

            migrationBuilder.DropTable(
                name: "question_option");

            migrationBuilder.DropTable(
                name: "quiz_result");

            migrationBuilder.DropTable(
                name: "recipe_ingredient");

            migrationBuilder.DropTable(
                name: "recipe_review");

            migrationBuilder.DropTable(
                name: "recipe_step_media");

            migrationBuilder.DropTable(
                name: "shopping_list_item");

            migrationBuilder.DropTable(
                name: "user_badge");

            migrationBuilder.DropTable(
                name: "user_dietary_preference");

            migrationBuilder.DropTable(
                name: "dish_collection");

            migrationBuilder.DropTable(
                name: "activity");

            migrationBuilder.DropTable(
                name: "meal_plan");

            migrationBuilder.DropTable(
                name: "quiz_question");

            migrationBuilder.DropTable(
                name: "media_url");

            migrationBuilder.DropTable(
                name: "recipe_step");

            migrationBuilder.DropTable(
                name: "ingredient");

            migrationBuilder.DropTable(
                name: "shopping_list");

            migrationBuilder.DropTable(
                name: "badge");

            migrationBuilder.DropTable(
                name: "dietary_preference");

            migrationBuilder.DropTable(
                name: "user_profile");

            migrationBuilder.DropTable(
                name: "quiz");

            migrationBuilder.DropTable(
                name: "recipe");

            migrationBuilder.DropTable(
                name: "lesson");

            migrationBuilder.DropTable(
                name: "course");

            migrationBuilder.DropTable(
                name: "application_user");
        }
    }
}
