import { Component, OnInit, OnDestroy } from "@angular/core";
import { ProductsService } from "src/app/services/products.service";
import { Router } from "@angular/router";
import { LoadingController } from "@ionic/angular";
import { FormGroup, FormControl, Validators } from "@angular/forms";
import { Subscription } from 'rxjs';
import { SupplierService } from 'src/app/services/supplier.service';
import { CategoriesService } from 'src/app/services/categories.service';
import { Category } from 'src/app/models/category.model';
import { Supplier } from 'src/app/models/supplier.model';

@Component({
  selector: "app-new-product",
  templateUrl: "./new-product.page.html",
  styleUrls: ["./new-product.page.scss"],
})
export class NewProductPage implements OnInit, OnDestroy {
  private suppliersSub: Subscription;
  private categoriesSub: Subscription;
  form: FormGroup;
  categories: Category[];
  suppliers: Supplier[];
  isLoading = false;

  constructor(
    private productsService: ProductsService,
    private router: Router,
    private loadingCtrl: LoadingController,
    private supplierService: SupplierService,
    private categoriesService: CategoriesService
   ) {}

  ngOnInit() {
    this.form = new FormGroup({
      name: new FormControl(null, {
        updateOn: "blur",
        validators: [Validators.required],
      }),
      description: new FormControl(null, {
        updateOn: "blur",
        validators: [Validators.required, Validators.maxLength(400)],
      }),
      price: new FormControl(null, {
        updateOn: "blur",
        validators: [Validators.required, Validators.min(0)],
      }),
      supplier: new FormControl(null, {
        updateOn: "blur",
        validators: [Validators.required],
      }),
      category: new FormControl(null, {
        updateOn: "blur",
        validators: [Validators.required],
      }),
      unitsInStock: new FormControl(null, {
        updateOn: "blur",
        validators: [Validators.required, Validators.min(0)],
      }),
      modelNumber: new FormControl(null, {
        updateOn: "blur",
        validators: [Validators.required],
      }),
      modelName: new FormControl(null, {
        updateOn: "blur",
        validators: [Validators.required],
      }),
      currentPrice: new FormControl(null, {
        updateOn: "blur",
        validators: [Validators.required , Validators.min(0)],
      }),
      isFeatured: new FormControl(false),
      image: new FormControl(null, {
        updateOn: "blur",
      }),
    });
    this.suppliersSub = this.supplierService.suppliers.subscribe((suppliers) => {
      this.suppliers = suppliers;
    });
    this.categoriesSub = this.categoriesService.categories.subscribe(
      (categories) => {
        this.categories = categories;
      }
    );
  }

  ionViewWillEnter() {
    this.isLoading = true;
    this.supplierService.fetchSuppliers().subscribe(() => {
      this.isLoading = false;
    });
    this.categoriesService.fetchCategories().subscribe(() => {
      this.isLoading = false;
    });
  }

  onImagePicked(imageData:  File) {
    let imageFile;
    imageFile = imageData;
    this.form.patchValue({ image: imageFile });
  }

  onCreateProduct() {
    if (!this.form.valid || !this.form.get('image').value) {
      return;
    }
    this.loadingCtrl
      .create({
        message: "Creando el producto...",
      })
      .then((loadingEl) => {
        loadingEl.present();
        this.productsService
          .addProduct(
            this.form.value.name,
            this.form.value.description,
            +this.form.value.price,
            this.form.value.isFeatured.checked,
            +this.form.value.unitsInStock,
            this.form.value.modelNumber,
            this.form.value.modelName,
            +this.form.value.currentPrice,
            ('assets/'+ this.form.value.image.name),
            ('assets/'+ this.form.value.image.name),
            ('assets/'+ this.form.value.image.name),
           +(this.form.value.supplier + 1),
           +(this.form.value.category + 1)
          )
          .subscribe(() => {
            loadingEl.dismiss();
            this.form.reset();
            this.router.navigate(["/admin/tabs/products"]);
          });
      });
  }

  ngOnDestroy() {
    if (this.suppliersSub) {
      this.suppliersSub.unsubscribe();
    }
    if (this.categoriesSub) {
      this.categoriesSub.unsubscribe();
    }
  }
}
