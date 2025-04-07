import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateGalleryComponent } from './create-gallery.component';

describe('CreateGalleryComponent', () => {
  let component: CreateGalleryComponent;
  let fixture: ComponentFixture<CreateGalleryComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateGalleryComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateGalleryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
