import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { DetailOrderPage } from './detail-order.page';

describe('DetailOrderPage', () => {
  let component: DetailOrderPage;
  let fixture: ComponentFixture<DetailOrderPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DetailOrderPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(DetailOrderPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
