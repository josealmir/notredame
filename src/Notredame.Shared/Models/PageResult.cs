namespace Notredame.Shared.Models;

public sealed class PageResult<T> where T: class
{
    public PageResult(IEnumerable<T> data,
        int totalData,
        int current,
        int size)
    {
        Data = data ?? [];
        Page = new Page(totalData, current, size);
    }
    
    public Page Page { get; set; }
    public IEnumerable<T> Data { get; set; }
}     

