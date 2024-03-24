using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using System.Data.SqlClient;
using System;

public class FormManager : MonoBehaviour
{
    public GameObject formPanel;
    public TMP_InputField nombreInputField;
    public TMP_InputField observacionInputField;

    private string connectionString = "Server=34.82.109.27;Database=geospatial;User Id=admin;Password=Admin123*;";

    public void ToggleForm()
    {
        formPanel.SetActive(!formPanel.activeSelf);
    }

    public void CloseForm()
    {
        formPanel.SetActive(false);
    }

    public void SendDataToDatabase()
    {
        string nombre = nombreInputField.text;
        string observacion = observacionInputField.text;

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                string query = "INSERT INTO formulario (nombre, observacion) VALUES (@nombre, @observacion)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@observacion", observacion);

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.LogError("Error al conectar con la base de datos: " + ex.Message);
            }
        }
    }
}
