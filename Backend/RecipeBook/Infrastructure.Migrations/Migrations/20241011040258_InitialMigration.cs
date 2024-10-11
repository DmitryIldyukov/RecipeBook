using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tags",
                columns: table => new
                {
                    tag_id = table.Column<int>(type: "int", nullable: false, comment: "Id тега")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, comment: "Название тега")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tags", x => x.tag_id);
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "Id пользователя")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "Имя пользователя"),
                    login = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false, comment: "Логин"),
                    email = table.Column<string>(type: "nvarchar(254)", maxLength: 254, nullable: false, comment: "Электронная почта"),
                    password = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Пароль"),
                    information = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Информация о себе")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_users", x => x.user_id);
                });

            migrationBuilder.CreateTable(
                name: "recipes",
                columns: table => new
                {
                    recipe_id = table.Column<int>(type: "int", nullable: false, comment: "Id рецепта")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    author_id = table.Column<int>(type: "int", nullable: false, comment: "Id автора"),
                    name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Название рецепта"),
                    description = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false, comment: "Описание"),
                    cook_time = table.Column<int>(type: "int", nullable: false, comment: "Время готовки в минутах"),
                    portion_count = table.Column<int>(type: "int", nullable: false, comment: "Порций в блюде"),
                    image_name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false, comment: "Название фото блюда")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipes", x => x.recipe_id);
                    table.ForeignKey(
                        name: "FK_recipes_users_author_id",
                        column: x => x.author_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "favorites",
                columns: table => new
                {
                    favorite_id = table.Column<int>(type: "int", nullable: false, comment: "Id избранного")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "Id пользователя"),
                    recipe_id = table.Column<int>(type: "int", nullable: false, comment: "Id рецепта")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_favorites", x => x.favorite_id);
                    table.ForeignKey(
                        name: "FK_favorites_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_favorites_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ingredients",
                columns: table => new
                {
                    ingredient_id = table.Column<int>(type: "int", nullable: false, comment: "Id ингредиента")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    recipe_id = table.Column<int>(type: "int", nullable: false, comment: "Id рецепта"),
                    title = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false, comment: "Заголовок для игредиентов"),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Список продуктов")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ingredients", x => x.ingredient_id);
                    table.ForeignKey(
                        name: "FK_ingredients_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "likes",
                columns: table => new
                {
                    like_id = table.Column<int>(type: "int", nullable: false, comment: "Id лайка")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "Id пользователя"),
                    recipe_id = table.Column<int>(type: "int", nullable: false, comment: "Id рецепта"),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false, comment: "Дата и время лайка")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_likes", x => x.like_id);
                    table.ForeignKey(
                        name: "FK_likes_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_likes_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "recipe_tags",
                columns: table => new
                {
                    RecipesId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_recipe_tags", x => new { x.RecipesId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_recipe_tags_recipes_RecipesId",
                        column: x => x.RecipesId,
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_recipe_tags_tags_TagsId",
                        column: x => x.TagsId,
                        principalTable: "tags",
                        principalColumn: "tag_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "steps",
                columns: table => new
                {
                    step_id = table.Column<int>(type: "int", nullable: false, comment: "Id шага")
                        .Annotation("SqlServer:Identity", "1, 1"),
                    recipe_id = table.Column<int>(type: "int", nullable: false, comment: "Id рецепта"),
                    description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false, comment: "Описание шага")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_steps", x => x.step_id);
                    table.ForeignKey(
                        name: "FK_steps_recipes_recipe_id",
                        column: x => x.recipe_id,
                        principalTable: "recipes",
                        principalColumn: "recipe_id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_favorites_recipe_id",
                table: "favorites",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "IX_favorites_user_id",
                table: "favorites",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_ingredients_recipe_id",
                table: "ingredients",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "IX_likes_recipe_id",
                table: "likes",
                column: "recipe_id");

            migrationBuilder.CreateIndex(
                name: "IX_likes_user_id",
                table: "likes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_recipe_tags_TagsId",
                table: "recipe_tags",
                column: "TagsId");

            migrationBuilder.CreateIndex(
                name: "IX_recipes_author_id",
                table: "recipes",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "IX_steps_recipe_id",
                table: "steps",
                column: "recipe_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "favorites");

            migrationBuilder.DropTable(
                name: "ingredients");

            migrationBuilder.DropTable(
                name: "likes");

            migrationBuilder.DropTable(
                name: "recipe_tags");

            migrationBuilder.DropTable(
                name: "steps");

            migrationBuilder.DropTable(
                name: "tags");

            migrationBuilder.DropTable(
                name: "recipes");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
