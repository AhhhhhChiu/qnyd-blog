namespace Qnyd.Data.Results
{
    public class EntityRangeResult<T> : EntityResult<T>
    {
        public long Total { get; set; }

        public int? Skip { get; set; }

        public int? Take { get; set; }
    }
}
