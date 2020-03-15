using System.Collections.Generic;
using System.Threading.Tasks;
using Fanex.Athena.ExtService.TagHelpers;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Moq;
using Xunit;

namespace Fanex.Athena.ExtService.UnitTests.TagHelpers
{
    public class MoneyTagHelperTest
    {
        [Fact]
        public async Task MoneyTagHelper_WhenCall_ReturnCorrectOutput()
        {
            // Arrange
            const string TagContent = "1000";
            var mockTagHelperContent = new Mock<TagHelperContent>();
            mockTagHelperContent.Setup(s => s.GetContent()).Returns(TagContent);

            var tagHelperContext = new TagHelperContext(new TagHelperAttributeList(), new Dictionary<object, object>(), string.Empty);

            var tagHelperOutput = new TagHelperOutput(string.Empty, new TagHelperAttributeList(),
                (a, b) => Task.FromResult<TagHelperContent>(mockTagHelperContent.Object))
            {
                Content = mockTagHelperContent.Object
            };

            // Action
            await new MoneyTagHelper().ProcessAsync(tagHelperContext, tagHelperOutput);

            // Assert
            Assert.Equal("span", tagHelperOutput.TagName);
            Assert.Equal("class", tagHelperOutput.Attributes[0].Name);
            Assert.Equal("underdog", tagHelperOutput.Attributes[0].Value);
        }
    }
}