import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthComponent } from './auth/auth.component';
import { AdminComponent } from './admin/admin.component';
import { EditProductComponent } from './admin/products/edit-product/edit-product.component';
import { NewProductComponent } from './admin/products/new-product/new-product.component';
import { ProductsComponent } from './admin/products/products.component';
import { OrdersComponent } from './admin/orders/orders.component';
import { AuthAdminGuard } from './auth/auth-admin.guard';

const routes: Routes = [
  {
    path: 'auth',
    component: AuthComponent,
  },
  {
    path: 'admin',
    component: AdminComponent,
    children: [
      {
        path: 'products/edit/:id',
        component: EditProductComponent,
        canLoad: [AuthAdminGuard],
      },
      {
        path: 'products/new',
        component: NewProductComponent,
        canLoad: [AuthAdminGuard],
      },
      {
        path: 'products',
        component: ProductsComponent,
        canLoad: [AuthAdminGuard],
      },
      { path: 'orders', component: OrdersComponent, canLoad: [AuthAdminGuard] },
    ],
  },
  { path: '**', redirectTo: 'auth' },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
