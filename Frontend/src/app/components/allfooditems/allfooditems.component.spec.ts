import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AllfooditemsComponent } from './allfooditems.component';

describe('AllfooditemsComponent', () => {
  let component: AllfooditemsComponent;
  let fixture: ComponentFixture<AllfooditemsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AllfooditemsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AllfooditemsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
