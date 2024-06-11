using System.ComponentModel.DataAnnotations;

namespace StackUnderFlow.Domains.Model;

public class Status
{
    [Key]
    public int StatusId { get; set; }
    public string Label { get; set; }
}
