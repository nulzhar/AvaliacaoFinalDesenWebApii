import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class BaseService {

  private token: string;

  setToken(token: string) {
    this.token = token;
  }

  getToken(): string {
    return this.token;
  }


  private isAuthenticated: boolean;

  setIsAuthenticated(isAuthenticated: boolean) {
    this.isAuthenticated = isAuthenticated;
  }

  getIsAuthenticated(): boolean {
    return this.isAuthenticated;
  }


  private nomeUsuario: string;

  setNomeUsuario(nomeUsuario: string) {
    this.nomeUsuario = nomeUsuario;
  }

  getNomeUsuario(): string {
    return this.nomeUsuario;
  }
}
