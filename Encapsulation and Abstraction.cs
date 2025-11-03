using System;

// ==========================================
// CLASE: UserProfile
// ==========================================
// Esta clase representa el perfil de un usuario del sistema.
// ANTES: todos los atributos eran públicos (inseguro y desordenado).
// AHORA: los atributos están encapsulados (privados)
// y solo se accede a ellos mediante métodos controlados (getters/setters).
// También se eliminó la responsabilidad de "exportar",
// que pasa a nuevas clases especializadas.
//
public class UserProfile
{
    // -----------------------
    // CAMPOS PRIVADOS
    // -----------------------
    // Se marcan como "private" para impedir acceso directo desde fuera.
    // Solo se pueden modificar o leer mediante las propiedades públicas.
    private string _username;
    private string _email;
    private string _password; // Nunca se debería exponer en texto plano.

    // -----------------------
    // CONSTRUCTOR
    // -----------------------
    // Se ejecuta cuando se crea un nuevo UserProfile.
    // Se usan los setters internos para aprovechar sus validaciones.
    public UserProfile(string username, string email, string password)
    {
        Username = username;  // Se ejecuta el set (con validación)
        Email = email;
        Password = password;
    }

    // -----------------------
    // PROPIEDADES (GET/SET)
    // -----------------------
    // Permiten acceder o modificar los campos, aplicando reglas.

    public string Username
    {
        get { return _username; } // Devuelve el valor interno
        set
        {
            // Validación básica: no puede estar vacío
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentException("El nombre de usuario no puede estar vacío.");
            }
            _username = value; // Asigna al campo privado
        }
    }

    public string Email
    {
        get { return _email; }
        set
        {
            // Validación básica: el email debe contener '@'
            if (string.IsNullOrWhiteSpace(value) || !value.Contains("@"))
            {
                throw new ArgumentException("El email no es válido (debe contener '@').");
            }
            _email = value;
        }
    }

    public string Password
    {
        // No devolvemos la contraseña real (seguridad básica)
        get { return "********"; }
        set
        {
            // La contraseña debe tener al menos 8 caracteres
            if (string.IsNullOrWhiteSpace(value) || value.Length < 8)
            {
                throw new ArgumentException("La contraseña debe tener al menos 8 caracteres.");
            }
            // En un sistema real se guardaría un hash, no la contraseña literal.
            _password = value;
        }
    }

    // -----------------------
    // MÉTODOS
    // -----------------------

    // Muestra los datos del perfil (sin exponer la contraseña real).
    public void DisplayProfile()
    {
        Console.WriteLine("=== Perfil de usuario ===");
        Console.WriteLine($"Username: {Username}");
        Console.WriteLine($"Email: {Email}");
        Console.WriteLine("Password: (oculta por seguridad)");
    }
}

// ==========================================
// INTERFAZ: IProfileExporter
// ==========================================
// Una interfaz define un "contrato": especifica qué métodos deben tener las clases
// que la implementen. No define CÓMO lo hacen, solo QUÉ deben hacer.
//
public interface IProfileExporter
{
    // Método que exporta la información de un perfil.
    void Export(UserProfile profile);
}

// ==========================================
// CLASE: JsonProfileExporter
// ==========================================
// Implementa la interfaz IProfileExporter.
// Esta clase SÍ sabe cómo exportar un perfil a formato JSON.
//
public class JsonProfileExporter : IProfileExporter
{
    public void Export(UserProfile profile)
    {
        // En un programa real usaríamos una librería JSON.
        string json = $"{{\"username\": \"{profile.Username}\", \"email\": \"{profile.Email}\"}}";
        Console.WriteLine("\nExportando perfil a JSON:");
        Console.WriteLine(json);
    }
}

// ==========================================
// CLASE: XmlProfileExporter
// ==========================================
// Otra implementación distinta de la misma interfaz.
// Esta exporta el perfil en formato XML.
//
public class XmlProfileExporter : IProfileExporter
{
    public void Export(UserProfile profile)
    {
        string xml = $"<UserProfile>\n" +
                     $"  <Username>{profile.Username}</Username>\n" +
                     $"  <Email>{profile.Email}</Email>\n" +
                     $"</UserProfile>";
        Console.WriteLine("\nExportando perfil a XML:");
        Console.WriteLine(xml);
    }
}

// ==========================================
// PROGRAMA PRINCIPAL (Main)
// ==========================================
public class ProgramActivity1
{
    public static void Main(string[] args)
    {
        // Creamos un usuario válido.
        var user = new UserProfile("junior_dev", "junior@example.com", "password123");

        // Mostramos sus datos.
        user.DisplayProfile();

        // Creamos exportadores (demostrando polimorfismo por interfaz)
        IProfileExporter json = new JsonProfileExporter();
        IProfileExporter xml = new XmlProfileExporter();

        // Exportamos en ambos formatos.
        json.Export(user);
        xml.Export(user);

        // Si probamos asignar datos inválidos, las validaciones se activan.
        /*
        try
        {
            user.Email = "invalido"; // Lanza excepción
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
        */
    }
}
