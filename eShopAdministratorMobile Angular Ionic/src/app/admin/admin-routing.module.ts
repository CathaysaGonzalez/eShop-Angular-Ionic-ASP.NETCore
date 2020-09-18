import { NgModule } from "@angular/core";
import { Routes, RouterModule } from "@angular/router";

import { AdminPage } from "./admin.page";
import { AuthAdminGuard } from '../auth/auth-admin.guard';

const routes: Routes = [
  {
    path: "tabs",
    component: AdminPage,
    children: [
      {
        path: "products",
        children: [
          {
            path: "",
            loadChildren: () =>
              import("./products/products.module").then(
                (m) => m.ProductsPageModule
              ),
              canLoad: [AuthAdminGuard]
          },
          {
            path: "new",
            loadChildren: () =>
              import("./products/new-product/new-product.module").then(
                (m) => m.NewProductPageModule
              ),
              canLoad: [AuthAdminGuard]
          },
          {
            path: "edit/:productId",
            loadChildren: () =>
              import("./products/edit-product/edit-product.module").then(
                (m) => m.EditProductPageModule
              ),
              canLoad: [AuthAdminGuard]
          },
          {
            path: ":productId",
            loadChildren: () =>
              import("./products/detail-product/detail-product.module").then(
                (m) => m.DetailProductPageModule
              ),
              canLoad: [AuthAdminGuard]
          },
        ],
      },
      {
        path: "search",
        loadChildren: () => import("./search/search.module").then((m) => m.SearchPageModule),
          canLoad: [AuthAdminGuard]
      },
      {
        path: "orders",
        children: [
          {
            path: "",
            loadChildren: () => import("./orders/orders.module").then((m) => m.OrdersPageModule),
            canLoad: [AuthAdminGuard]
          },
          {
            path: ":orderId",
            loadChildren: () => import("./orders/detail-order/detail-order.module").then((m) => m.DetailOrderPageModule),
            canLoad: [AuthAdminGuard]
          },
        ],
      },
      {
        path: "categories",
        loadChildren: () => import("./categories/categories.module").then((m) => m.CategoriesPageModule),
          canLoad: [AuthAdminGuard]
      },
      {
        path: "users",
        children: [
          {
            path: "",
            loadChildren: () => import("./users/users.module").then((m) => m.UsersPageModule),
            canLoad: [AuthAdminGuard]
          },
          {
            path: "new",
            loadChildren: () => import("./users/new-user/new-user.module").then((m) => m.NewUserPageModule),
            canLoad: [AuthAdminGuard]
          },
        ]
      },
      {
        path: "",
        redirectTo: "/admin/tabs/products",
        pathMatch: "full",
      },
    ],
  },
  {
    path: "",
    redirectTo: "/admin/tabs/products",
    pathMatch: "full",
  },
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class AdminPageRoutingModule {}
