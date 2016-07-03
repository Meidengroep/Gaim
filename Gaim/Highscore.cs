using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;

class Highscore
{
    public Highscore()
    {
        LaadHighscore();
    }

    private List<string> namen = new List<string>();
    private List<string> scores = new List<string>();
    private List<int> scoresInt = new List<int>();

    private string naamInvoer = "";
    public bool geUpdate = false;
    private Keys[] toetsen;
    private int plek;
    private bool vorigeStaat = false;
    private bool backspaceStaat = false;


    private void LaadHighscore()                                                    // Methode om de highscores uit files te lezen.
    {
        StreamReader fileLezer = new StreamReader("Content/Highscore/Namen.txt");   // De namen lezen en in een List zetten.
        string regel = fileLezer.ReadLine();
        while (regel != null)
        {
            namen.Add(regel);
            regel = fileLezer.ReadLine();
        }
        fileLezer.Close();

        fileLezer = new StreamReader("Content/Highscore/Scores.txt");               // De scores lezen en in een list zetten.
        regel = fileLezer.ReadLine();
        
        while (regel != null)
        {
            scores.Add(regel);
            regel = fileLezer.ReadLine();
        }

        foreach (string s in scores)                                                // Hier worden de scores van string naar int veranderd,
        {                                                                           // zodat ze later makkelijk gesorteerd kunnen worden.
            scoresInt.Add(int.Parse(s));
        }
        SorteerScores();
        fileLezer.Close();
    }

    public void SchrijfScores()
    {
        StreamWriter fileSchrijver = new StreamWriter("Content/Highscore/Namen.txt");       // Hier worden de scores en namen weer teruggeschreven 
        for (int i = 0; i < namen.Count; i++)                                               // naar hun respectievelijke files.
            fileSchrijver.WriteLine(namen[i]);
        fileSchrijver.Close();

        fileSchrijver = new StreamWriter("Content/Highscore/Scores.txt");
        for (int i = 0; i < namen.Count; i++)
            fileSchrijver.WriteLine(scores[i]);
        fileSchrijver.Close();
        naamInvoer = "";                                                                    // Na het schrijven wordt de huidig ingevoerde naam
    }                                                                                       // weer leeg.

    private void SorteerScores()
    {
        int aantalScores = scoresInt.Count;                                                 // Hier worden de scores gesorteerd. Dit gebeurd door 
        for (int j = 0; j < aantalScores - 1; j++)                                          // telkens te kijken of de score onder de score die gecheckt wordt
        {                                                                                   // groter is dan de huidige score. Zo ja, dan worden ze vewisseld,
            for (int i = 0; i < aantalScores - 1; i++)                                      // zodat de hogere score hoger in de lijst komt te staan.
                if (scoresInt[i] < scoresInt[i + 1])                                        // Door de dubbele for- loop wordt dit net genoeg keren gedaan,
                {                                                                           // zodat de score onderaan eventueel bovenaan kan komen te staan,
                    int opslaan = scoresInt[i];
                    scoresInt[i] = scoresInt[i + 1];
                    scoresInt[i + 1] = opslaan;
                    string opslaan2 = namen[i];
                    namen[i] = namen[i + 1];
                    namen[i + 1] = opslaan2;
                }
        }
        aantalScores = scores.Count;                        // Hier wordt de string-List van scores leeggemaakt, om vervolgens gevuld te worden met
        for (int z = aantalScores - 1; z >= 0; z--)         // de gesorteerde scores uit de int-List van scores.
            scores.RemoveAt(z);
  
        for (int p = 0; p < scoresInt.Count; p++)
        {
            scores.Add(scoresInt[p].ToString());
        }
    }

    public void VerwerkNaam(int behaaldeScore)
    {
        if (behaaldeScore > scoresInt.Min())
        {
            if (toetsen == null || vorigeStaat == false && toetsen.Length == 0)     // Als de vorigeStaat false is en er wordt niets ingedrukt,
                vorigeStaat = true;                                                 // of als de array nog niet bestaat, dan wordt de vorigeStaat true,
            else                                                                    // zodat er invoer plaats kan vinden.
                vorigeStaat = false;
                    
            KeyboardState toetsenbord = Keyboard.GetState();
            toetsen = toetsenbord.GetPressedKeys();

            if (backspaceStaat == true && toetsenbord.IsKeyUp(Keys.Back))           // Kijken of backspace vorige keer ingedrukt was en nu niet.
                backspaceStaat = false;

            if (vorigeStaat && naamInvoer.Length < 10)                              // De naam kan maximaal 10 karakters bevatten. Sorry, Annemaritha.
            {
                for (int i = 0; i < toetsen.Length; i++)
                {
                    if (toetsen[i] != Keys.Back                                     // Hier worden de toetsen die in het spel gebruikt worden
                        && toetsen[i] != Keys.Left                                  // uit de invoer gefilterd, zodat je geen LEFT, RIGHT en dat soort 
                        && toetsen[i] != Keys.Right                                 // dingen in je naam krijgt te staan. Enter hoeft niet, want als je 
                        && toetsen[i] != Keys.Up                                    // op enter drukt gaat het spel verder voordat de naaminvoer verwerkt wordt.
                        && toetsen[i] != Keys.Down
                        && toetsen[i] != Keys.Space)
                    {
                        naamInvoer += toetsen[i].ToString();
                        namen[plek] = naamInvoer;
                    }
                }
                vorigeStaat = false;
            }

            if (toetsenbord.IsKeyDown(Keys.Back) && naamInvoer.Length > 0 && backspaceStaat == false)
            {
                naamInvoer = naamInvoer.Remove(naamInvoer.Length - 1);              // Als je op backspace drukt, wordt de laatste letter van
                namen[plek] = naamInvoer;                                           // de naam die ja aan het invoeren bent verwijderd.
                backspaceStaat = true;
            }
        }
    }

    public void UpdateHighscore(int behaaldeScore)
    {
        if (behaaldeScore > scoresInt.Min() && geUpdate == false)
        {
            scoresInt.Add(behaaldeScore);
            if (namen.Count <= 6)                       // Er wordt een 6e plek in de List gereserveerd, zodat de nieuwe score hierin gezet kan worden
                namen.Add("");                          // en de sorteer-methode ermee aan de slag kan.
            SorteerScores();
            for (int i = 0; i < scoresInt.Count; i++)   // Na het sorteren wordt gekeken welke plek de speler heeft behaald.
                if (scoresInt[i] == behaaldeScore)
                    plek = i;
            scoresInt.RemoveAt(5);                      // De Lists worden afgekapt op een grootte van 5, want het is een top-5 highscore.
            scores.RemoveAt(5);
            namen.RemoveAt(5);
            geUpdate = true;
        }
    }

    public void Draw(SpriteBatch spriteBatch, SpriteFont font)
    {
        int teller = 0;
        spriteBatch.DrawString(font, "Highscores", new Vector2(460, 280), Color.Red);
        spriteBatch.DrawString(font, "Druk op enter om te herstarten", new Vector2(460, 300), Color.Red);
        foreach (string s in namen)                                                                                 // De scores worden onder elkaar
        {                                                                                                           // getekend, en de namen ook.
            spriteBatch.DrawString(font, s, new Vector2(460, 330) + new Vector2(0, 20 * teller), Color.Yellow);
            teller++;
        }
        teller = 0;
        foreach (string s in scores)
        {
            spriteBatch.DrawString(font, s, new Vector2(660, 330) + new Vector2(0, 20 * teller), Color.Yellow);
            teller++;
        }

    }

}

