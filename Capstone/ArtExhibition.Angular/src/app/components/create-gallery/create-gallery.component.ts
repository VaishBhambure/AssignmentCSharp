import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient ,HttpHeaders} from '@angular/common/http';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';


@Component({
  selector: 'app-create-gallery',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './create-gallery.component.html',
  styleUrls: ['./create-gallery.component.css']
})
export class CreateGalleryComponent implements OnInit {
  galleryForm: FormGroup;
  artworks: any[] = [];
  // successMessage=string;

  constructor(
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
    this.galleryForm = this.fb.group({
      name: ['', Validators.required],
      description: ['', Validators.required],
      location: ['', Validators.required],
      artworkIds: [[]]
    });
  }

  // ngOnInit(): void {
  //   this.fetchArtworks();
  //   console.log('Artworks:', this.artworks);
  // }
  ngOnInit() {
    this.http.get<any[]>('http://localhost:5000/api/artworks/my-artworks')
      .subscribe(data => {
        this.artworks = data;
        console.log("Artworks fetched:", this.artworks);
      }, error => {
        console.error("Error fetching artworks", error);
      });
  }

  fetchArtworks() {
    const token = localStorage.getItem('token');
    if (!token) {
      console.error('No token found, user not authenticated!');
      return;
    }
  
    this.http.get<any>('https://localhost:7168/api/Artist/my-artworks', {
      headers: { 'Authorization': `Bearer ${token}` }
    }).subscribe(
      response => {
        console.log('Raw API Response:', response); 
      },
      error => {
        console.error('Error fetching artworks', error);
      }
    );
  }
  

  

  onCheckboxChange(event: any, id: number) {
    const selectedArtworks = this.galleryForm.get('artworkIds')?.value;
    if (event.target.checked) {
      selectedArtworks.push(id);
    } else {
      const index = selectedArtworks.indexOf(id);
      if (index > -1) {
        selectedArtworks.splice(index, 1);
      }
    }
    this.galleryForm.patchValue({ artworkIds: selectedArtworks });
  }

  submitForm() {
    if (this.galleryForm.invalid) return;
  
    const token = localStorage.getItem('token');
  
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': `Bearer ${token}`
    });
  
    this.http.post('https://localhost:7168/api/Artist/create-gallery', this.galleryForm.value, { headers })
      .subscribe({
        next: (res) => {
         // this.successMessage = "Gallery created successfully!"; // Set success message
         // setTimeout(() => this.successMessage = "", 3000); // Hide after 3 seconds
        },
        error: (err) => {
          console.error('Error creating gallery', err);
        }
      });
  }
}  