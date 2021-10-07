using System.Collections.Generic;

namespace Compass.Security.Application.Commons.Dtos
{
    public class EmailDto
    {
        public SenderDto Sender { get; set; }
        
        public List<ToDto> To { get; set; }

        public string Subject { get; set; }

        public string TextContent { get; set; }

        public string HtmlContent { get; set; }
    }
    
    public class SenderDto
    {
        public int Id { get; set; }
    }

    public class ToDto
    {
        public string Email { get; set; }

        public string Name { get; set; }
    }
}