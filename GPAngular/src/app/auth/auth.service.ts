import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { Router } from '@angular/router';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  authUrl = "http://localhost:4200/";
  userUrl = "http://localhost:4200/user/"
  confirmEmailUrl = "test.com"
  constructor(private http: HttpClient,private _router:Router) { }

  login(model: any) {
    return this.http.post(this.authUrl + 'login', model).pipe(
      map((response: any) => {
        const user = response;
        if (user.result.succeeded) {
          localStorage.setItem('token', user.token);
        }
      })
    )
  }

  loggedin(){
    if(localStorage.getItem('token'))return true
    else return null;
  }

  getToken(){
    return localStorage.getItem('token')
  }

  logOut(){
    localStorage.removeItem('token');
    this._router.navigate(['/cases'])
  }

  register(model: any) {

   // let options = { headers: headers };
    return this.http.post(this.userUrl + 'create', model);
  }


}
