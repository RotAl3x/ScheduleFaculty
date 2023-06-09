﻿//using ScheduleFaculty.Core.Services;
//using ScheduleFaculty.Core.Services.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using ScheduleFaculty.Core.Entities;
using ScheduleFaculty.Core.Services;
using ScheduleFaculty.Core.Services.Abstractions;
using ScheduleFaculty.Core.Utils;


namespace ScheduleFaculty.Core;

public static class CoreSpecifications
{
    public static IServiceCollection AddCoreSpecifications(this IServiceCollection services)
    {
        services.AddScoped<IClassroomRepository, ClassroomRepository>();
        services.AddScoped<IStudyProgramRepository, StudyProgramRepository>();
        services.AddScoped<IIdentityService, IdentityService>();
        services.AddScoped<IStudyYearGroupRepository, StudyYearGroupRepository>();
        services.AddScoped<IStatusRepository, StatusRepository>();
        services.AddScoped<IHourTypeRepository, HourTypeRepository>();
        services.AddScoped<ICourseRepository, CourseRepository>();
        services.AddScoped<ICourseHourTypeRepository, CourseHourTypeRepository>();
        services.AddScoped<IHourStudyOfAYearRepository, HourStudyOfAYearRepository>();
        services.AddScoped<ICheckAvailabilityService, CheckAvailabilityService>();
        services.AddScoped<IGroupsOfAStudyHourRepository, GroupsOfAStudyHourRepository>();
        services.AddScoped<IAssignedCourseUserRepository, AssignedCourseUserRepository>();

        return services;
    }
}