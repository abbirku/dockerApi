namespace Docker.Core
{
    public class ResultModel<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
