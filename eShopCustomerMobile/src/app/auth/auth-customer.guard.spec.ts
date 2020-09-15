import { TestBed } from '@angular/core/testing';

import { AuthCustomerGuard } from './auth-customer.guard';

describe('AuthCustomerGuard', () => {
  let guard: AuthCustomerGuard;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    guard = TestBed.inject(AuthCustomerGuard);
  });

  it('should be created', () => {
    expect(guard).toBeTruthy();
  });
});
