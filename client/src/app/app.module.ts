import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms'

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './home/home.component';
import { PartDetailsComponent } from './parts/part-details/part-details.component';
import { PartCardComponent } from './parts/part-card/part-card.component';
import { PartBrowseComponent } from './parts/part-browse/part-browse.component';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { SupplySourceComponent } from './Parts/supply-source/supply-source.component';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TypeaheadModule } from 'ngx-bootstrap/typeahead';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NewSourceModalComponent } from './modals/new-source-modal/new-source-modal.component';
import { ProductsListComponent } from './products/products-list/products-list.component';
import { ProductDetailsComponent } from './products/product-details/product-details.component';
import { BomDetailsComponent } from './products/bom-details/bom-details.component';
import { BomEntryComponent } from './products/bom-details/bom-entry/bom-entry.component';
import { NewBomEntryComponent } from './products/bom-details/new-bom-entry/new-bom-entry.component';
import { RequisitionsComponent } from './requisitions/requisitions.component';
import { LoginModalComponent } from './modals/login-modal/login-modal.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { ToastrModule } from 'ngx-toastr';
import { ResponseCatchInterceptor } from './_interceptors/response-catch.interceptor';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    PartDetailsComponent,
    PartCardComponent,
    PartBrowseComponent,
    SupplySourceComponent,
    NewSourceModalComponent,
    ProductsListComponent,
    ProductDetailsComponent,
    BomDetailsComponent,
    BomEntryComponent,
    NewBomEntryComponent,
    RequisitionsComponent,
    LoginModalComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    PaginationModule.forRoot(),
    BsDropdownModule.forRoot(),
    TypeaheadModule.forRoot(),
    ModalModule.forRoot(),
    ToastrModule.forRoot({
      timeOut: 4000,
      positionClass: 'toast-bottom-right',
      preventDuplicates: true,
    })
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    },
    {
      provide: HTTP_INTERCEPTORS,
      useClass: ResponseCatchInterceptor,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
