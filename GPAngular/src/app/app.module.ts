import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { EditorModule, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { FileUploadModule } from 'primeng/fileupload';

import { AppComponent } from './app.component';
import { NewCaseComponent } from './case/new-case/new-case.component';
import { CaseFilesComponent } from './case/case-files/case-files.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { SearchComponent } from './case/search/search.component';


import { ChipsModule } from 'primeng/chips';
import { AppRoutingModule } from './app-routing.module';
import { HeaderComponent } from './header/header.component';
import { TestQueryParamsComponent } from './test-query-params/test-query-params.component';
import { CarouselModule } from 'primeng/carousel';
import { TableModule } from 'primeng/table';
import { AuthInterceptorService } from './services/auth-interceptor.service';


@NgModule({
  declarations: [
    HeaderComponent,
    AppComponent,
    NewCaseComponent,
    CaseFilesComponent,
    SearchComponent,
    TestQueryParamsComponent
  ],
  imports: [
    CarouselModule,
    TableModule,
    AppRoutingModule,
    BrowserModule,
    EditorModule,
    FormsModule,
    FileUploadModule,
    HttpClientModule,
    ChipsModule
  ],
  providers: [
    { provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' }
    // ,
    // {
    //   provide: HTTP_INTERCEPTORS,
    //   useClass: AuthInterceptorService,
    //   multi: true
    // }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
