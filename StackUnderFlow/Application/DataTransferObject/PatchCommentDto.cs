using System.ComponentModel.DataAnnotations;

namespace StackUnderFlow.Application.DataTransferObject
{
    public class PatchCommentDto
    {
        [Required]
        public int CommentId { get; set; }

        public int UserId { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
