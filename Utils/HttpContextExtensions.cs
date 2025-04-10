using Microsoft.EntityFrameworkCore;

namespace BuffBuddyAPI;

public static class HttpContextExtensions
{
    public async static Task InsertPageInHeader<T>(this HttpContext httpContext,
     IQueryable<T> queryable)
    {
        if (httpContext is null)
        {
            throw new ArgumentNullException(nameof(httpContext));
        }

        double count = await queryable.CountAsync();
        httpContext.Response.Headers.Append("total-count", count.ToString());
    }

}
