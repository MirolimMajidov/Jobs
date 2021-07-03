using FluentAssertions;
using JobService.Controllers;
using JobService.Models;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;

namespace JobService.UnitTests
{
    [TestFixture]
    public class CategoryControllerTests : BaseTestEntity<Category, CategoryController>
    {
        [Test]
        public void GetAllCategories()
        {
            List<Category> entities = TestCategories();
            var dtoEntities = entities.Select(u => new CategoryDTO { Id = u.Id, Name = u.Name, Description = u.Description });

            mockRepository.Setup(p => p.GetEntities()).ReturnsAsync(entities);
            foreach (var entity in entities)
                mockMapper.Setup(p => p.Map<CategoryDTO>(entity)).Returns(dtoEntities.FirstOrDefault(d => d.Id == entity.Id));

            var result = controller.Get().Result;
            result.Should().NotBeNull();
        }

        [Test]
        public void GetCategoryById()
        {
        }

        List<Category> TestCategories()
        {
            return new List<Category>()
                        {
                            new Category
                            {
                                Name = "Web, Mobile & Software Dev",
                                Description = "Web Development, Mobile Development, Desktop Software Developmen, QA & Testing",
                            },
                            new Category
                            {
                                Name = "Sales & Marketing",
                                Description = "Sales & Marketing Strategy",
                            },
                            new Category
                            {
                                Name = "Design & Writing",
                                Description = "Design, Writing, Photography & Translator",
                            },
                            new Category
                            {
                                Name = "Engineering & Architecture",
                                Description = "Engineering & Architecture",
                            }
                        };
        }
    }
}
