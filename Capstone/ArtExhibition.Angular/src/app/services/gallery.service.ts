import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class GalleryService {
  private apiUrl = 'https://localhost:7168/api/Artist/galleries';

  constructor(private http: HttpClient) { }

  getGalleries(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }
}
