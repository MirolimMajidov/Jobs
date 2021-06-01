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
            var grpcChannel = GrpcChannel.ForAddress("http://localhost:5101/");
            var client = new User.UserClient(grpcChannel);
            using (var clientData = client.GetUsers(new UsersRequest()))
            {
                while (await clientData.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
                {
                   var user = clientData.ResponseStream.Current;
                    Console.WriteLine(user);
                }
            }
            Console.ReadLine();

            var userResponce = await client.GetUserByIdAsync(new UserRequest { Id = "1987cf71-5519-440b-9cb9-7f62cbadfba8" });
            Console.WriteLine(userResponce);
            Console.ReadLine();
        }
    }
}
