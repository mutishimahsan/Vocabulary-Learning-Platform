import { Component } from '@angular/core';
import { ReviewService } from '../../core/services/review.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  dueWords: any[] = [];
  progress: any;

  constructor(private reviewService: ReviewService) {}

  ngOnInit() {
    this.reviewService.getTodayReviews().subscribe(res => this.dueWords = res);
    this.reviewService.getUserProgress().subscribe(res => this.progress = res);
  }
}
