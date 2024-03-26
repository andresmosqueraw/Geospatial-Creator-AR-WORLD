using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Data.SqlClient;
using System;

public class FormManager : MonoBehaviour
{
    public GameObject formCanvas;
    public TMP_InputField nombreInputField;
    public TMP_InputField observacionInputField;

    public RawImage imageDisplay; // Componente de UI para mostrar la imagen
    // public TMP_InputField imageInputField; // Campo opcional para almacenar la ruta de la imagen o el nombre

    private string imagePath;


    private string connectionString = "Server=34.82.109.27;Database=geospatial;User Id=admin;Password=Admin123*;";

    public void ToggleForm()
    {
        formCanvas.SetActive(!formCanvas.activeSelf);
    }

    public void CloseForm()
    {
        formCanvas.SetActive(false);
    }


    public void PickImage(int maxSize)
    {
        NativeGallery.Permission permission = NativeGallery.GetImageFromGallery((path) =>
        {
            Debug.Log("Image path: " + path);
            if (!string.IsNullOrEmpty(path))
            {
                Texture2D texture = NativeGallery.LoadImageAtPath(path, maxSize);
                if (texture == null)
                {
                    Debug.Log("No se pudo cargar la textura desde: " + path);
                    return;
                }

                imageDisplay.texture = texture;
                imagePath = path; // Guarda la ruta de la imagen
            }
        }, "Select a PNG image", "image/png");

        Debug.Log("Permission result: " + permission);
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
                string query = "INSERT INTO formulario (nombre, observacion, foto) VALUES (@nombre, @observacion, @foto)";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@nombre", nombre);
                    command.Parameters.AddWithValue("@observacion", observacion);
                    command.Parameters.AddWithValue("@foto", imagePath);

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
