namespace ApiProvaOnline.Application.DTOs.Responses
{
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public object Usuario { get; set; } = default!;
    }
}
