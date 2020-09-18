import { AppUser } from './app-user.model';
import { Payment } from './payment.model';
import { CartLine } from './cart-line.model';

export class Order{
    constructor(
      public id?: number,
      public name?: string,
      public address?: string,
      public shipped?: boolean,
      public total?: number,

      // public userId?: string,
      public userName?: string,

      public paymentNavigation?: Payment,
      public cartLines?: CartLine[],
      public appUser?: AppUser,
    ) {}
  }