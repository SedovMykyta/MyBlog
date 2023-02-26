using MyBlog.Service.Exception;

namespace MyBlog.Service.Helpers.ExtensionMethods;

public static class CollectionExtensions
{
    public static IQueryable<T> ThrowIfEmpty<T>(this IQueryable<T> list)
    {
        if (!list.Any())
        {
            throw new NotFoundException("This collection is empty");
        }

        return list;
    }
}