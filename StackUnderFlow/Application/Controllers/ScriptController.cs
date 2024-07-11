using System.Security.Claims;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using StackUnderFlow.Application.DataTransferObject.Request;
using StackUnderFlow.Domains.Services;

namespace StackUnderFlow.Application.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [EnableCors("AllowAll")]
    public class ScriptController(IScriptService scriptService,
        Bugsnag.IClient bugsnag) : ControllerBase
    {
        #region Script

        [HttpGet()]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetScripts([FromQuery] int offset = 0, [FromQuery] int records = 5, [FromQuery] string visibility = "Public")
        {
            BackgroundJob.Enqueue(() => Console.WriteLine("getting scripts with filters"));
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                var scripts = await scriptService.GetScripts(offset, records, visibility,userId);
                return Ok(scripts);
            }
            catch(Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpGet("{scriptId:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetScriptById(int scriptId)
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                BackgroundJob.Enqueue(() => Console.WriteLine("getting script by id"));
                var script = await scriptService.GetScriptById(scriptId, userId);
                if (script == null)
                {
                    return NotFound();
                }

                return Ok(script);
            }
            catch(Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("{scriptId:int}/versions")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetScriptVersionsByScriptId(int scriptId)
        {
            try
            {
                BackgroundJob.Enqueue(() => Console.WriteLine("getting script versions by script id"));
                var scriptVersions = await scriptService.GetScriptVersionsByScriptId(scriptId);
                return Ok(scriptVersions);
            }
            catch(Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddScript(ScriptUploadRequestDto? scriptUploadRequestDto)
        {
            if (scriptUploadRequestDto == null)
            {
                return BadRequest();
            }

            try
            {
                BackgroundJob.Enqueue(() => Console.WriteLine("adding script"));
                var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                scriptUploadRequestDto.UserId = userId;
                var response = await scriptService.AddScript(scriptUploadRequestDto);
                if (response == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return CreatedAtAction(
                    nameof(GetScriptById),
                    new { scriptId = response.ScriptId },
                    response
                );
            }
            catch(Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateScript(
            ScriptUpdateRequestDto? scriptUpdateRequestDto
        )
        {
            if (scriptUpdateRequestDto == null)
            {
                return BadRequest();
            }

            try
            {
                BackgroundJob.Enqueue(() => Console.WriteLine("updating script"));
                var result = await scriptService.UpdateScript(scriptUpdateRequestDto);
                return Ok(result);
            }
            catch(Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("{scriptId:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteScript(int scriptId)
        {
            try
            {
                BackgroundJob.Enqueue(() => Console.WriteLine("deleting script"));
                await scriptService.DeleteScriptAndVersions(scriptId);
                return NoContent();
            }
            catch(Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
        #region User

        [HttpGet("user")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetScriptsByUserId()
        {
            try
            {
                BackgroundJob.Enqueue(() => Console.WriteLine("getting scripts by user id"));
                var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var scripts = await scriptService.GetScriptsByUserId(userId,userId);
                return Ok(scripts);
            }
            catch(Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }
        
        [HttpPost("byVisibility")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOtherUsersScriptsByIdAndVisibility(ScriptRequestForOtherUserDto scriptRequest)
        {
            try
            {
                var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var scripts = await scriptService.GetScriptsByUserIdAndVisibility(userId,scriptRequest);
                return Ok(scripts);
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion"
        #region File

        [HttpGet("{scriptId:int}/file")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetScriptFileByScriptId(int scriptId)
        {
            try
            {
                BackgroundJob.Enqueue(() => Console.WriteLine("getting script file by script id"));
                var file = await scriptService.GetScriptFileByScriptId(scriptId);
                if (file == null)
                {
                    return NotFound();
                }

                HttpContext.Response.Headers.Append(
                    "Content-Disposition",
                    "attachment; filename=" + file.FileName
                );
                return File(file.File, "application/octet-stream");
            }
            catch(Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
        #region Version

        [HttpGet("version/{scriptVersionId:int}/file")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetScriptVersionFileById(int scriptVersionId)
        {
            try
            {
                BackgroundJob.Enqueue(() => Console.WriteLine("getting script version file by id"));
                var file = await scriptService.GetScriptVersionFileById(scriptVersionId);
                if (file == null)
                {
                    return NotFound();
                }

                HttpContext.Response.Headers.Append(
                    "Content-Disposition",
                    "attachment; filename=" + file.FileName
                );
                return File(file.File, "application/octet-stream");
            }
            catch(Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpPost("version")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddScriptVersion(
            ScriptVersionUploadRequestDto? scriptVersionUploadRequestDto
        )
        {
            if (scriptVersionUploadRequestDto == null)
            {
                return BadRequest();
            }
            BackgroundJob.Enqueue(() => Console.WriteLine("adding script version"));
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            scriptVersionUploadRequestDto.CreatorUserId = userId;
            try
            {
                var response = await scriptService.AddScriptVersion(scriptVersionUploadRequestDto);
                if (response == null)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }

                return CreatedAtAction(
                    nameof(GetScriptVersionFileById),
                    new { scriptVersionId = response.ScriptVersionId },
                    response
                );
            }
            catch(Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        [HttpDelete("version/{scriptVersionId:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteScriptVersionById(int scriptVersionId)
        {
            try
            {
                BackgroundJob.Enqueue(() => Console.WriteLine("deleting script version"));
                await scriptService.DeleteScriptVersionById(scriptVersionId);
                return NoContent();
            }
            catch(Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
        #region Search

        [HttpGet("search/{keyword}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetScriptsByKeyWord(string keyword, [FromQuery] int offset = 0, [FromQuery] int records = 5, [FromQuery] string visibility = "Public")
        {
            var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            try
            {
                BackgroundJob.Enqueue(() => Console.WriteLine("getting scripts by keyword"));
                var scripts = await scriptService.GetScriptsByKeyWord(keyword, userId, offset,records,visibility);
                return Ok(scripts);
            }
            catch(Exception e)
            {
                bugsnag.Notify(e);
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}
