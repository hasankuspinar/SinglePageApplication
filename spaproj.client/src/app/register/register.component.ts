import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent {
  username: string = '';
  password: string = '';
  role: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  onRegister(): void {

    this.authService.register(this.username, this.password, this.role).subscribe({
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

