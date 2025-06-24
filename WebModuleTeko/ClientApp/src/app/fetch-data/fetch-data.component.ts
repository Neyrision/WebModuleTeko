import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { PostApiService } from 'src/api/api-services';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public forecasts: WeatherForecast[] = [];

  constructor(
    private readonly postApiService: PostApiService, 
    @Inject('BASE_URL') baseUrl: string
  ) {
    
    postApiService.getTest().subscribe((wao) => {
      console.log(wao);
    });

  }
}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
