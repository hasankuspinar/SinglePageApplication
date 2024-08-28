import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { AuthService } from '../auth/auth.service';
interface Account {
  accountNumber: string;
  balance: number;
}
@Component({
  selector: 'app-user-page',
  templateUrl: './user-page.component.html',
  styleUrls: ['./user-page.component.css']
})
export class UserPageComponent implements OnInit {
  username: string = '';
  isAdmin: boolean = false;
  accounts: Account[] = [];

  constructor(private httpClient: HttpClient, private router: Router, private authService: AuthService) { }

  ngOnInit(): void {
    this.getUserInfo();
    this.getAccounts();
  }

  getUserInfo(): void {
    this.authService.userInfo()
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
  getAccounts(): void {
    this.authService.getAccounts() 
      .subscribe({
        next: (accounts) => {
          this.accounts = accounts;
        },
        error: (error) => {
          console.error('Failed to fetch accounts', error);
        }
      });
  }
  onLogout(): void {
    this.authService.logout()
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

