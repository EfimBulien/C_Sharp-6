using Newtonsoft.Json;
using System.Xml.Serialization;


namespace Converter
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.Clear();
            Console.WriteLine("Введите путь до файла: ");
            string path = Console.ReadLine();
            if (File.Exists(path))
            {
                Console.Clear();
                Loader(path);
            }
            else
            {
                Console.Clear();
                File.Create(path);
                Loader(path);
            }
        }
        public static void Loader(string path)
        {   
            Figure figure = new Figure();
            string extension = Path.GetExtension(path);
            List<Figure> figures_list = new List<Figure>();
            if (extension == ".txt")
            {
                try
                {
                    string[] txt = File.ReadAllLines(path);
                    for (int i = 0; i < txt.Length; i += 3)
                    {
                        figure.Name = txt[i];
                        figure.Width = double.Parse(txt[i + 1]);
                        figure.Height = double.Parse(txt[i + 2]);
                        figures_list.Add(figure);
                    }
                }
                catch
                {
                    Console.WriteLine("Файл не найден.");
                    return;
                }
                
            }
            else if (extension == ".json")
            {
                string json = File.ReadAllText(path);
                figures_list = JsonConvert.DeserializeObject<List<Figure>>(json);
            }
            else if (extension == ".xml")
            {
                XmlSerializer serializer = new XmlSerializer(typeof(List<Figure>));
                using (FileStream reader = new FileStream(path, FileMode.Open))
                {
                    figures_list = (List<Figure>)serializer.Deserialize(reader);
                }
            }
            else
            {
                Console.WriteLine("Недоступный формат");
                return;
            }
            Console.WriteLine("Файл успешно загружен");
            foreach(var i in figures_list)
            {
                Console.WriteLine($"Название: {i.Name}\nШирина: {i.Width}\nВысота: {i.Height}");
            }
            Converter(figures_list);
        }
        public static void Converter(List<Figure> figures_list)
        {
            Console.WriteLine("Сохранить файл в одном из форматов (.txt/Json/XML) - F1. Для закрытия программы нажмите Escape.");
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.F1)
            {
                Console.Clear();
                Console.Write("Введите путь для сохранения файла:\n");
                string pathToSave = Console.ReadLine();
                string extension2 = Path.GetExtension(pathToSave);
                if (extension2 == ".txt")
                {
                    string strings = "";
                    foreach(var figure in figures_list)
                    {
                        strings += figure.Name + "\n";
                        strings += figure.Width + "\n";
                        strings += figure.Height + "\n";
                    }
                    File.WriteAllText(pathToSave, strings);
                }
                else if (extension2 == ".json")
                {
                    string json = JsonConvert.SerializeObject(figures_list);
                    File.WriteAllText(pathToSave, json);
                }
                else if (extension2 == ".xml")
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<Figure>));
                    using (FileStream reader = new FileStream(pathToSave, FileMode.OpenOrCreate))
                    {
                        serializer.Serialize(reader, figures_list);
                    }
                }
                else
                {
                    Console.WriteLine("Недоступный формат.");
                    return;
                }
                Console.WriteLine("Файл успешно сохранен.");
            }

        }

    } 
}