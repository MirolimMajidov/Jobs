using Grpc.Net.Client;
using IdentityService.Protos;
using System;
using System.Threading.Tasks;

namespace TestGrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var data = new UserRequest { Id = "Id" };
            var grpcChannel = GrpcChannel.ForAddress("http://localhost:5005");
            var client = new TestUser.TestUserClient(grpcChannel);
            var response = await client.GetByIdAsync(data);
            Console.WriteLine(response);
            Console.ReadLine();
        }
    }
}
