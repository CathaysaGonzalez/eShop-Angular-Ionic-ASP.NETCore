import { Product } from './product.model';

export class Rating {
  constructor(
    public id?: number,
    public stars?: number,
    public product?: Product
  ) {}
}
