# EducaOnline - Plataforma de Gestão Educacional

Uma plataforma completa de gestão de cursos e alunos construída com arquitetura de microsserviços, desenvolvida como projeto final do **MBA DevXpert Full Stack .NET**.

## 📋 Sobre o Projeto

O EducaOnline é uma aplicação corporativa distribuída que oferece gestão completa de:
- 📚 Cursos e conteúdos educacionais
- 👨‍🎓 Alunos e matrículas
- 💳 Pedidos e pagamentos
- 🎓 Certificados de conclusão
- 🔐 Autenticação e autorização

## 👥 Autores

- **Ozias Manoel Costa Neto**

## 🏗️ Arquitetura

A solução utiliza uma arquitetura moderna baseada em:

- **Microsserviços independentes** - cada domínio com seu próprio banco de dados
- **Event-Driven Architecture** - comunicação assíncrona via RabbitMQ
- **BFF (Backend for Frontend)** - gateway unificado para o frontend
- **Domain-Driven Design (DDD)** - modelagem rica de domínio
- **CQRS** - separação de comandos e consultas
- **Clean Architecture** - camadas bem definidas e desacopladas

### Serviços

```
├── Identidade.API      → Autenticação JWT
├── Conteudo.API        → Gestão de cursos e aulas
├── Aluno.API           → Gestão de alunos, matrículas e certificados
├── Pedidos.API         → Processamento de pedidos
├── Financeiro.API      → Faturamento e pagamentos
└── BFF                 → Gateway de integração
```

### Frontend

- **Angular 17+** com arquitetura modular
- **Nx Monorepo** para organização de código
- Interface responsiva e moderna

## 🚀 Tecnologias

### Backend
- .NET 9.0
- ASP.NET Core Web API
- Entity Framework Core
- SQLite (Development)
- RabbitMQ + EasyNetQ
- JWT Bearer Authentication
- AutoMapper
- FluentValidation
- MediatR (CQRS)
- Dapper

### Frontend
- Angular 17+
- TypeScript
- RxJS
- Nx Monorepo

### Infraestrutura
- Docker
- RabbitMQ

## ⚙️ Pré-requisitos

Antes de começar, certifique-se de ter instalado:

- [.NET SDK 9.0+](https://dotnet.microsoft.com/download)
- [Node.js 20+ LTS](https://nodejs.org/)
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- [Git](https://git-scm.com/)

**Opcionais:**
- Visual Studio 2022 ou VS Code
- Cliente REST (Postman, Insomnia, etc.)

## 🔧 Instalação e Configuração

### 1. Clone o Repositório

```bash
git clone https://github.com/Hellstricker/EducaOnlineProjetoFinal.git
cd EducaOnlineProjetoFinal
```

### 2. Inicie o RabbitMQ

**IMPORTANTE:** O RabbitMQ deve estar rodando antes de iniciar as APIs!

```bash
docker run -d --hostname educa-rabbit --name educa-rabbit \
  -p 5672:5672 -p 15672:15672 rabbitmq:3-management
```

Aguarde 30-60 segundos para inicialização completa.

**Verificar status:**
```bash
docker ps | grep educa-rabbit
```

**Painel de gerenciamento:** http://localhost:15672 (guest/guest)

### 3. Configure o Frontend

```bash
cd frontend
npm install
```

### 4. Execute os Serviços Backend

Execute os serviços **nesta ordem** (cada um em um terminal separado):

```bash
# 1. Identidade (porta 7070)
cd backend/src/Services/EducaOnline.Identidade.API
dotnet run

# 2. Conteúdo (porta 7183)
cd backend/src/Services/EducaOnline.Conteudo.API
dotnet run

# 3. Aluno (porta 7094)
cd backend/src/Services/EducaOnline.Aluno.API
dotnet run

# 4. Pedidos (porta 7244)
cd backend/src/Services/EducaOnline.Pedidos.API
dotnet run

# 5. Financeiro (porta 7059)
cd backend/src/Services/EducaOnline.Financeiro.API
dotnet run

# 6. BFF (porta 7093)
cd backend/src/api_gateways/EducaOnline.Bff
dotnet run
```

### 5. Execute o Frontend

```bash
cd frontend
npx nx serve educa-online
```

Acesse: http://localhost:4200

## 🌐 Endpoints e Portas

| Serviço | HTTPS | HTTP | Swagger |
|---------|-------|------|---------|
| Frontend | - | 4200 | http://localhost:4200 |
| BFF | 7093 | 5051 | https://localhost:7093/swagger |
| Identidade | 7070 | 5244 | https://localhost:7070/swagger |
| Conteúdo | 7183 | 5105 | https://localhost:7183/swagger |
| Aluno | 7094 | 5152 | https://localhost:7094/swagger |
| Pedidos | 7244 | - | https://localhost:7244/swagger |
| Financeiro | 7059 | - | https://localhost:7059/swagger |
| RabbitMQ | - | 15672 | http://localhost:15672 |

## 🔑 Credenciais de Teste

O projeto cria automaticamente usuários padrão em ambiente de desenvolvimento:

**Administrador:**
- Email: `admin@educaonline.com.br`
- Senha: `Teste@123`

**Aluno:**
- Email: `aluno@educaonline.com.br`
- Senha: `Teste@123`

## 📝 Dados Iniciais

Ao executar em modo Development, o sistema cria automaticamente:

### Cursos
1. Introdução à Inteligência Artificial (20h, 2 aulas)
2. Desenvolvimento Web com Angular (20h, 2 aulas)
3. Arquitetura de Software com .NET (20h, 2 aulas)

### Aluno de Teste
- Matrícula nos 3 cursos
- Progresso de 50% em 1 aula
- Certificado emitido para o curso de IA

## 🧪 Testando a API

### 1. Obter Token de Autenticação

```bash
POST https://localhost:7070/api/identidade/autenticar
Content-Type: application/json

{
  "email": "aluno@educaonline.com.br",
  "senha": "Teste@123"
}
```

### 2. Usar o Token

Copie o `accessToken` da resposta e adicione no header:
```
Authorization: Bearer {seu-token-aqui}
```

### 3. Testar Endpoints

**Listar cursos:**
```bash
GET http://localhost:5105/api/cursos
```

**Obter detalhes do aluno:**
```bash
GET https://localhost:7094/api/alunos/{id}
Authorization: Bearer {token}
```

## 🐛 Troubleshooting

### Erro de RabbitMQ Connection

**Sintoma:** `TaskCanceledException` ou timeout

**Solução:**
1. Verificar se o Docker está rodando
2. Verificar status do container: `docker ps | grep educa-rabbit`
3. Iniciar/reiniciar: `docker start educa-rabbit`
4. Aguardar 30-60 segundos antes de iniciar as APIs

### Porta já em uso

**Solução:**
```bash
# Windows
netstat -ano | findstr :7070
taskkill /PID {numero-do-pid} /F

# Linux/Mac
lsof -ti:7070 | xargs kill -9
```

### Reset Completo

```bash
# Parar e remover RabbitMQ
docker stop educa-rabbit
docker rm educa-rabbit

# Deletar bancos de dados
rm backend/src/Services/*/*.db

# Limpar node_modules
rm -rf frontend/node_modules

# Reinstalar
docker run -d --hostname educa-rabbit --name educa-rabbit -p 5672:5672 -p 15672:15672 rabbitmq:3-management
cd frontend && npm install
```

## 📂 Estrutura de Pastas

```
EducaOnlineProjetoFinal/
├── backend/
│   └── src/
│       ├── ApiGateways/
│       │   └── EducaOnline.Bff/
│       ├── BuildingBlocks/
│       │   ├── EducaOnline.Core/
│       │   ├── EducaOnline.MessageBus/
│       │   └── EducaOnline.WebAPI.Core/
│       └── Services/
│           ├── Aluno/
│           ├── Conteudo/
│           ├── Financeiro/
│           ├── Identidade/
│           └── Pedidos/
└── frontend/
    ├── apps/
    └── libs/
```

## 🎯 Padrões de Arquitetura

### Por Microsserviço

```
API Layer
├── Controllers      → Endpoints REST
└── Configuration    → Injeção de dependências

Application Layer
├── Commands        → Operações de escrita (CQRS)
├── Queries         → Operações de leitura (CQRS)
├── Handlers        → Processadores (MediatR)
└── ViewModels      → DTOs

Domain Layer
├── Entities        → Agregados e entidades
├── ValueObjects    → Objetos de valor
├── Events          → Eventos de domínio
└── Interfaces      → Contratos

Infrastructure Layer
├── Data            → DbContext (EF Core)
├── Repositories    → Implementações
└── Integrations    → Message Bus, APIs
```

## 🔄 Comunicação entre Serviços

```
Identidade.API ──[UsuarioCriadoEvent]──> Aluno.API
    │
    └──> Cria usuário no Identity
         └──> Aluno.API cria perfil de aluno

Aluno.API ──[MatriculaCriadaEvent]──> Pedidos.API
    │
    └──> Processa matrícula
         └──> Pedidos.API cria pedido

Pedidos.API ──[PagamentoProcessadoEvent]──> Financeiro.API
    │
    └──> Confirma pedido
         └──> Financeiro.API registra pagamento
```

## 🚧 Melhorias Futuras

- [ ] Testes automatizados (unitários e integração)
- [ ] Kubernetes para orquestração

## 📄 Licença

Projeto acadêmico - Todos os direitos reservados ao autor.

## 📞 Contato

Projeto desenvolvido para entrega do módulo 5 do **MBA DevXpert Full Stack .NET**.

---

**Última atualização:** Dezembro 2024