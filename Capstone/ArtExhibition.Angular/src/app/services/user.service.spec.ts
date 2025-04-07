import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { AuthService } from './user.service';
import { LoginRequest, AuthResponseModel } from '../models/login';

describe('AuthService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;
  const apiUrl = 'https://localhost:7168/api/User/login';

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [AuthService],
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify(); // Ensure no extra requests were made
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  it('should perform a login API request', () => {
    const mockResponse: AuthResponseModel = {
      id: '1',
      userName: 'Vaish',
      token: 'fake-jwt-token',
      roles: ['User', 'Artist']
    };

    const loginData = new LoginRequest('vaish@gmail.com', 'Vaish@123');

    service.login(loginData).subscribe(response => {
      expect(response).toBeTruthy();
      expect(response.userName).toBe('Vaish');
      expect(response.token).toBe('fake-jwt-token');
      expect(response.roles).toContain('User');
    });

    const req = httpMock.expectOne(apiUrl);
    expect(req.request.method).toBe('POST');
    expect(req.request.body).toEqual(loginData);
    req.flush(mockResponse);
  });
});
