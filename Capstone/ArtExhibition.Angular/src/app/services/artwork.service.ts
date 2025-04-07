import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class ArtworkService {
  private apiUrl = 'https://localhost:7168/api/Artist/upload-artwork';

  constructor(private http: HttpClient) {}

  uploadArtwork(title: string, description: string, image: File): Observable<any> {
    const formData = new FormData();
    formData.append('Title', title);
    formData.append('Description', description);
    formData.append('Image', image);

    // Retrieve token from local storage
    const token = localStorage.getItem('access_token');

    const headers = new HttpHeaders({
      Authorization: `Bearer ${token}`,
    });

    return this.http.post<any>(this.apiUrl, formData, { headers });
  }
  
}
