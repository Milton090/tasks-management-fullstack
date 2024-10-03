import { Component, Inject, OnInit } from '@angular/core';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { FormBuilder, FormGroup, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { StateService } from '../../../services/state.service';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { State } from '../../../interfaces';
import { CommonModule } from '@angular/common';


@Component({
  selector: 'app-form-state',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatButtonModule,
    MatInputModule,
    FormsModule,
    CommonModule
  ],
  templateUrl: './form-state.component.html',
  styleUrl: './form-state.component.css'
})

export class FormStateComponent implements OnInit {
  newState: FormGroup;

  constructor(
    private fb: FormBuilder,
    private api: StateService,
    public dialogRef: MatDialogRef<FormStateComponent>,
    @Inject(MAT_DIALOG_DATA) public data: State
  ) {
    this.newState = this.fb.group({
      name: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    if (this.data) {
      this.newState.patchValue({
        name: this.data.name,
      });
    }
  }

  saveState(): void {
    const formValue = this.newState.value;

    const data: State = {
      id: this.data?.id,
      name: formValue.name as string,
    };

    if (this.data?.id) {
      this.api.updateState(data).subscribe({
        next: () => {
          this.dialogRef.close();
        },
      });
    } else {
      this.api.addState(data).subscribe({
        next: () => {
          this.dialogRef.close();
        },
      });
    }
  }
}
