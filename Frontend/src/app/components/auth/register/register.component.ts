import { Component } from '@angular/core';
import { AuthService } from '../../../services/auth.service';

import { FormsModule } from '@angular/forms'; // Import FormsModule
import { MatFormFieldModule } from '@angular/material/form-field'; // Import MatFormFieldModule
import { MatOptionModule } from '@angular/material/core';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss'],
  standalone: true,
  imports: [FormsModule, RouterLink]
})
export class RegisterComponent {
  form = {
    name: '',
    email: '',
    password: '',
    confirmPassword: '', // Added confirmPassword
    weight: null,
    height: null,
    age: null,
    goal: ''
  };

  constructor(private authService: AuthService, private route: Router) {}

  onSubmit() {
    // Basic password confirmation check
    if (this.form.password !== this.form.confirmPassword) {
      alert('Passwords do not match!');
      return;
    }

    // Call AuthService to register the user
    this.authService.register(this.form).subscribe({
      next: () => this.route.navigate(['/login']),
      error: (err) => alert('Error: ' + err.message)
    });
  }
}
