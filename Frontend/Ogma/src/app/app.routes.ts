import { Routes } from '@angular/router';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { canActivateAuth } from './core/auth/guards/auth-guard';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { ItemTypesListComponent } from './features/catalog/item-types/pages/item-types-list/item-types-list.component';
import { ItemsListComponent } from './features/catalog/items/pages/items-list/items-list.component';
import { CategoriesListComponent } from './features/catalog/categories/pages/categories-list/categories-list.component';

export const routes: Routes = [
  
  {
    path: 'app',
    component: MainLayoutComponent,
    canActivate: [canActivateAuth],
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'catalog/item-types', component: ItemTypesListComponent},
      { path: 'catalog/categories', component: CategoriesListComponent},
      { path: 'catalog/items', component: ItemsListComponent},
    ]
  },

  { path: '**', redirectTo: 'app' }
];
