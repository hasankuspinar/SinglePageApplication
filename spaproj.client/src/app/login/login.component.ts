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

  constructor(private httpClient: HttpClient, private router: Router) { }

  onLogin(): void {
    const loginData = {
      username: this.username,
      password: this.password
    };

    this.httpClient.post('https://localhost:7233/api/Auth/login', loginData).subscribe({
      next: (response) => {
        console.log('Login successful', response);
        this.router.navigate(['/']); 
      },
      error: (error) => {
        console.error('Login failed', error);
      }
    });
  }
}

