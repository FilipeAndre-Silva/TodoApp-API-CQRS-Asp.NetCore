using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Todo.Domain.Commands;
using Todo.Domain.Entities;
using Todo.Domain.Handlers;
using Todo.Domain.Repositories;

namespace Todo.Domain.Api.Controllers
{
    [ApiController]
    [Route("v1/todos")]
    public class TodoController : ControllerBase
    {
        [Route("")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAll([FromServices]ITodoRepository repository)
        {
            var listTodoItens = await repository.GetAll();

            if(!listTodoItens.Any())
            {
                return NoContent();
            }
            else
            {
                return Ok(listTodoItens);
            }
        }

        [Route("{todoId:Guid}")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetById([FromServices]ITodoRepository repository,
                                                                      Guid todoId)
        {
            var todoItem = await repository.GetById(todoId);

            if(todoItem == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(todoItem);
            }
        }

        [Route("done")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllDone([FromServices]ITodoRepository repository)
        {
            var listTodoItensDone = await repository.GetAllDone();

            if(!listTodoItensDone.Any())
            {
                return NoContent();
            }
            else
            {
                return Ok(listTodoItensDone);
            }
        }

        [Route("undone")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetAllUndone([FromServices]ITodoRepository repository)
        {
            var listTodoItensUndone = await repository.GetAllUndone();

            if(!listTodoItensUndone.Any())
            {
                return NoContent();
            }
            else
            {
                return Ok(listTodoItensUndone);
            }
        }

        [Route("done/today")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetDoneForToday(string user, 
                                                                               [FromServices]ITodoRepository repository)
        {
            var listTodoItensDoneForToday = await repository.GetByPeriod(user,
                                                                         DateTime.Now.Date,
                                                                         true);
            if(!listTodoItensDoneForToday.Any())
            {
                return NoContent();
            }
            else
            {
                return Ok(listTodoItensDoneForToday);
            }
        }

        [Route("done/tomorrow")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItem>>> GetDoneForTomorrow(string user,
                                                                                  [FromServices]ITodoRepository repository)
        {
            var listTodoItensDoneForTomorrow = await repository.GetByPeriod(user,
                                                                            DateTime.Now.Date.AddDays(1),
                                                                            true);
            if(!listTodoItensDoneForTomorrow.Any())
            {
                return NoContent();
            }
            else
            {
                return Ok(listTodoItensDoneForTomorrow);
            }
        }

        [Route("")]
        [HttpPost]
        public async Task<ObjectResult> Create([FromBody]CreateTodoCommand command,
                                               [FromServices]TodoHandler handler)
        {
            var commandResult = await handler.Handle(command);
            return StatusCode(commandResult.StatusCode, commandResult);
        }

        [Route("")]
        [HttpPut]
        public async Task<ObjectResult> Update([FromBody]UpdateTodoCommand command,
                                               [FromServices]TodoHandler handler)
        {
            var commandResult = await handler.Handle(command);
            return StatusCode(commandResult.StatusCode, commandResult);
        }

        [Route("mark-as-done")]
        [HttpPut]
        public async Task<ObjectResult> MarkAsDone([FromBody]MarkTodoAsDoneCommand command,
                                                   [FromServices]TodoHandler handler)
        {
            var commandResult = await handler.Handle(command);
            return StatusCode(commandResult.StatusCode, commandResult);
        }

        [Route("mark-as-undone")]
        [HttpPut]
        public async Task<ObjectResult> MarkAsUndone([FromBody]MarkTodoAsUndoneCommand command,
                                                             [FromServices]TodoHandler handler)
        {
            var commandResult = await handler.Handle(command);
            return StatusCode(commandResult.StatusCode, commandResult);
        }
    }
}