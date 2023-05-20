namespace ChatAppBackEndV1.Common.ServiceResponseResult
{
    public class SuccessResponseResult<T> : ResponseResult<T> 
    {
        public SuccessResponseResult() { IsSuccess = true; }
        public SuccessResponseResult(T resObj)
        {
            IsSuccess = true;
            Result= resObj;
        }
    }
}
