using System.Threading.Tasks;
using Todo.Domain.Commands;
using Todo.Domain.Entities;
using Todo.Domain.Handlers.Contracts;
using Todo.Domain.Repositories;

namespace Todo.Domain.Handlers
{
    public class TodoHandler : IHandler<CreateTodoCommand>,
                               IHandler<UpdateTodoCommand>,
                               IHandler<MarkTodoAsDoneCommand>,
                               IHandler<MarkTodoAsUndoneCommand>
    {
        private readonly ITodoRepository _repository;
        private int _statusCode;
        private GenericCommandResult _commandResult = new GenericCommandResult();

        public TodoHandler(ITodoRepository repository)
        {
            _repository = repository;
        }

        public async Task<GenericCommandResult> Handle(CreateTodoCommand command)
        {
            // TODO: Fail Fast Validation

            var todoItem = new TodoItem(command.Title, command.User, command.Date);

            var todoCreated = await _repository.Create(todoItem);

            if(todoCreated == null)
            {
                _commandResult.StatusCode = 400;
                _commandResult.Message = "Aconteceu algo de errado no ato do cadastro da Tarefa, tente novamente.";
                return _commandResult;
            }

            return new GenericCommandResult(true, "Tarefa salva.", todoCreated, 201);
        }

        public async Task<GenericCommandResult> Handle(UpdateTodoCommand command)
        {
           // TODO: Fail Fast Validation

            var todoItemFound = await _repository.GetById(command.Id);

            if(todoItemFound == null)
            {
                _commandResult.StatusCode = 404;
                _commandResult.Message = "Tarefa não existe.";
                return _commandResult;
            }
            
            todoItemFound.UpdateTitle(command.Title);

            await _repository.Update(todoItemFound);

            return new GenericCommandResult(true, "Tarefa atualizada.", todoItemFound, 200);
        }

        public async Task<GenericCommandResult> Handle(MarkTodoAsDoneCommand command)
        {
            // TODO: Fail Fast Validation

            var todoItemFound = await _repository.GetById(command.Id);

            if(todoItemFound == null)
            {
                _commandResult.StatusCode = 404;
                _commandResult.Message = "Tarefa não existe.";
                return _commandResult;
            }

            todoItemFound.MarkAsDone();

            await _repository.Update(todoItemFound);

            return new GenericCommandResult(true, "Tarefa atualizada.", todoItemFound, 200);
        }

        public async Task<GenericCommandResult> Handle(MarkTodoAsUndoneCommand command)
        {
            // TODO: Fail Fast Validation

            var todoItemFound = await _repository.GetById(command.Id);

            if(todoItemFound == null)
            {
                _commandResult.StatusCode = 404;
                _commandResult.Message = "Tarefa não existe.";
                return _commandResult;
            }

            todoItemFound.MarkAsUndone();

            var tudoUpdate = await _repository.Update(todoItemFound);

            return new GenericCommandResult(true, "Tarefa atualizada.", tudoUpdate);
        }
    }
}