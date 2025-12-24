import { ComponentFixture, TestBed } from '@angular/core/testing';

import { DeckWordsComponent } from './deck-words.component';

describe('DeckWordsComponent', () => {
  let component: DeckWordsComponent;
  let fixture: ComponentFixture<DeckWordsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [DeckWordsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(DeckWordsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
