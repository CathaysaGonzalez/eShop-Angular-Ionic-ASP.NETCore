using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BE.Dal.EfStructures;
using BE.Dal.Exceptions;
using BE.Dal.Repos.Base;
using BE.Dal.Repos.Interfaces;
using BE.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace BE.Dal.Repos
{
    public class CartLineRepo : RepoBase<CartLine>, ICartLineRepo
    {
        private readonly IProductRepo _productRepo;
        public CartLineRepo(BEIdentityContext context, IProductRepo productRepo) : base(context)
        {
            _productRepo = productRepo;
        }
        internal CartLineRepo(DbContextOptions<BEIdentityContext> options) : base(options)
        {
            _productRepo = new ProductRepo(Context);
        }
        public override void Dispose()
        {
            _productRepo.Dispose();
            base.Dispose();
        }
        public CartLine GetCartLineByProduct(long productId) => Table.FirstOrDefault(x => x.ProductId == productId);
        public IEnumerable<CartLine> GetCartLinesByOrder(long orderId) => Context.CartLines.Where(c => c.OrderId == orderId);
        public override int Add(CartLine entity, bool persist = true)
        {
            var product = _productRepo.FindAsNoTracking(entity.ProductId);
            if (product == null)
            {
                throw new BEInvalidProductException("Unable to locate the product");
            }
            return Add(entity, product, persist);
        }
        public int Add(CartLine entity, Product product, bool persist = true)
        {
            var item = GetCartLineByProduct(entity.ProductId);
            if (item == null)
            {
                if (entity.Quantity > product.UnitsInStock)
                {
                    throw new BEInvalidQuantityException("Can't add more product than available in stock");
                }
                entity.LineItemTotal = entity.Quantity * product.Price;
                return base.Add(entity, persist);
            }
            item.Quantity += entity.Quantity;
            return item.Quantity <= 0 ? Delete(item, persist) : Update(item, product, persist);
        }
        public override int Update(CartLine entity, bool persist = true)
        {
            var product = _productRepo.FindAsNoTracking(entity.ProductId);
            if (product == null)
            {
                throw new BEInvalidProductException("Unable to locate product");
            }
            return Update(entity, product, persist);
        }
        public int Update(CartLine entity, Product product, bool persist = true)
        {
            if (entity.Quantity <= 0)
            {
                return Delete(entity, persist);
            }
            if (entity.Quantity > product.UnitsInStock)
            {
                throw new BEInvalidQuantityException("Can't add more product than available in stock");
            }
            var dbRecord = Find(entity.Id);
            dbRecord.Quantity = entity.Quantity;
            dbRecord.LineItemTotal = entity.Quantity * product.Price;
            return base.Update(dbRecord, persist);
        }
    }
}
