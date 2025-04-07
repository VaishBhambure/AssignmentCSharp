// import { Component, EventEmitter, Output } from '@angular/core';
// import { AuthService } from '../../services/user.service';
// import { RegisterRequest } from '../../models/register';
// import { FormsModule } from '@angular/forms';
// import { Router } from '@angular/router';
// @Component({
//   selector: 'app-register',
//   standalone:true,
//   templateUrl: './register.component.html',
//   styleUrls: ['./register.component.css'],
//   imports:[FormsModule]
// })
// export class RegisterComponent {
//   userName: string = '';
//   firstName: string = '';
//   lastName: string = '';
//   birthDate: string = '';
//   email: string = '';
//   phoneNumber: string = '';
//   password: string = '';
//   confirmPassword: string = '';
//   isArtist: boolean = false; // Ensure this is defined

//   constructor(private authService: AuthService,private router: Router) {}

  // onRegister() {
  //   const user: RegisterRequest = {
  //     userName: this.userName,
  //     firstName: this.firstName,
  //     lastName: this.lastName,
  //     birthDate: this.birthDate,
  //     email: this.email,
  //     phoneNumber: this.phoneNumber,
  //     password: this.password,
  //     confirmPassword: this.confirmPassword,
  //     isArtist: this.isArtist // Include isArtist in the request
  //   };

  //   this.authService.register(user).subscribe(
  //     (response) => {
  //       console.log('Registration successful:', response);
  //       alert('Registration Successful');
  //     },
  //     // (error) => {
  //     //   console.error('Registration error:', error);
  //     // }
      
      
  //   );
  // }

//   
//   redirectToLogin() {
//     this.router.navigate(['/login']);
// }
// }

import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { RegisterRequest } from '../../models/register';

@Component({
  selector: 'app-register',
  standalone: true,
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css'],
  imports: [FormsModule]
})
export class RegisterComponent {
  userName: string = '';
  firstName: string = '';
  lastName: string = '';
  birthDate: string = '';
  email: string = '';
  phoneNumber: string = '';
  password: string = '';
  confirmPassword: string = '';
  isArtist: boolean = false;

  constructor(private http: HttpClient, private router: Router) {}

  onRegister() {
    const user: RegisterRequest = {
      userName: this.userName,
      firstName: this.firstName,
      lastName: this.lastName,
      birthDate: this.birthDate,
      email: this.email,
      phoneNumber: this.phoneNumber,
      password: this.password,
      confirmPassword: this.confirmPassword,
      isArtist: this.isArtist
    };
    this.http.post<any>('https://localhost:7168/api/User/register', user).subscribe({
      next: (response) => {
        console.log('Registration successful:', response);
        alert('Registration Successful');
        
      },
      error: (error) => {
        console.error('Registration error:', error);
        if (error.status === 400 && error.error && error.error.errors) {
          const validationErrors = error.error.errors;
          for (const field in validationErrors) {
            if (validationErrors.hasOwnProperty(field)) {
              console.error(`${field}: ${validationErrors[field]}`);
            }
          }
          alert('Validation error. Check console.');
        } else {
          alert('Registration failed. Check console for details.');
        }
      }
    }); 
  }
redirectToLogin(){
 this.router.navigate(['/login']);

}
}    