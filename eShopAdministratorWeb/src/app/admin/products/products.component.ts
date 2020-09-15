import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { Product } from 'src/app/models/product.model';
import { ProductsService } from 'src/app/services/products.service';

@Component({
  selector: 'app-products',
  templateUrl: './products.component.html',
  styleUrls: ['./products.component.css']
})
export class ProductsComponent implements OnInit, OnDestroy {
  isLoading = false;
  products: Product[];
  private productsSub: Subscription;

  constructor(private productsService: ProductsService) { }

  ngOnInit(): void {
    this.productsSub = this.productsService.products.subscribe((products) => {
      this.products = products;
    });
  }

  ngAfterViewInit() {
    this.isLoading = true;
    this.productsService.fetchProducts().subscribe((products) => {
      this.products=products;
      this.isLoading = false;
    });
  }

  onCancelProduct(id: number) {
    this.productsService.cancelProduct(id).subscribe(() => {
    });
  }
  ngOnDestroy() {
    if (this.productsSub) {
      this.productsSub.unsubscribe();
    }
  }

}
