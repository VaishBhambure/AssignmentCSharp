export class RegisterRequest {
    constructor(
      public userName: string,
      public firstName: string,
      public lastName: string,
      public birthDate: string, 
      public email: string,
      public phoneNumber: string,
      public password: string,
      public confirmPassword: string,
      public isArtist: boolean
    ) {}
  }
  