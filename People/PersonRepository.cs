using People.Models;
using SQLite;

namespace People;

public class PersonRepository
{
    // Campo privado
    private string _dbPath;

    // Propiedad pública para acceder al _dbPath
    public string DbPath
    {
        get { return _dbPath; }
    }

    public string StatusMessage { get; set; }
    private SQLiteConnection conn;

    private void Init()
    {
        if (conn != null)
            return;

        conn = new SQLiteConnection(_dbPath);
        conn.CreateTable<Person>(); // Inicialización del repositorio
    }

    public PersonRepository(string dbPath)
    {
        _dbPath = dbPath;
    }

    public void AddNewPerson(string name)
    {
        int result = 0;
        try
        {
            Init();

            // Validar que el nombre no esté vacío
            if (string.IsNullOrEmpty(name))
                throw new Exception("Valid name required");

            // Insertar la nueva persona en la base de datos
            result = conn.Insert(new Person { Name = name });

            StatusMessage = string.Format("{0} Registro añadido (Nombre: {1})", result, name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Error al agregar {0}. Error: {1}", name, ex.Message);
        }
    }

    public List<Person> GetAllPeople()
    {
        try
        {
            Init();
            return conn.Table<Person>().ToList();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Error al regresar los datos. {0}", ex.Message);
        }

        return new List<Person>();
    }

    public void DeletePerson(int id)
    {
        using var connection = new SQLiteConnection(DbPath);
        connection.Delete<Person>(id);
        StatusMessage = $"Registro con ID {id} eliminado.";
    }


}
