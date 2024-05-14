using System.Collections.Generic;

namespace Application.BaseResult
{
    public class BaseResult<T>
    {
        public T Data { get; set; }
        public bool Success { get; set; }
        public BaseResult()
        {
        }
    }
}
