import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-register',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './register.component.html',
  styleUrl: './register.component.css'
})
export class RegisterComponent {
  name = '';
  email = '';
  password = '';
  error = '';
  loading = false;

  constructor(
    private authService: AuthService,
    private router: Router
  ) { }

  register() {
    this.error = '';

    // 1️⃣ Empty field validation
    if (!this.name || !this.email || !this.password) {
      this.error = 'Please enter name, email and password';
      return;
    }

    // 2️⃣ Password length validation
    if (this.password.length < 6) {
      this.error = 'Password must be at least 6 characters';
      return;
    }

    // 3️⃣ Proceed with registration
    this.loading = true;

    this.authService.register({
      name: this.name,
      email: this.email,
      password: this.password
    }).subscribe({
      next: () => {
        this.router.navigate(['/login']);
      },
      error: () => {
        this.error = 'Registration failed';
        this.loading = false;
      }
    });
  }

}
