using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


namespace Database
{
    internal class Program
    {
        static void Main(string[] args)
        {


            ConsoleKeyInfo input;

            StudBase studbase = DeserializeObject<StudBase>("data.bin");



            while (true)
            {
                Console.WriteLine("==========База данных студентов ФКиСКД==========\n");
                Console.WriteLine("Выберите действие:\n1)Добавить студента\n2)Найти студента\n3)Удалить студента\n4)Показать всех студентов\n5)Выход");

                input = Console.ReadKey();
                Console.Clear();

                switch (input.Key)
                {
                    case ConsoleKey.D1:
                        CreateStudent(studbase);
                        
                        break;
                        case ConsoleKey.D2:
                            FindStudent(studbase);
                        break;
                        case ConsoleKey.D3:
                            Remove(studbase);
                        break;
                    case ConsoleKey.D4:
                        PrintStudentsList(studbase.GetList(), true);
                        break;
                    case ConsoleKey.F12:
                        SerializeObject("data.bin", studbase);
                        Console.WriteLine("Данные сохранены");
                        break;

                }

            }


        }
        static void Remove(StudBase studbase)
        {
            string id;
            Console.WriteLine("Введите ID студента:");
            id = Console.ReadLine();
            studbase.RemoveStudent(Guid.Parse(id));
            Console.Clear();
            Console.WriteLine("Студент удален.");
        }
        static void FindStudent(StudBase studbase)
        {
            ConsoleKeyInfo input;
            string request;
            Console.WriteLine("Поиск:\n1)По фамилии\n2)По имени\n3)По группе\n4)По месту жительства\n5)По инстаграму\n6)По телеграму");

            input = Console.ReadKey();


            switch (input.Key)
            {
                case ConsoleKey.D1:
                    Console.WriteLine("Введите фамилию");
                    request = Console.ReadLine();
                    PrintStudentsList(studbase.FindByLastName(request), false);
                    break;
                    case ConsoleKey.D2:
                    Console.WriteLine("Введите имя");
                    request = Console.ReadLine();
                    PrintStudentsList(studbase.FindByName(request), false);
                    break;
                case ConsoleKey.D3:
                    Console.WriteLine("Введите группу");
                    request = Console.ReadLine();
                    PrintStudentsList(studbase.FindByGroup(request), false);
                    break;
                case ConsoleKey.D4:
                    Console.WriteLine("Введите название населенного пункта");
                    request = Console.ReadLine();
                    PrintStudentsList(studbase.FindByLocation(request), false);
                    break;
                case ConsoleKey.D5:
                    Console.WriteLine("Введите инстаграм");
                    request = Console.ReadLine();
                    PrintStudentsList(studbase.FindByInst(request), false);
                    break;
                case ConsoleKey.D6:
                    Console.WriteLine("Введите инстаграм");
                    request = Console.ReadLine();
                    PrintStudentsList(studbase.FindByTelegram(request), false);
                    break;
            }
                  
        }
        static void CreateStudent(StudBase database)
        {
            string fullName;
            string group;
            bool gender;

            Console.WriteLine("Введите ФИО Студента:");
            fullName = Console.ReadLine();
            Console.WriteLine("Введите группу студента:");
            group = Console.ReadLine();
            Console.WriteLine("Укажите пол (по умолчанию мужской):");
            Console.WriteLine("0 - мужской, 1 - женский");
            Boolean.TryParse(Console.ReadLine(), out gender);
            database.Push(new Student(fullName, group, gender));
            Console.Clear();
            Console.WriteLine("Студент создан.");

        }

        static void SerializeObject<T>(string fileName, T obj)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(fs, obj);
            }
        }

        static T DeserializeObject<T>(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Open))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return (T)formatter.Deserialize(fs);
            }
        }

        static void PrintStudentsList(List<Student> list, bool showFullInfo)
        {
            ConsoleColor color = ConsoleColor.Blue;
            ConsoleColor oldColor = Console.ForegroundColor;
            int resultsOnPage = 2;
            for (int i = 0; i < list.Count; i++)
            {
                
                Console.ForegroundColor = color;
                Console.WriteLine("======================");
                Console.ForegroundColor = oldColor;
                if (showFullInfo)
                {
                    list[i].ShowFullInfo();
                }
                else
                {
                    list[i].ShowInfo();
                }
                Console.ForegroundColor = color;
                Console.WriteLine("======================");
                Console.ForegroundColor = oldColor;
                
            }
                    
        }
    }
    [Serializable]
    class Student
    {
        private string _firstName;
        private string _lastName;
        private string _surname;

        private bool _gender;

        private string _group;

        private string _location;

        private string _note;

        private Guid _id;

        private string _inst;
        private string _telegram;

        public string FirstName { get { return _firstName; } }
        public string LastName { get { return _lastName; } }
        public string Surname { get { return _surname; } }
        public string Group { get { return _group; } }
        public string Location { get { return _location; } }
        public string Inst { get { return _inst; } }
        public string Telegram { get { return _telegram; } }
        public Guid ID { get { return _id; } }

        public Student(string fullName, string group, bool gender = false)
        {
            string[] args = fullName.Split(' ');
            _lastName = args[0];
            _firstName = args[1];
            _surname = args.Length > 2 ? args[2] : "";

            _group = group;

            _gender = gender;

            _id = Guid.NewGuid();

        }

        public void ShowInfo()
        {
            Console.WriteLine($"Фио: {_lastName} {_firstName} {_surname}");
            Console.WriteLine($"Группа: {_group}");
            Console.WriteLine($"ID: {_id}");
        }
        public void ShowFullInfo()
        {
            Console.WriteLine($"Фио: {_lastName} {_firstName} {_surname}\nПол: " + (_gender ? "Женский" : "Мужской") + $"\nГруппа: {_group}\nID:{_id}\nInst: {_inst}\nTelegram:{_telegram}\nМесто жительства: {_location}\nПримечание: {_note}");

        }

        public void EditFullName(string fullName)
        {
            string[] args = fullName.Split(' ');
            _lastName = args[0];
            _firstName = args[1];
            _surname = args.Length > 2 ? args[2] : "";
        }
        public void EditGroup(string group)
        {
            _group = group;
        }
        public void EditLocation(string location)
        {
            _location = location;
        }
        public void EditGender(bool isFemale)
        {
            _gender = isFemale;
        }
    }
    [Serializable]
    class StudBase
    {
        private List<Student> _students = new List<Student>();

        public void Push(Student student)
        {
            _students.Add(student);

        }

        public List<Student> FindByName(string name)
        {
            List<Student> result = new List<Student>();
            foreach (var student in _students)
            {
                if (student.FirstName == name)
                {
                    result.Add(student);
                }
            }
            return result;
        }
        public List<Student> FindByLastName(string lastName)
        {
            List<Student> result = new List<Student>();
            foreach (var student in _students)
            {
                if (student.LastName == lastName)
                {
                    result.Add(student);
                }
            }
            return result;
        }
        public List<Student> FindByGroup(string group)
        {
            List<Student> result = new List<Student>();
            foreach (var student in _students)
            {
                if (student.Group == group)
                {
                    result.Add(student);
                }
            }
            return result;
        }
        public List<Student> FindByLocation(string location)
        {
            List<Student> result = new List<Student>();
            foreach (var student in _students)
            {
                if (student.Location.Contains(location))
                {
                    result.Add(student);
                }
            }
            return result;
        }
        public List<Student> FindByInst(string inst)
        {
            List<Student> result = new List<Student>();
            foreach (var student in _students)
            {
                if (student.Inst.Contains(inst))
                {
                    result.Add(student);
                }
            }
            return result;
        }
        public List<Student> FindByTelegram(string telegram)
        {
            List<Student> result = new List<Student>();
            foreach (var student in _students)
            {
                if (student.Inst.Contains(telegram))
                {
                    result.Add(student);
                }
            }
            return result;
        }
        public void RemoveStudent(Guid id)
        {
            for (int i = 0; i < _students.Count; i++)
            {
                if (_students[i].ID == id)
                {
                    _students.RemoveAt(i);
                    break;
                }
            }
        }
        public List<Student> GetList()
        {
            return _students;
        }

    }
}
