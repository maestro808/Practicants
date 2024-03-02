// See https://aka.ms/new-console-template for more information
using System.Data.SqlClient;
using System.Reflection.Metadata;
using System.Reflection.PortableExecutable;
using System.Xml.Serialization;

static DateTime KontrolaDatumuKoncePraxe(DateTime datZacPraxPraktikanta, DateTime datKonPraxPraktikanta)
{
    //zadává se vstup, dokud není datum konce praxe větší než začátek praxe
    while (datKonPraxPraktikanta < datZacPraxPraktikanta)
    {
        Console.Write("Konec praxe Praktikanta musí být později než začátek praxe\n" +
            "Zadej znovu datum konce praxe Praktikanta (dd.MM.yyyy): ");
        //přetypování datumu ve stringu na datum podle formátu
        while (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out datKonPraxPraktikanta))
        {
            Console.Write("Zadal jsi datum ve špatném formátu, zadej znovu: ");
        }
    }
    return datKonPraxPraktikanta;
}

static void VypisTabulky(SqlConnection pripojeni)
{
    /*
        Aplikace zobrazí všechny praktikanty uložené v databázi.
    */
    pripojeni.Open();
    SqlCommand zobraz = new SqlCommand("SELECT * FROM Praktikanti", pripojeni);
    SqlDataReader dataReader = zobraz.ExecuteReader();

    while (dataReader.Read())
    {
        //vypíše název sloupce a hodnotu v ní obsazené, opakuje dokud nedojde na konec tabulky
        for (int i = 0; i < dataReader.FieldCount; i++)
        {
            Console.WriteLine(dataReader.GetName(i) + ": " + dataReader.GetValue(i));
            
        }
        Console.WriteLine();
    }
    dataReader.Close();
    pripojeni.Close();
}

static void PridejPraktikanta(SqlConnection pripojeni)
{
    /*  
     *  Uživatel zadá jméno praktikanta, název školy, rok studia, 
        datum začátku a datum konce zácviku.
        Tyto informace jsou uloženy v databázi.
    */
    try
    {
        // zadávání parametrů pro vložení nového praktikanta
        Console.Write("Zadej jméno Praktikanta: ");
        string jmenoPraktikanta = Console.ReadLine();

        Console.Write("Zadej školu Praktikanta: ");
        string skolaPraktikanta = Console.ReadLine();

        Console.Write("Zadej rok studia Praktikanta: ");
        int rokStudiaPraktikanta = int.Parse(Console.ReadLine()); //Přetypování vstupu na int

        Console.Write("Zadej datum zacatku praxe Praktikanta: (dd.MM.yyyy) ");
        //přetypování datumu ve stringu na datum podle formátu
        DateTime datZacPraxPraktikanta;
        while(! DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out datZacPraxPraktikanta))
        {
            Console.Write("Zadal jsi datum ve špatném formátu, zadej znovu: ");
        }

        Console.Write("Zadej datum konce praxe Praktikanta: (dd.MM.yyyy) ");
        //přetypování datumu ve stringu na datum podle formátu
        DateTime datKonPraxPraktikanta;
        while (! DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out  datKonPraxPraktikanta))
        {
            Console.Write("Zadal jsi datum ve špatném formátu, zadej znovu: ");
        }

        //kontrola datumu konce praxe zda je větší než začátek praxe
        if (datKonPraxPraktikanta < datZacPraxPraktikanta)
        {
            datKonPraxPraktikanta = KontrolaDatumuKoncePraxe(datZacPraxPraktikanta, datKonPraxPraktikanta);
        }

        //deklarace dotazu
        string dotaz = ("INSERT INTO Praktikanti (Jmeno, Skola, RokStudia, DatumZacatkuPraxe, DatumKoncePraxe) VALUES (@Jmeno, @Skola, @RokStudia, @DatumZacatkuPraxe, @DatumKoncePraxe)");
        //vkládání paramentrů podle sloupce
        using (SqlCommand pridej = new SqlCommand(dotaz, pripojeni))
        {
            pripojeni.Open();
            pridej.Parameters.AddWithValue("@Jmeno", jmenoPraktikanta);
            pridej.Parameters.AddWithValue("@Skola", skolaPraktikanta);
            pridej.Parameters.AddWithValue("@RokStudia", rokStudiaPraktikanta);
            pridej.Parameters.AddWithValue("@DatumZacatkuPraxe", datZacPraxPraktikanta.Date);
            pridej.Parameters.AddWithValue("@DatumKoncePraxe", datKonPraxPraktikanta.Date);
            //výpis počtu změněných řádků
            int radky = pridej.ExecuteNonQuery();
            Console.WriteLine($"Počet upravených záznamů {radky}");
            pripojeni.Close();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("Chyba: " + ex.Message);
        pripojeni.Close ();
    }
}

static void VymazPraktikanta(SqlConnection pripojeni)
{
    /*
        Uživatel zadá ID praktikanta, kterého chce smazat.
        Aplikace odstraní odpovídající záznam z databáze.
    */
    try
    {
        Console.Write("Zadej Id Praktikanta k vymazání: ");
        int idPraktikanta = int.Parse(Console.ReadLine());

        string dotaz = "DELETE FROM Praktikanti WHERE Id=@Id";
        using (SqlCommand vymazPrakikanta = new SqlCommand(dotaz, pripojeni))
        {
            pripojeni.Open();
            vymazPrakikanta.Parameters.AddWithValue("@Id", idPraktikanta);
            int radky = vymazPrakikanta.ExecuteNonQuery();
            Console.WriteLine($"Počet upravených záznamů: {radky}");
            pripojeni.Close();
        }
    }
    catch (Exception ex){
        Console.WriteLine("Chyba: " + ex.Message);
        pripojeni.Close();
    }
}

static void AktualizujPraktikanta(SqlConnection pripojeni)
{
    /*
    Uživatel zadá ID existujícího praktikanta a nové informace 
    (jméno praktikanta, škola, rok studia, datum začátku, datum konce).
    */
    try
    {
        Console.Write("Zadej Id Praktikanta: ");
        int idPraktikanta = int.Parse(Console.ReadLine());

        Console.Write("Zadej nové jméno Praktikanta: ");
        string jmeno = Console.ReadLine();

        Console.Write("Zadej novou školu Praktikanta: ");
        string skola = Console.ReadLine();

        Console.Write("Zadej nový rok studia Praktikanta: ");
        int rokStudia = int.Parse(Console.ReadLine());

        Console.Write("Zadej nový datum začátku praxe Praktikanta (dd.MM.yyyy): ");
        //přetypování datumu ve stringu na datum podle formátu
        DateTime datZacPraxPraktikanta;
        while (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out datZacPraxPraktikanta))
        {
            Console.Write("Zadal jsi datum ve špatném formátu, zadej znovu: ");
        }

        Console.Write("Zadej nový datum konce praxe Praktikanta (dd.MM.yyyy): ");
        //přetypování datumu ve stringu na datum podle formátu
        DateTime datKonPraxPraktikanta;
        while (!DateTime.TryParseExact(Console.ReadLine(), "dd.MM.yyyy", null, System.Globalization.DateTimeStyles.None, out datKonPraxPraktikanta))
        {
            Console.Write("Zadal jsi datum ve špatném formátu, zadej znovu: ");
        }

        //v případě že je zadán datum konce praxe, opakuje vstup dokud není větší než začátek praxe
        if (datKonPraxPraktikanta < datZacPraxPraktikanta)
        {
            datKonPraxPraktikanta = KontrolaDatumuKoncePraxe(datZacPraxPraktikanta, datKonPraxPraktikanta);
        }
        
        string dotaz = "UPDATE Praktikanti SET Jmeno=@Jmeno, Skola=@Skola, RokStudia=@RokStudia, DatumZacatkuPraxe=@DatumZacatkuPraxe, DatumKoncePraxe=@DatumKoncePraxe WHERE Id=@Id";
        using (SqlCommand update = new SqlCommand(dotaz, pripojeni))
        {
            pripojeni.Open();
            update.Parameters.AddWithValue("@Id", idPraktikanta);
            update.Parameters.AddWithValue("@Jmeno", jmeno);
            update.Parameters.AddWithValue("@Skola", skola);
            update.Parameters.AddWithValue("@RokStudia", rokStudia);
            update.Parameters.AddWithValue("@DatumZacatkuPraxe", datZacPraxPraktikanta.Date);
            update.Parameters.AddWithValue("@DatumKoncePraxe", datKonPraxPraktikanta.Date);
            int radky = update.ExecuteNonQuery();
            Console.WriteLine($"Počet upravených záznamů: {radky}");
            pripojeni.Close();
        }
    }
    catch(Exception ex)
    {
        Console.WriteLine ("Chyba: " +  ex.Message);
        pripojeni.Close();
    }
}  

static void VyhledejPraktiktany(SqlConnection pripojeni)
{
    /*
        Uživatel zadá název školy nebo rok studia, 
        a aplikace zobrazí všechny praktikanty odpovídající hledanému kritériu.
     */
    Console.Write("Zadej školu nebo rok studia pro vyhledání Praktikanta: ");
    string vstup = Console.ReadLine();

    //kontrola numerického vstupu a přetypování RokStudia na NVARCHAR a porovná se vstupem,   -"- kontrola zda nazev obsahuje část vstup
    string dotaz = "SELECT * FROM Praktikanti WHERE (ISNUMERIC(@vstup) = 1 AND CAST(RokStudia AS NVARCHAR) = @vstup) OR (ISNUMERIC(@vstup) = 0 AND Skola LIKE @vstup)";
    using (SqlCommand hledej = new SqlCommand(dotaz, pripojeni))
    {
        pripojeni.Open();
        hledej.Parameters.AddWithValue("@vstup", vstup);
        SqlDataReader dataReader = hledej.ExecuteReader();
        //čtení dokud se neprojdou všechny záznamy
        while (dataReader.Read())
        {
            Console.WriteLine(dataReader["Jmeno"]);
        }
        pripojeni.Close() ;
    }
}

//Data Source = (localdb)\MSSQLLocalDB; AttachDbFilename = C:\Users\Dell\Dropbox\C#\projekt_praktikanti\praktikanti\praktikanti\PraktikantiDB.mdf;Initial Catalog=PraktikantiDB;Integrated Security=True

string connectionString = @"Data Source=(localdb)\MSSQLLocalDB; AttachDbFilename = C:\Users\Dell\Dropbox\C#\projekt_praktikanti\praktikanti\praktikanti\PraktikantiDB.mdf;Initial Catalog=PraktikantiDB;Integrated Security=True";


using (SqlConnection pripojeni = new SqlConnection(connectionString))
{
    //deklarace spustece a cyklu while, dokud se nezadá END program se bude dotazovat na příkaz
    bool spoustec = true;
    while (spoustec)
    {
        //výpis menu
        Console.WriteLine(
            "\n********************************" +
            "\n OBSLUHA DATABÁZE PRAKTIKANT\n" +
            "********************************\n" +
            "ADD    -   Přidej Praktikanta \n" +
            "SHOW   -   Zobraz seznam Praktukantů\n" +
            "UPDATE -   Aktualizuj Praktikanta\n" +
            "DEL    -   Smaž Praktikanta\n" +
            "FIND   -   Vyhledej Praktikanta\n" +
            "END    -   Ukončí program");
        Console.WriteLine("------------------------------------");
        
        Console.Write("Zadej Příkaz: ");
        //vstup a jeho ošetření
        string prikaz_uzivatele = Console.ReadLine().ToUpper();
        Console.WriteLine();

        //deklarace switch, podle vstupu uživatele vykoná funkci
        switch (prikaz_uzivatele)
        {
            case "ADD":
                PridejPraktikanta(pripojeni);
                break;
            
            case "SHOW":
                VypisTabulky(pripojeni);
                break;
            
            case "UPDATE":
                AktualizujPraktikanta(pripojeni);
                break;
            
            case "DEL":
                VymazPraktikanta(pripojeni);
                break;
            
            case "FIND":
                VyhledejPraktiktany(pripojeni);
                break;

            default:
                Console.WriteLine("Neplatný příkaz.");
                break;
            
            case "END":
                spoustec = false;
                Console.WriteLine("Program ukončen.");
                break;
        }
    }
    pripojeni.Close();
}
Console.ReadKey();