using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using StackUnderFlow.Domains.Model;

namespace StackUnderFlow.Application.DataTransferObject.Request;

public class ScriptUploadRequestDto
{
    [Required]
    public string ScriptName { get; set; }
    [Required]
    public string Description { get; set; }
    [Required]
    public string InputScriptType { get; set; }
    [Required]
    public string OutputScriptType { get; set; }
    [Required]
    public string ProgrammingLanguage { get; set; }
    [Required]
    public string Visibility { get; set; }
    [Required]
    public int UserId { get; set; }
}