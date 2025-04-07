// artwork-upload.component.ts
import { Component } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

// import { decode as jwt_decode } from 'jwt-decode';
// import jwt_decode from 'jwt-decode';
// import * as jwt_decode from 'jwt-decode';
import { jwtDecode } from 'jwt-decode';
import { Router } from '@angular/router';



@Component({
  selector: 'app-artwork-upload',
  standalone:true,
  imports: [CommonModule, FormsModule],
  templateUrl: './artist.component.html',
  styleUrls: ['./artist.component.css'],
})
export class ArtworkUploadComponent {
  title = '';
  description = '';
  image: File | null = null;
  message = '';
  userProfile: any = {
   
  };
  showUploadForm: boolean = false; 
 

  constructor(private http: HttpClient,private router: Router) {}
  ngOnInit(): void {
    this.fetchUserProfile(); 
    // this.verifyUserRole(); // 

  }
  private profileUrl = 'https://localhost:7168/api/User/profile';

  
  fetchUserProfile(): void {
    const token = localStorage.getItem('token');

    if (!token) {
      console.error('No token found');
      return;
    }

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
      'Content-Type': 'application/json'
    });

    this.http.get<any>(this.profileUrl, { headers }).subscribe(
      (data) => {
        console.log('Fetched Profile:', data); 
        this.userProfile = data;
         
      },
      (error) => {
        console.error('Error fetching profile:', error);
      }
    );
  }toggleUploadForm(): void {
    this.showUploadForm = !this.showUploadForm;
  }

  onFileSelected(event: any): void {
    this.image = event.target.files[0];
  }

  // upload(): void {
  //   if (this.title && this.description && this.image) {
  //     const formData = new FormData();
  //     formData.append('Title', this.title);
  //     formData.append('Description', this.description);
  //     formData.append('Image', this.image);
  
  //     // Get token from localStorage
  //     const token = localStorage.getItem('token');
  
  //     if (!token) {
  //       this.message = 'Authentication error: No token found. Please log in again.';
  //       console.error('No token found in localStorage');
  //       return;
  //     }
  
  //     console.log('Using token:', token); // Debugging: Check if token exists
  
  //     const headers = new HttpHeaders({
  //       'Authorization': `Bearer ${token}`
  //     });
  
  //     this.http.post('https://localhost:7168/api/Artist/upload-artwork', formData, { headers })
  //       .subscribe(
  //         response => {
  //           this.message = 'Upload successful!';
  //           console.log(response);
  //         },
  //         error => {
  //           console.error('Upload failed:', error);
  //           if (error.status === 401) {
  //             this.message = 'Unauthorized: Please log in again.';
  //           } else {
  //             this.message = 'Upload failed. Try again.';
  //           }
  //         }
  //       );
  //   } else {
  //     this.message = 'Please fill in all fields';
  //   }
  // }

  // verifyUserRole(): void {
  //   const role = localStorage.getItem('roles');  // 👈 assuming you're storing it as 'role'
  
  //   if (!role) {
  //     console.error('No role found in localStorage');
  //     this.showUploadForm = false;
  //     this.message = 'Access denied: Role not found. Please log in again.';
  //     return;
  //   }
  
  //   if (role === 'Artist') {
  //     this.showUploadForm = true;
  //   } else {
  //     this.showUploadForm = false;
  //     this.message = 'Access denied: Only artists can upload artworks.';
  //   }
  // }
  upload(): void {
    if (this.title && this.description && this.image) {
      const formData = new FormData();
      formData.append('Title', this.title);
      formData.append('Description', this.description);
      formData.append('Image', this.image);
  
      const token = localStorage.getItem('token');
  
      if (!token) {
        this.message = 'Authentication error: No token found. Please log in again.';
        console.error('No token found in localStorage');
        return;
      }
  
      const headers = new HttpHeaders({
        'Authorization': `Bearer ${token}`
      });
  
      this.http.post('https://localhost:7168/api/Artist/upload-artwork', formData, { headers })
        .subscribe(
          response => {
            this.message = 'Upload successful!';
            console.log(response);
  
            // Reset form fields
            this.title = '';
            this.description = '';
            this.image = null;
  
            //  Reset file input element
            const fileInput = document.getElementById('fileInput') as HTMLInputElement;
            if (fileInput) fileInput.value = '';
          },
          error => {
            console.error('Upload failed:', error);
            if (error.status === 401) {
              this.message = 'Unauthorized: Please log in again.';
            } else {
              this.message = 'Upload failed. Try again.';
            }
          }
        );
    } else {
      this.message = 'Please fill in all fields';
    }
  }
  
  goToCreateGallery() {
    this.router.navigate(['/gallery']); // Adjust the route based on your routing setup
  }
  isLoggedIn(): boolean {
    return !!localStorage.getItem('token'); // Check if token exists
  }
  
  // Logout function
  logout() {
    localStorage.removeItem('token'); // Remove token from storage
    this.router.navigate(['/login']); // Redirect to login page
  }
}