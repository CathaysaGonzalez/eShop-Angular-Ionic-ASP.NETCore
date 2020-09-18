using System;
using System.Collections.Generic;
using System.Text;

namespace BE.Models.Entities.Base
{
    public class RatingBase : EntityBase
    {
        public int Stars { get; set; }
        public long ProductId { get; set; }
    }
}
