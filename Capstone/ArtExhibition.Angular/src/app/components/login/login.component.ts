import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';  
import { CommonModule } from '@angular/common';
import { AuthService } from '../../services/user.service';
import { Router } from '@angular/router';  

@Component({
  selector: 'app-login',
  standalone: true,  
  imports: [CommonModule, FormsModule],  //  Add FormsModule
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent {
  email: string = '';
  password: string = '';
  errorMessage: string = ''; 
  isAuthPage = false;
  showContent: boolean = true;
  isLoggedIn = false;  

  constructor(private authService: AuthService , private router: Router ) {}
  redirectToRegister() {
    this.router.navigate(['/register']);
  }
  // onLogin() {
  //   this.authService.login({ email: this.email, password: this.password }).subscribe(
  //     (response) => {
  //       console.log('Login successful:', response);
        
  //       localStorage.setItem('token', response.token);  // ✅ Store token
        
  //       alert('Login Successful');
  //       this.router.navigate(['/profile']); 
  //     },
  //     (error) => {
  //       this.errorMessage = 'Invalid email or password';
  //       console.error('Login error:', error);
  //       alert("Login Failed, please check email or password");
  //     }
  //   );
  // }
  
  // 
  onLogin() {
    this.showContent = false;
    this.isAuthPage = false; 
  

    this.authService.login({ email: this.email, password: this.password }).subscribe(
      (response) => {
        console.log('Login successful:', response);
  
        // ✅ Store token in localStorage
        localStorage.setItem('token', response.token);
  
        // Extract roles correctly
        let rolesArray = response.roles?.$values || []; // Ensure it's an array
  
        //  Store roles as key-value pairs in localStorage
        localStorage.setItem('roles', JSON.stringify(rolesArray));
  
        // Redirect based on role
        if (rolesArray.includes('Artist')) {
          this.router.navigate(['/artist']);
        } else {
          this.router.navigate(['/profile']);
        }
  
        alert('Login Successful');
      },
      (error) => {
        this.errorMessage = 'Invalid email or password';
        console.error('Login error:', error);
        alert("Login Failed, please check email or password");
      }
    );
  }  
  
 
}
