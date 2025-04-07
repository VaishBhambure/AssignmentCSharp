import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';


@Component({
  selector: 'app-gallery',
  standalone:true,
  imports:[CommonModule,FormsModule,ReactiveFormsModule],
  templateUrl: './gallery.component.html',
  styleUrls: ['./gallery.component.css']
})
export class GalleryComponent implements OnInit {
  galleryForm: FormGroup;
  artworks: any[] = []; // Will store artworks fetched from API
  selectedArtworks: number[] = [];
   isArtist: boolean = false; 

  constructor(private fb: FormBuilder) {
    this.galleryForm = this.fb.group({
      name: '',
      description: '',
      location: '',
      selectedArtworks: [[]]
    });
  }

  ngOnInit() {
  
    
      this.fetchArtworks();
    
  }
  

  // async fetchArtworks() {
  //   const token = localStorage.getItem('token');
    
  //   if (!token) {
  //     console.error('No token found, user not authenticated!');
  //     return;
  //   }
  
  //   try {
  //     const response = await fetch('https://localhost:7168/api/Artist/my-artworks', {
  //       method: 'GET',
  //       headers: {
  //         'Authorization': `Bearer ${token}`,
  //         'Accept': 'application/json'
  //       }
  //     });
  
  //     if (response.ok) {
  //       const data = await response.json();
  //       this.artworks = data.$values;
  //       console.log('Fetched Artworks:', this.artworks);
  //     } else {
  //       console.error('Failed to fetch artworks, Status:', response.status);
  //     }
  //   } catch (error) {
  //     console.error('Error fetching artworks:', error);
  //   }
  // }
  
  async fetchArtworks() {
    const token = localStorage.getItem('token');
  
    if (!token) {
      console.error('No token found, user not authenticated!');
      return;
    }
  
    try {
      const response = await fetch('https://localhost:7168/api/Artist/my-artworks', {
        method: 'GET',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Accept': 'application/json'
        }
      });
  
      if (response.ok) {
        const data = await response.json();
  
        // Fix image URLs by prepending the API base URL
        const baseURL = 'https://localhost:7168'; // Ensure this matches your API
        this.artworks = data.$values.map((artwork: any) => ({
          ...artwork,
          imageURL: `${baseURL}${artwork.imageURL}` // Prepend base URL
        }));
  
        console.log('Updated Artworks:', this.artworks);
      } else {
        console.error('Failed to fetch artworks, Status:', response.status);
      }
    } catch (error) {
      console.error('Error fetching artworks:', error);
    }
  }
  
  onArtworkSelect(id: number, event: any) {
    if (event.target.checked) {
      this.selectedArtworks.push(id);
    } else {
      this.selectedArtworks = this.selectedArtworks.filter(artId => artId !== id);
    }
    this.galleryForm.patchValue({ selectedArtworks: this.selectedArtworks });
  }

  // createGallery() {
  //   const galleryData = this.galleryForm.value;
  //   console.log('Gallery Data:', galleryData);

  //   // Here you can send the galleryData to your API if needed
  // }
  

  async createGallery() {
    
    const galleryData = {
      name: this.galleryForm.value.name.trim(),
      description: this.galleryForm.value.description.trim(),
      location: this.galleryForm.value.location.trim(),
      artworkIds: this.selectedArtworks.filter(id => id) // Ensure no empty values
    };
  
    console.log('Gallery Data before sending:', galleryData);
  
    // Validate input before sending request
    if (!galleryData.name || !galleryData.description || galleryData.artworkIds.length === 0) {
      console.error('Validation failed: Missing required fields or no artworks selected.');
      return;
    }
  
    try {
      const token = localStorage.getItem('token');
      if (!token) {
        console.error('No token found, user not authenticated!');
        return;
      }
  
      const response = await fetch('https://localhost:7168/api/Artist/create-gallery', {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`,
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(galleryData),
      });
  
      if (response.ok) {
        const result = await response.json();
        console.log('Gallery created successfully:', result);
        alert('Gallery created successfully');
      } else {
        console.error('Failed to create gallery. Status:', response.status);
        const errorResponse = await response.json();
        console.error('Error details:', errorResponse);
      }
    } catch (error) {
      console.error('Error creating gallery:', error);
    }
  }
  
}
