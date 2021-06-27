import { NgModule } from '@angular/core';
import { registerLocaleData } from '@angular/common';
import en from '@angular/common/locales/en'
import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { EditorModule, TINYMCE_SCRIPT_SRC } from '@tinymce/tinymce-angular';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

import { AppComponent } from './app.component';
import { NewCaseComponent } from './case/new-case/new-case.component';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SearchComponent } from './case/search/search.component';


import { AppRoutingModule } from './app-routing.module';
import { HeaderComponent } from './header/header.component';
import { TestQueryParamsComponent, DialogConfimationComponent } from './test-query-params/test-query-params.component';
import { CaseStepsComponent } from './case/case-steps/case-steps.component';
import { EditableModule } from '@ngneat/edit-in-place';
import { EditCaseComponent } from './case/edit-case/edit-case.component';
// import { NotifierModule } from 'angular-notifier';
import { MatChipsModule } from '@angular/material/chips';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatIconModule } from '@angular/material/icon';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatButtonModule } from '@angular/material/button';
import { TagInputModule } from 'ngx-chips';
import { FileComponent } from './case/file/file.component';
import { CaseFiles2Component } from './case/case-files2/case-files2.component'
import { FlexLayoutModule } from '@angular/flex-layout';
import { MatProgressBarModule } from '@angular/material/progress-bar'
import { AuthComponent } from './auth2/auth.component';
import { LoadingSpinnerComponent } from './shared/loading-spinner/loading-spinner.component';
import { AuthGuard } from './auth2/auth.guard';
import { NotAuthGuard } from './auth2/not-auth.guard';
import { AuthInterceptorService } from './auth2/auth-interceptor.service';
import { MustMatchDirective } from './auth2/must.match.directive';
import { MatToolbarModule } from '@angular/material/toolbar';
import { LoginComponent } from './auth2/login/login.component';
import { SignupComponent } from './auth2/signup/signup.component';
import { CaseDetailsComponent } from './case/case-details/case-details.component';
import { MatInputModule } from '@angular/material/input';
import { CaseTagsComponent } from './case/case-tags/case-tags.component';
import { MatSelectModule } from '@angular/material/select'
import { MatTableModule } from '@angular/material/table';
import { MatDialogModule } from '@angular/material/dialog';
import { MatSnackBarModule } from '@angular/material/snack-bar';



registerLocaleData(en);
@NgModule({
  declarations: [
    DialogConfimationComponent,
    HeaderComponent,
    AppComponent,
    NewCaseComponent,
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
    CaseDetailsComponent,
    CaseTagsComponent
  ],
  imports: [
    MatToolbarModule,
    MatDialogModule,
    MatTableModule,
    ReactiveFormsModule,
    MatInputModule,
    BrowserAnimationsModule,
    EditableModule,
    AppRoutingModule,
    BrowserModule,
    EditorModule,
    MatSelectModule,
    FormsModule,
    HttpClientModule,
    // NotifierModule,
    MatChipsModule,
    MatAutocompleteModule,
    MatProgressBarModule,
    TagInputModule,
    MatIconModule,
    MatProgressSpinnerModule,
    MatGridListModule,
    MatButtonModule,
    FlexLayoutModule,
    MatSnackBarModule,
  ],
  providers: [
    { provide: TINYMCE_SCRIPT_SRC, useValue: 'tinymce/tinymce.min.js' },
    AuthGuard,
    NotAuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
