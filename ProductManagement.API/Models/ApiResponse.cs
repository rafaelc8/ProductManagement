namespace ProductManagement.API.Models
{
    public class ApiResponse<T>
    {
        public bool Succeeded { get; set; }
        public T? Data { get; set; }

        public static ApiResponse<T> Success(T data)
        {
            return new ApiResponse<T>
            {
                Succeeded = true,
                Data = data
            };
        }
    }
}
