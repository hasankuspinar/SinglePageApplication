import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrls: ['./user-page.component.css']
})
export class UserPageComponent implements OnInit {
  username: string = '';
  isAdmin: boolean = false;

  constructor(private httpClient: HttpClient,private router: Router) { }

  ngOnInit(): void {
    this.getUserInfo();
  }

  getUserInfo(): void {
    this.httpClient.get < { username: string,role: string }>('https://localhost:7233/api/Auth/userInfo', { withCredentials: true })
      .subscribe({
        next: (response) => {
          this.username = response.username;
          this.isAdmin = response.role === 'admin';
        },
        error: (error) => {
          console.error('Failed to fetch user info', error);
        }
      });
  }
  onLogout(): void {
    this.httpClient.post('https://localhost:7233/api/Auth/logout', {}, { withCredentials: true })
      .subscribe({
        next: () => {
          console.log('Logout successful');
          this.router.navigate(['/login']);
        },
        error: (error) => {
          console.error('Logout failed', error);
        }
      });
  }
  onNavigateToAdmin(): void {
    if (this.isAdmin) {
      this.router.navigate(['/admin']);
    }
    else {
      console.log("Navigation blocked: User is not admin");
    }
  }

}

