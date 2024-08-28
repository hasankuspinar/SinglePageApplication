import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  username: string = '';
  password: string = '';
  loginMessage: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  onLogin(): void {

    this.authService.login(this.username, this.password).subscribe({
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
}


