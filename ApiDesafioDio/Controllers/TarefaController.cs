using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(int id)
        {
            try
            {
                if (id <= 0)
                    return BadRequest(new { Erro = "Id incorreto" });

                var tarefa = _context.Tarefas.Find(id);

                if (tarefa == null)
                    NotFound("Tarefa não encontrada");

                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObterTodos")]
        public IActionResult ObterTodos()
        {
            try
            {
                var tarefas = _context.Tarefas.ToList();

                if (tarefas == null || tarefas.Count == 0)
                    return NotFound();
                else
                    return Ok(tarefas);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObterPorTitulo")]
        public IActionResult ObterPorTitulo(string titulo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(titulo))
                   return BadRequest(new { Erro = "titulo não pode ser nulo" });

                var tarefa = _context.Tarefas.Where(x => x.Titulo == titulo);

                if (tarefa == null)
                    return NotFound();

                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObterPorData")]
        public IActionResult ObterPorData(DateTime data)
        {
            try
            {
                if (data == DateTime.MinValue)
                    return BadRequest(new { Erro = "A data da tarefa não pode ser vazia." });
                var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);

                if (tarefa == null)
                    return NotFound();

                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ObterPorStatus")]
        public IActionResult ObterPorStatus(EnumStatusTarefa status)
        {
            try
            {
                var tarefa = _context.Tarefas.Where(x => x.Status == status);

                if (tarefa == null)
                    return NotFound();

                return Ok(tarefa);
            }
            catch (Exception ex)
            {
                return BadRequest(new { Erro = ex.Message });
            }
        }

        [HttpPost]
        public IActionResult Criar(Tarefa tarefa)
        {
            try
            {
                // Validar a entrada
                if (tarefa.Data == DateTime.MinValue)
                    return BadRequest(new { Erro = "A data da tarefa não pode ser vazia." });
                if (string.IsNullOrWhiteSpace(tarefa.Descricao))
                    return BadRequest(new { Erro = "A descrição da tarefa não pode estar vazia." });
                if (string.IsNullOrWhiteSpace(tarefa.Titulo))
                    return BadRequest(new { Erro = "O título da tarefa não pode estar vazio." });

                _context.Add(tarefa);
                _context.SaveChanges();

                // Retorna o status 201 (Created) e o URI para a tarefa criada
                return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
            }
            catch (Exception ex)
            {
                // Retorna uma mensagem de erro genérica em caso de exceção
                return BadRequest(new { Erro = "Ocorreu um erro ao criar a tarefa." });
            }
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            try
            {
                // Verifica se o ID da tarefa é válido
                if (id <= 0)
                    return BadRequest(new { Erro = "ID da tarefa inválido." });

                // Busca a tarefa no banco de dados
                var tarefaBanco = _context.Tarefas.FirstOrDefault(t => t.Id == id);

                // Verifica se a tarefa existe no banco de dados
                if (tarefaBanco == null)
                    return NotFound();

                // Verifica se a data da tarefa é válida
                if (tarefa.Data == DateTime.MinValue)
                    return BadRequest(new { Erro = "A data da tarefa não pode ser vazia." });

                // Atualiza os dados da tarefa
                tarefaBanco.Descricao = tarefa.Descricao;
                tarefaBanco.Titulo = tarefa.Titulo;
                tarefaBanco.Status = tarefa.Status;
                tarefaBanco.Data = tarefa.Data.Date;

                // Salva as alterações no banco de dados
                _context.SaveChanges();

                // Retorna um status 200 (OK) e a tarefa atualizada
                return Ok(tarefaBanco);
            }
            catch (Exception ex)
            {
                // Retorna uma mensagem de erro genérica em caso de exceção
                return BadRequest(new { Erro = "Ocorreu um erro ao atualizar a tarefa." });
            }
        }

        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            try
            {
                // Verifica se o ID da tarefa é válido
                if (id <= 0)
                    return BadRequest(new { Erro = "ID da tarefa inválido." });

                // Busca a tarefa no banco de dados
                var tarefaBanco = _context.Tarefas.FirstOrDefault(t => t.Id == id);

                // Verifica se a tarefa existe no banco de dados
                if (tarefaBanco == null)
                    return NotFound();

                // Remove a tarefa do contexto e salva as alterações no banco de dados
                _context.Remove(tarefaBanco);
                _context.SaveChanges();

                // Retorna um status 204 (NoContent) para indicar que a tarefa foi excluída com sucesso
                return NoContent();
            }
            catch (Exception ex)
            {
                // Retorna uma mensagem de erro genérica em caso de exceção
                return BadRequest(new { Erro = "Ocorreu um erro ao excluir a tarefa." });
            }
        }

    }
}