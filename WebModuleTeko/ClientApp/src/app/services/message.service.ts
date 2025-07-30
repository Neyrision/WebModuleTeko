import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

interface QueueEntry {
  message: string;
  type: 'info' | 'success' | 'error',
  action: string | undefined;
  duration: number | undefined;
}

@Injectable({
  providedIn: 'root',
})
export class MessageService {
  protected snackQueue: QueueEntry[] = [];
  protected isShowing: boolean = false;

  constructor(private readonly snackBar: MatSnackBar) {}

  show(message: string, type: 'info' | 'success' | 'error' = 'info', duration: number = 3000, action?: string): void {
    this.snackQueue.push({message, type, action, duration});
    this.showNext();
  }

  private showNext(): void {
    if(this.isShowing || this.snackQueue.length <= 0) return;

    const message = this.snackQueue.shift()!;

    this.isShowing = true;
    const messageReference = this.snackBar.open(message.message, message.action, {
        duration: message.duration,
        panelClass: `${message.type}-message`
    });

    messageReference.afterDismissed().subscribe(() => {
        this.isShowing = false;
        this.showNext();
    })
  }
}
