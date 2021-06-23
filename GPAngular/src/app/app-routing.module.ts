import { Component, NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { EditCaseComponent } from './case/edit-case/edit-case.component';
import { NewCaseComponent } from './case/new-case/new-case.component';
import { SearchComponent } from './case/search/search.component';
import { TestQueryParamsComponent } from './test-query-params/test-query-params.component';

const appRoutes: Routes = [
  { path: '', redirectTo: '/Login', pathMatch: 'full' },
  {
    path: 'cases',
    component: TestQueryParamsComponent
  },
  {
    path: 'cases/add',
    component: NewCaseComponent
  },
  {
    path: 'cases/edit/:id',
    component: EditCaseComponent
  }
  // ,
  // { path: 'profiles/details/:id', component: ProfileDetailsComponent, canActivate: [AuthGuard] },
  // { path: 'profiles/edit/:id', component: ProfileEditComponent, canActivate: [AuthGuard] }
];

@NgModule({
  imports: [RouterModule.forRoot(appRoutes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
