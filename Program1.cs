using System;
using System.Collections.Generic;
 //hola
 public class Carta
{
    public string Valor { get; set; }
    public string Palo { get; set; }
 
    // Constructor
    public Carta(string valor, string palo)
    {
        Valor = valor;
        Palo = palo;
    }
 
    // Obtener el valor de la carta
    public int ObtenerValor()
    {
        if (Valor == "J" || Valor == "Q" || Valor == "K") // Las cartas J, Q, K valen 10 puntos
            return 10;
        if (Valor == "A") // El As vale 11, pero puede ajustarse si se pasa de 21
            return 11;
        return int.Parse(Valor); 
    }
 
    // Mostrar la carta
    public override string ToString()
    {
        return $"{Valor} de {Palo}";
    }
}
 
class Baraja
{
    private List<Carta> cartas;
    private Random random;
 
    // Constructor
    public Baraja()
    {
        cartas = new List<Carta>();
        random = new Random();
        string[] valores = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
        string[] palos = { "Corazones", "Tréboles", "Picas", "Diamantes" };
 
        // Llenar la baraja con las cartas
        foreach (var palo in palos)
        {
            foreach (var valor in valores)
            {
                cartas.Add(new Carta(valor, palo));
            }
        }
 
        // Barajar las cartas
        Barajar();
    }
 
    // Barajar la baraja
    public void Barajar()
    {
        cartas.Sort((x, y) => random.Next(-1, 2));
    }
 
    // Repartir una carta
    public Carta RepartirCarta()
    {
        Carta carta = cartas[0];
        cartas.RemoveAt(0);
        return carta;
    }
}
 
class Jugador
{
    public List<Carta> Mano { get; private set; }
    public int Puntaje
    {
        get
        {
            int puntaje = 0;
            int ases = 0;
            foreach (var carta in Mano)
            {
                puntaje += carta.ObtenerValor();
                if (carta.Valor == "A")
                    ases++;
            }
 
            // Ajustar el valor de los ases si el puntaje es mayor a 21
            while (puntaje > 21 && ases > 0)
            {
                puntaje -= 10; // Convertir un As de 11 a 1
                ases--;
            }
 
            return puntaje;
        }
    }
 
    // Constructor
    public Jugador()
    {
        Mano = new List<Carta>();
    }
 
    // Recibir una carta
    public void RecibirCarta(Carta carta)
    {
        Mano.Add(carta);
    }
 
    // Mostrar la mano
    public void MostrarMano(bool mostrarTodas = false)
    {
        Console.WriteLine("Mano del jugador:");
        foreach (var carta in Mano)
        {
            Console.WriteLine(carta);
        }
 
        Console.WriteLine($"Puntaje: {Puntaje}");
    }
}
 
class Juego
{
    private Baraja baraja;
    private Jugador jugador;
    private Jugador banca;
 
    // Constructor
    public Juego()
    {
        baraja = new Baraja();
        jugador = new Jugador();
        banca = new Jugador();
    }
 
    // Iniciar el juego
    public void Iniciar()
    {
        // Repartir las cartas iniciales
        jugador.RecibirCarta(baraja.RepartirCarta());
        jugador.RecibirCarta(baraja.RepartirCarta());
        banca.RecibirCarta(baraja.RepartirCarta());
        banca.RecibirCarta(baraja.RepartirCarta());
 
        // Mostrar la mano del jugador y la carta visible de la banca
        jugador.MostrarMano();
        Console.WriteLine($"Carta visible de la banca: {banca.Mano[0]}");
 
        // Turno del jugador
        if (TurnoJugador())
        {
            // Si el jugador no se pasa de 21, la banca juega
            TurnoBanca();
        }
 
        // Determinar el ganador
        DeterminarGanador();
    }
 
    // Turno del jugador
    private bool TurnoJugador()
    {
        while (true)
        {
            Console.WriteLine("¿Deseas pedir otra carta? (s/n): ");
            string respuesta = Console.ReadLine().ToLower();
            if (respuesta == "s")
            {
                jugador.RecibirCarta(baraja.RepartirCarta());
                jugador.MostrarMano();
 
                // Si el jugador se pasa de 21, termina su turno
                if (jugador.Puntaje > 21)
                {
                    Console.WriteLine("Te has pasado de 21. ¡Perdiste!");
                    return false;
                }
            }
            else
            {
                return true;
            }
        }
    }
 
    // Turno de la banca
    private void TurnoBanca()
    {
        Console.WriteLine("Es el turno de la banca.");
        banca.MostrarMano();
 
        // La banca pide cartas hasta que tenga 17 o más puntos
        while (banca.Puntaje < 17)
        {
            Console.WriteLine("La banca pide una carta.");
            banca.RecibirCarta(baraja.RepartirCarta());
            banca.MostrarMano();
        }
 
        // Si la banca se pasa de 21, termina el juego
        if (banca.Puntaje > 21)
        {
            Console.WriteLine("La banca se pasó de 21. ¡Ganaste!");
        }
    }
 
    // Determinar el ganador
    private void DeterminarGanador()
    {
        if (banca.Puntaje > 21)
        {
            Console.WriteLine("La banca se pasó de 21. ¡Ganaste!");
        }
        else if (jugador.Puntaje > banca.Puntaje)
        {
            Console.WriteLine("¡Ganaste!");
        }
        else if (jugador.Puntaje < banca.Puntaje)
        {
            Console.WriteLine("¡Perdiste!");
        }
        else
        {
            Console.WriteLine("¡Empate!");
        }
    }
}
 
class Program
{
    static void Main()
    {
        Juego juego = new Juego();
        juego.Iniciar();
    }
}
