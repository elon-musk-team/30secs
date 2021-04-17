namespace WebApp.Models
{
    /// <summary>
    /// таблица-связка между юзером и его контактом
    /// </summary>
    public class UserToContact
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        /// <summary>
        /// пользователь, чьи контакты отображаются
        /// </summary>
        public ApplicationUser User { get; set; }

        public string ContactId { get; set; }

        /// <summary>
        /// его контакт
        /// </summary>
        public ApplicationUser Contact { get; set; }
    }
}