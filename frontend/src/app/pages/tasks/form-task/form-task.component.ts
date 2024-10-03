import { Component, OnInit, Inject } from '@angular/core';
import { MatOptionModule } from '@angular/material/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormControl, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { TaskService } from '../../../services/task.service';
import { State, Task } from '../../../interfaces';
import { StateService } from '../../../services/state.service';
import { Observable } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-form-task',
  standalone: true,
  imports: [
    MatSelectModule,
    MatOptionModule,
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    FormsModule,
    CommonModule,
  ],
  templateUrl: './form-task.component.html',
  styleUrl: './form-task.component.css',
})

export class FormTaskComponent implements OnInit {
  newTask: FormGroup;
  states$: Observable<State[]>;

  constructor(
    private api: TaskService,
    private stateService: StateService,
    public dialogRef: MatDialogRef<FormTaskComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Task
  ) {
    this.newTask = new FormGroup({
      title: new FormControl('', Validators.required),
      stateId: new FormControl('', Validators.required),
    });

    this.states$ = this.stateService.states$;
  }

  ngOnInit(): void {
    this.stateService.loadStates();

    if (this.data) {
      this.newTask.patchValue({
        title: this.data.title,
        stateId: this.data.stateId
      });
    }
  }

  saveTask(): void {
    const formValue = this.newTask.value;

    const data: Task = {
      id: this.data?.id,
      title: formValue.title as string,
      stateId: formValue.stateId as number,
    };

    if (this.data?.id) {
      this.api.updateTask(data).subscribe({
        next: () => {
          this.dialogRef.close();
        },
      });
    } else {
      this.api.addTask(data).subscribe({
        next: () => {
          this.dialogRef.close();
        },
      });
    }
  }
}