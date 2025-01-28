using System;
using System.Collections.Generic;

namespace Blackjack
{
    class Program
    {
        static void Main(string[] args)
        {
            var deck = GenerateDeck();
            var playerHand = new List<int>();
            var dealerHand = new List<int>();

            ShuffleDeck(deck);

            Console.WriteLine("Bienvenido al Blackjack!");

            // Turno del jugador
            Console.WriteLine("Turno del jugador:");
            while (true)
            {
                int card = DrawCard(deck);
                playerHand.Add(card);
                Console.WriteLine($"Carta obtenida: {CardToString(card)}");
                int playerScore = CalculateScore(playerHand);
                Console.WriteLine($"Puntuación actual: {playerScore}");

                if (playerScore > 21)
                {
                    Console.WriteLine("Te has pasado de 21. Pierdes.");
                    return;
                }

                Console.WriteLine("¿Quieres otra carta? (s/n)");
                if (Console.ReadLine()?.ToLower() != "s")
                    break;
            }

            // Turno de la banca
            Console.WriteLine("Turno de la banca:");
            while (true)
            {
                int dealerScore = CalculateScore(dealerHand);
                if (dealerScore >= 17)
                {
                    Console.WriteLine($"La banca se planta con una puntuación de {dealerScore}.");
                    break;
                }

                int card = DrawCard(deck);
                dealerHand.Add(card);
                Console.WriteLine($"La banca obtiene: {CardToString(card)}");
            }

            // Determinar el ganador
            int finalPlayerScore = CalculateScore(playerHand);
            int finalDealerScore = CalculateScore(dealerHand);

            Console.WriteLine($"Puntuación final - Jugador: {finalPlayerScore}, Banca: {finalDealerScore}");

            if (finalPlayerScore > 21 || (finalDealerScore <= 21 && finalDealerScore > finalPlayerScore))
            {
                Console.WriteLine("La banca gana.");
            }
            else if (finalDealerScore > 21 || finalPlayerScore > finalDealerScore)
            {
                Console.WriteLine("¡Felicidades! Has ganado.");
            }
            else
            {
                Console.WriteLine("Es un empate.");
            }
        }

        static List<int> GenerateDeck()
        {
            var deck = new List<int>();
            for (int i = 1; i <= 10; i++)
                for (int j = 0; j < 4; j++)
                    deck.Add(i);

            for (int j = 0; j < 4; j++)
            {
                deck.Add(10); // J
                deck.Add(10); // Q
                deck.Add(10); // K
            }

            return deck;
        }

        static void ShuffleDeck(List<int> deck)
        {
            var random = new Random();
            for (int i = deck.Count - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                (deck[i], deck[j]) = (deck[j], deck[i]);
            }
        }

        static int DrawCard(List<int> deck)
        {
            int card = deck[0];
            deck.RemoveAt(0);
            return card;
        }

        static int CalculateScore(List<int> hand)
        {
            int score = 0;
            int aceCount = 0;

            foreach (int card in hand)
            {
                score += card;
                if (card == 1)
                    aceCount++;
            }

            while (aceCount > 0 && score + 10 <= 21)
            {
                score += 10;
                aceCount--;
            }

            return score;
        }

        static string CardToString(int card)
        {
            return card switch
            {
                1 => "As",
                10 => "10/J/Q/K",
                _ => card.ToString()
            };
        }
    }
}
