import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environment/environment';
import { ResponseAPI, Task } from '../interfaces';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { AlertService } from './alert.service';


@Injectable({
  providedIn: 'root'
})

export class TaskService {

  private url = environment.apiUrl + environment.taskController;

  public tasks$ = new BehaviorSubject<Task[]>([]);

  constructor(private http: HttpClient, private alert: AlertService) { }

  getTasks(): void {
    this.http.get<ResponseAPI>(`${this.url}`).pipe(
      map((res: ResponseAPI) => res.data),
      tap((tasks: Task[]) => this.tasks$.next(tasks))
    ).subscribe();
  }

  addTask(task: Task): Observable<ResponseAPI> {
    return this.http.post<ResponseAPI>(this.url, task).pipe(
      tap((res: ResponseAPI) => this.handleSuccess(res, 'Tarea agregada', 'La tarea fue agregada correctamente'))
    );
  }

  updateTask(task: Task): Observable<ResponseAPI> {
    return this.http.put<ResponseAPI>(`${this.url}/${task.id}`, task).pipe(
      tap((res: ResponseAPI) => this.handleSuccess(res, 'Tarea actualizada', 'La tarea fue actualizada correctamente'))
    );
  }

  deleteTask(id: string): Observable<ResponseAPI> {
    return this.http.delete<ResponseAPI>(`${this.url}/${id}`).pipe(
      tap((res: ResponseAPI) => this.handleSuccess(res, 'Tarea eliminada', 'La tarea fue eliminada correctamente'))
    );
  }

  private handleSuccess(res: ResponseAPI, title: string, message: string): void {
    if (res.success) {
      this.alert.success(title, message);
      this.getTasks();
    }
  }
}