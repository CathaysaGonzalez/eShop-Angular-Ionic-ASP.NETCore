import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Product } from 'src/app/models/product.model';
import { ModalController, NavController } from '@ionic/angular';

@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.scss'],
})
export class CreateOrderComponent implements OnInit {
  @Input() selectedProduct: Product;
  //To get access to the form, to get access to each element
  @ViewChild("f", {static: false}) form: NgForm;

  constructor(private modalCtrl: ModalController, private navCtrl: NavController) { }

  ngOnInit() {}

  onCancel() {
    this.modalCtrl.dismiss(null, "cancel");
  }

  onBuyProduct() {
    if (!this.form.valid) {
      return;
    }
    // this.modalCtrl.dismiss({message: 'This is a dummy message!'}, 'confirm');
    //I can name ProductData the name that I want
    //The keys have to match the name ['name']
    this.modalCtrl.dismiss(
      {
        OrderData: {
          name: this.form.value["name"],
          address: this.form.value["address"],
          cardNumber: this.form.value["cardNumber"],
          cardSecurityCode: this.form.value["cardSecurityCode"],
          cardExpiry: this.form.value["cardExpiry"]
        },
      },
      "confirm"
    );
    this.navCtrl.navigateBack("/store/tabs/orders");

  }

}
