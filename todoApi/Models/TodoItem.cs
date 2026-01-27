using System;

namespace todoApi;

public class TodoItem
{
    public long Id {get; set;}
    public string? Name {get; set;}
    public bool IsDone {get; set;}

    public bool IsDeleted {get; set;}
}
