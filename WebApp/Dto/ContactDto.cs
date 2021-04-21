namespace WebApp.Dto
{
    /// <summary>
    /// публичное представление пользователя. Нужно, чтобы не палить данные с идентити
    /// </summary>
    public class ContactDto
    {
        /// <summary>
        /// никнейм/id/имя пользователя
        /// </summary>
        public string ScreenName { get; init; }
        
        /// <summary>
        /// ссылка на аватарку
        /// </summary>
        public string LinkToAvatar { get; init; }
        
        // добавить сюда время последнего входа, статус и прочее по необходимости
    }
}