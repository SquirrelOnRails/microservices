namespace Mango.Services.CouponAPI.Dto
{
    public class ResponseDto
    {
        public bool IsSuccess { get; set; } = true;

        public string? Message { get; set; }

        public List<string>? ErrorMessages { get; set; }

        public object? Result { get; set; }
    }
}
