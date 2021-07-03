using IdentityService.Controllers;
using IdentityService.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Identity.UnitTests
{
    [TestClass]
    public class UserControllerTests : BaseTestEntity<User, UserController>
    {
        [TestMethod]
        public void GetAllUsers()
        {            
        }

        [TestMethod]
        public void GetUserById()
        {
        }
    }
}
