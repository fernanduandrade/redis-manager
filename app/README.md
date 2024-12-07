# Redis Manager App

Este é um aplicativo desktop desenvolvido com **Rust Tauri** e **Vue 3** usando **TypeScript**, com uma API backend implementada em **.NET C#**. O objetivo do projeto é fornecer uma interface amigável para gerenciar conexões Redis, criar e visualizar chaves, bem como editar seus valores.

## Funcionalidades

- Conexão com instâncias Redis.
- Visualização hierárquica de chaves com base no separador `:` (simulando pastas e subpastas).
- Visualização dos valores das chaves no formato JSON.
- Edição de valores das chaves diretamente no aplicativo.
- Suporte para criação de novas chaves.

## Tecnologias Utilizadas

### Frontend
- **Rust Tauri**: Framework para desenvolvimento de aplicativos desktop.
- **Vue 3**: Framework para a construção da interface de usuário.
- **TypeScript**: Tipagem estática para um desenvolvimento mais robusto.

### Backend
- **.NET C#**: API responsável pelo processamento de dados e comunicação com o Redis.

## Pré-requisitos

- [Node.js](https://nodejs.org/) para rodar o frontend.
- [Rust](https://www.rust-lang.org/) para compilar o aplicativo Tauri.
- [Redis](https://redis.io/) como banco de dados.
- [.NET SDK](https://dotnet.microsoft.com/) para rodar a API backend.

## Como Executar o Projeto

### Configuração Inicial
1. Clone o repositório:
   ```bash
   git clone https://github.com/fernanduandrade/redis-manager.git
   cd redis-manager
   npm install
   ```

2. Instale as dependências do frontend:
   ```bash
   cd app
   npm install
   ```

2. Instale as dependências da api:
   ```bash
   cd api && cd RedisManagerApi
   dotnet restore
   dotnet run
   ```

### Rodando o Aplicativo
Retorne à pasta frontend e inicie o aplicativo:

```bash
npm run tauri dev
```
O aplicativo será aberto automaticamente em uma janela desktop.

# Contribuição
Sinta-se à vontade para contribuir com melhorias ou novas funcionalidades. Abra uma issue ou envie um pull request para discussão.

# Licença
Este projeto está licenciado sob a MIT License.
