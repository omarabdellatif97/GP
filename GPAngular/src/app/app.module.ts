import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { EditorModule, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { FileUploadModule } from 'primeng/fileupload';

import { AppComponent } from './app.component';
import { NewCaseComponent } from './case/new-case/new-case.component';
import { CaseFilesComponent } from './case/case-files/case-files.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { SearchComponent } from './case/search/search.component';

import {ChipsModule} from 'primeng/chips';


@NgModule({
  declarations: [
    AppComponent,
    NewCaseComponent,
    CaseFilesComponent,
    SearchComponent
  ],
  imports: [
    BrowserModule,
    EditorModule,
    FormsModule,
    FileUploadModule,
    HttpClientModule,
    ChipsModule
  ],
  providers: [{ provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' }],
  bootstrap: [AppComponent]
})
export class AppModule { }
