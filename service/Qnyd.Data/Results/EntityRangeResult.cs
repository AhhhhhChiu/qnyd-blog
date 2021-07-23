namespace Qnyd.Data.Results
{
    public class EntityRangeResult<T> : EntityResult<T>
    {
        public long Total { get; set; }

        public long Skip { get; set; }

        public long Take { get; set; }
    }
}
