import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  loginMessage: string = '';

  constructor(private httpClient: HttpClient, private router: Router) { }

  onLogin(): void {
    const loginData = {
      username: this.username,
      password: this.password
    };

    this.httpClient.post('https://localhost:7233/api/Auth/login', loginData, { withCredentials: true }).subscribe({
      next: (response) => {
        console.log('Login successful', response);
        this.loginMessage = 'Login successful!';
        this.router.navigate(['/user']); 
      },
      error: (error) => {
        console.error('Login failed', error);
        this.loginMessage = 'Login failed. Please try again.';
      }
    });
  }
  onLogout(): void {
    this.httpClient.post('https://localhost:7233/api/Auth/logout', {}, { withCredentials: true })
      .subscribe({
        next: () => {
          console.log('Logout successful');
          this.loginMessage = 'Logout successful!';
          //this.router.navigate(['/']);
        },
        error: (error) => {
          console.error('Logout failed', error);
          this.loginMessage = 'Logout failed. Please try again.';
        }
      });
  }
}


