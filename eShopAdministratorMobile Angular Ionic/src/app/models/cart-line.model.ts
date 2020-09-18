import { Product } from './product.model';

export class CartLine {
    constructor(
        public productId: number, 
        public quantity: number,
        public productNavigation: Product
        ) {}
  }