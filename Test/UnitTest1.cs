using WebApplication20.Helpers;

namespace Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void EmailValidationPositive()
        {
            if (Email.IsValidEmail("samplemail@mail.com"))
            {
                Assert.Pass();
            }
        Assert.Fail();
        }

        [Test]
        public void EmailValidationNegative()
        {
            if (Email.IsValidEmail("@samplemailmail.com"))
            {
                Assert.Pass();
            }
            Assert.Fail();
        }
    }
}