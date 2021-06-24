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
import { AutoCompleteModule } from 'primeng/autocomplete';
import { TableModule } from 'primeng/table';
import { CaseStepsComponent } from './case/case-steps/case-steps.component';
import { EditableModule } from '@ngneat/edit-in-place';
import { FieldsetModule } from 'primeng/fieldset';
import { EditCaseComponent } from './case/edit-case/edit-case.component';
import { NotifierModule } from 'angular-notifier';
import { MatChipsModule } from '@angular/material/chips';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { NzSelectModule } from 'ng-zorro-antd/select';
import { NZ_I18N } from 'ng-zorro-antd/i18n';
import { en_US } from 'ng-zorro-antd/i18n';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en'
import { MatFormFieldModule } from '@angular/material/form-field';
// import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { TagInputModule } from 'ngx-chips';
// import { TagInputDropdown } from 'ngx-chips';
import { MultiSelectModule } from 'primeng/multiselect';
import { ButtonModule } from 'primeng/button';
import { TagModule } from 'primeng/tag'
import { TabViewModule } from 'primeng/tabview';
import { FileComponent } from './case/file/file.component';
import { CaseFiles2Component } from './case/case-files2/case-files2.component'
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatProgressBarModule } from '@angular/material/progress-bar'
// import { AuthModule } from './auth/auth.module';
import { AuthComponent } from './auth2/auth.component';
import { LoadingSpinnerComponent } from './shared/loading-spinner/loading-spinner.component';
import { AuthGuard } from './auth2/auth.guard';
import { NotAuthGuard } from './auth2/not-auth.guard';
import { AuthInterceptorService } from './auth2/auth-interceptor.service';
import { MustMatchDirective } from './auth2/must.match.directive';
import { ConfirmDialogModule } from 'primeng/confirmdialog';
import { ConfirmationService } from 'primeng/api';
import { LoginComponent } from './auth2/login/login.component';
import { SignupComponent } from './auth2/signup/signup.component';
import { CaseDetailsComponent } from './case/case-details/case-details.component';


registerLocaleData(en);
@NgModule({
  declarations: [
    HeaderComponent,
    AppComponent,
    NewCaseComponent,
    CaseFilesComponent,
    SearchComponent,
    TestQueryParamsComponent,
    CaseStepsComponent,
    EditCaseComponent,
    FileComponent,
    CaseFiles2Component,
    AuthComponent,
    LoadingSpinnerComponent,
    MustMatchDirective,
    LoginComponent,
    SignupComponent,
    CaseDetailsComponent
  ],
  imports: [
    // AuthModule,
    ConfirmDialogModule,
    BrowserAnimationsModule,
    FieldsetModule,
    CarouselModule,
    EditableModule,
    TagModule,
    TableModule,
    AppRoutingModule,
    BrowserModule,
    EditorModule,
    FormsModule,
    FileUploadModule,
    HttpClientModule,
    ChipsModule,
    NotifierModule,
    AutoCompleteModule,
    NzSelectModule,
    MatChipsModule,
    MatAutocompleteModule,
    MatProgressBarModule,
    TagInputModule,
    MatIconModule,
    MatGridListModule,
    MatButtonModule,
    MultiSelectModule,
    ButtonModule,
    TabViewModule,
    FlexLayoutModule
  ],
  providers: [
    { provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' },
    { provide: NZ_I18N, useValue: en_US },
    AuthGuard,
    NotAuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true
    },
    ConfirmationService
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
