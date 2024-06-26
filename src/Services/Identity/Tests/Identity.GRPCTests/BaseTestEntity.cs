using Grpc.Net.Client;
using Microsoft.AspNetCore.TestHost;

namespace Identity.GRPCTests
{
    public abstract class BaseTestEntity
    {
        protected TestServer Server;
        protected GrpcChannel _grpcChannel;

        [SetUp]
        public void SetUp()
        {
            Server = new ServerApiFactory().Server;

            // Create a gRPC channel
            _grpcChannel = GrpcChannel.ForAddress(Server.BaseAddress, new GrpcChannelOptions
            {
                HttpClient = Server.CreateClient()
            });
        }

        [TearDown]
        public void TearDown()
        {
            Server?.Dispose();
            _grpcChannel?.Dispose();
            Server = null;
            _grpcChannel = null;
        }
    }
}
