import { Routes } from '@angular/router';
import { MainLayoutComponent } from './layout/main-layout/main-layout.component';
import { canActivateAuth } from './core/auth/guards/auth-guard';
import { DashboardComponent } from './features/dashboard/dashboard.component';
import { ItemTypesListComponent } from './features/catalog/components/item-types-list/item-types-list.component';
import { CategoriesListComponent } from './features/catalog/components/categories-list/categories-list.component';

export const routes: Routes = [
  
  {
    path: 'app',
    component: MainLayoutComponent,
    canActivate: [canActivateAuth],
    children: [
      { path: 'dashboard', component: DashboardComponent },
      { path: 'catalog/item-types', component: ItemTypesListComponent},
      { path: 'catalog/categories', component: CategoriesListComponent}
    ]
  },

  { path: '**', redirectTo: 'app' }
];
