using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using Newtonsoft.Json;

//vyse jsou zbytecne usingy

namespace Notino.Homework
{
    //Tahle trida (DTO) patri do samostatneho souboru
    public class Document
    {
        public string Title { get; set; }
        public string Text { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            //Cesty by se mely ziskavat z parametru metody
            var sourceFileName = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Source Files\\Document1.xml");
            var targetFileName = Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\Target Files\\Document1.json");

            try
            {
                FileStream sourceStream = File.Open(sourceFileName, FileMode.Open); //chybi using kvuli IDisposable
                var reader = new StreamReader(sourceStream);    //chybi using kvuli IDisposable
                string input = reader.ReadToEnd();  //inicializace var input patri pred blok Try, nevyuziti Async
            }
            catch (Exception ex)
            {
                //zbytecne vyhozeni nove vyjimky a ztrata StackTrace. Nedava to smysl. Hodilo by se zde zalogovat vyjimku a pak ji jen vyhodit
                throw new Exception(ex.Message);
            }

            var xdoc = XDocument.Parse(input);  //input je out of scope, nelze zkompilovat
            var doc = new Document  //xdoc je nullable, pro kontrolu bych pouzil if
            {
                Title = xdoc.Root.Element("title").Value,   //parsovani prostrednictvim stringu... Lze to udelat lepe, typove. Navic je nullable, takze osetrit
                Text = xdoc.Root.Element("text").Value      //viz vyse
            };

            var serializedDoc = JsonConvert.SerializeObject(doc);

            var targetStream = File.Open(targetFileName, FileMode.Create, FileAccess.Write);    //chybi using, pokud se ma souboru prepisovat tak FileMode.OpenOrCreate
            var sw = new StreamWriter(targetStream);    //chybi using
            sw.Write(serializedDoc);    //pouzil bych async z duvodu neblokovani hlavniho threadu

            //POZN. Vse v jednom souboru... Urcite by stalo za to rozdelit logicke celky do mensich metod a tridy do samostatnych souboru.
        }
    }
}
