import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { StorePage } from './store.page';
import { CategoriesPage } from './categories/categories.page';
const routes: Routes = [
  {
    path: 'tabs',
    component: StorePage,
    children:[
      {
        path: 'categories',
        loadChildren: () => import('./categories/categories.module').then( m => m.CategoriesPageModule)
      },
      {
        path: 'search',
        loadChildren: () => import('./search/search.module').then( m => m.SearchPageModule)
      },
      {
        path: 'cart',
        loadChildren: () => import('./cart/cart.module').then( m => m.CartPageModule)
      },
      {
        path: 'orders',
        loadChildren: () => import('./orders/orders.module').then( m => m.OrdersPageModule)
      },
      {
        path: 'checkout',
        loadChildren: () => import('./checkout/checkout.module').then( m => m.CheckoutPageModule)
      },
      {
        path: 'products',
        loadChildren: () => import('./products/products.module').then( m => m.ProductsPageModule),
        children:[
          {
            path: ':productId',
            loadChildren: () => import('./products/detail/detail.module').then( m => m.DetailPageModule)
          },
        ]
      },
      {
        path: '',
        redirectTo: '/store/tabs/categories',
        pathMatch: 'full'
      },
    ]
  },
  {
    path: '',
    redirectTo: '/store/tabs/categories',
    pathMatch: 'full'
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class StorePageRoutingModule {}
