using System;
using System.Collections.Generic;
using System.Text;

namespace Docker.Infrastructure.DTO
{
    public class EmailSettingsDTO
    {
        public string SMTPEmail { get; set; }
        public string SMTPPassword { get; set; }
        public string SMTPPort { get; set; }
        public string SMTPHostname { get; set; }
    }
}
