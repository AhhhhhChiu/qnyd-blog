namespace Qnyd.Data.Results
{
    public class EntityResult<T> : Result
    {
        public T Entity { get; set; }
    }
}
