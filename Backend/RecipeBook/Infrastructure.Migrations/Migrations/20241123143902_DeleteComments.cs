using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class DeleteComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "Пароль");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldComment: "Имя пользователя");

            migrationBuilder.AlterColumn<string>(
                name: "login",
                table: "users",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30,
                oldComment: "Логин");

            migrationBuilder.AlterColumn<string>(
                name: "information",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "Информация о себе");

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "users",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(254)",
                oldMaxLength: 254,
                oldComment: "Электронная почта");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "users",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id пользователя")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "tags",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldComment: "Название тега");

            migrationBuilder.AlterColumn<int>(
                name: "tag_id",
                table: "tags",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id тега")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "recipe_id",
                table: "steps",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id рецепта");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "steps",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "Описание шага");

            migrationBuilder.AlterColumn<int>(
                name: "step_id",
                table: "steps",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id шага")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "portion_count",
                table: "recipes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Порций в блюде");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "recipes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldComment: "Название рецепта");

            migrationBuilder.AlterColumn<string>(
                name: "image_name",
                table: "recipes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldComment: "Название фото блюда");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "recipes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldComment: "Описание");

            migrationBuilder.AlterColumn<int>(
                name: "cook_time",
                table: "recipes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Время готовки в минутах");

            migrationBuilder.AlterColumn<int>(
                name: "author_id",
                table: "recipes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id автора");

            migrationBuilder.AlterColumn<int>(
                name: "recipe_id",
                table: "recipes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id рецепта")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "likes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id пользователя");

            migrationBuilder.AlterColumn<int>(
                name: "recipe_id",
                table: "likes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id рецепта");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "likes",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldComment: "Дата и время лайка");

            migrationBuilder.AlterColumn<int>(
                name: "like_id",
                table: "likes",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id лайка")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "ingredients",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40,
                oldComment: "Заголовок для игредиентов");

            migrationBuilder.AlterColumn<int>(
                name: "recipe_id",
                table: "ingredients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id рецепта");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "ingredients",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255,
                oldComment: "Список продуктов");

            migrationBuilder.AlterColumn<int>(
                name: "ingredient_id",
                table: "ingredients",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id ингредиента")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "favorites",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id пользователя");

            migrationBuilder.AlterColumn<int>(
                name: "recipe_id",
                table: "favorites",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id рецепта");

            migrationBuilder.AlterColumn<int>(
                name: "favorite_id",
                table: "favorites",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldComment: "Id избранного")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "password",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Пароль",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "users",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                comment: "Имя пользователя",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "login",
                table: "users",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                comment: "Логин",
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "information",
                table: "users",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Информация о себе",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "users",
                type: "nvarchar(254)",
                maxLength: 254,
                nullable: false,
                comment: "Электронная почта",
                oldClrType: typeof(string),
                oldType: "nvarchar(254)",
                oldMaxLength: 254);

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "users",
                type: "int",
                nullable: false,
                comment: "Id пользователя",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "tags",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                comment: "Название тега",
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);

            migrationBuilder.AlterColumn<int>(
                name: "tag_id",
                table: "tags",
                type: "int",
                nullable: false,
                comment: "Id тега",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "recipe_id",
                table: "steps",
                type: "int",
                nullable: false,
                comment: "Id рецепта",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "steps",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Описание шага",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "step_id",
                table: "steps",
                type: "int",
                nullable: false,
                comment: "Id шага",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "portion_count",
                table: "recipes",
                type: "int",
                nullable: false,
                comment: "Порций в блюде",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "name",
                table: "recipes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "Название рецепта",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "image_name",
                table: "recipes",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                comment: "Название фото блюда",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "recipes",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                comment: "Описание",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<int>(
                name: "cook_time",
                table: "recipes",
                type: "int",
                nullable: false,
                comment: "Время готовки в минутах",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "author_id",
                table: "recipes",
                type: "int",
                nullable: false,
                comment: "Id автора",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "recipe_id",
                table: "recipes",
                type: "int",
                nullable: false,
                comment: "Id рецепта",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "likes",
                type: "int",
                nullable: false,
                comment: "Id пользователя",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "recipe_id",
                table: "likes",
                type: "int",
                nullable: false,
                comment: "Id рецепта",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<DateTime>(
                name: "created_at",
                table: "likes",
                type: "datetime2",
                nullable: false,
                comment: "Дата и время лайка",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<int>(
                name: "like_id",
                table: "likes",
                type: "int",
                nullable: false,
                comment: "Id лайка",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "title",
                table: "ingredients",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                comment: "Заголовок для игредиентов",
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);

            migrationBuilder.AlterColumn<int>(
                name: "recipe_id",
                table: "ingredients",
                type: "int",
                nullable: false,
                comment: "Id рецепта",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "description",
                table: "ingredients",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                comment: "Список продуктов",
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "ingredient_id",
                table: "ingredients",
                type: "int",
                nullable: false,
                comment: "Id ингредиента",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "user_id",
                table: "favorites",
                type: "int",
                nullable: false,
                comment: "Id пользователя",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "recipe_id",
                table: "favorites",
                type: "int",
                nullable: false,
                comment: "Id рецепта",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "favorite_id",
                table: "favorites",
                type: "int",
                nullable: false,
                comment: "Id избранного",
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1")
                .OldAnnotation("SqlServer:Identity", "1, 1");
        }
    }
}
