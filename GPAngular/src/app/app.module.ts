import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

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
import { CaseStepsComponent } from './case/case-steps/case-steps.component';
import { EditableModule } from '@ngneat/edit-in-place';
import { FieldsetModule } from 'primeng/fieldset';
import { EditCaseComponent } from './case/edit-case/edit-case.component';
import { NotifierModule } from 'angular-notifier';
@NgModule({
  declarations: [
    HeaderComponent,
    AppComponent,
    NewCaseComponent,
    CaseFilesComponent,
    SearchComponent,
    TestQueryParamsComponent,
    CaseStepsComponent,
    EditCaseComponent
  ],
  imports: [
    BrowserAnimationsModule,
    FieldsetModule,
    CarouselModule,
    EditableModule,
    TableModule,
    AppRoutingModule,
    BrowserModule,
    EditorModule,
    FormsModule,
    FileUploadModule,
    HttpClientModule,
    ChipsModule,
    NotifierModule
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
