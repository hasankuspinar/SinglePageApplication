import { HttpClient } from '@angular/common/http';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  username: string = '';
  password: string = '';
  role: string = '';

  constructor(private httpClient: HttpClient, private router: Router) { }

  onRegister(): void {
    const registerData = {
      username: this.username,
      password: this.password,
      role: this.role
    };

    this.httpClient.post('https://localhost:7233/api/Auth/register', registerData).subscribe({
      next: (response) => {
        console.log('Registration successful', response);
        this.router.navigate(['/login']); 
      },
      error: (error) => {
        console.error('Registration failed', error);
      }
    });
  }
}

