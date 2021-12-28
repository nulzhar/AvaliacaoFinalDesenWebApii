import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';
import { BaseService } from '../base.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
  nomeUsuario: string;
  isAuthenticated: boolean;

  loginForm = this.formBuilder.group({
    nomeUsuario: '',
    senha: ''
  });

  constructor(
    private http: HttpClient, @Inject('BASE_URL')
    private baseUrl: string,
    private formBuilder: FormBuilder,
    private baseService: BaseService
  ) {
    this.nomeUsuario = baseService.getNomeUsuario();
    this.isAuthenticated = baseService.getIsAuthenticated();
  }


  onSubmit(): void {
    console.warn('Login foi submetido.', this.loginForm.value);
    this.nomeUsuario = this.loginForm.value.nomeUsuario;
    this.baseService.setNomeUsuario(this.loginForm.value.nomeUsuario);
    this.http.post<any>(this.baseUrl + 'api/auth', { "NomeUsuario": this.loginForm.value.nomeUsuario, "Senha": this.loginForm.value.senha })
      .subscribe({
        next: data => {
          this.baseService.setToken(data.token);
          this.baseService.setIsAuthenticated(true);
          this.isAuthenticated = true;
          console.log("Sucesso", data);
        },
        error: error => {
          console.error('There was an error!', error);
        }
      });
    this.loginForm.reset();
  }
}
