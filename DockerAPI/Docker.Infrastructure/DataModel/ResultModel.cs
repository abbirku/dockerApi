namespace Docker.Infrastructure.DataModel
{
    public class ResultModel<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
