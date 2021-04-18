namespace WebApp.Dto
{
    /// <summary>
    /// возвращать вместе с кодом http 400 
    /// </summary>
    public class ErrorDto
    {
        public int ErrorCode { get; set; }
        public string ErrorText { get; set; }
    }
}