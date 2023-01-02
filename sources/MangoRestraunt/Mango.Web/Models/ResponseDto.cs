namespace Mango.Web.Models
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
