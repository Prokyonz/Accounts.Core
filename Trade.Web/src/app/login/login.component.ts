import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { SharedService } from '../common/shared.service';
import { AuthService } from '../auth.service';

export class RememberLogin {
  username: string;
  password: string;
  rememberme: string;
}

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})

export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  isLoggedIn = false;
  showCompanySelection = false;
  loading = false;
  rememberMe: boolean = false;
  RememberLogin: RememberLogin = new RememberLogin;
  constructor(private fb: FormBuilder, private router: Router, private shared: SharedService, private authService: AuthService) {
    this.isLoggedIn = false;
    this.loginForm = fb.group({
      username: ['', Validators.required],
      password: ['', Validators.required],
      rememberMe: [null, null]
    });
  }

  ngOnInit(): void {
    var data = localStorage.getItem('loginremember');
    if (data != null) {
      var loginData = JSON.parse(data);
      if (loginData.rememberme == "true") {
        this.loginForm.get("username")?.setValue(loginData.username);
        this.loginForm.get("password")?.setValue(loginData.password);
        this.rememberMe = true
      }
      else {
        this.rememberMe = false
      }
    }
  }

  onLogin() {
    if (this.loginForm.valid) {
      this.loading = true;
      this.authService.login(this.loginForm.get("username")?.value, this.loginForm.get("password")?.value)
        .subscribe((response: any) => {
          // if (response.success == true){
          if (response != null) {
            localStorage.setItem('AuthorizeData', JSON.stringify(response));
            localStorage.setItem("userid", response.id);
            if (this.rememberMe) {
              this.RememberLogin.username = this.loginForm.get("username")?.value;
              this.RememberLogin.password = this.loginForm.get("password")?.value;
              this.RememberLogin.rememberme = this.rememberMe.toString();
              localStorage.setItem("loginremember", JSON.stringify(this.RememberLogin))
            }
            else {
              localStorage.removeItem("loginremember")
            }
            this.loading = false;
            this.showCompanySelection = true;
            this.router.navigate(['/dashboard']);
            //   }
          }
          else {
            localStorage.removeItem("userid");
          }
        }, (ex: any) => {
          this.loading = false;
          localStorage.removeItem("loginremember")
          alert('Invalid Username/Password.');
        });
    }
  }
}
