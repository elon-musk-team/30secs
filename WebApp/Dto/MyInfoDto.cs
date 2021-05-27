using System.Collections.Generic;

namespace WebApp.Dto
{
    /// <summary>
    /// для запроса my-info
    /// </summary>
    public class MyInfoDto : ContactDto
    {
        public List<ContactDto> MyContacts { get; init; }
    }
}