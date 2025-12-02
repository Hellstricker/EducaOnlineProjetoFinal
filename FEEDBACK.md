## Funcionalidade 30%

Avalie se o projeto atende a todos os requisitos funcionais definidos.
* Será revisado na avalição final.

## Qualidade do Código 20%

Considere clareza, organização e uso de padrões de codificação.

### Data
* Os repositórios estão fazendo dispose de dependências injetadas, o que não é bom. Quando se usa injeção de dependência, o ciclo de vida do objeto é gerenciado pelo contêiner de injeção, e não deve ser descartado manualmente. Isso pode levar a erros difíceis de rastrear, como tentar usar um objeto que já foi descartado. O `Dispose()` deve apenas descartar objetos que a prórpria classe criou e é responsável por gerenciar. Ex:
```csharp
// Errado
class AlunoRepository(DbContext dbContext) : IDisposable
{
    public void Dispose()
    {
        dbContext.Dispose(); // Remova esta linha, pois dbContext é injetado e gerenciado externamente
    }
}
// Correto
class AlunoRepository : IDisposable
{
    private readonly DbContext dbContext;
    public AlunoRepository()
    {
        dbContext = new AppDbContext(); // A classe é responsável por criar e gerenciar o ciclo de vida do dbContext
    }
    public void Dispose()
    {
        dbContext.Dispose(); // Mantenha essa linha
    }
}

```
* Nos repositórias, a cada ação que modifica o estado do contexto, como `Add`, `Update`, ou `Remove`, você está chamando `SaveChangesAsync` imediatamente após. Isso pode ser ineficiente, especialmente se você estiver fazendo várias modificações em uma única operação. Considere acumular as mudanças e chamar `SaveChangesAsync` uma única vez no final da operação.
* O conceito de Repository Pattern é sobre a criação de repositórios específicos. Eles não deve retornar `IQueryable<T>`, mas sim coleções ou entidades específicas. Isso ajuda a encapsular a lógica de consulta e manter a separação de preocupações. Por exemplo, o `BuscaAlunos()` não está fazendo nenhuma busca, pelo contrário, está apenas expondo todo o `DbSet` permitindo consultas arbitrárias fora do repositório, o que quebra o encapsulamento.
* Em `AlunoDbContext`, no métodos `bool Commit()`, entendo que retorna booleano para indicar sucesso ou falha, o que pode trazer um falso negativo. Ex: Tentar excluir um registro que não existe, o `SaveChanges` retornará 0, e o método retornará `false`, mesmo que a operação tenha sido bem-sucedida.
* Assim como `DbSet`, nomes de tabelas devem ser no plural. 

### Api
* Em `AlunoController.ObterAlunoId(id)` espera um `id` como argumento, mas não o usa para nada. Pelo design, parece um endpoint para exibir dados de perfil do usuario autenticado, então uma controller de `Profile`/`Perfil` é mais apropriado.
* Rotas de API RESTful também devem ser no plural, então `api/alunos` é mais adequado.
* Em `ConteudoController`, a rota é `api/cursos`. Não faz sentido, o nome da controller e a rota devem estar alinhados.
* Em `ConteudoController`, possui dois endpoints com método `AlterarNomeCurso()`.
* Não é claro o endpoint para apenas renomear um nome de curso, esperar um `Model` completo para isso. Crie um `DTO` específico para essa ação.

### Application
* Observei que existe um único handler para tratar diversas mensagens de comando e consulta. Embora isso possa funcionar, não é uma prática recomendada. Cada comando ou consulta deve ter seu próprio handler dedicado. Isso melhora a clareza, facilita a manutenção, os testes, e não quebre o princípio de responsabilidade única (SRP, SOLID Principles).
* Os métodos `Handle()` aguardam um argumento do tipo `Command` mas com nome de `request`. Isso pode causar confusão, pois o nome do parâmetro não reflete seu tipo. Considere renomear o parâmetro para `command`, conforme apropriado.

### Core
* Interessante e confuso ver em `CommandHandler` o método `PersistirDados()`, que chama `uow.Commit()` sendo que este já é chamado em cada ação de repositório. Ainda que não ideal, manter apenas dentro do Handler é uma melhor opção.

### Geral
* Remover códigos comentados
* Remover `using` não utilizados
* É bem clara a separação de contextos delimitados, porém nào é ideal ter todas as responsabilidades em um único projeto. Considere quebrar os projetos por responsabilidade, como `WebAPI`, `Application`, `Domain`, `Infrastructure`, `CrossCutting` e `Tests`, etc.
* Não há testes, e neste módulo é necessário cobertura de mais de 80% das branchs/block de código.
* Revisem os comentários, existem muitos erros de digitação e gramática.

## Eficiência e Desempenho 20%

Avalie o desempenho e a eficiência das soluções implementadas.
* Em `AlunoCommandHandler` existe o seguinte código:
```csharp
        var ra = !_alunoRepository.BuscarAlunos().Any()
            ? 10000
            : (await _alunoRepository.BuscarUltimoRa()) + 1;
```
Ele é ineficiente e desnecessário. O método `BuscarUltimoRa()` pode retornar `10000` se o retorno de `MaxAsync()` for nulo. 
* Todos os handlers esperam um `CancellationToken` como argumento, mas nenhum o utiliza. Propagem o token para chamadas assíncronas que suportam cancelamento, como operações de banco de dados, para permitir que a operação seja cancelada corretamente.

## Inovação e Diferenciais 10%

Considere a criatividade e inovação na solução proposta.
* Será revisado na avalição final.


## Documentação e Organização 10%

Verifique a qualidade e completude da documentação, incluindo README.md.

* Comentários e recomendações
  - A navegação dos arquivos deve ser similar a navegação dentro da Solução. Procure seguir algo parecido com `<Solucao>/<Contexto>/<Projeto>`, para os arquivos, dentro da solução e também as `namespaces`.
  - A pasta `./src/api gateways` possui um espaço no nome, o que não é recomendado. Pode facilmente trazer problemas não esperados nas automações ou deixar o projeto sem suporte à multi plataforma.
  - Os passos de execução estão incompletos, portanto não vou conseguir avaliar as funcionalidades implementadas.

## Resolução de Feedbacks 10%

Avalie a resolução dos problemas apontados na primeira avaliação de frontend
* Será revisado na avalição final.

