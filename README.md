# ELC 

Projeto para gerenciamento de arquivos dos professores, plataforma web para gerenciar seus arquivos 
e ajudar na análise dos dados das turmas e alunos. 

## ⚽ Pontapé inicial
Esse arquivo tem como objetivo fazer com que um desenvolvedor que esteja a ver 
esse projeto pela primeira vez, consiga entender como está estruturado, rodar e já 
conseguir dar continuidade.

### 🛠️ Projeto construído utilizando como principais tecnologias: 
- C# - https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-12
- NET 8 - https://learn.microsoft.com/pt-pt/dotnet/core/whats-new/dotnet-8/overview
- EntityFramework Core - https://learn.microsoft.com/en-us/ef/core/
- MVC - https://learn.microsoft.com/en-us/aspnet/core/tutorials/first-mvc-app/start-mvc?view=aspnetcore-8.0&tabs=visual-studio
- Bootstrap - https://getbootstrap.com/docs/5.0/getting-started/introduction/
- Jquery - https://jquery.com/
- ClosedXML - https://github.com/ClosedXML/ClosedXML
- XUnit - https://github.com/xunit/xunit

### 🔧 Passos para inicializar o projeto

1- Restaurar pacotes: 

```
dotnet restore
```

2- Alterar connection string no arquivo de configuração no projeto Web. 
**Atenção ao ASPNET_ENVIRONMENT que está utilizando. 

Para alterar o arquivo para o environment _Development_ altere o arquivo: **appsettings.Development.json**

```
"ConnectionStrings": {
    "SQLConnection": "<SUA-CONNECTION-STRING-AQUI>"
  }
```

3- Rodar o projeto: 
```
dotnet run
```

### Arquitetura do projeto 

Construímos a arquitetura usando pitadas de Clean Code e Onion Archtecture
O projeto está dividido em 3 principais frentes: 
**Web** - Projeto ASPNET MVC com a interface para o usuário 
**Infra** - Camada de infraesturutura, gerenciamento de informações e relação com acesso à dados. 
**Core** - Camada com as lógicas importantes para o negócio. A ideia é que aqui estejam centralizadas as regras de negócio,
interfaces, serviços e classes que podem ser compartilhadas ao longo do sistema.




