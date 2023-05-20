namespace ChatAppBackEndV1.Common.ServiceResponseResult
{
    public class ResponseResult<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public T Result { get; set; }
    }
}
