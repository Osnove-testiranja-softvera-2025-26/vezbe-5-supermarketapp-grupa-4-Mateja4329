using NUnit.Framework;
using OTS_Supermarket.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OTS_Supermarket.Test
{
    [TestFixture]
    public class CartTest
    {
        // ok
        [Test]
        public void AddOneToCart_ShouldAddItemToCart_Success()
        {
            // Arrange
            Cart cart = new Cart();
            OTS_Supermarket.Models.Monitor monitor = new OTS_Supermarket.Models.Monitor("Dell", 1000);

            // Act
            cart.AddOneToCart(monitor);

            // Assert
            Assert.That(cart.Size, Is.EqualTo(1));

        }
    }
}
