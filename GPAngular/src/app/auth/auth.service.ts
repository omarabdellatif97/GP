import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { map } from 'rxjs/operators';


@Injectable({
  providedIn: 'root'
})
export class AuthService {

  authUrl = "http://localhost:4200/";
  userUrl = "http://localhost:4200/user/"
  confirmEmailUrl = "test.com"
  constructor(private http: HttpClient) { }

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
  register(model: any) {

   // let options = { headers: headers };
    return this.http.post(this.userUrl + 'create', model);
  }


}
