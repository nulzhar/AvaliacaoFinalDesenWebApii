import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormBuilder } from '@angular/forms';
import { BaseService } from '../base.service';

@Component({
  selector: 'app-produto-component',
  templateUrl: './produto.component.html'
})
export class ProdutoComponent {
  private token: string;

  produtoForm = this.formBuilder.group({
    idProduto: 0,
    nomeProduto: 'Nome do Produto',
    categoria: 'Categoria',
    quantidadeEstoque: 0,
    precoVenda: 0.0,
  });

  constructor(
    private http: HttpClient, @Inject('BASE_URL')
    private baseUrl: string,
    private formBuilder: FormBuilder,
    private baseService: BaseService
  ) {
    this.token = baseService.getToken();
  }

  onSubmit(): void {
    console.warn('Produto cadastro foi submetido.', this.produtoForm.value);
    const headers = { 'Authorization': 'Bearer ' + this.baseService.getToken() };
    const produto = {
      "IdProduto": 0,
      "NomeProduto": this.produtoForm.value.nomeProduto,
      "Categoria": this.produtoForm.value.categoria,
      "PrecoVenda": this.produtoForm.value.precoVenda,
      "QuantidadeEstoque": this.produtoForm.value.quantidadeEstoque

    };
    console.log("produto", produto);
    this.http.post<any>(this.baseUrl + 'api/produto',
      produto, { headers })
      .subscribe({
        next: data => {
          console.log("Sucesso", data);
        },
        error: error => {
          console.error('There was an error!', error);
        }
      });
    this.produtoForm.reset();
  }
}
