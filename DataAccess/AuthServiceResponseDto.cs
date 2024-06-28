namespace DataAccess
{
    public class AuthServiceResponseDto
    {
        public bool IsSucceed { get; set; }
        public string Message { get; set; }

        public byte Role { get; set; }
    }
}
