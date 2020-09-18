import { Order } from './order.model';

export class AppUser {
    constructor(
      public id?: string,
      public userName?: string,
      public normalizedUserName?: string,
      public email?: string,
      public normalizedEmail?: string,
      public emailConfirmed?: boolean,
      public passwordHash?: string,
      public securityStamp?: string,
      public concurrencyStamp?: string,
      public phoneNumber?: string,
      public phoneNumberConfirmed?: boolean,
      public twoFactorEnabled?: boolean,
      public lockoutEnd?: Date,
      public lockoutEnabled?: boolean,
      public accessFailedCount?: number,
      public role?: string,
      public orders?: Order[],
      private _token?: string,
      private _tokenExpirationDate?: Date,
    ) {}
  
    get token() {
      if (!this._tokenExpirationDate || this._tokenExpirationDate <= new Date()) {
        return null;
      }
      return this._token;
    }
  
    get tokenExpirationDate(){
      if(!this.token){
        return 0;
      }
      return  this._tokenExpirationDate.getTime() - new Date().getTime();
    }
  }