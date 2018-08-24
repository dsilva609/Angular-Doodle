import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Component } from '@angular/core';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'registration',
  templateUrl: './registration.component.html',
})

export class AccountComponent {
  passwordsMatch: boolean;
  invalidRegistration: boolean;

  constructor(private http: HttpClient) { }

  register(form: NgForm) {
    let registration = JSON.stringify(form.value);
    this.http.post("https://localhost:44388/api/account/register",
      registration,
      {
        headers: new HttpHeaders({
          "Content-Type": "application/json"
        })
      }).subscribe(response => {
        /* check model
         * succss => register, signin, redirect to home
         * failure => clear password fields, display error
         */
    }, err => { this.invalidRegistration = true; });
  }
}
