using System.Security.Claims;
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
    public class ScriptController(IScriptService scriptService) : ControllerBase
    {
        #region Script

        [HttpGet("{scriptId:int}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetScriptById(int scriptId)
        {
            try
            {
                var script = await scriptService.GetScriptById(scriptId);
                if (script == null)
                {
                    return NotFound();
                }

                return Ok(script);
            }
            catch
            {
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
                var scriptVersions = await scriptService.GetScriptVersionsByScriptId(scriptId);
                return Ok(scriptVersions);
            }
            catch
            {
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
            catch
            {
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
                var result = await scriptService.UpdateScript(scriptUpdateRequestDto);
                return Ok(result);
            }
            catch
            {
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
                await scriptService.DeleteScriptAndVersions(scriptId);
                return NoContent();
            }
            catch
            {
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
                var userId = Convert.ToInt32(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
                var scripts = await scriptService.GetScriptsByUserId(userId);
                return Ok(scripts);
            }
            catch
            {
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
            catch
            {
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
            catch
            {
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
            catch
            {
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
                await scriptService.DeleteScriptVersionById(scriptVersionId);
                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
        #region Search

        [HttpGet("search/{keyword}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetScriptsByKeyWord(string keyword)
        {
            try
            {
                var scripts = await scriptService.GetScriptsByKeyWord(keyword);
                return Ok(scripts);
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        #endregion
    }
}
