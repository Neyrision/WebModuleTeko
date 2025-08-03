import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { PostApiService } from '../../../api/api-services';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MessageService } from '../../services/message.service';

@Component({
  imports: [
    MatInputModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatCardModule,
  ],
  selector: 'app-home-component',
  styleUrls: ['./home.component.scss'],
  templateUrl: './home.component.html',
})
export class HomeComponent {
  constructor(
    private readonly messageService: MessageService,
    private readonly service: PostApiService
  ) {}

  test1(): void {
    this.service.getTest1().subscribe({
      next: (result) => {
        this.messageService.show('Test1 succeeded', 'success');
      },
      error: (err) => {
        this.messageService.show('Test1 failed', 'error');
      },
    });
  }

  test2(): void {
    this.service.getTest2().subscribe({
      next: (result) => {
        this.messageService.show('Test2 succeeded', 'success');
      },
      error: (err) => {
        this.messageService.show('Test2 failed', 'error');
      },
    });
  }
}
