import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { Router } from '@angular/router';
import { NotifierService } from 'angular-notifier';
import { Observable } from 'rxjs';
import { AuthResponseData, AuthService } from '../auth.service';

@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css']
})
export class SignupComponent implements OnInit {


  isLoading = false;

  constructor(private authService: AuthService, private router: Router, private notifier: NotifierService) { }
  ngOnInit(): void { }



  onSubmit(form: NgForm) {
    if (!form.valid) {
      this.notifier.notify('warning', 'All fields are required')
      return;
    }

    const email = form.value.email;
    const userName = form.value.username;
    const password = form.value.password;
    const confirmPassword = form.value.confirmPassword;

    if (password != confirmPassword) {
      this.notifier.notify('warning', "Passwords Don't Match")
      return;
    }

    let authObs: Observable<AuthResponseData>;

    this.isLoading = true;
    authObs = this.authService.signup(email, userName, password, confirmPassword);


    authObs.subscribe(
      resData => {
        this.isLoading = false;
        this.notifier.notify('success', "Successfully Registered Please Login")
      },
      errorMessage => {
        this.notifier.notify('error', errorMessage ? errorMessage : "Failed to register")
        this.isLoading = false;
      }
    );

    // form.reset();
  }

}
