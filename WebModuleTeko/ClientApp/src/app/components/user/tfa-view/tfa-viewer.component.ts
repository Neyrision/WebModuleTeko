import { Component, OnDestroy } from '@angular/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { Router } from '@angular/router';
import { Subject } from 'rxjs';
import { TotpModel } from '../../../../api/api-services';

@Component({
  imports: [
    MatCardModule,
    MatButtonModule,
  ],
  selector: 'app-tfa-viewer-component',
  styleUrls: ['./tfa-viewer.component.scss'],
  templateUrl: './tfa-viewer.component.html',
})
export class TfaViewerComponent {

  protected totpModel: TotpModel | null = null;

  constructor(
    private readonly router: Router
  ) {
    this.totpModel = this.router.getCurrentNavigation()?.extras.state as TotpModel;
  }

  protected onLoginClick(): void {
    this.router.navigate(['login']);
  }

}
