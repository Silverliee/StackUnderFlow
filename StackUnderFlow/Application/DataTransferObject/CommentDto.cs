using System.ComponentModel.DataAnnotations;

namespace StackUnderFlow.Application.DataTransferObject
{
    public class CommentDto
    {
        [Required]
        public int CommentId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int ScriptId { get; set; }

        [Required]
        public string Description { get; set; }

    }
}
