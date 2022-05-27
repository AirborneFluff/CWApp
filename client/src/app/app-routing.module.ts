import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PartDetailsComponent } from './parts/part-details/part-details.component';
import { PartBrowseComponent } from './parts/part-browse/part-browse.component';
import { ProductsListComponent } from './products/products-list/products-list.component';
import { ProductDetailsComponent } from './products/product-details/product-details.component';
import { BomDetailsComponent } from './products/bom-details/bom-details.component';
import { RequisitionsComponent } from './requisitions/requisitions.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'parts', component: PartBrowseComponent },
  { path: 'parts/:partcode', component: PartDetailsComponent },
  { path: 'products', component: ProductsListComponent },
  { path: 'products/:id', component: ProductDetailsComponent },
  { path: 'products/:id/boms/:bomid', component: BomDetailsComponent },
  { path: 'requisitions', component: RequisitionsComponent },
  { path: '**', component: HomeComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
