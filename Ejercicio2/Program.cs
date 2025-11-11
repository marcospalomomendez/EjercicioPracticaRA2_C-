using System;
using System.Collections.Generic;
using System.Reflection.PortableExecutable;
using System.Runtime.InteropServices;

public abstract class Embarcacion
{
    private int id;

    public int ID
    {
        get => id;
        set
        {
            if (value > 0)
            {
                id = value;
            }
            else
            {
                id = 0;
            }
        }
    }

    private double consumoCombustible10millas;

    public double ConsumoCombustible10Millas
    {
        get => consumoCombustible10millas;
        set
        {
            if (value < 0)
            {
                consumoCombustible10millas = 0.0;
            }
            else
            {
                consumoCombustible10millas = value;
            }
        }
    }

    public double costoCombustibleFijo{ get => 0.20;}

    public abstract double CostoMilla();

    public Embarcacion(int id, double consumoCombustible10millas)
    {
        ID = id;
        ConsumoCombustible10Millas = consumoCombustible10millas;
    }

    public override string ToString()
    {
        return $"ID: {id} | Consumo de Combustible: {consumoCombustible10millas} L/10 millas | ";
    }
}

public abstract class EmbarcacionPasajeros : Embarcacion
{
    private int capacidadMaximaPasajeros;

    public int CapacidadMaximaPasajeros
    {
        get => capacidadMaximaPasajeros;
        set
        {
            if (value > 0)
            {
                capacidadMaximaPasajeros = value;
            }
            else
            {
                capacidadMaximaPasajeros = 0;
            }
        }
    }
    protected EmbarcacionPasajeros(int id, double consumoCombustible10millas, int capacidadMaximaPasajeroscapa) : base(id, consumoCombustible10millas)
    {
        CapacidadMaximaPasajeros = capacidadMaximaPasajeroscapa;
    }

    public override string ToString()
    {
        return base.ToString() + $"Capacidad Máxima : {CapacidadMaximaPasajeros}";
    }
}

class Ferrie : EmbarcacionPasajeros
{
    public double FactorServicio { get => 1.15; }

    public Ferrie(int id, double consumoCombustible10millas, int capacidadMaximaPasajeroscapa) : base(id, consumoCombustible10millas, capacidadMaximaPasajeroscapa)
    {
    }

    public override double CostoMilla()
    {
        return ConsumoCombustible10Millas * costoCombustibleFijo * FactorServicio;
    }

    public override string ToString()
    {
        return "[Ferrie] " + base.ToString();
    }

}
public abstract class EmbarcacionCarga : Embarcacion
{
    private double impuestoPortadoAnual;


    public double ImpuestoPortadoAnual
    {
        get => impuestoPortadoAnual;
        set
        {
            if (value > 0)
            {
                impuestoPortadoAnual = value;
            }
            else
            {
                impuestoPortadoAnual = 0;
            }
        }
    }

    protected EmbarcacionCarga(int id, double consumoCombustible10millas, double impuestoPortadoAnual) : base(id, consumoCombustible10millas)
    {
        ImpuestoPortadoAnual = impuestoPortadoAnual;
    }

    public override string ToString()
    {
        return base.ToString() + $"Impuesto portado anual : {ImpuestoPortadoAnual}";
    }
}

class BuqueCarga : EmbarcacionCarga
{
    
    public BuqueCarga(int id, double consumoCombustible10millas, double impuestoPortadoAnual) : base(id, consumoCombustible10millas, impuestoPortadoAnual)
    {
    }

    public override double CostoMilla()
    {
        return ConsumoCombustible10Millas * costoCombustibleFijo * (ImpuestoPortadoAnual / 50000);
    }

    public override string ToString()
    {
        return "[Buque de Carga]" + base.ToString();
    }
}

class Program
{
    static List <Embarcacion> embarcadero = new List <Embarcacion>();

    static void Main()
    {
        int opcion = 0;

        do
        {
            Console.WriteLine("\n=== PortManager S.A. ===");
            Console.WriteLine("1. Registrar Embarcación");
            Console.WriteLine("2. Ver Costos Operacionales");
            Console.WriteLine("3. Calcular Costo Total de la Flota (con 50,000 millas por embarcación)");
            Console.WriteLine("4. Salir");
            Console.Write("Seleccione una opción: ");

            if (!int.TryParse(Console.ReadLine(), out opcion))
            {
                Console.WriteLine("Opción no válida.");
                continue;
            }

            switch (opcion)
            {
                case 1: RegistrarEmbarcacion(); break;
                case 2: VerCostosOperacionales(); break;
                case 3: CalcularCostoTotal(); break;
                case 4: Console.WriteLine("Saliendo del sistema..."); break;
                default: Console.WriteLine("Opción no válida."); break;
            }

        } while (opcion != 4);
    }

    static void RegistrarEmbarcacion()
    {
        Console.WriteLine("\nTipo de embarcación:");
        Console.WriteLine("1. Ferry");
        Console.WriteLine("2. Buque de Carga");
        Console.Write("Seleccione: ");
        if (!int.TryParse(Console.ReadLine(), out int tipo)) return;

        Console.Write("ID: ");
        if (!int.TryParse(Console.ReadLine(), out int id))
            id = 0;

        Console.Write("Consumo (L/10 millas): ");
        if (!double.TryParse(Console.ReadLine(), out double consumo))
            consumo = 0.0;

        if (tipo == 1)
        {
            Console.Write("Capacidad Máxima: ");
            if (!int.TryParse(Console.ReadLine(), out int capacidad))
            {
                capacidad = 0;
            }

            embarcadero.Add(new Ferrie(id, consumo, capacidad));
            Console.WriteLine("Ferry registrado correctamente.");
        }
        else if (tipo == 2)
        {
            Console.Write("Impuesto Portuario Anual (€): ");
            if (!double.TryParse(Console.ReadLine(), out double impuesto))
            {
                impuesto = 0.0;
            }
            embarcadero.Add(new BuqueCarga(id, consumo, impuesto));
            Console.WriteLine("Buque de carga registrado correctamente.");
        }
        else
        {
            Console.WriteLine("Tipo no válido.");
        }
    }

    static void VerCostosOperacionales()
    {
        Console.WriteLine("\n--- COSTOS OPERACIONALES ---");
        if (embarcadero.Count == 0)
        {
            Console.WriteLine("No hay barcos registrados.");
            return;
        }

        foreach (var v in embarcadero)
        {
            Console.WriteLine($"{v.ToString()} → Costo por 10 millas: {v.CostoMilla():0.0000} €");
        }
    }

    static void CalcularCostoTotal()
    {
        double suma = 0.0;

        Console.WriteLine("\n--- COSTOS OPERACIONALES ---");
        if (embarcadero.Count == 0)
        {
            Console.WriteLine("No hay barcos registrados.");
            return;
        }
        else
        {

            foreach (var v in embarcadero)
            {
                suma =  v.CostoMilla() * 50000;
            }
        }

        Console.WriteLine($"\nCosto total estimado de la Embacación (50000 millas por vehículo): {suma:0.00} €");

    }

}