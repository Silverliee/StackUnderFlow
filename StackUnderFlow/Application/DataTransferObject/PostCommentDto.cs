using System.ComponentModel.DataAnnotations;

namespace StackUnderFlow.Application.DataTransferObject
{
    public class PostCommentDto
    {

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ScriptId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
