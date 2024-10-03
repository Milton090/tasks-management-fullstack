import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../environment/environment';
import { ResponseAPI, State } from '../interfaces';
import { BehaviorSubject, map, Observable, tap } from 'rxjs';
import { AlertService } from './alert.service';


@Injectable({
  providedIn: 'root'
})

export class StateService {

  private url = environment.apiUrl + environment.stateController;

  public states$ = new BehaviorSubject<State[]>([]);

  constructor(private http: HttpClient, private alert: AlertService) { }

  loadStates(): void {
    this.http.get<ResponseAPI>(`${this.url}`).pipe(
      map((res: ResponseAPI) => res.data),
      tap((states: State[]) => this.states$.next(states))
    ).subscribe();
  }

  addState(state: State): Observable<ResponseAPI> {
    return this.http.post<ResponseAPI>(this.url, state).pipe(
      tap((res: ResponseAPI) => this.handleSuccess(res, 'Estado agregado', 'El estado fue agregado correctamente'))
    );
  }

  updateState(state: State): Observable<ResponseAPI> {
    return this.http.put<ResponseAPI>(`${this.url}/${state.id}`, state).pipe(
      tap((res: ResponseAPI) => this.handleSuccess(res, 'Estado actualizado', 'El estado fue actualizado correctamente'))
    );
  }

  deleteState(id: string): Observable<ResponseAPI> {
    return this.http.delete<ResponseAPI>(`${this.url}/${id}`).pipe(
      tap((res: ResponseAPI) => this.handleSuccess(res, 'Estado eliminado', 'El estado fue eliminado correctamente'))
    );
  }

  private handleSuccess(res: ResponseAPI, title: string, message: string): void {
    if (res.success) {
      this.alert.success(title, message);
      this.loadStates();
    }
  }
}