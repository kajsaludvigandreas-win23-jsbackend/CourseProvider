﻿
using Infrastructure.Data.Contexts;
using Infrastructure.Factories;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services;

public interface ICourseService
{
    Task<Course> CreateCourseAsync(CourseCreateRequest request);

    Task<Course> GetCourseByIdAsync(string id);

    Task<IEnumerable<Course>> GetCoursesAsync();
    Task<Course> UpdateCourseAsync(CourseUpdateRequest request);

    Task<bool> DeleteCourseAsync(string id);
}
public class CourseService(IDbContextFactory<DataContext> contextFactory) : ICourseService
{
    private readonly IDbContextFactory<DataContext> _contextFactory = contextFactory;

    public async Task<Course> CreateCourseAsync(CourseCreateRequest request)
    {
        await using var context = _contextFactory.CreateDbContext();

        var courseEntity = CourseFactory.Create(request);
        context.Courses.Add(courseEntity);
        await context.SaveChangesAsync();

        return CourseFactory.Create(courseEntity);

    }

    public async Task<bool> DeleteCourseAsync(string id)
    {
        await using var context = _contextFactory.CreateDbContext();

        var courseEntity = await context.Courses.FirstOrDefaultAsync(x => x.Id == id);
        if (courseEntity == null)
        {
            return false;
        }
        context.Courses.Remove(courseEntity);
        await context.SaveChangesAsync();

        return true;
    }

    public async Task<Course> GetCourseByIdAsync(string id)
    {
        await using var context = _contextFactory.CreateDbContext();

        var courseEntity = await context.Courses.FirstOrDefaultAsync(x => x.Id == id);

        if(courseEntity == null)
        {
            return null!;
        }
        return CourseFactory.Create(courseEntity);   
    }

    public async Task<IEnumerable<Course>> GetCoursesAsync()
    {
        await using var context = _contextFactory.CreateDbContext();

        var courseEntites = await context.Courses.ToListAsync();
        return courseEntites.Select(CourseFactory.Create);

    }

    public async Task<Course> UpdateCourseAsync(CourseUpdateRequest request)
    {
        await using var context = _contextFactory.CreateDbContext();

        var courseEntity = await context.Courses.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (courseEntity == null)
        {
            return null!;
        }
        var updatedCourseEntity = CourseFactory.Create(request);

        updatedCourseEntity.Id = courseEntity.Id;
        context.Entry(courseEntity).CurrentValues.SetValues(updatedCourseEntity);
        await context.SaveChangesAsync();
        return CourseFactory.Create(courseEntity);


    }
}
