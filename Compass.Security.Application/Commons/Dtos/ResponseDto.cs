namespace Compass.Security.Application.Commons.Dtos
{
    public static class ResponseDto
    {
        public static ResponseDto<T> Fail<T>(string message, T data = default) => new ResponseDto<T>(data, message, false);
        public static ResponseDto<T> Ok<T>(string message, T data = default) => new ResponseDto<T>(data, message?? "Success", true);
        public static ResponseDto<T> Ok<T>(T data = default) => new ResponseDto<T>(data, "Success", true);
    }
    
    public class ResponseDto<T>
    {
        public ResponseDto(T data, string message, bool success)
        {
            Message = message;
            Data = data;
            Success = success;
        }
        
        public bool Success { get; set; }
        
        public T Data { get; set; }
        
        public string Message { get; set; }
    }
}