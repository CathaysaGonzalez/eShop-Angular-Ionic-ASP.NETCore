import { AppUser } from "./app-user.model";
import { CartLine } from "./cart-line.model";
import { Payment } from "./payment.model";

export class Order {
  constructor(
    public id?: number,
    public name?: string,
    public address?: string,
    public shipped?: boolean,
    public total?: number,
    public userName?: string,
    public paymentNavigation?: Payment,
    public cartLines?: CartLine[],
    public appUser?: AppUser
  ) {}
}

export class OrderData{
  constructor(
    public name?: string,
    public address?: string,
    public shipped?: boolean,
    public total?: number,
    public userName?: string,
    public paymentNavigation?: Payment,
    public cartLines?: CartLine[],
    public appUser?: AppUser
  ){}
}
