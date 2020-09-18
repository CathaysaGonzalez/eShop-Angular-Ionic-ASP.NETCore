import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { DetailProductPage } from './detail-product.page';

const routes: Routes = [
  {
    path: '',
    component: DetailProductPage
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class DetailProductPageRoutingModule {}
