import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { PartDetailsComponent } from './parts/part-details/part-details.component';
import { PartBrowseComponent } from './parts/part-browse/part-browse.component';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'parts', component: PartBrowseComponent },
  { path: 'parts/:partcode', component: PartDetailsComponent },
  { path: '**', component: HomeComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
