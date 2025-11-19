📚 Sistema de Gestão de Ensino - API
Este projeto é uma API RESTful desenvolvida como parte do Checkpoint da Alura, simulando um sistema de gerenciamento de cursos, estudantes e matrículas.

O objetivo foi aplicar práticas avançadas de desenvolvimento de software, incluindo Clean Architecture, Entity Framework Core, Autenticação JWT e Padrões de Projeto.

🚀 Tecnologias e Práticas Utilizadas
.NET 9.0 (Web API)

Entity Framework Core (ORM)

SQL Server (Banco de Dados)

ASP.NET Core Identity (Gestão de Usuários e Roles)

JWT Bearer (Autenticação e Autorização)

Clean Architecture (Separação em camadas: Domain, Application, Infrastructure, API)

Repository Pattern & Unit of Work

AutoMapper (Mapeamento de Objetos)

NSwag (Documentação OpenAPI/Swagger)

Paginação e Filtros

⚙️ Funcionalidades
🔐 Autenticação & Autorização
Registro de novos estudantes (Público).

Login e geração de Token JWT.

Controle de acesso baseado em Roles: Admin, Instructor, Student.

🎓 Cursos (Courses)
CRUD completo de cursos.

Listagem paginada para o público geral.

Criação e Edição restrita a Admins e Instrutores.

🧑‍🎓 Estudantes (Students)
Perfil de estudante vinculado ao usuário do sistema (Identity).

Consulta de perfil (O próprio estudante ou Admin).

Exclusão lógica (Soft Delete).

📝 Matrículas (Enrollments)
Matrícula de estudantes em cursos.

Regras de negócio: Impede matrícula duplicada.

Listagem de matrículas por estudante.

🛠️ Como Rodar o Projeto
Pré-requisitos
.NET 9 SDK instalado.

SQL Server (LocalDB ou Docker).

Visual Studio 2022 ou VS Code.

Passo a Passo
Clone o repositório:

Bash

git clone https://github.com/seu-usuario/seu-repositorio.git
cd seu-repositorio
Configure a Chave JWT (Segredos do Usuário): Por segurança, a chave secreta não está no código. Execute o comando abaixo na pasta Curso.API para definir uma chave local:

Bash

cd Curso.API
dotnet user-secrets set "Jwt:Key" "SUA_CHAVE_SUPER_SECRETA_COM_PELO_MENOS_32_CARACTERES"
Configure o Banco de Dados: Verifique a Connection String no appsettings.json. O padrão está configurado para (localdb)\mssqllocaldb.

Aplique as migrações para criar o banco:

Bash

dotnet ef database update --project ../Curso.Infrastructure --startup-project .
Execute a Aplicação:

Bash

dotnet run
Acesse a Documentação: Abra o navegador em https://localhost:7133/swagger (ou a porta indicada no terminal) para ver e testar os endpoints via Swagger UI.

👤 Usuários de Teste (Seed)
Ao rodar a aplicação pela primeira vez, o sistema cria automaticamente os seguintes papéis e um usuário administrador:

Admin User: admin@sistema.com

Senha: Admin@123 (ou a senha definida no seu DataSeeder.cs)

🏗️ Estrutura do Projeto (Clean Architecture)
Curso.Domain: O núcleo do projeto. Contém as Entidades (Student, Course, Enrollment), Enums e Regras de Negócio. Não depende de nada.

Curso.Application: Contém as Regras da Aplicação. Interfaces (Services, Repositories), DTOs, Mappings (AutoMapper) e Implementações dos Serviços.

Curso.Infrastructure: Implementação técnica. DbContext, Migrations, Repositórios concretos, Identity e Serviços externos.

Curso.API: A camada de entrada. Controllers, Injeção de Dependência e Configurações HTTP.

Curso.Shared: Classes compartilhadas e utilitários (ex: PagedResult, QueryParameters).

✒️ Autor
Desenvolvido por Samuel Gomes.
