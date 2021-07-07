using Grpc.Net.Client;
using IdentityClient.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TestGrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var grpcChannel = GrpcChannel.ForAddress("http://localhost:7101/");
            var client = new User.UserClient(grpcChannel);

            //Get all users
            Console.WriteLine("All users: ");
            var users = await GetUsers(client);
            foreach (var user in users)
                Console.WriteLine(user);

            Console.WriteLine();

            //Get user info by ID
            Console.WriteLine("User info: ");
            var userInfo = await GetUserById(client, users.FirstOrDefault()?.Id);
            Console.WriteLine(userInfo);
        }

        static async Task<List<UserReply>> GetUsers(User.UserClient client)
        {
            List<UserReply> users = new();
            using var clientData = client.GetUsers(new UsersRequest());
            while (await clientData.ResponseStream.MoveNext(new System.Threading.CancellationToken()))
            {
                var user = clientData.ResponseStream.Current;
                users.Add(user);
            }
            return users;
        }

        static async Task<UserReply> GetUserById(User.UserClient client, string userId)
        {
            return await client.GetUserByIdAsync(new UserRequest { Id = userId });
        }
    }
}
