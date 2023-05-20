namespace ChatAppBackEndV1.Common.ServiceResponseResult
{
    public class ErrorResponseResult<T> : ResponseResult<T>
    {
        public ErrorResponseResult()
        {
            IsSuccess= false;
        }
        public ErrorResponseResult(string errorMessage)
        {
            IsSuccess = false;
            Message= errorMessage;
        }
    }
}
