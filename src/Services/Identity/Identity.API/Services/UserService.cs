using Grpc.Core;
using IdentityService.Protos;
using Jobs.Service.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using static IdentityService.Protos.User;

namespace IdentityService.Services
{
    public class UserService : UserBase
    {
        private readonly IEntityQueryableRepository<Models.User> _repository;
        private readonly ILogger _logger;

        public UserService(ILogger<UserService> logger, IEntityQueryableRepository<Models.User> repository)
        {
            _logger = logger;
            _repository = repository;
        }

        public override async Task<UserReply> GetUserById(UserRequest request, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call UserService.GetUserById for user id {Id}", request.Id);

            if (Guid.TryParse(request.Id, out Guid userId))
            {
                var user = await _repository.GetEntityByID(userId);
                if (user == null)
                {
                    context.Status = new Status(StatusCode.NotFound, $"User with id {userId} do not exist");
                    return null;
                }

                return await Task.FromResult(ConvertToGRPCUser(user));
            }
            else
            {
                context.Status = new Status(StatusCode.InvalidArgument, $"Id must be Guid type (received {request.Id})");
                return null;
            }
        }

        public override async Task GetUsers(UsersRequest request, IServerStreamWriter<UserReply> responseStream, ServerCallContext context)
        {
            _logger.LogInformation("Begin grpc call UserService.GetUsers");

            var users = await _repository.GetEntities();
            foreach (var user in users)
                await responseStream.WriteAsync(ConvertToGRPCUser(user));
        }

        static UserReply ConvertToGRPCUser(Models.User user)
        {
            return new UserReply
            {
                Id = user.Id.ToString(),
                Name = user.Name
            };
        }
    }
}
