using Application.Common.CQRS.Command;
using Application.Common.CQRS.Query;
using Application.Common.Result;
using Application.UseCases.Recipes.Dtos;
using Application.UseCases.Recipes.Queries.GetFavoriteRecipes;
using Application.UseCases.Recipes.Queries.GetUserRecipes;
using Application.UseCases.Users.Commands.Create;
using Application.UseCases.Users.Commands.Login;
using Application.UseCases.Users.Commands.Update;
using Application.UseCases.Users.Dtos;
using Application.UseCases.Users.Queries.GetById;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Dtos.Recipe;
using WebAPI.Dtos.User;

namespace WebAPI.Controllers;

[Route( "api/[controller]" )]
public class UsersController(
    ICommandHandler<CreateUserCommand, Result> createUserHandler,
    ICommandHandler<UpdateUserCommand, Result> updateUserHandler,
    ICommandHandler<LoginUserCommand, ResultT<TokenInfoDto>> loginUserHandler,
    IQueryHandler<GetUserByIdQuery, ResultT<GetUserQueryDto>> getUserByIdHandler,
    IQueryHandler<GetUserFavoriteRecipesQuery, ResultT<IReadOnlyList<GetRecipeQueryDto>>> getFavoriteRecipesHandler,
    IQueryHandler<GetUserRecipesQuery, ResultT<IReadOnlyList<GetRecipeQueryDto>>> getUserRecipesHandler,
    IMapper mapper
) : BaseController
{
    [HttpPost]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Register( [FromBody] UserRegisterDto dto )
    {
        CreateUserCommand command = mapper.Map<CreateUserCommand>( dto );
        Result result = await createUserHandler.Handle( command );

        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }

    [Authorize]
    [HttpGet( "{userId:int}" )]
    [ProducesResponseType( typeof( GetUserQueryDto ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetUserById( [FromRoute] int userId )
    {
        if ( UserId != userId )
        {
            return Forbid( "Невозможно получить данные другого пользователя." );
        }

        GetUserByIdQuery query = new GetUserByIdQuery()
        {
            Id = UserId.Value
        };
        ResultT<GetUserQueryDto> result = await getUserByIdHandler.Handle( query );

        if ( result.IsSuccess )
        {
            return Ok( result.Value );
        }

        return BadRequest( result.ErrorMessages );
    }

    [Authorize]
    [HttpPut( "{userId:int}" )]
    [ProducesResponseType( StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> EditUser( [FromRoute] int userId, [FromBody] UserEditDto dto )
    {
        if ( UserId != userId )
        {
            return Forbid( "Невозможно изменить данные другого пользователя." );
        }

        UpdateUserCommand command = new()
        {
            UserId = UserId.Value,
            Name = dto.Name,
            Login = dto.Login,
            Password = dto.Password,
            Information = dto.Information
        };
        Result result = await updateUserHandler.Handle( command );

        if ( result.IsSuccess )
        {
            return Ok();
        }

        return BadRequest( result.ErrorMessages );
    }

    [HttpPost( "login" )]
    [ProducesResponseType( typeof( TokenInfoDto ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> Login( [FromBody] LoginDto dto )
    {
        LoginUserCommand command = mapper.Map<LoginUserCommand>( dto );

        ResultT<TokenInfoDto> result = await loginUserHandler.Handle( command );

        if ( result.IsSuccess )
        {
            Response.Cookies.Append( "refresh-token", result.Value.RefreshToken );

            return Ok( result.Value );
        }

        return BadRequest( result.ErrorMessages );
    }

    [Authorize]
    [HttpGet( "{userId:int}/recipes" )]
    [ProducesResponseType( typeof( IReadOnlyList<GetRecipeQueryDto> ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetUserRecipes( [FromRoute] int userId )
    {
        if ( UserId != userId )
        {
            return Forbid( "Невозможно получить рецепты другого пользователя." );
        }

        GetUserRecipesQuery query = new GetUserRecipesQuery()
        {
            UserId = UserId.Value
        };

        ResultT<IReadOnlyList<GetRecipeQueryDto>> result = await getUserRecipesHandler.Handle( query );

        if ( result.IsSuccess )
        {
            return Ok( result.Value );
        }

        return BadRequest( result.ErrorMessages );
    }

    [Authorize]
    [HttpGet( "{userId:int}/favorites" )]
    [ProducesResponseType( typeof( IReadOnlyList<GetRecipeQueryDto> ), StatusCodes.Status200OK )]
    [ProducesResponseType( typeof( IReadOnlyList<string> ), StatusCodes.Status400BadRequest )]
    public async Task<IActionResult> GetUserFavoritesRecipes( [FromRoute] int userId, [FromQuery] FavoriteRecipesDto recipesDto )
    {
        if ( UserId != userId )
        {
            return Forbid( "Невозможно получить избранные рецепты другого пользователя." );
        }

        GetUserFavoriteRecipesQuery query = mapper.Map<GetUserFavoriteRecipesQuery>( recipesDto ) with { UserId = UserId.Value };

        ResultT<IReadOnlyList<GetRecipeQueryDto>> result = await getFavoriteRecipesHandler.Handle( query );

        if ( result.IsSuccess )
        {
            return Ok( result.Value );
        }

        return BadRequest( result.ErrorMessages );
    }
}
