export interface Task {
   id: number;
   title: string;
   stateId: number;
   stateName?: string;
   userId?: number;
}

export interface State {
   id: number;
   name: string;
}

export interface Auth {
   id?: number;
   email: string;
   password: string;
}

export interface ResponseAPI {
   success: boolean;
   message: string;
   data: any;
}