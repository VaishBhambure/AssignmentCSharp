import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { LoginRequest, AuthResponseModel } from '../models/login';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = 'https://localhost:7168/api/User';

  constructor(private http: HttpClient) {}
  getToken(): string | null {
    return localStorage.getItem('token'); // Retrieves JWT token from local storage
  }

  // Register User
  register(userData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/register`, userData);
  }

  // Login User
  login(loginData: any): Observable<any> {
    return this.http.post(`${this.apiUrl}/login`, loginData);
}
getUserProfile(): Observable<any> {
  return this.http.get(`${this.apiUrl}/profile`); // Make sure API URL is correct

}
getUserId(): string | null {
  const token = this.getToken();
  if (!token) return null;

  try {
    const payload = JSON.parse(atob(token.split('.')[1])); // Decode JWT payload
    return payload.sub; // 'sub' usually contains user ID
  } catch (error) {
    console.error("Error decoding token:", error);
    return null;
  }
}
}

