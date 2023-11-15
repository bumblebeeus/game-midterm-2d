// write schema for user to read and write data from sql database
// this will be applied in unity game

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Data;
using Mono.Data.Sqlite;
using System.IO;

public class  ode
{
    private string username;

    private int currentSkinId;

    public bool loggedIn {get {return username != null;}}
    
    private string tableName;

    public static Player Instance = new Player();

    private Player()
    {
        username = null;
        currentSkinId = null;
    }

    public void logOut() {
        username = null;
        currentSkinId = null;
    }

    public bool registerUser(string username, string password)
    {
        string url = "http://localhost/phpmyadmin/"; // Replace with your phpMyAdmin URL
        string databaseName = "myDatabase"; // Replace with your database name
        string tableName = "myTable"; // Replace with your table name
        string connectionString = $"URI={url};Database={databaseName};User ID=root;Password=password;"; // Replace with your connection string

        try
        {
            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = $"INSERT INTO {tableName} (username, password) VALUES (@username, @password)";
                    command.Parameters.AddWithValue("@username", username);
                    command.Parameters.AddWithValue("@password", password);
                    command.ExecuteNonQuery();
                }
            }
            return true; // registration successful
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error registering user: {ex.Message}");
            return false; // registration failed
        }
    }
}