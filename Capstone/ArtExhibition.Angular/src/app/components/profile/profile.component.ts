// import { Component, OnInit } from '@angular/core';
// import { HttpClient, HttpHeaders } from '@angular/common/http';
// import { CommonModule } from '@angular/common';

// @Component({
//   selector: 'app-profile',
//   standalone: true,
//   imports: [CommonModule],
//   templateUrl: './profile.component.html',
//   styleUrls: ['./profile.component.css'],
// })
// export class ProfileComponent implements OnInit {
//   userProfile: any = { favoriteArtworks: { $values: [] } };
//   artworks: any[] = []; // Store all artworks
//   backendUrl = 'https://localhost:7168/api/User/profile'; // Profile API URL
//   artworksUrl = 'https://localhost:7168/api/User/artworks'; // Fetch all artworks API
//   favoriteUrl = 'https://localhost:7168/api/User/favorite/'; // Add favorite API

//   constructor(private http: HttpClient) {}

//   ngOnInit() {
//     this.getUserProfile();
//     this.getAllArtworks();
//   }

//   /** Fetch User Profile */
//   getUserProfile() { 
//     const token = localStorage.getItem('token');
//     if (!token) {
//       alert('User not authenticated. Please log in.');
//       return;
//     }

//     const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

//     this.http.get(this.backendUrl, { headers }).subscribe({
//       next: (response) => {
//         console.log("User Profile Fetched:", response);
//         this.userProfile = response;
//       },
//       error: (error) => {
//         console.error('Error fetching user profile:', error);
//         alert('Failed to load profile. Check console for details.');
//       }
//     });
//   }
//   getAllArtworks() {
//     const token = localStorage.getItem('token');
//     if (!token) {
//       alert('User not authenticated. Please log in.');
//       return;
//     }
  
//     const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
  
//     this.http.get<any>(this.artworksUrl, { headers }).subscribe({
//       next: (response) => {
//         console.log("🔍 Full API Response:", response); 
  
//         if (response?.$values) {
//           this.artworks = response.$values;
//           console.log("🎨 Artworks array:", this.artworks);
  
//           // ✅ Log each ArtworkID
//           this.artworks.forEach((artwork, index) => {
//             console.log(`🎨 Artwork ${index} -> ArtworkID:`, artwork.ArtworkID);
//           });
//         } else {
//           console.error("❌ Unexpected API response format:", response);
//           this.artworks = [];
//         }
//       },
//       error: (error) => {
//         console.error('❌ Error fetching artworks:', error);
//         alert('Failed to load artworks.');
//       }
//     });
//   }
  
  
//   /** Fetch Specific Artwork by ID */
//   getArtworkById(artworkID: number) {
//     const token = localStorage.getItem('token');
//     if (!token) {
//       alert('User not authenticated. Please log in.');
//       return;
//     }

//     const headers = new HttpHeaders({ 'Authorization': `Bearer ${token}` });

//     this.http.get<any>(`${this.artworksUrl}/${artworkID}`, { headers }).subscribe({
//       next: (response) => {
//         console.log("Fetched Artwork:", response);
//         if (response?.artworkID) {
//           this.addToFavorites(response.artworkID);
//         } else {
//           alert('Invalid artwork received.');
//         }
//       },
//       error: (error) => {
//         console.error('Error fetching artwork:', error);
//         alert('Failed to fetch artwork details.');
//       }
//     });
//   }
//   addToFavorites(artwork: any) {
//     console.log("🖼️ Artwork Object Before API Call:", artwork);
  
//     if (!artwork || !artwork.id) {
//       console.error("❌ Invalid artwork object. Missing ID:", artwork);
//       return;
//     }
  
//     // Ensure correct API call format
//     this.http.post(`https://localhost:7168/api/User/favorite/${artwork.id}`, {})
//       .subscribe({
//         next: (response) => console.log(" Successfully added to favorites:", response),
//         error: (error) => console.error("Error adding artwork to favorites:", error)
//       });
//   }
  
  
// /** Remove from Favorites */
// removeFromFavorites(artworkId: number) {
//   const token = localStorage.getItem('token');
//   if (!token) {
//       alert('User not authenticated. Please log in.');
//       return;
//   }

//   const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

//   this.http.delete(`${this.favoriteUrl}${artworkId}`, { headers }).subscribe({
//       next: () => {
//           console.log(`✅ Removed artwork ${artworkId} from favorites`);
//           // ✅ Update UI: Remove the artwork from the local list
//           this.userProfile.favoriteArtworks.$values = this.userProfile.favoriteArtworks.$values.filter((a: { artworkID: number }) => a.artworkID !== artworkId);
//         },
//       error: (error) => {
//           console.error('❌ Error removing artwork from favorites:', error);
//           alert('Failed to remove from favorites.');
//       }
//   });
// // }
// //}
// import { Component, OnInit } from '@angular/core';
// import { HttpClient, HttpHeaders } from '@angular/common/http';
// import { CommonModule } from '@angular/common';

// @Component({
//   selector: 'app-profile',
//   standalone: true,
//   imports: [CommonModule],
//   templateUrl: './profile.component.html',
//   styleUrls: ['./profile.component.css'],
// })
// export class ProfileComponent implements OnInit {
//   userProfile: any = { favoriteArtworks: { $values: [] } };
//   artworks: any[] = [];
//   favoriteArtworkIds: Set<number> = new Set(); // ✅ Store favorite artwork IDs

//   constructor(private http: HttpClient) {}

//   ngOnInit() {
//     this.getUserProfile();
//     this.getAllArtworks();
//   }

//   /** Fetch User Profile */
//   getUserProfile() {
//     const token = localStorage.getItem('token');
//     if (!token) return alert('User not authenticated. Please log in.');

//     const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

//     this.http.get('/api/user/profile', { headers }).subscribe({
//       next: (response: any) => {
//         this.userProfile = response;
//         this.favoriteArtworkIds = new Set(response.favoriteArtworks.$values.map((art: any) => art.artworkID)); // ✅ Track favorite IDs
//       },
//       error: (error) => console.error('Error fetching profile:', error)
//     });
//   }

//   /** Fetch All Artworks */
//   getAllArtworks() {
//     const token = localStorage.getItem('token');
//     if (!token) return alert('User not authenticated. Please log in.');

//     const headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);

//     this.http.get<any>('/api/user/artworks', { headers }).subscribe({
//       next: (response) => {
//         this.artworks = response?.$values || [];
//       },
//       error: (error) => console.error('Error fetching artworks:', error)
//     });
//   }

//   /** Toggle favorite status */
//   toggleFavorite(artwork: any) {
//     if (this.favoriteArtworkIds.has(artwork.artworkID)) {
//       this.removeFromFavorites(artwork.artworkID);
//     } else {
//       this.addToFavorites(artwork);
//     }
//   }

//   /** Add Artwork to Favorites */
//   addToFavorites(artwork: any) {
//     const artworkId = artwork.artworkID;
//     if (!artworkId) return;

//     const token = localStorage.getItem('token');
//     if (!token) return;

//     this.http.post(`/api/user/favorite/${artworkId}`, {}, {
//       headers: new HttpHeaders().set('Authorization', `Bearer ${token}`)
//     }).subscribe({
//       next: () => {
//         this.favoriteArtworkIds.add(artworkId);
//       },
//       error: (error) => console.error('Error adding to favorites:', error)
//     });
//   }

//   /** Remove Artwork from Favorites */
//   removeFromFavorites(artworkId: number) {
//     const token = localStorage.getItem('token');
//     if (!token) return;

//     this.http.delete(`/api/user/favorite/${artworkId}`, {
//       headers: new HttpHeaders().set('Authorization', `Bearer ${token}`)
//     }).subscribe({
//       next: () => {
//         this.favoriteArtworkIds.delete(artworkId);
//       },
//       error: (error) => console.error('Error removing from favorites:', error)
//     });
//   }
//   viewArtworks(){

//   }
//   viewFavorites(){}
// }



import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';

interface Artwork {
  title: string;
  description: string;
  imageURL: string;
    artworkID: number; 
}

@Component({
  selector: 'app-profile',
  standalone: true,
  imports: [CommonModule], 
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent {
 
  userProfile: any = {
   
  };

  searchResults: Artwork[] = [];  // ✅ Add this
  favoriteArtworks: Artwork[] = [];  // ✅ Add this
  showFavorites: boolean = false;
  apiUrl = 'https://localhost:7168/api/User/favorite-artworks';
  private profileUrl = 'https://localhost:7168/api/User/profile';
  private getUrl = 'https://localhost:7168/api/User/favorite/';

  constructor(private http: HttpClient,private router: Router) {}
  ngOnInit(): void {
    this.fetchUserProfile();  // ✅ Ensure the function runs when the component loads

  }

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
        console.log('Fetched Profile:', data); // ✅ Debugging Log
        this.userProfile = data;
        this.favoriteArtworks = data.favoriteArtworks?.$values || []; 
      },
      (error) => {
        console.error('Error fetching profile:', error);
      }
    );
  }
  
  viewFavorites(): void {
    this.showFavorites = true;

    const token = localStorage.getItem('token');
    if (!token) {
        console.error('No token found');
        return;
    }

    const headers = new HttpHeaders({
      'Authorization': `Bearer ${token}`,
      'Accept': '*/*'
    });

    this.http.get<any>(this.apiUrl, { headers }).subscribe({
      next: (response) => {
        console.log('Favorite artworks:', response.$values);
        
        // Define the type for artwork
        const baseUrl = 'https://localhost:7168'; // Adjust this if needed
        this.favoriteArtworks = response.$values.map((artwork: { imageURL: string }) => ({
          ...artwork,
          imageURL: artwork.imageURL.startsWith('/') ? baseUrl + artwork.imageURL : artwork.imageURL
        }));

      },
      error: (err) => console.error('Error fetching favorites:', err)
    });
}


// viewArtworks(): void {
//   this.showFavorites = false;

//   const token = localStorage.getItem('token');
//   if (!token) {
//       console.error('No token found');
//       return;
//   }

//   const headers = new HttpHeaders({
//       'Authorization': `Bearer ${token}`,
//       'Accept': 'application/json'
//   });

//   this.http.get<any>('https://localhost:7168/api/User/artworks', { headers }).subscribe({
//       next: (response) => {
//           console.log('Fetched artworks:', response.$values);

//           const baseUrl = 'https://localhost:7168'; // Adjust if necessary

//           this.searchResults = response.$values.map((artwork: any) => ({
//             artworkID: artwork.artworkID, // Use artworkID instead of id
//             title: artwork.title,
//             description: artwork.description,
//             imageURL: artwork.imageURL.startsWith('/') ? baseUrl + artwork.imageURL : artwork.imageURL
//           }));
//       },
//       error: (err) => console.error('Error fetching artworks:', err)
//   });
// }
viewArtworks(): void {
  this.showFavorites = false;

  const token = localStorage.getItem('token');
  if (!token) {
    console.error('No token found');
    return;
  }

  const headers = new HttpHeaders({
    'Authorization': `Bearer ${token}`,
    'Accept': 'application/json'
  });

  this.http.get<any>('https://localhost:7168/api/User/artworks', { headers }).subscribe({
    next: (response) => {
      console.log('Fetched artworks:', response.$values);

      const baseUrl = 'https://localhost:7168'; // Adjust if necessary

      this.searchResults = response.$values.map((artwork: any) => ({
        artworkID: artwork.artworkID,// Use `$id` as artworkID if confirmed correct
        title: artwork.title,
        description: artwork.description,
        imageURL: artwork.imageURL.startsWith('/') ? baseUrl + artwork.imageURL : artwork.imageURL
      }));

      console.log('Mapped searchResults:', this.searchResults);
    },
    error: (err) => console.error('Error fetching artworks:', err)
  });
}


removeFromFavorites(artwork: Artwork): void {
  const token = localStorage.getItem('token');
  if (!token) {
    console.error('No token found');
    return;
  }

  const headers = new HttpHeaders({
    'Authorization': `Bearer ${token}`,
    'Accept': '*/*'
  });

  this.http.delete(`https://localhost:7168/api/User/favorite/${artwork.artworkID}`, 
    { headers, responseType: 'text' } // Fix: Expect text response
  ).subscribe({
    next: (response) => {
      console.log('Server Response:', response); // Log response text
      // Remove from UI
      this.favoriteArtworks = this.favoriteArtworks.filter(a => a.artworkID !== artwork.artworkID);
    },
    error: (err) => console.error('Error removing from favorites:', err)
  });
}


addToFavorites(artworkId: any): void {
  artworkId = Number(artworkId); // ✅ convert to number just in case

  console.log("✅ Sending to backend ID:", artworkId); // extra debug log

  const token = localStorage.getItem('token');
  if (!token) {
    alert("❌ You need to log in first!");
    return;
  }

  const headers = new HttpHeaders().set("Authorization", `Bearer ${token}`);

  this.http.post(`https://localhost:7168/api/User/favorite/${artworkId}`, {}, {
    headers,
    responseType: 'text'
  }).subscribe({
    next: (response: any) => alert("✅ " + response),
    error: (error) => {
      console.error("❌ Error:", error);
      if (error.status === 500 && error.error.includes("already in favorites")) {
        alert("⚠️ This artwork is already in your favorites!");
      } else {
        alert("❌ Error: " + (error.message || "Something went wrong"));
      }
    }
  });
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
