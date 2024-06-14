using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Final.Enum;

namespace Final.Domain.Dto
{
    public class ChangePostStateDto
    {
        public int PostID { get; set; }

        public EState State { get; set; }
        public string AdminId { get; set; }
    }
}
