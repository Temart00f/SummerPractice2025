using System;

namespace task04;

public interface ISpaceship
{
    string MoveForward();      // Движение вперед
    string Rotate(int angle);  // Поворот на угол (градусы)
    string Fire();             // Выстрел ракетой
    string StopMoving();       // Отсановка
    int Speed { get; }       // Скорость корабля
    int FirePower { get; }   // Мощность выстрела
}

public class Cruiser() : ISpaceship
{
    public int Speed { get; } = 50;
    public int FirePower { get; } = 500;
    public string MoveForward()
    {
        return $"Крейсер летит вперед со скоростью {Speed}!";
    }
    public string Rotate(int angle)
    {
        return $"Крейсер совершает поворот на {angle}°";
    }
    public string Fire()
    {
        return $"Крейсер выпускает залп и наносит {FirePower} урона";
    }
    public string StopMoving()
    {
        return "Крейсер остановился";
    }
}

public class Fighter() : ISpaceship
{
    public int Speed { get; } = 100;
    public int FirePower { get; } = 250;
    public string MoveForward()
    {
        return $"Истребитель летит вперед со скоростью {Speed}!";
    }
    public string Rotate(int angle)
    {
        return $"Истребитель совершает поворот на {angle}°";
    }
    public string Fire()
    {
        return $"Истребитель выпускает залп и наносит {FirePower} урона";
    }
    public string StopMoving()
    {
        return "Истребитель остановился";
    }
}
