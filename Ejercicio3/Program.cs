using System;
using System.Collections.Generic;


abstract class Vehiculo
{
    public String Matricula;

    private double consumo;

    public Vehiculo(String matricula, double consumo)
    {
        Consumo = consumo;
        Matricula = matricula;
    }
    public double Consumo
    {
        get => consumo;
        set
        {
            if (value > 0)
            {
                consumo = value;
            }
            else
            {
                consumo = 0.0;
            }
        }
    }


    public double costoBase { get => 0.20; }

    public abstract double calcularCoste();


    public override string ToString()
    {
        return $"Matricula: {Matricula} | Consumo: {Consumo:0.000}";
    }
}

abstract class Transporte : Vehiculo
{
    private int capacidadMaxima;

    protected Transporte(string matricula, double consumo, int capacidadMaxima) : base(matricula, consumo)
    {
        CapacidadMaxima = capacidadMaxima;
    }

    public int CapacidadMaxima
    {
        get => capacidadMaxima;
        set
        {
            if(value > 0)
            {
                capacidadMaxima = value;
            }
            else
            {
                capacidadMaxima = 0;
            }
        }
    }

    public override string ToString()
    {
        return base.ToString() + $" | Capacidad Máxima: {CapacidadMaxima}";
    }
}

class Autobuses : Transporte
{
    public double FactorDesgaste { get => 1.15; }
    public Autobuses(string matricula, double consumo, int capacidadMaxima) : base(matricula, consumo, capacidadMaxima)
    {
    }

    public override double calcularCoste()
    {
        return Consumo * costoBase * FactorDesgaste;
    }

    public override string ToString()
    {
        return "[Autobus Eléctrico] " + base.ToString();
    }
}

abstract class Mantenimiento : Vehiculo
{
    private double gastoMantenimiento;

    protected Mantenimiento(string matricula, double consumo, double gastoMantenimiento) : base(matricula, consumo)
    {
        GastoMantenimiento = gastoMantenimiento;
    }

    public double GastoMantenimiento
    {
        get => gastoMantenimiento;
        set
        {
            if (value > 0)
            {
                gastoMantenimiento = value;
            }
            else
            {
                gastoMantenimiento = 0;
            }
        }
    }

    public override string ToString()
    {
        return base.ToString() + $" | Gasto Mantenimiento: {GastoMantenimiento}";
    }
}

class Camiones : Mantenimiento
{
    public Camiones(string matricula, double consumo, double gastoMantenimiento) : base(matricula, consumo, gastoMantenimiento)
    {
    }

    public double CostoFijoPorKm { get => GastoMantenimiento / 100000; }
   

    public override double calcularCoste()
    {
        return Consumo * costoBase + CostoFijoPorKm;
    }

    public override string ToString()
    {
        return "[Camiones Eléctrico] " + base.ToString();
    }
}

class Program
{
    static List<Vehiculo> flota = new List<Vehiculo>();

    static void Main()
    {
        int opcion = 0;

        do
        {
            Console.WriteLine("\n=== EcoDelivery S.A. ===");
            Console.WriteLine("1. Registrar Vehículo");
            Console.WriteLine("2. Ver Costos Operacionales");
            Console.WriteLine("3. Calcular Costo Total de Flota (100,000 km)");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opción: ");
            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                Console.WriteLine("Opción no válida.");
                continue;
            }

            switch (opcion)
            {
                case 1: RegistrarVehiculo(); break;
                case 2: VerCostosOperacionales(); break;
                case 3: CalcularCostoTotalFlota(); break;
                case 4: Console.WriteLine("Saliendo del sistema..."); break;
                default: Console.WriteLine("Opción no válida."); break;
            }

        } while (opcion != 4);
    }

    static void RegistrarVehiculo()
    {
        Console.WriteLine("\nTipo de vehículo:");
        Console.WriteLine("1. Autobús");
        Console.WriteLine("2. Camión");
        Console.Write("Seleccione: ");
        if (!int.TryParse(Console.ReadLine(), out int tipo)) return;

        Console.Write("Matrícula: ");
        string matricula = Console.ReadLine();

        Console.Write("Consumo (L/100km): ");
        if (!double.TryParse(Console.ReadLine(), out double consumo))
            consumo = 0.0;

        if (tipo == 1)
        {
            Console.Write("Capacidad Máxima: ");
            if (!int.TryParse(Console.ReadLine(), out int capacidad))
            {
                capacidad = 0;
            }

            flota.Add(new Autobuses(matricula, consumo, capacidad));
            Console.WriteLine("Autobús registrado correctamente.");
        }
        else if (tipo == 2)
        {
            Console.Write("Costo Mantenimiento Anual (€): ");
            if (!double.TryParse(Console.ReadLine(), out double mantenimiento)){
                mantenimiento = 0.0;
            }
            flota.Add(new Camiones(matricula, consumo, mantenimiento));
            Console.WriteLine("Camión registrado correctamente.");

        }
    }
    static void VerCostosOperacionales()
    {
        Console.WriteLine("\n--- COSTOS OPERACIONALES ---");
        if (flota.Count == 0)
        {
            Console.WriteLine("No hay vehículos registrados.");
            return;
        }
        else
        {
            foreach (var flota in flota)
            {
                Console.WriteLine(flota.ToString() + $" | Costo Operacional: {flota.calcularCoste():0.00}");
            }
        }

    }
    static void CalcularCostoTotalFlota()
    {
        Console.WriteLine("\n--- COSTOS TOTAL OPERACIONALES ---");
        double suma = 0;
        if (flota.Count == 0)
        {
            Console.WriteLine("No hay vehículos registrados.");
            return;
        }
        else
        {
            foreach (var flota in flota)
            {
                suma += flota.calcularCoste() * 100000;
            }
            Console.WriteLine($"\nCosto total estimado de flota (100,000 km por vehículo): {suma:0.00} €");

        }
    }
}