Para teste de autenticação caso não tenha nenhum usuário cadastrado o usuário é "admin" e senha "123".

Dentro do diretório da solução contém o arquivo com as chamadas do postman utilizadas "Atividade.postman_collection.json".

A conexão com o banco de dados é via connection string configurado no arquivo appsettings.json.

TABELAS UTILIZADAS:
# Tabela Produto
CREATE TABLE Produto
(
	Id INT PRIMARY KEY IDENTITY,
	NomeProduto VARCHAR (100),
	Categoria VARCHAR (100),
	QuantidadeEstoque INTEGER,
	PrecoVenda DECIMAL(19,2)
)

# Tabela Usuario
CREATE TABLE Usuario
(
	Id INT PRIMARY KEY IDENTITY,
	NomeUsuario VARCHAR(100) UNIQUE,
	Senha VARCHAR(100)
)