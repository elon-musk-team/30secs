namespace WebApp.Dto
{
    /// <summary>
    /// получатель сообщения
    /// </summary>
    public class SymbolReceiverDto
    {
        /// <summary>
        /// имя получателя
        /// </summary>
        public string ScreenName { get; set; }
        
        /// <summary>
        /// true - личное сообщение, false - беседа
        /// </summary>
        public bool IsPrivate { get; set; }
    }
}