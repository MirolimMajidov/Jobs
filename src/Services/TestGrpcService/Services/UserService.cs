using Grpc.Core;
using IdentityService.Protos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestGrpcService
{
    public class UserService : TestUser.TestUserBase
    {
        private readonly ILogger<UserService> _logger;
        public UserService(ILogger<UserService> logger)
        {
            _logger = logger;
        }

        public override Task<UserReply> GetById(UserRequest request, ServerCallContext context)
        {
            return Task.FromResult(new UserReply
            {
                Id = request.Id,
                Name = "Test"
            });
        }
    }
}
