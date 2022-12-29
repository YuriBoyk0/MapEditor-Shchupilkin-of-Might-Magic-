using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityNpgsql;

public class Test : MonoBehaviour
{
    public static void Postgres()
        {
            string connectionString = "Server=127.0.0.1;Port=6666;User Id=postgres;Password=123;Database=Heroes";
            NpgsqlConnection conn = new NpgsqlConnection(connectionString);
            conn.Open();
            NpgsqlCommand dbcom = conn.CreateCommand();
            string command =
            "SELECT Name " +
            "FROM Hero";
            dbcom.CommandText = command;
            NpgsqlDataReader reader = dbcom.ExecuteReader();
            while (reader.Read())
            {
                Debug.LogError(reader.GetString(0));
            }
            reader.Close();
            dbcom.Dispose();
            conn.Close();
        }
}
