using System;
using System.Collections.Generic;
using BE.Dal.Repos.Base;
using BE.Models.Entities;


namespace BE.Dal.Repos.Interfaces
{
    public interface ICartLineRepo : IRepo<CartLine>
    {
        public IEnumerable<CartLine> GetCartLinesByOrder(long orderId);
    }
}
