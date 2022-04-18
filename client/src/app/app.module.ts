import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HomeComponent } from './home/home.component';
import { PartDetailsComponent } from './parts/part-details/part-details.component';
import { PartCardComponent } from './parts/part-card/part-card.component';
import { PartBrowseComponent } from './parts/part-browse/part-browse.component';

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    PartDetailsComponent,
    PartCardComponent,
    PartBrowseComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
