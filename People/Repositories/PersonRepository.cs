using SQLite;
using People.Models;
using System.Xml.Linq;


namespace People.Repositories;

public class PersonRepository
{
    string _dbPath;

    public string StatusMessage { get; set; }
    private SQLiteConnection conn;

    private void Init()
    {
        conn = new SQLiteConnection(_dbPath);
        conn.CreateTable<Person>();
    }

    public PersonRepository(string dbPath)
    {
        _dbPath = dbPath;
    }

    public void AddNewPerson(Person person)
    {
        try
        {
            Init();

            // Validar que el nombre no esté vacío o nulo
            if (string.IsNullOrEmpty(person.Name))
            {
                StatusMessage = "ERROR: Debe Ingresar un Nombre";
                return;
            }

            conn.Insert(person);
            StatusMessage = "Persona: " + person.Name + " Guardada Correctamente!";

        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", person.Name, ex.Message);
            throw;
        }
    }

    public void UpdatePerson(Person person)
    {
        try
        {
            Init();
            conn.Update(person);
        }
        catch (Exception ex)
        {
            throw;
        }
    }



    public void AddNewPerson(string name)
    {
        int result = 0;
        try
        {
            // TODO: Call Init()

            // basic validation to ensure a name was entered
            if (string.IsNullOrEmpty(name))
                throw new Exception("Valid name required");


            result = conn.Insert(new Person { Name = name });

            StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, name);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to add {0}. Error: {1}", name, ex.Message);
        }

    }

    public List<Person> GetAllPeople()
    {
        try
        {
            Init();
            return conn.Table<Person>().ToList().OrderBy(x => x.Name).ToList();
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }

        return new List<Person>();
    }

    public Person GetPerson(int id)
    {
        try
        {
            Init();
            var people = conn.Table<Person>().ToList();
            Person person = people.Find(x => x.Id == id);
            return person;
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
        }
        return new Person();
    }

    public void DeletePerson(Person person)
    {
        try
        {
            Init();
            conn.Delete(person);
        }
        catch (Exception ex)
        {
            StatusMessage = string.Format("Failed to delete data. {0}", ex.Message);
        }
    }
}
