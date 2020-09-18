import { Component, OnInit, OnDestroy } from "@angular/core";
import { ProductsService } from "src/app/services/products.service";
import { ActivatedRoute, Router } from "@angular/router";
import {
  NavController,
  LoadingController,
  AlertController,
} from "@ionic/angular";
import { FormGroup, FormControl, Validators, Form } from "@angular/forms";
import { Product } from "src/app/models/product.model";
import { Subscription } from "rxjs";
import { SupplierService } from "src/app/services/supplier.service";
import { CategoriesService } from "src/app/services/categories.service";
import { Category } from "src/app/models/category.model";
import { Supplier } from "src/app/models/supplier.model";

@Component({
  selector: "app-edit-product",
  templateUrl: "./edit-product.page.html",
  styleUrls: ["./edit-product.page.scss"],
})
export class EditProductPage implements OnInit, OnDestroy {
  form: FormGroup;
  product: Product;
  private productsSub: Subscription;
  productId: number;
  isLoading = false;
  categories: Category[];
  suppliers: Supplier[];
  private suppliersSub: Subscription;
  private categoriesSub: Subscription;

  constructor(
    private route: ActivatedRoute,
    private productsService: ProductsService,
    private navCtrl: NavController,
    private router: Router,
    private loadingCtrl: LoadingController,
    private alertCtrl: AlertController,
    private supplierService: SupplierService,
    private categoriesService: CategoriesService
  ) {}

  ngOnInit() {
    this.route.paramMap.subscribe((paramMap) => {
      if (!paramMap.has("productId")) {
        this.navCtrl.navigateBack("/admin/tabs/products");
        return;
      }
      this.suppliersSub = this.supplierService.suppliers.subscribe(
        (suppliers) => {
          this.suppliers = suppliers;
        }
      );
      this.categoriesSub = this.categoriesService.categories.subscribe(
        (categories) => {
          this.categories = categories;
        }
      );
      this.productId = +paramMap.get("productId");
      this.isLoading = true;
      this.productsSub = this.productsService
        .getProduct(+paramMap.get("productId"))
        .subscribe(
          (product) => {
            this.product = product;
            this.form = new FormGroup({
              name: new FormControl(this.product.name, {
                updateOn: "blur",
                validators: [Validators.required],
              }),
              description: new FormControl(this.product.description, {
                updateOn: "blur",
                validators: [Validators.required, Validators.maxLength(400)],
              }),
              price: new FormControl(this.product.price, {
                updateOn: "blur",
                validators: [Validators.required, Validators.min(1)],
              }),
              supplier: new FormControl(this.product.supplierId - 1, {
                updateOn: "blur",
              }),
              category: new FormControl(this.product.categoryId - 1, {
                updateOn: "blur",
              }),
              unitsInStock: new FormControl(this.product.unitsInStock, {
                updateOn: "blur",
                validators: [Validators.required, Validators.min(0)],
              }),
              modelNumber: new FormControl(this.product.modelNumber, {
                updateOn: "blur",
                validators: [Validators.required],
              }),
              modelName: new FormControl(this.product.modelName, {
                updateOn: "blur",
                validators: [Validators.required],
              }),
              currentPrice: new FormControl(this.product.currentPrice, {
                updateOn: "blur",
                validators: [Validators.required, Validators.min(0)],
              }),
              isFeatured: new FormControl(this.product.isFeatured),
              image: new FormControl(this.product.productImage, {
                updateOn: "blur",
                validators: [Validators.required],
              }),
            });
            this.isLoading = false;
          },
          (error) => {
            this.alertCtrl
              .create({
                header: "¡Ha ocurrido un error!",
                message:
                  "El producto no puede ser mostrado. Inténtalo de nuevo más tarde.",
                buttons: [
                  {
                    text: "Okay",
                    handler: () => {
                      this.router.navigate(["/admin/tabs/products"]);
                    },
                  },
                ],
              })
              .then((alertEl) => {
                alertEl.present();
              });
          }
        );
    });
  }

  ionViewWillEnter() {
    this.isLoading = true;
    this.supplierService.fetchSuppliers().subscribe(() => {
      this.categoriesService.fetchCategories().subscribe(() => {
        this.isLoading = false;
      });
    });
  }

  onImagePicked(imageData: File) {
    let imageFile;
    imageFile = imageData;
    this.form.patchValue({ image: imageFile });
  }

  onUpdateProduct() {
    if (!this.form.valid) {
      return;
    }
    this.loadingCtrl
      .create({
        message: "Actualizando producto...",
      })
      .then((loadingEl) => {
        loadingEl.present();
        this.productsService
          .updateProduct(
            this.product.id,
            this.form.value.name,
            this.form.value.description,
            this.form.value.price,
            +(this.form.value.supplier + 1),
            +(this.form.value.category + 1),
            this.form.value.unitsInStock,
            this.form.value.modelNumber,
            this.form.value.modelName,
            this.form.value.currentPrice,
            this.form.value.isFeatured,
            ('assets/'+ this.form.value.image.name)
          )
          .subscribe(() => {
            loadingEl.dismiss();
            this.form.reset();
            this.router.navigate(["/admin/tabs/products"]);
          });
      });
  }

  ngOnDestroy() {
    if (this.productsSub) {
      this.productsSub.unsubscribe();
    }
    if (this.suppliersSub) {
      this.suppliersSub.unsubscribe();
    }
    if (this.categoriesSub) {
      this.categoriesSub.unsubscribe();
    }
  }
}
