import { LoginRequest, AuthResponseModel } from './login';

describe('Login Models', () => {
  it('should create a LoginRequest instance', () => {
    const login = new LoginRequest('test@example.com', 'password123');
    expect(login).toBeTruthy();
    expect(login.email).toBe('test@example.com');
    expect(login.password).toBe('password123');
  });

  it('should create an AuthResponseModel instance', () => {
    const authResponse = new AuthResponseModel('1', 'TestUser', 'fakeToken123', ['User']);
    expect(authResponse).toBeTruthy();
    expect(authResponse.id).toBe('1');
    expect(authResponse.userName).toBe('TestUser');
    expect(authResponse.token).toBe('fakeToken123');
    expect(authResponse.roles).toContain('User');
  });
});
