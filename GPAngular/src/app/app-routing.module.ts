import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditCaseComponent } from './case/edit-case/edit-case.component';
import { AuthGuard } from './auth2/auth.guard';

import { NewCaseComponent } from './case/new-case/new-case.component';
import { SearchComponent } from './case/search/search.component';
import { TestQueryParamsComponent } from './test-query-params/test-query-params.component';
import { AuthComponent } from './auth2/auth.component';
import { NotAuthGuard } from './auth2/not-auth.guard';
import { LoginComponent } from './auth2/login/login.component';
import { SignupComponent } from './auth2/signup/signup.component';

const appRoutes: Routes = [
  { path: '', redirectTo: '/cases', pathMatch: 'full' },
  {
    path: 'cases',
    component: TestQueryParamsComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'cases/add',
    component: NewCaseComponent,
    canActivate: [AuthGuard]
  },
  {
    path: 'login',
    component: LoginComponent,
    canActivate: [NotAuthGuard]
  },
  {
    path: 'signup',
    component: SignupComponent,
    canActivate: [NotAuthGuard]
  },

  // {
  //   path: 'auth',
  //   component: AuthComponent,
  //   canActivate: [NotAuthGuard]
  // },
  {
    path: 'cases/edit/:id',
    component: EditCaseComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
