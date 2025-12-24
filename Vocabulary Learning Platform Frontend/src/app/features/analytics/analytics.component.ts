import { Component, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, registerables } from 'chart.js';
import { ReviewService } from '../../core/services/review.service';

@Component({
  selector: 'app-analytics',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './analytics.component.html',
  styleUrl: './analytics.component.css'
})
export class AnalyticsComponent {
  constructor(private reviewService: ReviewService) {
    // Register all chart types, elements, scales, and plugins
    Chart.register(...registerables);
  }

  ngAfterViewInit(): void {
    this.loadAnalytics();
  }

  loadAnalytics() {
    this.reviewService.getUserProgress().subscribe(data => {
      this.createCharts(data);
    });
  }

  private createCharts(data: any) {
    // Accuracy chart
    new Chart('accuracyChart', {
      type: 'doughnut',
      data: {
        labels: ['Correct', 'Incorrect'],
        datasets: [{
          data: [
            data.accuracy * 100,
            100 - data.accuracy * 100
          ],
          backgroundColor: ['#4caf50', '#f44336']
        }]
      }
    });

    // XP Progress
    new Chart('xpChart', {
      type: 'bar',
      data: {
        labels: ['XP'],
        datasets: [{
          label: 'XP Progress',
          data: [data.totalXP],
          backgroundColor: '#2196f3'
        }]
      }
    });

    // Streak Chart
    new Chart('streakChart', {
      type: 'line',
      data: {
        labels: ['Current Streak'],
        datasets: [{
          label: '🔥 Streak',
          data: [data.streak],
          borderColor: '#ff9800',
          fill: false
        }]
      }
    });
  }
}
