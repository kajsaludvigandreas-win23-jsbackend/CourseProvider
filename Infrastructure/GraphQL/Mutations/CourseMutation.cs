
using Infrastructure.Services;

namespace Infrastructure.GraphQL.Mutations;

public class CourseMutation(ICourseService courseService)
{
    private readonly ICourseService _courseService = courseService;
}
