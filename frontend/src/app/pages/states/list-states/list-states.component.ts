import { Component, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { StateService } from '../../../services/state.service';
import { NgStyle } from '@angular/common';
import { MatMenuModule } from '@angular/material/menu';
import { MatIconModule } from '@angular/material/icon';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule, MatLabel } from '@angular/material/form-field';
import { MenuComponent } from '../../../components/menu/menu.component';
import { MatButtonModule } from '@angular/material/button';
import { FormStateComponent } from '../form-state/form-state.component';
import { MatDialog } from '@angular/material/dialog';
import { AlertService } from '../../../services/alert.service';
import { SweetAlertResult } from 'sweetalert2';
import { State } from '../../../interfaces';
import { Observable } from 'rxjs';


@Component({
  selector: 'app-list-states',
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
  ],
  templateUrl: './list-states.component.html',
  styleUrl: './list-states.component.css'
})

export class ListStatesComponent {

  states$: Observable<State[]> = this.api.states$;
  dataSource = new MatTableDataSource<State>([]);

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  constructor(
    private api: StateService,
    private alert: AlertService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.api.loadStates();
    this.states$.subscribe((res: State[]) => {
      this.dataSource.data = res;
    });
  }

  ngAfterViewInit() {
    this.dataSource.paginator = this.paginator;
  }


  openDialog(state?: State) {
    this.dialog.open(FormStateComponent, {
      width: '500px',
      data: state
    });
  }


  deleteState(id: any): void {
    this.alert.question('¿Estás seguro que quieres eliminar este estado?')
      .then((result: SweetAlertResult) => {
        if (result.isConfirmed) {
          this.api.deleteState(id).subscribe();
        }
      });
  }



  applyFilter(event: Event) {
    const filterValue = (event.target as HTMLInputElement).value;
    this.dataSource.filter = filterValue.trim().toLowerCase();
  }
}
