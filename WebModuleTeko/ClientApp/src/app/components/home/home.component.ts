import { Component } from '@angular/core';
import { ReactiveFormsModule } from '@angular/forms';
import { MatInputModule } from '@angular/material/input';
import { PostApiService } from '../../../api/api-services';

@Component({
  imports: [
    MatInputModule,
    ReactiveFormsModule
  ],
  selector: 'app-home-component',
  styleUrls: ['./home.component.scss'],
  templateUrl: './home.component.html'
})
export class HomeComponent {
  

  constructor(private readonly service: PostApiService) {
    service.getTest().subscribe((result) => {
      console.log(result);
    });
  }

}
