# argos-api

- http://localhost:5095
- https://localhost:7202/index.html (Swagger UI)

## Estrutura
```text
ArgosApi
├── Common/                 # Extensões e utilitários compartilhados
│   └── Extensions/
│
├── Data/                   # Contexto do EF Core e configurações das entidades
│   └── Configurations/
│
├── Domain/                 # Entidades de domínio
│   └── Entities/
│
├── Features/               # Organização por features
│   ├── Dashboard/
│   │   ├── DashboardController.cs
│   │   └── DashboardService.cs
│   ├── Projetos/
│   ├── Relatorios/
│   └── Usuarios/
│
├── Infrastructure/         # Implementações de infraestrutura
│   └── Authentication/
│
├── Migrations/             # Migrations do Entity Framework Core
├── Program.cs              # Ponto de entrada da aplicação
└── ...
```