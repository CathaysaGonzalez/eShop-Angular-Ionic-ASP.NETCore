import { Rating } from "./rating.model";
import { Supplier } from "./supplier.model";
import { Category } from "./category.model";

export class CartRecord {
  constructor(
    public id?: number,
    public name?: string,
    public description?: string,
    public price?: number,
    public unitsInStock?: number,
    public modelNumber?: string,
    public modelName?: string,
    public productImage?: string,
    public productImageLarge?: string,
    public productImageThumb?: string,
    public isFeatured?: boolean,
    public currentPrice?: number,

    public supplierId?: number,
    public categoryId?: number,

    public ratings?: Rating[],
    public supplier?: Supplier,
    public categoryNavigation?: Category,

    public quantity?: number
  ) {}
}