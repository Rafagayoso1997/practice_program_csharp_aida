using System.Collections.Generic;

namespace CoffeeMachineApp.core;

public class CoffeeMachine
{
    private readonly DrinkMakerDriver _drinkMakerDriver;
    private readonly Dictionary<DrinkType, decimal> _prices;
    private Order _order;
    private decimal _totalMoney;
    private readonly MessageNotificator _messageNotificator;


    public CoffeeMachine(DrinkMakerDriver drinkMakerDriver, Dictionary<DrinkType, decimal> prices,
        MessageNotificator messageNotificator)
    {
        _drinkMakerDriver = drinkMakerDriver;
        _prices = prices;
        InitializeState();
        _messageNotificator = messageNotificator;
    }

    public void SelectChocolate()
    {
        _order.SelectDrink(DrinkType.Chocolate);
    }

    public void SelectTea()
    {
        _order.SelectDrink(DrinkType.Tea);
    }

    public void SelectCoffee()
    {
        _order.SelectDrink(DrinkType.Coffee);
    }

    public void AddOneSpoonOfSugar()
    {
        _order.AddSpoonOfSugar();
    }

    public void AddMoney(decimal money)
    {
        _totalMoney += money;
    }

    public void MakeDrink()
    {
        if (NoDrinkWasSelected())
        {
            _messageNotificator.NotifySelectDrink();
            return;
        }

        if (IsThereEnoughMoney())
        {
            _drinkMakerDriver.Send(_order);
            InitializeState();
        }
        else
        {
            _messageNotificator.NotifyMissingAmount(ComputeMissingMoney());
        }
    }

    private void InitializeState()
    {
        _totalMoney = 0;
        _order = new Order();
    }

    private decimal ComputeMissingMoney()
    {
        return _prices[_order.GetDrinkType()] - _totalMoney;
    }

    private bool IsThereEnoughMoney()
    {
        return _totalMoney >= _prices[_order.GetDrinkType()];
    }

    private bool NoDrinkWasSelected()
    {
        return _order.GetDrinkType() == DrinkType.None;
    }
}