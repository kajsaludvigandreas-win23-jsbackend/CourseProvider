using Infrastructure.Models;
using Infrastructure.Services;

namespace Infrastructure.GraphQL.Query;

public class Query(ICourseService courseService)
{
    private readonly ICourseService _courseService = courseService;

    [GraphQLName("getAllCourses")]
    public async Task<IEnumerable<Course>> GetAllCoursesAsync()
    {
        return await _courseService.GetCoursesAsync();

    }


    [GraphQLName("getCourseById")]
    public async Task<Course> GetCourseByIdAsync(string id)
    {

        return await _courseService.GetCourseByIdAsync(id);
    }

}
