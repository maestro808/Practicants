Vytvořte aplikaci v jazyce C#, která slouží k evidenci praktikantů, kteří přicházejí do naší organizace na praxi. Úkolem je vytvořit systém pro správu informací o praktikantech, který umožní přidávat, zobrazovat, aktualizovat a mazat informace. SQL databáze by měla obsahovat tabulku s následujícími sloupci:

ID (int, primární klíč, auto-increment)
Jméno praktikanta (nvarchar(100))
Škola (nvarchar(50))
Rok studia (int)
Datum začátku praxe (date)
Datum konce praxe (date)
Aplikace by měla umožňovat následující operace:

Přidání praktikanta:

Uživatel zadá jméno praktikanta, název školy, rok studia, datum začátku a datum konce zácviku.
Tyto informace jsou uloženy v databázi.
Zobrazení seznamu praktikantů:

Aplikace zobrazí všechny praktikanty uložené v databázi.
Aktualizace informací o praktikantovi:

Uživatel zadá ID existujícího praktikanta a nové informace (jméno praktikanta, škola, rok studia, datum začátku, datum konce).
Aplikace aktualizuje odpovídající záznam v databázi.
Smazání praktikanta:

Uživatel zadá ID praktikanta, kterého chce smazat.
Aplikace odstraní odpovídající záznam z databáze.
Vyhledávání podle školy nebo roku studia:

Uživatel zadá název školy nebo rok studia, a aplikace zobrazí všechny praktikanty odpovídající hledanému kritériu.
