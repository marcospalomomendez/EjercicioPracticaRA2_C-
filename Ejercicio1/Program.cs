using System;
using System.Collections.Generic;

abstract class Vehiculo
{
    public string matricula { get; }
    private double consumopor100km;
    public double Consumopor100km
    {
        get => consumopor100km;
        set
        {
            if (value < 0)
                consumopor100km = 0.0;
            else
                consumopor100km = value;
        }
    }

    public double Costo_Operacional_Base { get => 0.15; }

    public Vehiculo(string matricula, double consumopor100km)
    {
        this.matricula = matricula;
        Consumopor100km = consumopor100km;
    }

    public abstract double CalcularCostoPorKm();

    public override string ToString()
    {
        return $"Matrícula: {matricula}, Consumo: {consumopor100km} L/100km";
    }
}

abstract class TransportePasajeros : Vehiculo
{
    private double capacidadMaxima;
    public double CapacidadMaxima
    {
        get => capacidadMaxima;
        set
        {
            if (value < 0)
                capacidadMaxima = 0.0;
            else
                capacidadMaxima = value;
        }
    }

    protected TransportePasajeros(string matricula, double consumopor100km, double capacidadMaxima)
        : base(matricula, consumopor100km)
    {
        CapacidadMaxima = capacidadMaxima;
    }

    public override string ToString()
    {
        return base.ToString() + $", Capacidad Máxima: {CapacidadMaxima} pasajeros";
    }
}

class Autobus : TransportePasajeros
{
    public double costoDegradante { get => 1.20; }

    public Autobus(string matricula, double consumopor100km, double capacidadMaxima)
        : base(matricula, consumopor100km, capacidadMaxima)
    { }

    public override double CalcularCostoPorKm()
    {
        // Coste Por KM = Consumo x CostoOperacionalBase x Factor de Desgaste
        return Consumopor100km * Costo_Operacional_Base * costoDegradante;
    }

    public override string ToString()
    {
        return "[Autobús] " + base.ToString();
    }
}

abstract class TransporteCarga : Vehiculo
{
    private double peajeAnual;
    public double PeajeAnual
    {
        get => peajeAnual;
        set
        {
            if (value < 0)
                peajeAnual = 0.0;
            else
                peajeAnual = value;
        }
    }

    protected TransporteCarga(string matricula, double consumopor100km, double peajeAnual)
        : base(matricula, consumopor100km)
    {
        PeajeAnual = peajeAnual;
    }

    public override string ToString()
    {
        return base.ToString() + $", Peaje Anual: {PeajeAnual} €";
    }
}

class Camion : TransporteCarga
{
    public double CostoFijoPeaje { get => PeajeAnual / 100000; }

    public Camion(string matricula, double consumopor100km, double peajeAnual)
        : base(matricula, consumopor100km, peajeAnual)
    { }

    public override double CalcularCostoPorKm()
    {
        // Coste Por KM = Consumo x CostoOperacionalBase x PeajeAnual / 100,000
        return Consumopor100km * Costo_Operacional_Base * (PeajeAnual / 100000.0);
    }

    public override string ToString()
    {
        return "[Camión] " + base.ToString();
    }
}

class Program
{
    static List<Vehiculo> flota = new List<Vehiculo>();

    static void Main()
    {
        int opcion;
        do
        {
            Console.WriteLine("\n=== FLEET MANAGER S.A. ===");
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
            if (!double.TryParse(Console.ReadLine(), out double capacidad))
                capacidad = 0.0;

            flota.Add(new Autobus(matricula, consumo, capacidad));
            Console.WriteLine("✅ Autobús registrado correctamente.");
        }
        else if (tipo == 2)
        {
            Console.Write("Peaje Anual (€): ");
            if (!double.TryParse(Console.ReadLine(), out double peaje))
                peaje = 0.0;

            flota.Add(new Camion(matricula, consumo, peaje));
            Console.WriteLine("✅ Camión registrado correctamente.");
        }
        else
        {
            Console.WriteLine("Tipo no válido.");
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

        foreach (var v in flota)
        {
            Console.WriteLine($"{v}\n  → Costo por Km: {v.CalcularCostoPorKm():0.0000} €");
        }
    }

    static void CalcularCostoTotalFlota()
    {
        const double distanciaFija = 100000.0;
        double total = 0.0;

        foreach (var v in flota)
            total += v.CalcularCostoPorKm() * distanciaFija;

        Console.WriteLine($"\nCosto total estimado de flota (100,000 km por vehículo): {total:0.00} €");
    }
}
