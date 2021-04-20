namespace WebApp.Constants
{
    /// <summary>
    /// то что приходит в errorCode
    /// </summary>
    public enum ApiErrors
    {
        /// <summary>
        /// нет юзера с такими данными
        /// </summary>
        NoSuchUser = 1,
        
        /// <summary>
        /// действие уже выполнили и еще раз нельзя
        /// </summary>
        Already = 2,
    }
}