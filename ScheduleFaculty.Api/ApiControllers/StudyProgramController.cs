using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScheduleFaculty.Api.DTOs;
using ScheduleFaculty.Core.Services.Abstractions;

namespace ScheduleFaculty.Api.ApiControllers;

[ApiController]
[Route("/api/studyProgram")]
public class StudyProgramController : ControllerBase
{
    private readonly IStudyProgramRepository _studyProgramRepository;
    private readonly IMapper _mapper;

    public StudyProgramController(IStudyProgramRepository studyProgramRepository, IMapper mapper)
    {
        _studyProgramRepository = studyProgramRepository;
        _mapper = mapper;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetStudyProgramById([FromRoute] Guid id)
    {
        var studyProgram = await _studyProgramRepository.GetStudyProgram(id);
        if (studyProgram.HasErrors())
        {
            return BadRequest(studyProgram.Errors);
        }

        var response = _mapper.Map<StudyProgramDto>(studyProgram.Item);
        return Ok(response);
    }

    [HttpGet("getAll")]
    public async Task<ActionResult> GetAllStudyProgram()
    {
        var studyPrograms = await _studyProgramRepository.GetAllStudyProgramS();
        var response = _mapper.Map<List<StudyProgramDto>>(studyPrograms.Item);

        return Ok(response);
    }

    [HttpPost]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> CreateStudyProgram([FromBody] StudyProgramDto studyProgramDto)
    {
        var studyProgram = await _studyProgramRepository.CreateStudyProgram(studyProgramDto.Name, studyProgramDto.Year,
            studyProgramDto.WeeksInASemester,studyProgramDto.NumberOfSemiGroups,studyProgramDto.HowManySemiGroupsAreInAGroup);
        if (studyProgram.HasErrors())
        {
            return BadRequest(studyProgram.Errors);
        }

        var response = _mapper.Map<StudyProgramDto>(studyProgram.Item);
        return Ok(response);
    }

    [HttpPatch("edit")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> EditStudyProgram([FromBody] StudyProgramDto studyProgramDto)
    {
        var studyProgram = await _studyProgramRepository.EditStudyProgram(studyProgramDto.Id, studyProgramDto.Name,
            studyProgramDto.Year, studyProgramDto.WeeksInASemester,studyProgramDto.NumberOfSemiGroups,studyProgramDto.HowManySemiGroupsAreInAGroup);
        if (studyProgram.HasErrors())
        {
            return BadRequest(studyProgram.Errors);
        }

        var response = _mapper.Map<StudyProgramDto>(studyProgram.Item);
        return Ok(response);
    }

    [HttpDelete("delete/{id}")]
    [Authorize(Roles = "Secretary",
        AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<ActionResult> DeleteStudyProgram([FromRoute] Guid id)
    {
        var studyProgram = await _studyProgramRepository.DeleteStudyProgram(id);
        if (studyProgram.HasErrors())
        {
            return BadRequest(studyProgram.Errors);
        }

        return Ok();
    }
}