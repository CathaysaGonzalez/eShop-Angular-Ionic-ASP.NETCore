import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription, BehaviorSubject } from 'rxjs';
import { ProductsService } from 'src/app/services/products.service';
import { ModalController, AlertController, IonItemSliding, LoadingController } from '@ionic/angular';
import { Router } from '@angular/router';
import { Product } from 'src/app/models/product.model';

@Component({
  selector: 'app-search',
  templateUrl: './search.page.html',
  styleUrls: ['./search.page.scss'],
})
export class SearchPage implements OnInit, OnDestroy {
  products: Product[];
  selectedProducts: Product[];
  private productsSub: Subscription;
  isLoading = false;
  result: string;
  cartResult: string;

  constructor(
    private productsService: ProductsService,
    private loadingCtrl: LoadingController,
    private router: Router,
  ) { }

  ngOnInit() {
    this.productsSub = this.productsService.products.subscribe((products) => {
      this.products = products;
      this.selectedProducts = products;
    });
  }

  ionViewWillEnter() {
    this.isLoading = true;
    this.productsService.fetchProducts().subscribe(() => {
      this.isLoading = false;
    });
  }

  onSearchChange(search) {
    let searchValue = search.detail.value;
    if (searchValue == "") {
      this.selectedProducts = this.products;
    }
    else{
    this.productsService.searchProductsByName(searchValue).subscribe(
      (res) => {
        this.selectedProducts = res;
      },
      (err) => {
        this.selectedProducts = [];
      }
    );
    }
  }

  onEditProduct(id: number, slidingEl: IonItemSliding) {
    slidingEl.close();
    this.router.navigate(['/', 'admin', 'tabs','products', 'edit', id]);
  }  

  onCancelProduct(id: number, slidingEl: IonItemSliding) {
    slidingEl.close();
    this.loadingCtrl
      .create({ message: "Cancelling..." })
      .then((loadingEl) => {
        loadingEl.present();
         this.productsService
          .cancelProduct(id)
          .subscribe(() => {
            loadingEl.dismiss();
          });
      });
  }

  ngOnDestroy() {
    if (this.productsSub) {
      this.productsSub.unsubscribe();
    }
  }

}
