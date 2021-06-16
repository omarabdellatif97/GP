import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { NewCaseComponent } from './case/new-case/new-case.component';
import { SearchComponent } from './case/search/search.component';
import { TestQueryParamsComponent } from './test-query-params/test-query-params.component';

const appRoutes: Routes = [
  { path: '', redirectTo: '/cases', pathMatch: 'full' },
  {
    path: 'cases',
    component: TestQueryParamsComponent
  },
  {
    path: 'cases/add',
    component: NewCaseComponent
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
