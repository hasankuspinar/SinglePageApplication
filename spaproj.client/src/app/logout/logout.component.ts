import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';

@Component({
  selector: 'app-logout',
  templateUrl: './logout.component.html',
  styleUrls: ['./logout.component.css']
})
export class LogoutComponent implements OnInit {

  constructor(private httpClient: HttpClient, private router: Router, private authService: AuthService) { }

  ngOnInit(): void {
    this.performLogout();
  }

  performLogout(): void {
    this.authService.logout()
      .subscribe({
        next: () => {
          console.log('Logout successful');
          setTimeout(() => {
            this.router.navigate(['/login']);
          }, 3000);  
        },
        error: (error) => {
          console.error('Logout failed', error);
          this.router.navigate(['/login']);  
        }
      });
  }
}

