using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

namespace FinalTask;
class Program
{
    static void Main(string[] args)
    {
        string file = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Students.dat";
        string dirWrite = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + @"\Students\";

        ParseBin(file, dirWrite);
        Console.ReadKey();
    }
    
    static void ParseBin(string file, string directory)
    {
        if (File.Exists(file))
        {
            BinaryFormatter formatter = new();
            try
            {
                //throw new SerializationException();
                //throw new ArgumentNullException();
                using (var fs = new FileStream(file.ToString(), FileMode.Open))
                {
                    Student[]? students = formatter.Deserialize(fs) as Student[];
                    foreach (Student item in students)
                    {
                        //Console.WriteLine($"Group {item.Group} \n Name {item.Name} \n  Date{item.DateOfBirth} ");    
                        var print = (Student student, string directory) =>
                        {
                            if (!Directory.Exists(directory))
                            {
                                Directory.CreateDirectory(directory);
                            }
                            try
                            {
                                StreamWriter output = File.AppendText(directory + student.Group + ".txt");
                                output.WriteLine($"Имя студента : {student.Name}");
                                output.WriteLine($"Дата рождения: {student.DateOfBirth:dd MMMM yyyy}");
                                output.WriteLine("<---------------------------->");
                                output.Close(); // скопировал с проекта под Net 4.7.2, там using не поможет, и обратная совместимость, и лень програмиста
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine($"Ошибка записи файла. {e.Message}");
                            }
                        };
                        print(item, directory);
                    }
                }
                Console.WriteLine("Работа програмы завершена успешно");
            }
            catch(Exception e)
            {
                if (e is SerializationException)  e = new Exception("проверьте объект десериализации на коректность.");
                if (e is ArgumentNullException) e = new Exception("скорее всего где-то потерялись данные.");
                //Вывод ошибки
                Console.WriteLine($"Ошибка десериализации, {e.Message}");
            }
        }
    }
}