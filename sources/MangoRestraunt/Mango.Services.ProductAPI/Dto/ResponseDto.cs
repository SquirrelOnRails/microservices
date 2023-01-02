namespace Mango.Services.ProductAPI.Dto
{
    public class ResponseDto<T> : ResponseDto
    {
        public T? Result { get; set; }
    }

    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;

        public string? Message { get; set; }

        public List<string>? ErrorMessages { get; set; }
    }
}
