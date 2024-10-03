import { Injectable } from '@angular/core';
import Swal from 'sweetalert2';


@Injectable({
   providedIn: 'root'
})

export class AlertService {

   success(title: string, text: string) {
      return Swal.fire({
         title,
         text,
         icon: 'success',
         toast: true,
         position: 'top-end',
         showConfirmButton: false,
         showCloseButton: true,
         timer: 5000,
         timerProgressBar: true,
      });
   }

   question(text: string) {
      return Swal.fire({
         title: 'Confirmar',
         text,
         icon: 'question',
         reverseButtons: true,
         showCancelButton: true,
         confirmButtonText: 'Confirmar',
         cancelButtonText: 'Cancelar',
      });
   }

   error(text: string) {
      return Swal.fire({
         title: 'Ha ocurrido un error',
         text,
         icon: 'error',
      });
   }


   info(title: string, text: string) {
      return Swal.fire({
         title,
         text,
         icon: 'info',
         toast: true,
         position: 'top-end',
         showConfirmButton: false,
         showCloseButton: true,
         timer: 10000,
         timerProgressBar: true,
      });
   }

   warning(title: string, text: string) {
      return Swal.fire({
         title,
         text,
         icon: 'warning',
      });
   }

}
