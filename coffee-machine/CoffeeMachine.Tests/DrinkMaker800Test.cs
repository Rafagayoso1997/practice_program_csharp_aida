﻿using CoffeeMachine.Core;
using CoffeeMachine.Infrastructure;
using NSubstitute;
using NUnit.Framework;

namespace CoffeeMachine.Tests
{
    public class DrinkMaker800Test
    {
        [Test]
        public void Serve_Coffee()
        {
            var drinkMaker = Substitute.For<DrinkMaker>();
            var drinkMaker800 = new DrinkMaker800(drinkMaker);

            var order = new Order()
            {
                Drink = "Coffee"
            };

            drinkMaker800.Serve(order);

            
            drinkMaker.Received().Execute("C::");
        }

        [Test]
        public void Serve_Tea() {
            var drinkMaker = Substitute.For<DrinkMaker>();
            var drinkMaker800 = new DrinkMaker800(drinkMaker);

            var order = new Order() {
                Drink = "Tea"
            };

            drinkMaker800.Serve(order);


            drinkMaker.Received().Execute("T::");
        }

        [Test]
        public void Serve_Chocolate() {
            var drinkMaker = Substitute.For<DrinkMaker>();
            var drinkMaker800 = new DrinkMaker800(drinkMaker);

            var order = new Order() {
                Drink = "Chocolate"
            };

            drinkMaker800.Serve(order);


            drinkMaker.Received().Execute("H::");
        }
    }
}
