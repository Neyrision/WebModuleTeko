import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { PostApiService } from '../../../api/api-services';
import { MatButtonModule } from '@angular/material/button';

@Component({
  imports: [MatInputModule, ReactiveFormsModule, MatButtonModule],
  selector: 'app-home-component',
  styleUrls: ['./home.component.scss'],
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(private readonly service: PostApiService) {}

  test1(): void {
    this.service.getTest1().subscribe((result) => {
      console.log('T1', result);
    });
  }

  test2(): void {
    this.service.getTest2().subscribe((result) => {
      console.log('T2', result);
    });
  }
}
