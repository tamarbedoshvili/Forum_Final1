using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.Domain.Dto
{
    public class UpdatePostDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public string CreatorId { get; set; }
    }
}
