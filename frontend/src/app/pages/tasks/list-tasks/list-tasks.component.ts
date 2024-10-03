import { Component, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { TaskService } from '../../../services/task.service';
import { AsyncPipe, DatePipe, NgStyle } from '@angular/common';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MenuComponent } from '../../../components/menu/menu.component';
import { MatButtonModule } from '@angular/material/button';
import { FormTaskComponent } from '../form-task/form-task.component';
import { MatDialog } from '@angular/material/dialog';
import { AlertService } from '../../../services/alert.service';
import { SweetAlertResult } from 'sweetalert2';
import { Observable } from 'rxjs';
import { State, Task } from '../../../interfaces';
import { StateService } from '../../../services/state.service';
import { MatSelectModule } from '@angular/material/select';
import { MatOptionModule } from '@angular/material/core';


@Component({
  selector: 'app-list-tasks',
  standalone: true,
  imports: [
    MenuComponent,
    MatPaginatorModule,
    NgStyle,
    MatMenuModule,
    MatIconModule,
    MatTableModule,
    MatLabel,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    DatePipe,
    MatSelectModule,
    MatOptionModule,
    AsyncPipe,
  ],
  templateUrl: './list-tasks.component.html',
  styleUrl: './list-tasks.component.css'
})

export class ListTasksComponent {

  tasks$: Observable<Task[]> = this.api.tasks$;
  states$: Observable<State[]> = this.stateService.states$;

  dataSource = new MatTableDataSource<Task>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private api: TaskService,
    private alert: AlertService,
    private dialog: MatDialog,
    private stateService: StateService
  ) {
    this.stateService.loadStates();
  }

  ngOnInit(): void {
    this.api.getTasks();
    this.tasks$.subscribe((res: Task[]) => {
      this.dataSource.data = res;
    });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }


  openDialog(task?: Task) {
    this.dialog.open(FormTaskComponent, {
      width: '500px',
      data: task
    });
  }


  deleteTask(id: any): void {
    this.alert.question('¿Estás seguro que quieres eliminar esta tarea?')
      .then((result: SweetAlertResult) => {
        if (result.isConfirmed) {
          this.api.deleteTask(id).subscribe();
        }
      });
  }


  filterTasksByState(stateId: number): void {
    this.tasks$.subscribe((tasks: Task[]) => {
      const filteredTasks = stateId ? tasks.filter(task => task.stateId === stateId) : tasks;
      this.dataSource.data = filteredTasks;
    });
  }


  searchFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }

}
