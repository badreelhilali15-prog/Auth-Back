namespace Auth_Back.DTOs.Auth.Register
{
    public class RegisterResponseDto
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; }= new List<string>();
    }
}
