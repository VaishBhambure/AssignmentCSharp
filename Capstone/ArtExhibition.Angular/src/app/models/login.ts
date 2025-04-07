export class LoginRequest {
    constructor(public email: string, public password: string) {}
  }
  
  export class AuthResponseModel {
    id: string;
    userName: string;
    token: string;
    roles: string[];
  
    constructor(id: string, userName: string, token: string, roles: string[]) {
      this.id = id;
      this.userName = userName;
      this.token = token;
      this.roles = roles;
    }
  }
  