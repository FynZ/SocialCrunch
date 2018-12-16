using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SocialToken
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string AccessToken { get; set; }
        public string TokenSecret { get; set; }
    }
}
