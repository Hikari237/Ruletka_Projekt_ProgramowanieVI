using Microsoft.Data.Sqlite;
using Ruletka.Models;
using System;
using System.Collections.Generic;
using System.IO;

namespace Ruletka.Database
{
    public static class DbManager
    {
        private static string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "game_data.db");
        private static string connectionString = $"Data Source={dbPath}";

        public static void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS Players (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Name TEXT UNIQUE NOT NULL, 
                        Sympathy INTEGER DEFAULT 0,
                        Deaths INTEGER DEFAULT 0,
                        Wins INTEGER DEFAULT 0
                    )"; // DODANO Wins

                var command = connection.CreateCommand();
                command.CommandText = createTableQuery;
                command.ExecuteNonQuery();
            }
        }

        public static List<Character> GetAllPlayers()
        {
            var players = new List<Character>();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = "SELECT Name, Sympathy, Deaths, Wins FROM Players"; // DODANO Wins

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new Character(reader.GetString(0), 100, reader.GetInt32(1));
                        p.Deaths = reader.GetInt32(2);
                        p.Wins = reader.GetInt32(3); // DODANO Wins
                        players.Add(p);
                    }
                }
            }
            return players;
        }

        public static void SaveOrUpdatePlayer(Character player)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var query = @"
                    INSERT INTO Players (Name, Sympathy, Deaths, Wins) 
                    VALUES (@name, @sympathy, @deaths, @wins)
                    ON CONFLICT(Name) DO UPDATE SET 
                        Sympathy = @sympathy,
                        Deaths = @deaths,
                        Wins = @wins"; // DODANO Wins

                var command = connection.CreateCommand();
                command.CommandText = query;
                command.Parameters.AddWithValue("@name", player.Name);
                command.Parameters.AddWithValue("@sympathy", player.Sympathy);
                command.Parameters.AddWithValue("@deaths", player.Deaths);
                command.Parameters.AddWithValue("@wins", player.Wins); // DODANO Wins
                command.ExecuteNonQuery();
            }
        }

        public static List<Character> GetTopPlayers()
        {
            var players = new List<Character>();
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var command = connection.CreateCommand();
                // Sortujemy: najpierw Wygrane (DESC), potem Sympatia (DESC)
                command.CommandText = "SELECT Name, Sympathy, Deaths, Wins FROM Players ORDER BY Wins DESC, Sympathy DESC LIMIT 10";

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var p = new Character(reader.GetString(0), 100, reader.GetInt32(1));
                        p.Deaths = reader.GetInt32(2);
                        p.Wins = reader.GetInt32(3);
                        players.Add(p);
                    }
                }
            }
            return players;
        }
    }
}