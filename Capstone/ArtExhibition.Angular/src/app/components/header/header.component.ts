import { Component } from '@angular/core';
import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { CommonModule } from '@angular/common'; // For ngIf and ngFor
import { RouterModule, Router } from '@angular/router'; import { FormsModule } from '@angular/forms'; // For ngModel
import { LoginComponent } from '../login/login.component';
import { Observable } from 'rxjs';
import { RegisterComponent } from '../register/register.component'; // Import RegisterComponent
import { ProfileComponent } from '../profile/profile.component';



// Define the interface for Artwork (All Artworks contains artistName)
interface ArtworkAll {
  title: string;
  description: string;
  imageURL: string; // The URL of the artwork image
  artistName: string; // Artist name for all artworks
}

// Define the interface for Search Artwork (without artistName)
interface ArtworkSearch {
  title: string;
  description: string;
  imageURL: string; 
  
}

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule,LoginComponent,RegisterComponent], 
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css'],
})
export class HeaderComponent {
  searchKeyword: string = '';          // For binding toartworks the search input
  searchResults: ArtworkSearch[] = []; // To hold search results (without artistName)
  allArtworks: ArtworkAll[] = []; 
  galleries: any[] = [];       // To hold all artworks (with artistName)
  isAuthPage = false;
  showContent: boolean = true;
  showLogin: boolean = false;
  showRegister: boolean = false;
  
  private apiUrl = 'https://localhost:7168/api/Artist/galleries';

  navigate() {
    this.router.navigateByUrl('/').then(() => {
      window.scrollTo(0, 0);  // Scrolls to top
    });
   
  }
  constructor(private http: HttpClient, private router: Router) {
    this.router.events.subscribe(() => {
      this.isAuthPage = this.router.url === '/login' || this.router.url === '/register';
    });
    
    
  }
  getAllArtworks(): void {
    this.showContent = false;
    this.isAuthPage = false; 
    this.galleries = [];  

    this.http.get<{ $values: ArtworkAll[] }>('https://localhost:7168/api/User/artworks')  // API URL for all artworks
      .subscribe(
        (data) => {
          this.allArtworks = data.$values;  // Populate allArtworks with data from the backend
          this.searchResults = [];  // Clear search results when fetching all artworks
        },
        (error: HttpErrorResponse) => {
          console.error('Error fetching all artworks:', error);  // Handle error
        }
      );
  }

  // Updated searchArtworks method
searchArtworks(): void {
  this.galleries = [];  
  this.allArtworks = [];  
 
   this.isAuthPage = false; 
  if (this.searchKeyword.trim() === '') {
    this.searchResults = [];  // Clear the search results if no keyword is entered
    return;
  }

  // Fetch search results based on the search keyword
  this.http.get<{ $values: ArtworkSearch[] }>(`https://localhost:7168/api/User/search?keyword=${this.searchKeyword}`)
    .subscribe(
      (data) => {
        // Ensure the imageURL is appended correctly with the base URL
        this.searchResults = data.$values.map(item => ({
          title: item.title,
          description: item.description,
          imageURL: 'https://localhost:7168' + item.imageURL,  // Append base URL
        }));
        this.allArtworks = [];  // Clear all artworks when performing a search
      },
      (error: HttpErrorResponse) => {
        console.error('Error searching artworks:', error);  // Handle error
      }
    );
  }
  getAllGalleries(): void {
    this.showContent = false; // Hide other content
    this.isAuthPage = false; 
    this.allArtworks = []; // Hide all artworks when displaying galleries
    this.searchResults = []; // Clear search results
  
    this.http.get<any>('https://localhost:7168/api/Artist/galleries')
      .subscribe(response => {
        this.galleries = response.$values || [];
      }, error => {
        console.error('Error fetching galleries:', error);
      });

  }
  redirectToRegister() {
    this.router.navigate(['/register']);
  }
}

