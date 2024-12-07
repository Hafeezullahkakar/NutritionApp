import { Component } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { FormsModule } from '@angular/forms'; // Import FormsModule

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss'],
  standalone: true,
  imports: [FormsModule, RouterLink]
})
export class LoginComponent {
  form = {
    email: '',
    password: ''
  };

  constructor(private authService: AuthService, private router: Router) {}

  onSubmit() {
    this.authService.login(this.form).subscribe({
      next: (response: any) => {
        this.authService.saveToken(response.token);
     
        this.router.navigate(['/dashboard']);
      },
      error: (err) => alert('Login Failed: ' + err.message)
    });
  }
}
