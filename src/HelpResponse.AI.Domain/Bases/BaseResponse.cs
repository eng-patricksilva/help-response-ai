using System;

namespace HelpResponse.AI.Domain.Bases
{
    public class BaseResponse<T>
    {
        public T Data { get; init; }

        public static BaseResponse<T> Create(T data)
        {
            ArgumentNullException.ThrowIfNull(data);
            return new BaseResponse<T>
            {
                Data = data
            };
        }
    }
}