using Ambev.DeveloperEvaluation.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Ambev.DeveloperEvaluation.WebApi.Common
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController : ControllerBase
    {
        protected int GetCurrentUserId() =>
            int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? throw new NullReferenceException());

        protected string GetCurrentUserEmail() =>
            User.FindFirst(ClaimTypes.Email)?.Value ?? throw new NullReferenceException();

        protected bool UserHasRole(string role) =>
            User?.IsInRole(role) ?? false;

        protected bool UserHasRole(UserRole role) =>
            User?.IsInRole(role.ToString()) ?? false;

        protected bool IsAdmin() =>
            UserHasRole(UserRole.Admin);

        protected bool IsManagerOrAdmin() =>
            UserHasRole(UserRole.Manager) || UserHasRole(UserRole.Admin);

        protected bool CanAccessUserData(string userId) =>
            GetCurrentUserId().ToString() == userId || IsManagerOrAdmin();

        protected IActionResult Ok<T>(T data) =>
            base.Ok(new ApiResponseWithData<T> { Data = data, Success = true });

        protected IActionResult Created<T>(string routeName, object routeValues, T data) =>
            base.CreatedAtRoute(routeName, routeValues, new ApiResponseWithData<T> { Data = data, Success = true });

        protected IActionResult BadRequest(string message) =>
            base.BadRequest(new ApiResponse { Message = message, Success = false });

        protected IActionResult NotFound(string message = "Resource not found") =>
            base.NotFound(new ApiResponse { Message = message, Success = false });

        protected IActionResult Forbidden(string message = "You don't have permission to access this resource") =>
            StatusCode(403, new ApiResponse { Message = message, Success = false });

        protected IActionResult OkPaginated<T>(PaginatedList<T> pagedList) =>
            Ok(new PaginatedResponse<T>
            {
                Data = pagedList,
                CurrentPage = pagedList.CurrentPage,
                TotalPages = pagedList.TotalPages,
                TotalCount = pagedList.TotalCount,
                Success = true
            });
    }
}