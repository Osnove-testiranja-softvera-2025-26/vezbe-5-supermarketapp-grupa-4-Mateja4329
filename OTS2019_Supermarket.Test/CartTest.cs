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
        private Cart cart;
        [SetUp]
        public void Setup()
        {
            cart = new Cart();
        }

        [Test]
        public void AddOneToCart_ShouldAddItemToCart_Success()
        {
            cart.AddOneToCart(new Monitor());

            Assert.That(cart.Size, Is.EqualTo(1));
        }

        [Test]
        public void AddOneToCart_WhenCartIsFull_ShouldThrowException()
        {
            // Arrange
            cart.Size = 10;

            // Act
            Assert.Throws<Exception>(() => cart.AddOneToCart(new Monitor()));

            // Assert
            Assert.That(cart.Size, Is.EqualTo(10),
                "Cart size should remain 10 when trying to add an item to a full cart.");
        }

        [Test]
        public void AddMultipleItemsToCart_ShouldAddItemsToCart_Success()
        {
            // Arrange
            Monitor monitor = new Monitor();

            // Act
            cart.AddMultipleToCart(monitor, 3);

            // Assert
            Assert.That(cart.Size, Is.EqualTo(3), "Cart size should be 3 after adding 3 items to the cart.");
        }

        [Test]
        public void AddMultipleToCart_WhenAddingMoreThanCapacity_ShouldThrowException()
        {
            Assert.Throws<Exception>(() => cart.AddMultipleToCart(new Monitor(), 11));

            Assert.That(cart.Size, Is.EqualTo(0),
                "Cart size should remain 0 when trying to add more items than capacity.");
        }

        [TestCase(0, 1)]
        [TestCase(5, 6)]
        public void AddOneItem_TDD_Success(int initSize, int result)
        {
            // Arrange
            cart.Size = initSize;

            // Act
            cart.AddOneToCart(new Monitor());

            // Assert
            Assert.That(cart.Size, Is.EqualTo(result));
        }

        [Test]
        public void DeleteAll_deleteItems_Success()
        {
            cart.AddMultipleToCart(new Keyboard(), 5);
            cart.DeleteAll();

            Assert.That(cart.Items.Count, Is.EqualTo(0));
        }

        [Test]
        public void DeleteAll_HasNoItemsToDelete_Exception()
        {
            Exception exception = Assert.Throws<Exception>(() => cart.DeleteAll());

            Assert.That(exception.Message, Is.EqualTo("Cannot restore empty cart!"));
        }

        [Test]
        public void Print_PrintCart_Success()
        {
            // Arrange
            Laptop laptop = new Laptop();

            // Act
            cart.AddOneToCart(laptop);
            string printResult = cart.Print();

            // Assert
            Assert.That(printResult, Is.EqualTo(laptop.ToString()));
        }

        [Test]
        public void Print_TryPrintingEmptyCart_Exception()
        {
            Exception exception = Assert.Throws<Exception>(() => cart.Print());

            Assert.That(exception.Message, Is.EqualTo("Cannot print empty cart!"));
        }

        [Test]
        public void Calculate_DateTimeIsToday_Exception()
        {
            // Arrange
            String invalidDate = DateTime.Now.ToString("yyyy-MM-dd");

            // Act
            Exception exception = Assert.Throws<Exception>(() => cart.Calculate(invalidDate));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Date of delivery can't be today's date!"));
        }

        [Test]
        public void Calculate_DateTimeIsMoreThan7Days_Exception()
        {
            // Arrange
            String invalidDate = DateTime.Now.AddDays(8).ToString("yyyy-MM-dd");

            // Act
            Exception exception = Assert.Throws<Exception>(() => cart.Calculate(invalidDate));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Days for delivery must be less than 7!"));
        }

        [Test]
        public void Calculate_DateTimeIsInWrongFormat_Exception()
        {
            // Arrange
            String invalidDate = DateTime.Now.ToString("MM-dd-yyyy");

            // Act
            Exception exception = Assert.Throws<Exception>(() => cart.Calculate(invalidDate));

            // Assert
            Assert.That(exception.Message, Is.EqualTo("Wrong date format! Date must be in format yyyy-MM-dd"));
        }

        [TestCaseSource(typeof(CartTxtParser), "GetTestCaseData", new object[] { "PICTResults.txt" })]
        public void Calculate_PICTTest_ReturnExpectedDiscount(string stringDate, int amount, int cartSize, int laptopCount, int monitorCount, int computerCount, int chairCount, int keyboardCount, double expectedDiscount)
        {
            // 1. ARRANGE
            cart.Size = cartSize;
            cart.Amount = amount;
            cart.Laptop_counter = laptopCount;
            cart.Monitor_counter = monitorCount;
            cart.Computer_counter = computerCount;
            cart.Chair_counter = chairCount;
            cart.Keyboard_counter = keyboardCount;

            double initialBudget = 10000;
            cart.Budget = initialBudget;

            // 2. ACT
            cart.Calculate(stringDate);

            // 3. ASSERT
            // Računamo koliko bi trebalo da košta sa našim očekivanim PICT popustom
            double expectedPrice = amount - (amount * expectedDiscount);
            // Računamo koliko bi para trebalo da ostane u kasi
            double expectedRemainingBudget = initialBudget - expectedPrice;

            // Proveravamo da li se budžet u aplikaciji poklapa sa našom matematikom
            Assert.That(expectedRemainingBudget, Is.EqualTo(cart.Budget));
        }
    }
}
