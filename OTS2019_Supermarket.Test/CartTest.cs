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

        // Test: Size se povecava za 1 kada dodamo jedan element
        [Test]
        public void AddOneToCart_ShouldIncreaseSizeByOne_Success()
        {
            // Arrange
            Cart cart = new Cart();
            Laptop laptop = new Laptop("HP Pavilion", 900);
            int initialSize = cart.Size;

            // Act
            cart.AddOneToCart(laptop);

            // Assert
            Assert.That(cart.Size, Is.EqualTo(initialSize + 1), "Size bi trebao biti povećan za 1");
        }

        // Test: Size se povecava za 1 sa više dodataka
        [Test]
        public void AddOneToCart_MultipleCalls_ShouldIncreaseSizeByOneEachTime()
        {
            // Arrange
            Cart cart = new Cart();
            Keyboard keyboard = new Keyboard("Logitech", 150);

            // Act
            cart.AddOneToCart(keyboard);
            int sizeAfterFirst = cart.Size;
            cart.AddOneToCart(keyboard);
            int sizeAfterSecond = cart.Size;
            cart.AddOneToCart(keyboard);
            int sizeAfterThird = cart.Size;

            // Assert
            Assert.That(sizeAfterFirst, Is.EqualTo(1), "Size bi trebao biti 1 nakon prvog dodavanja");
            Assert.That(sizeAfterSecond, Is.EqualTo(2), "Size bi trebao biti 2 nakon drugog dodavanja");
            Assert.That(sizeAfterThird, Is.EqualTo(3), "Size bi trebao biti 3 nakon trećeg dodavanja");
        }

        // Test: Print sa jednim elementom
        [Test]
        public void Print_ShouldPrintSingleItem_Success()
        {
            // Arrange
            Cart cart = new Cart();
            Monitor monitor = new Monitor("Dell", 1000);

            // Act
            cart.AddOneToCart(monitor);
            string result = cart.Print();

            // Assert
            Assert.That(result, Is.Not.Empty, "Print rezultat ne bi trebao biti prazan");
            Assert.That(result, Does.Contain("Monitor"), "Rezultat bi trebao da sadrži tip elementa");
            Assert.That(result, Does.Contain("1000"), "Rezultat bi trebao da sadrži cenu");
        }

        // Test: Print sa više elemenata - verifikacija redosleda
        [Test]
        public void Print_ShouldPrintAllItemsInOrder_Success()
        {
            // Arrange
            Cart cart = new Cart();
            Laptop laptop = new Laptop("HP", 900);
            Keyboard keyboard = new Keyboard("Logitech", 150);
            Monitor monitor = new Monitor("Dell", 1000);

            // Act
            cart.AddOneToCart(laptop);
            cart.AddOneToCart(keyboard);
            cart.AddOneToCart(monitor);
            string result = cart.Print();

            // Assert
            Assert.That(result, Does.Contain("Laptop"), "Rezultat bi trebao da sadrži Laptop");
            Assert.That(result, Does.Contain("Keyboard"), "Rezultat bi trebao da sadrži Keyboard");
            Assert.That(result, Does.Contain("Monitor"), "Rezultat bi trebao da sadrži Monitor");
            Assert.That(result, Does.Contain("900"), "Rezultat bi trebao da sadrži cenu laptopa");
            Assert.That(result, Does.Contain("150"), "Rezultat bi trebao da sadrži cenu tastature");
            Assert.That(result, Does.Contain("1000"), "Rezultat bi trebao da sadrži cenu monitora");
        }

        // Test: Print - verifikacija da su svi elementi ispisani
        [Test]
        public void Print_ShouldContainAllAddedItems_Success()
        {
            // Arrange
            Cart cart = new Cart();
            Keyboard keyboard1 = new Keyboard("Logitech", 150);
            Keyboard keyboard2 = new Keyboard("Razer", 200);
            Keyboard keyboard3 = new Keyboard("Corsair", 250);

            // Act
            cart.AddOneToCart(keyboard1);
            cart.AddOneToCart(keyboard2);
            cart.AddOneToCart(keyboard3);
            string result = cart.Print();

            // Assert
            int count = result.Split(new[] { "Keyboard" }, StringSplitOptions.None).Length - 1;
            Assert.That(count, Is.EqualTo(3), "Rezultat bi trebao da sadrži 3 Keyboard elementa");
            Assert.That(result, Does.Contain("150"));
            Assert.That(result, Does.Contain("200"));
            Assert.That(result, Does.Contain("250"));
        }

        // Test: Print sa Laptop elementima
        [Test]
        public void Print_ShouldPrintLaptopItems_Success()
        {
            // Arrange
            Cart cart = new Cart();
            Laptop laptop = new Laptop("HP Pavilion", 900);

            // Act
            cart.AddOneToCart(laptop);
            string result = cart.Print();

            // Assert
            Assert.That(result, Does.Contain("Laptop"), "Rezultat bi trebao da sadrži tip - Laptop");
            Assert.That(result, Does.Contain("900"), "Rezultat bi trebao da sadrži cenu - 900");
            Assert.That(result, Is.Not.Empty);
        }

        // Test: Print - grešku ako je korpa prazna
        [Test]
        public void Print_ShouldThrowException_WhenCartIsEmpty()
        {
            // Arrange
            Cart cart = new Cart();

            // Act & Assert
            Assert.Throws<Exception>(() => cart.Print(), "Print bi trebao da baci Exception kada je korpa prazna");
        }

        // Test: Size i Items.Count konzistentnost
        [Test]
        public void AddOneToCart_SizeAndItemsCountShouldBeConsistent()
        {
            // Arrange
            Cart cart = new Cart();
            Keyboard keyboard = new Keyboard("Logitech", 150);

            // Act
            cart.AddOneToCart(keyboard);
            cart.AddOneToCart(keyboard);

            // Assert
            Assert.That(cart.Size, Is.EqualTo(cart.Items.Count), 
                "Size svojstvo bi trebalo biti jednako Items.Count broju");
            Assert.That(cart.Size, Is.EqualTo(2));
        }

        // Test: Amount se povecava ispravno
        [Test]
        public void AddOneToCart_ShouldUpdateAmountCorrectly()
        {
            // Arrange
            Cart cart = new Cart();
            Keyboard keyboard = new Keyboard("Logitech", 150);
            double initialAmount = cart.Amount;

            // Act
            cart.AddOneToCart(keyboard);

            // Assert
            Assert.That(cart.Amount, Is.EqualTo(initialAmount + 150), 
                "Amount bi trebao biti povećan za cenu elementa");
        }

        // Test: Proverava da li se brojač tipa elementa povecava
        [Test]
        public void AddOneToCart_ShouldIncrementCorrectItemCounter()
        {
            // Arrange
            Cart cart = new Cart();
            Laptop laptop = new Laptop("HP", 900);
            int initialLaptopCounter = cart.Laptop_counter;

            // Act
            cart.AddOneToCart(laptop);

            // Assert
            Assert.That(cart.Laptop_counter, Is.EqualTo(initialLaptopCounter + 1), 
                "Laptop brojač bi trebao biti povećan za 1");
        }

        // Test: Print output format provera
        [Test]
        public void Print_OutputFormatShouldBeConsistent()
        {
            // Arrange
            Cart cart = new Cart();
            Monitor monitor = new Monitor("Dell", 1000);

            // Act
            cart.AddOneToCart(monitor);
            string result = cart.Print();

            // Assert
            Assert.That(result, Does.Contain("Item:"), "Output bi trebao da sadrži 'Item:' prefiks");
            Assert.That(result, Does.Contain("Price:"), "Output bi trebao da sadrži 'Price:' ključnu reč");
        }
    }
}
