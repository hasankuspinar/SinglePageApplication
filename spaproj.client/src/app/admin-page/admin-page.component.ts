import { Component } from '@angular/core';
import { AuthService } from '../auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-page',
  templateUrl: './admin-page.component.html',
  styleUrls: ['./admin-page.component.css']
})
export class AdminPageComponent {
  username: string = '';
  status: number = 1; // Default to 1 (Active)
  updateMessage: string = '';

  constructor(private authService: AuthService, private router: Router) { }

  onUpdateStatus(): void {
    this.authService.updateUserStatus(this.username, this.status).subscribe({
      next: (response) => {
        console.log('Update successful', response);
        this.updateMessage = 'User status updated successfully!';
      },
      error: (error) => {
        console.error('Update failed', error);
        this.updateMessage = 'Failed to update user status. Please try again.';
      }
    });
  }
}

