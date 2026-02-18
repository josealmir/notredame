namespace Notredame.Shared.Models;

public class Page
{
    public Page(int totalData, 
        int current,
        int size)
    {
        Current = current;
        Size = size;
        Next = Current + 1;
        Previous = Current - 1;
        TotalData = totalData;
        TotalPage = (int)Math.Ceiling(TotalData / (double)Size);
    }
    
    // EF Core
    protected Page() 
    { }
    
    public int Previous { get; set; }
    public int Current { get; set; } = 1;
    public int Next { get; set; }
    public int Size { get; set; } = 10;
    public int TotalPage { get; init; } 
    public int TotalData { get; set; }
    
}